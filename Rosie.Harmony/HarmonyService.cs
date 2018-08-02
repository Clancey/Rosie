using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rosie.Services;
using System.Linq;
using Harmony;

namespace Rosie.Harmony
{
	public class HarmonyService : IRosieService, IDisposable
	{
		bool _isConnected;

		List<Hub> hubs = new List<Hub>();
		ILogger<HarmonyService> _logger;
		IDeviceManager _deviceManager;
		IServicesManager _serviceManager;

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
			{
				await GetActivities();
			}
		}

		async Task GetActivities()
		{
			var existingActivities = await _deviceManager.GetAllDevices();

			var activities = new Dictionary<Hub, Activity>();

			Parallel.ForEach<Hub>(hubs, async (hub) =>
			{
				_logger.LogInformation("Syncing Hub Configuration: {0}", hub.Info.FriendlyName);
				await hub.SyncConfigurationAsync();

				foreach (var activity in hub.Activities)
					activities.Add(hub, activity);
			});

			foreach (var hubActivity in activities)
			{
				var globalActivityId = hubActivity.Key.Info.AccountId + hubActivity.Key.Info.RemoteId + hubActivity.Value.Id;

				var oldDevice = existingActivities.FirstOrDefault(cw => cw.ServiceDeviceId == globalActivityId);
				if (oldDevice != null)
					return;
				var device = new Device
				{
					ServiceDeviceId = globalActivityId,
					Service = ServiceIdentifier,
					Description = hubActivity.Value.Label,
					Manufacturer = "Logitech",
					ManufacturerId = "LOGITECH",
					ProductId = hubActivity.Key.Info.ProductId,
					Name = hubActivity.Value.Label,
					DeviceType = DeviceTypeKeys.Switch,
				};
				device.Discoverable = !string.IsNullOrWhiteSpace(device.Name);
				await _deviceManager.AddDevice(device);
			}
		}

		public Task Stop()
		{
			return Task.Run(async () =>
			{
				foreach (var hub in hubs)
					await hub.Disconnect();

				_logger.LogInformation("Stop");
			});
		}

		public void Dispose()
		{

		}

		async Task Connect()
		{
			hubs.Clear();

			_logger.LogInformation("Discovering Harmony Hubs...");

			// Discover hubs on the network
			var discovery = new OneShotHubDiscovery(_logger);
			var hubInfos = await discovery.DiscoverAsync();

			Parallel.ForEach<HubInfo>(hubInfos, async (hubInfo) => {
				_logger.LogInformation("Found Hub: {0} ({1})", hubInfo.FriendlyName, hubInfo.IP);
				var hub = new Hub(hubInfo);

				_logger.LogInformation("Connecting to Hub: {0} ({1})", hubInfo.FriendlyName, hubInfo.IP);
				await hub.ConnectAsync(DeviceID.GetDeviceDefault());

				hubs.Add(hub);
			});

			_isConnected = hubs.Any();

			if (!_isConnected)
				_logger.LogWarning("No Harmony Hubs discovered!");
			else
				_logger.LogInformation("Connected");
		}

		public async Task<bool> HandleRequest(Device device, DeviceUpdate request)
		{
			try {
				Activity activity = null;
				Hub hubForActivity = null;

				// Look through each hub to find a matching activity
				foreach (var hub in hubs) {
					activity = hub.Activities.FirstOrDefault(a => hub.Info.AccountId + hub.Info.RemoteId + a.Id == device.ServiceDeviceId);
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

		class OneShotHubDiscovery
		{
			public OneShotHubDiscovery(ILogger<HarmonyService> logger)
			{
				_logger = logger;
			}

			ILogger<HarmonyService> _logger;
			Task _delayForMoreHubs;
			TaskCompletionSource<IEnumerable<HubInfo>> _tcsDiscover;
			DiscoveryService _discoveryService;
			readonly List<HubInfo> _discoveredHubs = new List<HubInfo>();

			public Task<IEnumerable<HubInfo>> DiscoverAsync()
			{
				// This should only be called once, return the completion source on subsequent calls
				if (_tcsDiscover != null)
					return _tcsDiscover.Task;

				_tcsDiscover = new TaskCompletionSource<IEnumerable<HubInfo>>();

				// Start up the discovery service and listen for found hubs
				_discoveryService = new DiscoveryService();
				_discoveryService.HubFound += Ds_HubFound;
				_discoveryService.StartDiscovery();

				return _tcsDiscover.Task;
			}

			void Ds_HubFound(object sender, HubFoundEventArgs e)
			{
				const int delayForMoreMs = 2000;

				// Add the discovered hub
				_discoveredHubs.Add(e.HubInfo);

				// If we haven't already started a delayed task to return the found hubs
				// do this now, which gives us a bit of time to wait for more hubs to be found
				if (_delayForMoreHubs == null) {
					_logger.LogInformation($"Waiting {delayForMoreMs}ms for any more hubs...");

					_delayForMoreHubs = Task.Delay(delayForMoreMs);
					_delayForMoreHubs.ContinueWith(t =>
					{
						// Once we've waited for more hubs to be found stop discovery
						_discoveryService?.StopDiscovery();

						// Set the completion source result to all the hubs we found
						_tcsDiscover.TrySetResult(_discoveredHubs);
					});
				}
			}
		}

	}
}
