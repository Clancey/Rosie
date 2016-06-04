using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Rosie.Server;
namespace Rosie.Server.Routes
{
	[Path ("api/Devices")]
	public class DevicesRoute : Route<List<Device>>
	{
		public DevicesRoute ()
		{
		}
		public override System.Threading.Tasks.Task<List<Device>> GetResponse (string method, System.Net.HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			return DeviceDatabase.Shared.GetAllDevices ();
		}
		public override bool SupportsMethod (string method)
		{
			return method == "GET";
		}
	}
}

