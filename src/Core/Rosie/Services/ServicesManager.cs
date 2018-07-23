using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Rosie.Services
{
	public class ServicesManager : IServicesManager
	{
		Dictionary<string, ServiceHandler> _services;

		public Dictionary<string, ServiceHandler> GetAvailableServices() => _services;
		 
		public ServicesManager()
		{
			_services = new Dictionary<string, ServiceHandler>();
		}

		public void CallService(string senderDomain, string domain, string name, ExpandoObject data = null)
		{
			var key = $"{domain}-{name}";
			if (!_services.ContainsKey(key))
			{
				throw new Exception($"key isnt registered _| {key} |_");
			}
			_services[key]?.Invoke(new Data() { Description = $"service called by {senderDomain}", DataS = data });
		}

		public void RegisterService(string domain, string name, ServiceHandler serviceHandler , string description = "")
		{
			var key = $"{domain}-{name}";
			if(_services.ContainsKey(key))
			{
				throw new Exception($"key already registered _| {key} |_");
			}
			_services.Add(key, serviceHandler);
		}
	}
}
