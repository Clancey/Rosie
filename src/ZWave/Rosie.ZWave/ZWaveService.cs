using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rosie.Services;
using System.Linq;
using ZWaveLib;

namespace Rosie.ZWave
{
	public class ZWaveService : IRosieService
	{
		readonly IDeviceManager deviceManager;
		readonly IServicesManager serviceManager;
		ILogger<ZWaveService> _logger;
		ZWaveController controller;

		public ZWaveService(ILoggerFactory loggerFactory, IDeviceManager deviceManager, IServicesManager serviceManager)
		{
			if (loggerFactory != null)
				_logger = loggerFactory.AddConsole(LogLevel.Information).CreateLogger<ZWaveService>();
			_logger?.LogInformation("Setup ZWAve lights");
			_logger?.LogInformation(Description);
			this.deviceManager = deviceManager;
			this.serviceManager = serviceManager;
		}

		public string Domain => "ZWAVE";

		public string Name => "ZWave Service";

		public string Description => "ZWave Service";

		public string ServiceIdentifier => "zwave";

		public ControllerStatus controllerStatus { get; private set; }

		public Task<bool> HandleRequest(Device device, DeviceUpdate request)
		{
			throw new NotImplementedException();
		}

		public Task Received(object data)
		{
			throw new NotImplementedException();
		}

		public Task Send()
		{
			throw new NotImplementedException();
		}

		public async Task Start()
		{
			try
			{
				_logger.LogInformation("Starting");
				controller = new ZWaveController(Settings.ZWavePort);
				controller.ControllerStatusChanged += Controller_ControllerStatusChanged;
				controller.DiscoveryProgress += Controller_DiscoveryProgress;
				controller.NodeOperationProgress += Controller_NodeOperationProgress;
				controller.NodeUpdated += Controller_NodeUpdated;
				controller.Connect();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error loading ZWave Controller: {1}",ex);
			}
		}

		public Task Stop()
		{
			throw new NotImplementedException();
		}

		void Controller_ControllerStatusChanged(object sender, ControllerStatusEventArgs args)
		{
			_logger.LogInformation("ControllerStatusChange {0}", args.Status);
			controllerStatus = args.Status;
			switch (controllerStatus)
			{
				case ControllerStatus.Connected:
					// Initialize the controller and get the node list
					controller.GetControllerInfo();
					controller.GetControllerCapabilities();
					controller.GetHomeId();
					controller.GetSucNodeId();
					controller.Initialize();
					break;
				case ControllerStatus.Disconnected:
					break;
				case ControllerStatus.Initializing:
					break;
				case ControllerStatus.Ready:
					controller.Discovery();
					break;
				case ControllerStatus.Error:
					break;
			}
		}

		public void ChangePort(string serialPortName)
		{
			Settings.ZWavePort = serialPortName;
			controller.PortName = serialPortName;
			controller.Connect();
		}

		void Controller_DiscoveryProgress(object sender, DiscoveryProgressEventArgs args)
		{
			_logger.LogInformation("DiscoveryProgress {0}", args.Status);
			switch (args.Status)
			{
				case DiscoveryStatus.DiscoveryStart:
					break;
				case DiscoveryStatus.DiscoveryEnd:
					break;
			}
		}

		void Controller_NodeOperationProgress(object sender, NodeOperationProgressEventArgs args)
		{
			_logger.LogInformation("NodeOperationProgress {0} {1}", args.NodeId, args.Status);
		}

		void Controller_NodeUpdated(object sender, NodeUpdatedEventArgs args)
		{
			_logger.LogInformation("NodeUpdated {0} Event Parameter {1} Value {2}", args.NodeId, args.Event.Parameter, args.Event.Value);
		}

	}
}
