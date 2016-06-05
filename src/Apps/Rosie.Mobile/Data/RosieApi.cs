using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAuth;
namespace Rosie.Mobile
{
	public class RosieApi : ApiKeyApi
	{
		static RosieApi shared;
		public static RosieApi Shared { 
			get { return shared ?? (shared = new RosieApi(Settings.ApiKey)); }
			set { shared = value;}
		}
		public RosieApi (string apiKey) : base (apiKey, "apikey", AuthLocation.Query)
		{
			var url = Settings.ApiUrl;
			if(!string.IsNullOrWhiteSpace(url))
				BaseAddress = new Uri (url);
		}

		Task<bool> _syncTask;
		public Task<bool> Sync ()
		{
			if (_syncTask?.IsCompleted ?? true)
				_syncTask = sync ();
			return _syncTask;
		}

		async Task<bool> sync ()
		{
			try {
				var devices = await GetDevices ();
				await Database.Shared.ExecuteAsync ("delete from Device");
				await Database.Shared.InsertAllAsync (devices);
				NotificationManager.Shared.ProcDeviceListUpdated ();
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}

		[Path ("api/Devices")]
		public Task<List<Device>> GetDevices ()
		{
			return this.Get<List<Device>> ();
		}
	}
}

