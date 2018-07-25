using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Rosie.Services;
using System.Linq;

namespace Rosie.Hue
{
	public class HueService : IRosieService, IDisposable
	{
		//cUIpxMwkuC0xb-JVYsQOaBI-4BzKM6fl02fnQbCf

		string _apiToken = "cUIpxMwkuC0xb-JVYsQOaBI-4BzKM6fl02fnQbCf";
		bool _isConnected;

		ILocalHueClient _hueClient;
		ILogger<HueService> _logger;
		IDeviceManager _deviceManager;
		IServicesManager _serviceManager;
		public HueService(ILoggerFactory loggerFactory, IDeviceManager deviceManager, IServicesManager serviceManager)
		{
			if (loggerFactory != null)
				_logger = loggerFactory.AddConsole(LogLevel.Information).CreateLogger<HueService>();
			_logger?.LogInformation("Setup HUE lights");
			_logger?.LogInformation(Description);
			_deviceManager = deviceManager;
			_serviceManager = serviceManager;
			_serviceManager?.RegisterService(Domain, nameof(TurnOn), TurnOn);
			_serviceManager?.RegisterService(Domain, nameof(TurnOff), TurnOff,"Send deviceid");
		}
		public string Domain => "HUE";

		public string Name => "HUESERVICE";

		public string Description => "Integration with Phillips Hue";

		public string ServiceIdentifier => nameof(HueService);

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
				await GetLights();
			}
		}

		async Task GetLights()
		{
			var lights = await _hueClient.GetLightsAsync();
			foreach (var light in lights)
			{
				var device = new Device
				{
					Id = light.Id,
					Service = ServiceIdentifier,
					Description = light.Name,
					Manufacturer = light.ManufacturerName,
					ManufacturerId = light.ModelId,
					ProductId = light.ProductId,
					Name = light.Name,
					DeviceType = DeviceTypeKeys.Switch,
				};
				device.Discoverable = !string.IsNullOrWhiteSpace(device.Name);
				await _deviceManager.AddDevice(device);
			}
		}

		public Task Stop()
		{
			return Task.Run(() =>
			{
				_logger.LogInformation("Stop");
			});
		}

		public void Dispose()
		{

		}

		async Task Connect()
		{
			IBridgeLocator locator = new HttpBridgeLocator();
			var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
			var ip = bridgeIPs.FirstOrDefault();
			if (ip == null)
			{
				_logger.LogError("Didn't find a HUE Bridge...");
				return;
			}
			_hueClient = new LocalHueClient(ip.IpAddress);
			try
			{
				if (string.IsNullOrEmpty(_apiToken))
					_apiToken = await _hueClient.RegisterAsync(Domain, "Rosie");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed Connect", ex);
				if (ex.Message == "Link button not pressed")
				{
					_logger.LogWarning("Please press the button");
				}
			}
			_isConnected = !string.IsNullOrEmpty(_apiToken);
			_hueClient.Initialize(_apiToken);
			_logger.LogInformation("Connected");
		}

		public Task<bool> HandleRequest(Device device, DeviceUpdate request)
		{
			throw new NotImplementedException();
		}

		public async Task TurnOn(Data data)
		{
			await SetState(true, data);
		}

		public async Task TurnOff(Data data)
		{
			await SetState(false, data);
		}

		async Task SetState(bool isOn, Data data)
		{
			if (data.DataS != null)
			{
				try
				{
					await _hueClient.SendCommandAsync(new LightCommand()
					{
						On = isOn
					});
				}
				catch (Exception ex)
				{
					_logger.LogError(nameof(TurnOn), ex);
				}
			}
			_logger.LogInformation("TurnOn");
		}


	}
}
