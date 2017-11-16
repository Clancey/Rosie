using System;
using System.Net;
using System.Threading.Tasks;
using Rosie.Echo;
using Rosie.Server.Echo;
using System.Net.Http;

namespace Rosie.Server.Routes.Echo
{
	[Path("upnp/{DeviceId}/setup.xml")]
	public class SetupRoute : Route
	{
		public SetupRoute ()
		{
			IsSecured = false;
		}

		#region implemented abstract members of Route


		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };

		public override string ContentType {
			get {
				return "application/xml";
			}
		}

		#endregion

		public override Task<string> GetResponseString (HttpMethod method, HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var deviceId = queryString ["DeviceId"];
			var server = request.Url.Host;
			var resp = MessageTemplates.GetSetupTemplate (server, AmazonEchoWebServer.EchoWebServerPort, EchoDiscoveryService.GetDeviceId ());
			return Task.FromResult(resp);
		}
	}
}

