using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleAuth;
namespace Rosie.Node
{
	class NodeApi : ApiKeyApi
	{
		static string NodeServerApiKey {
			get { return Rosie.Settings.GetSecretString (); }
		}
		public NodeApi () : this (NodeServerApiKey)
		{

		}
		public NodeApi (string apiKey) : base(apiKey,"apikey", AuthLocation.Query)
		{
			BaseAddress = new Uri (NodeManager.NodeServerUrl);
		}
		[Path("api/devices")]
		public Task<List<NodeDevice>> GetDevices ()
		{
			return Get<List<NodeDevice>> ();
		}

	}
}

