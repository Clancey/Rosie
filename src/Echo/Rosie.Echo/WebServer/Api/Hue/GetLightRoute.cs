using System;
using System.Threading.Tasks;
using System.Net.Http;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}/lights/{lightId}")]
	public class GetLightRoute : Route
	{
		public GetLightRoute ()
		{
			IsSecured = false;
		}
		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };

		public override async Task<string> GetResponseString<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
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

