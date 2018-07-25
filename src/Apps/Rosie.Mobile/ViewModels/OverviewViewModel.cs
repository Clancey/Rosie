using System;
using System.Threading.Tasks;
using System.Linq;

namespace Rosie.Mobile
{
	public class OverviewViewModel
	{
		public OverviewViewModel ()
		{
			Refresh ();
		}
	
		async Task Refresh ()
		{
			var devices = await Database.Shared.Table<Device> ().ToListAsync ();
			var deviceTypes = devices.Select (x => x.DeviceType).Distinct().ToList();
			//TODO: filter out stale data
			var states = (await Database.Shared.Table<DeviceState> ().ToListAsync ()).Where (x => !x.PropertyKey.Contains ("Unknown"));
			var groups = states.Select (x => x.GroupType).Distinct ();

				//.GroupBy(x=> x.DeviceId);


		}
	}
}

