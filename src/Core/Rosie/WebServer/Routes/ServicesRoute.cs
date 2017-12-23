using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using Rosie.Server;
using Rosie.Services;

namespace Rosie.Server.Routes
{
	[Path("api/services")]
	public class ServicesRoute : Route<List<string>>
	{
		IServicesManager _servicesManager;
		public ServicesRoute(IServicesManager deviceManager)
		{
			_servicesManager = deviceManager;
		}

		public override async Task<List<string>> GetResponse<HttpRequest>(HttpMethod method, HttpRequest request, NameValueCollection queryString, string data)
		{
			var result = new List<string>();
			foreach (var item in _servicesManager.GetAvailableServices())
			{
				result.Add(item.Key);
			}
			return result;
		}

		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get };
	}
}
