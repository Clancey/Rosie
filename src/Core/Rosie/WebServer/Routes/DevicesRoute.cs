using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Rosie.Server;
using System.Net.Http;
namespace Rosie.Server.Routes
{
	[Path("api/Devices")]
	public class DevicesRoute : Route<List<Device>>
	{
		public DevicesRoute()
		{
		}
		public override System.Threading.Tasks.Task<List<Device>> GetResponse(HttpMethod method, System.Net.HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			return DeviceDatabase.Shared.GetAllDevices();
		}
		public override HttpMethod[] GetSupportedMethods() =>  new HttpMethod[] { HttpMethod.Get };
	}
}

