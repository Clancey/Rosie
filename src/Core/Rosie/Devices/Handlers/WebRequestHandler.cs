using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rosie
{
	public class WebRequestHandler : IDeviceHandler
	{
		public WebRequestHandler ()
		{
		}

		public static string Identifier = "Web";

		public string ServiceIdentifier => Identifier;

		HttpClient client = new HttpClient ();
		public async Task<bool> HandleRequest (Device device, SetDeviceStateRequest request)
		{
			try {
				var resp = await client.GetAsync (request.On ? device.OnUrl : device.OffUrl);
				resp.EnsureSuccessStatusCode ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}
	}
}

