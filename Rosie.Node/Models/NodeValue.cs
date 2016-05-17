using System;
using Newtonsoft.Json;

namespace Rosie.Node
{
	public class NodeValue
	{
		[JsonProperty ("value_id")]
		public string ValueId { get; set; }

		[JsonProperty ("node_id")]
		public int NodeId { get; set; }

		[JsonProperty ("class_id")]
		public int ClassId { get; set; }

		[JsonProperty ("type")]
		public string Type { get; set; }

		[JsonProperty ("genre")]
		public string Genre { get; set; }

		[JsonProperty ("instance")]
		public int Instance { get; set; }

		[JsonProperty ("index")]
		public int Index { get; set; }

		[JsonProperty ("label")]
		public string Label { get; set; }

		[JsonProperty ("units")]
		public string Units { get; set; }

		[JsonProperty ("help")]
		public string Help { get; set; }

		[JsonProperty ("read_only")]
		public bool ReadOnly { get; set; }

		[JsonProperty ("write_only")]
		public bool WriteOnly { get; set; }

		[JsonProperty ("is_polled")]
		public bool IsPolled { get; set; }

		[JsonProperty ("min")]
		public int Min { get; set; }

		[JsonProperty ("max")]
		public int Max { get; set; }

		[JsonProperty ("value")]
		public object Value { get; set; }
	}
}

