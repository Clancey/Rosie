using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Rosie.Node;
using Rosie.Server;
using System.Net.Http;

namespace Rosie.Server.Routes.Node
{
	[Path ("api/NodeDevice/{deviceId}")]
	public class NodeDeviceRoute : Route<NodeDevice>
	{
		public override Task<NodeDevice> GetResponse<HttpListenerRequest> (HttpMethod method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			var deviceId = queryString ["deviceId"];
			return NodeDatabase.Shared.GetDevice (deviceId);
		}


		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };
	}
}

