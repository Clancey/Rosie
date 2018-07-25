using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rosie
{
	public class WebRequestHandler : IDeviceService
	{
		public WebRequestHandler ()
		{
		}

		public static string Identifier = "Web";

		public string ServiceIdentifier => Identifier;

		HttpClient client = new HttpClient ();
		public async Task<bool> HandleRequest (Device device, DeviceUpdate request)
		{
			try {
				if (request.DataType != DataTypes.Bool)
					return false;
				//TODO: Fixme
				//var url = device.ad
				//var resp = await client.GetAsync (request.BoolValue.Value ? device.OnUrl : device.OffUrl);
				//resp.EnsureSuccessStatusCode ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}
	}
}

