#if ROSIE
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Rosie.Services;
using Microsoft.Extensions.Logging;

namespace Rosie.SmartThings
{
	public class SmartThingsManager : IRosieService, IDisposable
	{
		public string ServiceIdentifier => "SmartThings";

		public string Domain => "SmartThings";

		public string Name => "SmartThings";

		public string Description => "Integration with Phillips Hue";

		public Device[] Devices = new Device[];

		public async Task<bool> HandleRequest(Rosie.Device device, DeviceUpdate request)
		{
			
			switch (request.Key)
			{
				case DeviceState.SwitchState:
					if (request.BoolValue.Value)
						await api.TurnOnDevice();
					return true;
			}
		}
		SmartThingsApi api = new SmartThingsApi("SmartThings", "fdsjfhe545343uiryeui43hd1hfdsfy8");
		SmartThingsUpdateListener updater;
		readonly ILoggerFactory loggerFactory;
		readonly IDeviceManager deviceManager;
		readonly IServicesManager serviceManager;

		public SmartThingsManager(ILoggerFactory loggerFactory, IDeviceManager deviceManager, IServicesManager serviceManager)
		{
			this.loggerFactory = loggerFactory;
			this.deviceManager = deviceManager;
			this.serviceManager = serviceManager;
		}



		public async Task Start()
		{
			if (!api.HasLoggedIn())
			{
				return;
			}
			await Login();
		}

		public async Task<bool> Login()
		{
			var account = await api.Authenticate();
			if (account == null)
				return false;
			Devices = await api.GetDevices();
			updater = new SmartThingsUpdateListener(api);
			updater.UpdateReceived += (obj) =>
			{
				Console.WriteLine("Status update");
				if (obj.ShouldIgnoreUpdate())
					return;

			};
			await updater.StartListening();
			return true;
		}

		public Task Stop()
		{
			updater.Close();
			updater = null;
			return Task.FromResult(true);
		}

		public Task Send()
		{
			throw new NotImplementedException();
		}

		public Task Received(object data)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
#endif