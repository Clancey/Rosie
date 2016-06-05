using System;
namespace Rosie.Mobile
{
	public class NotificationManager
	{
		public static NotificationManager Shared { get; set; } = new NotificationManager ();

		public event EventHandler<EventArgs<string>> DeviceUpdated;
		public void ProcDeviceUpdated (string deviceId)
		{
			DeviceUpdated?.InvokeOnMainThread (this, deviceId);
		}

		public event EventHandler DeviceListUpdated;
		public void ProcDeviceListUpdated ()
		{
			DeviceListUpdated?.InvokeOnMainThread (this);
		}
	}
}

