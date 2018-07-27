using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rosie.Services;
using System.Linq;
using Rosie.Server.Routes.Node;
using System.IO;
using System.Reflection;

namespace Rosie.Node
{
	public class NodeService : IRosieService
	{
		IDeviceManager _deviceManager;
		ILogger<NodeService> _logger;
		IServicesManager _serviceManager;
		public NodeService(ILoggerFactory loggerFactory, IDeviceManager deviceManager, IServicesManager serviceManager)
		{
			if (loggerFactory != null)
				_logger = loggerFactory.AddConsole(LogLevel.Information).CreateLogger<NodeService>();
			_logger?.LogInformation("Setup Node ZWave");
			_logger?.LogInformation(Description);
			_deviceManager = deviceManager;
			_serviceManager = serviceManager;
		}

		public NodeService()
		{
		}

		internal static string NodeServerUrl
		{
			get { return Rosie.Settings.GetSecretString(); }
		}
		NodeApi nodeApi;
		SocketManager sockets;
		NodeSession nodeSession;


		Task<bool> connectTask;
		public Task<bool> Connect()
		{
			if (connectTask?.IsCompleted ?? true)
				connectTask = connect();
			return connectTask;
		}
		const int ConnectRetryCount = 5;
		async Task<bool> connect()
		{
			int tryCount = 0;
			while (!IsConnected && tryCount < ConnectRetryCount)
			{
				try
				{
					return (await Task.WhenAll(SetupNodeApi(),
												 SetupSockets())).All(x => x);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
				if (!IsConnected)
				{
					Console.WriteLine("Error connecting to the node server, trying again");
					tryCount++;
					await Task.Delay(1000);
				}
			}
			return IsConnected;
		}

		async Task<bool> SetupNodeApi()
		{
			nodeApi = new NodeApi();
			Devices = await nodeApi.GetDevices().ConfigureAwait(false) ?? new List<NodeDevice>();

			await AddDevices(Devices);

			return true;
		}

		async Task<bool> SetupSockets()
		{
			sockets = new SocketManager();
			sockets.StatusChanged = async () =>
			{
				if (IsConnected)
					return;
				Console.WriteLine("Socket disconnected. Attempting recconect");
				await Connect();
			};
			sockets.NodeValueUpdated = async (nodeUpdate) =>
			{
				try
				{
					var update = await nodeUpdate.ToDeviceUpdate();
					if (update == null)
						return;
					await _deviceManager.UpdateCurrentState(update);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			};
			return await sockets.Connect("http://localhost:8080");
		}

		public bool IsConnected => sockets?.Connected ?? false;

		public string ServiceIdentifier => "node";

		public string Domain => "node";

		public string Name => "Node ZWave";

		public string Description => "Node ZWave service";

		List<NodeDevice> Devices = new List<NodeDevice>();

		async Task AddDevice(NodeDevice nodeDevice)
		{
			var nodeId = nodeDevice.Classes.FirstOrDefault().Value?.FirstOrDefault().Value?.NodeId ?? -1;
			if (nodeId <= 0)
				return;
			var oldNodeDevice = await NodeDatabase.Shared.GetDevice(nodeId);
			Device device = null;
			if (!string.IsNullOrWhiteSpace(oldNodeDevice?.Id))
			{
				device = await _deviceManager.GetDevice(oldNodeDevice.Id);
			}
			if(device == null)
			{
				var oldDevices = await _deviceManager.GetAllDevices();
				device = oldDevices.FirstOrDefault(x => x.Service == this.ServiceIdentifier && x.ServiceDeviceId == nodeId.ToString()) ?? new Device { Service = ServiceIdentifier, Id = oldNodeDevice?.Id };
			}
			if (!device.Update(nodeDevice))
				return;
			device.Discoverable = !string.IsNullOrWhiteSpace(device.Name);
			await _deviceManager.AddDevice(device);
			nodeDevice.Id = device.Id;
			await NodeDatabase.Shared.InsertDevice(nodeDevice);

		}
		static string DecentName(NodeDevice device)
		{
			if (!string.IsNullOrWhiteSpace(device.Name))
				return device.Name;
			if (!string.IsNullOrEmpty(device.Manufacturer))
				return $"{device.Manufacturer} {device.Type}";
			return null;
		}

		async Task AddDevices(List<NodeDevice> devices)
		{
			await Task.WhenAll(devices.Select(x => AddDevice(x)));
		}

		public async Task<bool> HandleRequest(Device device, DeviceUpdate request)
		{
			try
			{
				var nodeId = int.Parse(device.ServiceDeviceId);

				var nodeDevice = await NodeDatabase.Shared.GetDevice(nodeId);
				var command = await nodeDevice.GetPerferedCommand();
				var s = await nodeApi.SetState(command, request.Value);
				return s;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return false;
		}

		public async Task Start()
		{
			try
			{
				//LocalWebServer.Shared.Router.AddRoute<NodeDevicesRoute>();
				//LocalWebServer.Shared.Router.AddRoute<NodeDeviceRoute>();
				//LocalWebServer.Shared.Router.AddRoute<NodePerferedCommandRoute>();
				nodeSession = new NodeSession();
				await Task.Run(()=>nodeSession.Start(Path.Combine( Directory.GetCurrentDirectory(),"..","src","Node","Rosie.Node", "NodeServer")));
				//_deviceManager.RegisterHandler(this);
				await Connect();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public Task Stop()
		{
			nodeSession.Stop();
			sockets.Stop();
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
	}
}

