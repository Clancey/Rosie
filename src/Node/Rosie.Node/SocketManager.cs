using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

namespace Rosie.Node
{
	class SocketManager
	{
		public bool DebugMode { get; set; } = true;
		public bool Connected { get; private set; }

		public Action StatusChanged { get; set; }

		static Socket socket;

		public Action<NodeValueUpdate> NodeValueUpdated;

		public async Task<bool> Connect (string server = "http://192.168.86.10:3000")
		{
			var tcs = new TaskCompletionSource<bool> ();
			socket = IO.Socket (server);
			socket.On (Socket.EVENT_DISCONNECT, () => {
				Connected = false;
				StatusChanged?.Invoke ();
			});
			socket.On (Socket.EVENT_CONNECT_TIMEOUT, () => {
				Connected = false;
				tcs.TrySetResult (false);
				StatusChanged?.Invoke ();
			});
			socket.On (Socket.EVENT_CONNECT_ERROR, (data) => {
				Connected = false;
				tcs.TrySetResult (false);
			});
			socket.On (Socket.EVENT_CONNECT, (object obj) => {
				Connected = true;
				tcs.TrySetResult (true);
				StatusChanged?.Invoke ();
			});
			socket.On ("node-added", (data) => {
				Console.WriteLine ("Node Added");
				Console.WriteLine (data);

				//var obj = (JContainer)data;
				//var sessionId = (string)obj ["sessionId"];
				//Console.WriteLine ("Touch from session: {0}", sessionId);
				//var evt = obj.ToObject<PointerEvent> ();
				//Task.Run (() => {
				//	DeviceService.ForSession (sessionId).Send (evt);
				//});
			});
			socket.On ("node-naming", (data) => {
				Log ($"Node Naming: {data}");
				
			});
			socket.On ("node-available", (data) => {
				Log ($"Node Available: {data}");

			});
			socket.On ("node-ready", (data) => {
				Log ($"Node Ready: {data}");

			});
			socket.On ("node-event", (data) => {
				Log ($"Node event: {data}");

			});

			socket.On ("scene-event", (data) => {
				Log ($"Scene event: {data}");
			});

			socket.On ("value-added", (data) => {

				Log ($"Node Value Added: {data}");

				var obj = ((JObject)data).ToObject<NodeValueUpdate> ();
				NodeValueUpdated?.Invoke (obj);

			});
			socket.On ("value-changed", (data) => {

				Log ($"Node Value Changed: {data}");

				var obj = ((JObject)data).ToObject<NodeValueUpdate> ();
				NodeValueUpdated?.Invoke (obj);

			});
			socket.On ("notification", (data) => {
				Log ($"Node Notification: {data}");
			});

			var success = await tcs.Task;
			return success;
		}

		void Log (string message)
		{
			if (DebugMode)
				Console.WriteLine (message);
		}
	}
}

