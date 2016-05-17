using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rosie.Node
{
	public class NodeManager
	{

		internal static string NodeServerUrl {
			get { return Rosie.Settings.GetSecretString (); }
		}
		NodeApi nodeApi;
		SocketManager sockets;
		public static NodeManager Shared { get; set; } = new NodeManager ();

		public Task Init ()
		{
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
					nodeApi = new NodeApi ();
					Devices = await nodeApi.  GetDevices () ?? new List<NodeDevice>();

					sockets = new SocketManager ();
					sockets.StatusChanged = async () => {
						if (IsConnected)
							return;
						Console.WriteLine ("Socket disconnected. Attempting recconect");
						await Connect ();
					};
					return await sockets.Connect (NodeServerUrl);
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

		public bool IsConnected => sockets?.Connected ?? false;


		List<NodeDevice> Devices = new List<NodeDevice> ();
	}
}

