using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}/lights")]
	public class GetLightsRoute : Route
	{
		public override bool SupportsMethod (string method) => method == "GET";

		public override async Task<string> GetResponseString (string method, System.Net.HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var devices = await DeviceDatabase.Shared.GetAllDevices ();
			var resp = await Task.Run(()=> devices.ToDictionary(x=> x.Id, x=> x.Name).ToJson());
			return resp;
		}
	}
}

