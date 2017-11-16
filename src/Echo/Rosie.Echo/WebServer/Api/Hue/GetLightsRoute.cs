using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}/lights")]
	public class GetLightsRoute : Route
	{
		public GetLightsRoute ()
		{
			IsSecured = false;
		}

		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };

		public override async Task<string> GetResponseString (HttpMethod method, System.Net.HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var devices = await DeviceDatabase.Shared.GetEchoDevices ();
			var resp = await Task.Run(()=> devices.ToDictionary(x=> x.Id, x=> x.Name).ToJson());
			return resp;
		}
	}
}

