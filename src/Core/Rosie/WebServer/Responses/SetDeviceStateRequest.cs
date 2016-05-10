using System;
using Newtonsoft.Json;

namespace Rosie
{
	public class SetDeviceStateRequest
	{
		[JsonProperty ("on")]
		public bool On { get; set; }

		[JsonProperty ("bri")]
		public int Bri { get; set; }

		public object Value { get; set; }
	}
}

