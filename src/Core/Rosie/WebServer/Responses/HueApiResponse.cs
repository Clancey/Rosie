using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Rosie
{
	public class HueApiResponse
	{
		[JsonProperty("lights")]
		public Dictionary<string, DeviceResponse> Lights { get; set;} = new Dictionary<string, DeviceResponse> ();
	}
}

