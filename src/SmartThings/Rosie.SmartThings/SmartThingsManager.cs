#if ROSIE
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Rosie.SmartThings
{
	public class SmartThingsManager : IDeviceHandler
	{
		public static SmartThingsManager Shared { get; set; } = new SmartThingsManager ();

		public string ServiceIdentifier => "SmartThings";

		public Task<bool> HandleRequest (Rosie.Device device, SetDeviceStateRequest request)
		{
			throw new NotImplementedException ();
		}
		SmartThingsApi api = new SmartThingsApi ("SmartThings");
		SmartThingsUpdateListener updater;
		public async Task<bool> Init ()
		{
			try {
				var account = await api.Authenticate ();
				if (account == null)
					return false;
				updater = new SmartThingsUpdateListener (api);
				updater.UpdateReceived += (obj) => {
					Console.WriteLine ("Status update");
				};
				await updater.StartListening ();

				#region testing
				var devices = await api.GetDevices ();
				devices.ToList ().ForEach (x => Console.WriteLine ($"Name: {x.Name} Label: {x.Label}"));
				var officeLights = devices.FirstOrDefault((x) => (x.Label?.Contains("Office") ?? false)  && (x.Label?.Contains("Light") ?? false) );
				await api.TurnOnDevice (officeLights);
				await Task.Delay (10000);
				await api.TurnOffDevice (officeLights);
				#endregion
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}

	}
}
#endif