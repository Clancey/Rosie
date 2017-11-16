using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using Rosie.Node;
using Rosie.Server;
using System.Net.Http;

namespace Rosie.Server.Routes.Node
{
	[Path ("api/NodeDevices")]
	public class NodeDevicesRoute : Route<List<NodeDevice>>
	{
		public NodeDevicesRoute ()
		{
		}

		public override Task<List<NodeDevice>> GetResponse (HttpMethod method, HttpListenerRequest request, NameValueCollection queryString, string data)
		{
			return NodeDatabase.Shared.DatabaseConnection.Table<NodeDevice>().ToListAsync();
		}

		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };
	}
}

