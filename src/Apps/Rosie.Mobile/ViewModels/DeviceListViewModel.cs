using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Rosie.Mobile
{
	public class DeviceListViewModel : BaseViewModel
	{
		public List<Device> Devices { get; set; }
		public override async void Refresh ()
		{
			Devices = await Database.Shared.Table<Device> ().ToListAsync ();
			NotifyPropertyChanged (nameof (Devices));
		}

		void DevicesUpdated (object sender, EventArgs e)
		{
			Refresh ();
		}

		public override void SetupEvents ()
		{
			base.SetupEvents ();
			NotificationManager.Shared.DeviceListUpdated += DevicesUpdated;
		}
		public override void TearDownEvents ()
		{
			base.TearDownEvents ();
			NotificationManager.Shared.DeviceListUpdated -= DevicesUpdated;
		}

	}
}

