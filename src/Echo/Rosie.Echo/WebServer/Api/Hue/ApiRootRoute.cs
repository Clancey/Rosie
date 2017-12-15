using System;
using System.Threading.Tasks;
using System.Net.Http;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/")]
	public class ApiRootRoute : Route
	{
		public ApiRootRoute ()
		{
			IsSecured = false;
		}

		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };

		public override Task<string> GetResponseString<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			return Task.FromResult("[{\"success\":{\"username\":\"lights\"}}]");
		}
	}
}

