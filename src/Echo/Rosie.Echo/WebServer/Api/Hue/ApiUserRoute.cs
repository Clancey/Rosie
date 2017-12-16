using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}")]
	public class ApiUserRoute : Route
	{
		public ApiUserRoute ()
		{
			IsSecured = false;
		}

		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };

		public override async Task<string> GetResponseString<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var devices = await DeviceDatabase.Shared.GetEchoDevices ();
			var resp = await new HueApiResponse {
				Lights = devices.ToDictionary (x => x.Id, x => new DeviceResponse { Name = x.Name, Uniqueid = x.Id }),
			}.ToJsonAsync ();
			return resp;
		}
	}
}

