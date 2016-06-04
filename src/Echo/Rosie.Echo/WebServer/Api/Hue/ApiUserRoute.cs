using System;
using System.Linq;
using System.Threading.Tasks;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/{userId}")]
	public class ApiUserRoute : Route
	{
		public ApiUserRoute ()
		{
			IsSecured = false;
		}

		public override bool SupportsMethod (string method) => method == "GET";

		public override async Task<string> GetResponseString (string method, System.Net.HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var devices = await DeviceDatabase.Shared.GetEchoDevices ();
			var resp = await new HueApiResponse {
				Lights = devices.ToDictionary (x => x.Id, x => new DeviceResponse { Name = x.Name, Uniqueid = x.Id }),
			}.ToJsonAsync ();
			return resp;
		}
	}
}

