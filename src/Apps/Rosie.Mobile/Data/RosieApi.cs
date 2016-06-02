using System;
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
	}
}

