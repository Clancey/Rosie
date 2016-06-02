using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Rosie.Mobile
{
	public class RoomsListViewModel : BaseViewModel
	{
		public RoomsListViewModel ()
		{
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

		public List<IGrouping<string, Device>> Rooms { get; set; }
		public override async void Refresh ()
		{
			var devices = await Database.Shared.Table<Device> ().ToListAsync ();
			Rooms = devices.GroupBy (x => x.Location).ToList ();
			NotifyPropertyChanged (nameof (Rooms));
		}

		void DevicesUpdated (object sender, EventArgs e)
		{
			Refresh ();
		}
	}
}

