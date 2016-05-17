using System;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;

namespace Rosie.Node
{
	class SocketManager
	{
		public bool Connected { get; private set; }

		public Action StatusChanged { get; set; }

		static Socket socket;

		public async Task<bool> Connect (string server = "http://localhost:3000")
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
				Console.WriteLine (data);
				
			});
			socket.On ("node-available", (data) => {
				Console.WriteLine (data);

			});
			socket.On ("node-ready", (data) => {
				Console.WriteLine (data);

			});
			socket.On ("node-event", (data) => {
				Console.WriteLine (data);

			});

			socket.On ("scene-event", (data) => {

				Console.WriteLine (data);
			});

			socket.On ("value-added", (data) => {
				Console.WriteLine (data);

			});
			socket.On ("notification", (data) => {

				Console.WriteLine (data);
			});

			//socket.On ("screenshot-received", (data) => {
			//	try {
			//		var obj = (JContainer)data;
			//		var session = (string)obj ["sessionId"];
			//		SocketServer.ForSession (session).ScreenShotReceived (obj);
			//	} catch (Exception ex) {
			//		Console.WriteLine (ex);
			//	}
			//});
			//socket.On ("load-app", (data) => {
			//	var obj = (JContainer)data;
			//	var sessionId = (string)obj ["sessionId"];
			//	var path = (string)obj ["path"];
			//	DeviceService.ForSession (sessionId).UploadApk (path);
			//});
			//socket.On ("connect-to-device", async (data) => {
			//	Console.WriteLine (data);
			//	var obj = (JContainer)data;
			//	var id = (string)obj ["clientid"];
			//	var deviceId = (string)obj ["device"];
			//	var service = DeviceService.CreateForDevice (deviceId);
			//	await service.Connect ();
			//	var jobj = new JObject ();
			//	jobj.Add ("clientid", id);
			//	jobj.Add ("deviceId", deviceId);
			//	jobj.Add ("screenWidth", service.Device.ScreenWidth);
			//	jobj.Add ("screenHeight", service.Device.ScreenHeight);
			//	jobj.Add ("sessionId", service.SessionId);
			//	socket.Emit ("device-connected", jobj);
			//});
			//socket.On ("check-available", (data) => SendAvailableDevices ());

			//socket.On ("client-disconnected", async (data) => {
			//	var obj = (JContainer)data;
			//	var sessionId = (string)obj ["sessionId"];
			//	DeviceService.ShutDown (sessionId);
			//	SocketServer.ShutDown (sessionId);
			//});
			var success = await tcs.Task;
			return success;
		}
	}
}

