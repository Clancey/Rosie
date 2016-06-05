using System;
namespace Rosie.Mobile
{
	public class SettingsViewModel : BaseViewModel
	{
		string serverUrl = Settings.ApiUrl;
		public string ServerUrl {
			get { return serverUrl;}
			set {
				if (!this.NotifyPropertyChanged (ref serverUrl, value))
					return;
				Settings.ApiUrl = serverUrl;
				RosieApi.Shared = null;
			}
		}

		string apiKey = Settings.ApiKey;
		public string ApiKey { 
			get { return apiKey; }
			set {
				if (!this.NotifyPropertyChanged (ref apiKey, value))
					return;
				Settings.ApiKey = apiKey;
				RosieApi.Shared = null;
			}
		}
	}
}

