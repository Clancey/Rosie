using System;
using System.Net;
using System.Threading.Tasks;
using Rosie.Echo;
using Rosie.Server.Echo;

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


		public override System.Net.Http.HttpMethod[] GetSupportedMethods() => new System.Net.Http.HttpMethod[] { System.Net.Http.HttpMethod.Get };

		public override string ContentType {
			get {
				return "application/xml";
			}
		}

		#endregion

		public override Task<string> GetResponseString<HttpListenerRequest> (System.Net.Http.HttpMethod method, HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			var deviceId = queryString ["DeviceId"];
			var server = (request as System.Net.HttpListenerRequest).Url.Host;
			var resp = MessageTemplates.GetSetupTemplate (server, AmazonEchoWebServer.EchoWebServerPort, EchoDiscoveryService.GetDeviceId ());
			return Task.FromResult(resp);
		}
	}
}

