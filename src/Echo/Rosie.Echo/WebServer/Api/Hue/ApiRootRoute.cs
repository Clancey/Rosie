using System;
using System.Threading.Tasks;
namespace Rosie.Server.Routes.Echo
{
	[Path ("api/")]
	public class ApiRootRoute : Route
	{
		public ApiRootRoute ()
		{
			IsSecured = false;
		}
		public override bool SupportsMethod (string method) => method == "GET";

		public override Task<string> GetResponseString (string method, System.Net.HttpListenerRequest request, System.Collections.Specialized.NameValueCollection queryString, string data)
		{
			return Task.FromResult("[{\"success\":{\"username\":\"lights\"}}]");
		}
	}
}

