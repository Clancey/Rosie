using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rosie.Node
{
	public class NodeManager : IDeviceHandler
	{

		internal static string NodeServerUrl {
			get { return Rosie.Settings.GetSecretString (); }
		}
		NodeApi nodeApi;
		SocketManager sockets;
		public static NodeManager Shared { get; set; } = new NodeManager ();

		public Task Init ()
		{
			DeviceManager.Shared.RegisterHandler(this);
			return Connect ();
		}

		Task<bool> connectTask;
		public Task<bool> Connect ()
		{
			if (connectTask?.IsCompleted ?? true)
				connectTask = connect ();
			return connectTask;
		}
		const int ConnectRetryCount = 5;
		async Task<bool> connect ()
		{
			int tryCount = 0;
			while (!IsConnected && tryCount < ConnectRetryCount) {
				try {
					return (await Task.WhenAll ( SetupNodeApi (),
					                             SetupSockets ())).All(x=> x);
				} catch (Exception ex) {
					Console.WriteLine (ex);
				}
				if (!IsConnected) {
					Console.WriteLine ("Error connecting to the node server, trying again");
					tryCount++;
					await Task.Delay (1000);
				}
			}
			return IsConnected;
		}

		async Task<bool> SetupNodeApi ()
		{
			nodeApi = new NodeApi ();
			Devices = await nodeApi.GetDevices ().ConfigureAwait (false) ?? new List<NodeDevice> ();

			await AddDevices (Devices);

			return true;
		}

		async Task<bool> SetupSockets ()
		{
			sockets = new SocketManager ();
			sockets.StatusChanged = async () => {
				if (IsConnected)
					return;
				Console.WriteLine ("Socket disconnected. Attempting recconect");
				await Connect ();
			};
			sockets.NodeValueUpdated = async (nodeUpdate) => {
				var update = await nodeUpdate.ToDeviceUpdate ();
				await DeviceManager.Shared.UpdateCurrentState (update);
			};
			return await sockets.Connect (NodeServerUrl);
		}

		public bool IsConnected => sockets?.Connected ?? false;

		public string ServiceIdentifier => "node";

		List<NodeDevice> Devices = new List<NodeDevice> ();

		async Task AddDevice (NodeDevice nodeDevice)
		{
			var nodeId = nodeDevice.Classes.FirstOrDefault ().Value?.FirstOrDefault ().Value?.NodeId ?? -1;
			if (nodeId <= 0)
				return;
			var device = new Device {
				Id = nodeDevice.Id,
				Service = ServiceIdentifier,
				Description = nodeDevice.Name,
				Location = nodeDevice.Loc,
				Manufacturer = nodeDevice.Manufacturer,
				ManufacturerId = nodeDevice.ManufacturerId,
				Product = nodeDevice.Product,
				ProductType = nodeDevice.ProductType,
				ProductId = nodeDevice.ProductId,
				Name = DecentName(nodeDevice),
				Type = nodeDevice.Type,
				DeviceType = FromNodeType (nodeDevice.Type),
			};
			device.Discoverable = !string.IsNullOrWhiteSpace (device.Name);
			await DeviceManager.Shared.AddDevice (device);
			nodeDevice.NodeId = nodeId;
			await NodeDatabase.Shared.InsertDevice (nodeDevice);

		}
		static string DecentName(NodeDevice device)
		{
			if (!string.IsNullOrWhiteSpace (device.Name))
				return device.Name;
			if (!string.IsNullOrEmpty (device.Manufacturer))
				return $"{device.Manufacturer} {device.Type}";
			return null;
		}

		internal static DeviceType FromNodeType (string type)
		{
			switch (type) {
			case "Binary Power Switch":
				return DeviceType.Switch;
			}
			return DeviceType.Unknown;
		}

		async Task AddDevices (List<NodeDevice> devices)
		{
			await Task.WhenAll(devices.Select (x => AddDevice (x)));
		}

		public async Task<bool> HandleRequest (Device device, SetDeviceStateRequest request)
		{
			try {
				var nodeId = int.Parse(device.Id.Replace ("node-", ""));

				var nodeDevice = await NodeDatabase.Shared.GetDevice (nodeId);
				var command = await nodeDevice.GetPerferedCommand ();
				var s = await nodeApi.SetState (command, request.On);
				return s;

			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}
	}
}

