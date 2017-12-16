using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Rosie
{
	public class SetDeviceArgs : EventArgs
	{
		public Device Device { get; set; }

		public SetDeviceStateRequest Request { get; set; }

		public bool Success { get; set; }
	}

	internal class DeviceManager : IDeviceManager
	{
		public DeviceManager (IDeviceLogger defaultDeviceLogger)
		{
			RegisterHandler<WebRequestHandler> ();
			if(defaultDeviceLogger != null)
			{
				RegisterHandler(defaultDeviceLogger);
			}
		}

		public Task<bool> AddDevice (Device device)
		{
			if (DeviceLogHandlers.Any ()) {
				Task.Run (async () => {
					foreach (var handler in DeviceLogHandlers) {
						try {
							await handler.AddDevice (device);
						} catch (Exception ex) {
							Console.WriteLine (ex);
						}
					}
				});
			}
			return DeviceDatabase.Shared.InsertDevice (device);
		}

		public delegate bool SetDeviceStateHandler (object sender, SetDeviceArgs args);

		public event SetDeviceStateHandler SetDeviceEvent;

		public async Task<bool> SetDeviceState (Device device, SetDeviceStateRequest state)
		{
			var events = SetDeviceEvent?.GetInvocationList ().ToList ();
			//Let the last subscription get first dibs
			events?.Reverse ();
			var handled = false;
			var args = new SetDeviceArgs {
				Device = device,
				Request = state,
			};
			if(events != null)
			foreach (var e in events) {
				try {
					handled = await Task.Run (() => (e as SetDeviceStateHandler)?.Invoke (this, args) ?? false);
					if (handled)
						return args.Success;
				} catch (Exception ex) {
					Console.WriteLine (ex);
				}
			}


			var handlerList = GetHandlerList (device.Service ?? WebRequestHandler.Identifier);
			for (var i = handlerList.Count - 1; i >= 0; i--) {
				var handler = handlerList [i];
				handled = await handler.HandleRequest (device, state);
				if (handled)
					return true;
			}

			return false;
		}

		public Task UpdateCurrentState (DeviceState state)
		{
			if (DeviceLogHandlers.Any ()) {
				Task.Run (async () => {
					foreach (var handler in DeviceLogHandlers) {
						try {
							await handler.DeviceUpdate (state);
						} catch (Exception ex) {
							Console.WriteLine (ex);
						}
					}
				});
			}
			return DeviceDatabase.Shared.InsertDeviceState (state);
		}

		Dictionary<string, List<IDeviceHandler>> handlers = new Dictionary<string, List<IDeviceHandler>> ();

		public void RegisterHandler<T> () where T : IDeviceHandler
		{
			var handler = (T)Activator.CreateInstance (typeof (T));
			RegisterHandler (handler);
		}

		public void RegisterHandler (IDeviceHandler handler)
		{
			var handlerList = GetHandlerList (handler.ServiceIdentifier);
			handlerList.Add (handler);
		}

		List<IDeviceHandler> GetHandlerList (string service)
		{
			List<IDeviceHandler> handlerList;
			if (!handlers.TryGetValue (service, out handlerList))
				handlers [service] = handlerList = new List<IDeviceHandler> ();
			return handlerList;
		}

		List<IDeviceLogger> DeviceLogHandlers = new List<IDeviceLogger> ();
		public void RegisterDeviceLogHandler<T> () where T : IDeviceLogger
		{
			var handler = (T)Activator.CreateInstance (typeof (T));
			RegisterHandler (handler);
		}

		public void RegisterHandler (IDeviceLogger handler)
		{
			DeviceLogHandlers.Add (handler);
		}

	}
}

