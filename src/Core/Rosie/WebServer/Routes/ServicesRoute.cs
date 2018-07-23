using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

			if (method == HttpMethod.Get)
			{
				foreach (var item in _servicesManager.GetAvailableServices())
				{
					result.Add(item.Key);
				}
			}

			if (method == HttpMethod.Post)
			{
				var req = JsonConvert.DeserializeObject<ServiceCallRequest>(data);
				if(req != null)
				{
					//we need some validation here
					var sender = nameof(ServicesRoute);
					_servicesManager.CallService(sender, req.Domain, req.Service, req.Data);
				}
			}

			return result;
		}

		public override HttpMethod[] GetSupportedMethods() => new HttpMethod[] { HttpMethod.Get, HttpMethod.Post };

		class ServiceCallRequest {
			public string Description
			{
				get;
				set;
			}
			public string Domain
			{
				get;
				set;
			}

			public string Service
			{
				get;
				set;
			}

			public ExpandoObject Data
			{
				get;
				set;
			}
		}
	}
}
