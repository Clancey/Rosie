using System;
namespace Rosie.Mobile
{
	public class NotificationManager
	{
		public static NotificationManager Shared { get; set; } = new NotificationManager();

		public event EventHandler<DeviceUpdatedEventArgs> DeviceUpdated;
		public void ProcDeviceUpdated(string deviceId)
		{
			DeviceUpdated?.InvokeOnMainThread(this, new DeviceUpdatedEventArgs(deviceId));
		}

		public event EventHandler DeviceListUpdated;
		public void ProcDeviceListUpdated()
		{
			DeviceListUpdated?.InvokeOnMainThread(this);
		}

		public class DeviceUpdatedEventArgs : EventArgs
		{
			public string DeviceId
			{
				get;
				set;
			}
			public DeviceUpdatedEventArgs(string deviceid)
			{
				DeviceId = deviceid;
			}
		}

	}
}

