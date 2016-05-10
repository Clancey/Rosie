using System;
using System.Threading.Tasks;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}/lights/{lightId}")]
	public class GetLightRoute : Route
	{
		public override bool SupportsMethod (string method) => method == "GET";

		public override async Task<string> GetResponseString (string method, System.Net.HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var lightId = queryString ["lightId"];
			var device = await DeviceDatabase.Shared.GetDevice (lightId);
			var resp = await new DeviceResponse {
				Name = device.Name,
				Uniqueid = device.Id,
			}.ToJsonAsync ();
			return resp;
		}
	}
}

