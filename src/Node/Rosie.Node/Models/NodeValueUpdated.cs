using System;
using Newtonsoft.Json;

namespace Rosie.Node
{
	public class NodeValueUpdate
	{
		[JsonProperty ("nodeId")]
		public int NodeId { get; set; }

		[JsonProperty ("comclass")]
		public int Comclass { get; set; }

		[JsonProperty ("value")]
		public NodeValue Value { get; set; }
	}
}

