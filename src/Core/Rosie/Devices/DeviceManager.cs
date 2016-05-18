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
	public class DeviceManager
	{
		public static DeviceManager Shared { get; set; } = new DeviceManager ();

		public DeviceManager ()
		{
			RegisterHandler<WebRequestHandler> ();
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
			//TODO: Push updates
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
	}
}

