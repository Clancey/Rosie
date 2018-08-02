using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rosie.Services;
using System.Linq;
using Harmony;
using Harmony.DeviceWrappers;
using System.Threading;

namespace Rosie.Harmony
{
	public class HarmonyService : IRosieService
	{
		public static void LinkerPreserve()
		{
		}

		bool _isConnected;
		bool _isConnecting;

		List<Hub> hubs = new List<Hub>();
		ILogger<HarmonyService> _logger;
		IDeviceManager _deviceManager;
		IServicesManager _serviceManager;

        public event EventHandler<DeviceState> CurrentStateUpdated;
        public event EventHandler<Device> DeviceAdded;

        public HarmonyService(ILoggerFactory loggerFactory, IDeviceManager deviceManager, IServicesManager serviceManager)
		{
			if (loggerFactory != null)
				_logger = loggerFactory.AddConsole(LogLevel.Information).CreateLogger<HarmonyService>();
			_logger?.LogInformation("Setup Harmony Hub");
			_logger?.LogInformation(Description);
			_deviceManager = deviceManager;
			_serviceManager = serviceManager;
		}
		public string Domain => "HARMONY";

		public string Name => "HARMONYSERVICE";

		public string Description => "Integration with Logitech Harmony Hub";

		public string ServiceIdentifier => nameof(HarmonyService);

		public Task Send()
		{
			throw new NotImplementedException();
		}

		public Task Received(object data)
		{
			throw new NotImplementedException();
		}

		public async Task Start()
		{
			_logger.LogInformation("Starting");

			await Connect();

			if (_isConnected)
				await GetActivities();
		}

		async Task GetActivities()
		{

			// Add all the activities from the hubs as devices
			foreach (var hub in hubs) {

				if ((hub?.Activities?.Length ?? 0) <= 0)
					_logger.LogWarning("No Activities found for Hub: {0}", hub.Info.FriendlyName);

				foreach (var activity in hub.Activities) {

					// Create an id with the hub account/remoteid and the activity id
					// in case of multiple hubs, so the id is guaranteed to be unique
					var globalActivityId = GetActivityId(hub.Info, activity);

					// See if the device exists yet
                    var oldDevice = _deviceManager.GetDevice(ServiceIdentifier, globalActivityId);
					if (oldDevice != null)
						return;

					// Add the hub name in the case of multiple hubs (activity names could be the same on diff hubs)
					var deviceName = activity.Label + (hubs.Count > 1 ? $"({hub.Info.FriendlyName})" : string.Empty);

					var device = new Device {
						ServiceDeviceId = globalActivityId,
						Service = ServiceIdentifier,
						Description = deviceName,
						Manufacturer = "Logitech",
						ManufacturerId = "LOGITECH",
						ProductId = hub.Info.ProductId,
						Name = deviceName,
						DeviceType = DeviceTypeKeys.Switch,
						Discoverable = !string.IsNullOrEmpty(deviceName)
					};

					_logger.LogInformation("Adding Activity as device: {0}", device.Name);

                    // Add the device
                    DeviceAdded?.Invoke(this, device);
				}
			}
		}

		public Task Stop()
		{
			return Task.Run(async () => {
				foreach (var hub in hubs) {
					_logger.LogInformation("Disconnecting from hub: {0} ({1})", hub.Info.FriendlyName, hub.Info.IP);
					await hub.Disconnect();
				}

				_logger.LogInformation("Stop");
			});
		}

		async Task Connect()
		{
			if (_isConnected || _isConnecting)
				return;

			_isConnecting = true;

			hubs.Clear();

			_logger.LogInformation("Discovering Harmony Hubs...");

			// Discover hubs on the network
			var discovery = new OneShotHubDiscovery(_logger);
			var hubInfos = await discovery.DiscoverAsync();

			_logger.LogInformation("Discovered {0} Harmony Hubs.", hubInfos.Count());

			foreach (var hubInfo in hubInfos) {
				var hub = new Hub(hubInfo);

				// Connect to the hub
				_logger.LogInformation("Connecting to Hub: {0} ({1})", hubInfo.FriendlyName, hubInfo.IP);
				await hub.ConnectAsync(DeviceID.GetDeviceDefault());

				// Sync config which populates hub.Activities
				_logger.LogInformation("Syncing Hub Configuration: {0} ({1})", hub.Info.FriendlyName, hubInfo.IP);
				await hub.SyncConfigurationAsync();;

				hubs.Add(hub);
			};

			// Our connected state depends on if we connected to any hubs
			_isConnected = hubs.Any();
			_isConnecting = false;

			if (!_isConnected)
				_logger.LogWarning("No Harmony Hubs discovered!");
			else
				_logger.LogInformation("Connected to {0} hubs.", hubs.Count());
		}

		public async Task<bool> HandleRequest(Device device, DeviceUpdate request)
		{
			try {
				Activity activity = null;
				Hub hubForActivity = null;

				// Look through each hub to find a matching activity
				foreach (var hub in hubs) {
					activity = hub.Activities.FirstOrDefault(a => GetActivityId(hub.Info, a) == device.ServiceDeviceId);
					if (activity != null) {
						hubForActivity = hub;
						break;
					}
				}

				// See if we matched an activity
				if (activity != null) {
					if (request.PropertyKey == DevicePropertyKey.SwitchState && request.BoolValue.HasValue) {
						var on = request.BoolValue.Value;

						// If off, then just end the current activity
						if (!on)
							await hubForActivity.EndActivity();
						else // Otherwise start the activity we found
							await hubForActivity.StartActivity(activity);
					}
				}
			} catch (Exception ex) {
				_logger.LogError(ex.ToString());
			}
			return false;
		}

		string GetActivityId(HubInfo hubInfo, Activity activity)
			=> $"{hubInfo.AccountId}-{hubInfo.RemoteId}-{activity.Id}";

		class OneShotHubDiscovery
		{
			public OneShotHubDiscovery(ILogger<HarmonyService> logger)
			{
				_logger = logger;
			}

			ILogger<HarmonyService> _logger;
			TaskCompletionSource<IEnumerable<HubInfo>> _tcsDiscover;
			CancellationTokenSource _ctsDiscovering;
			DiscoveryService _discoveryService;
			readonly List<HubInfo> _discoveredHubs = new List<HubInfo>();
			bool foundAtLeastOneHub = false;

			public Task<IEnumerable<HubInfo>> DiscoverAsync()
			{
				// This should only be called once, return the completion source on subsequent calls
				if (_tcsDiscover != null)
					return _tcsDiscover.Task;

				_tcsDiscover = new TaskCompletionSource<IEnumerable<HubInfo>>();

				// Cancel discovery after a timeout
				_ctsDiscovering = new CancellationTokenSource(20000);
				_ctsDiscovering.Token.Register(() => {
					_tcsDiscover.TrySetResult(_discoveredHubs);
					try { _discoveryService?.StopDiscovery(); } catch {}
				}, true);

				// Start up the discovery service and listen for found hubs
				_discoveryService = new DiscoveryService();
				_discoveryService.HubFound += Ds_HubFound;
				_discoveryService.StartDiscovery();

				return _tcsDiscover.Task;
			}

			void Ds_HubFound(object sender, HubFoundEventArgs e)
			{
				// Add the discovered hub
				_discoveredHubs.Add(e.HubInfo);

				// Found a hub, let's hurry up cancellation since others should be detected around
				// the same time, or rather quickly
				if (!foundAtLeastOneHub) {
					foundAtLeastOneHub = true;
					_ctsDiscovering.CancelAfter(5000);
				}
			}
		}
	}
}
