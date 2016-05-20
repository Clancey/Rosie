using System;
using SimpleAuth;
namespace Rosie.Mobile
{
	public class DeviceApi : ApiKeyApi
	{
		public DeviceApi () : base(Settings.ApiKey, "apiKey",AuthLocation.Query)
		{
			
		}
	}
}

