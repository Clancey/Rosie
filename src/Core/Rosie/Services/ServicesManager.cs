using System;
using System.Collections.Generic;

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

		public void RegisterService(string domain, string name, ServiceHandler serviceHandler , string description = "")
		{
			var key = $"{domain}-{name}";
			if(_services.ContainsKey(name))
			{
				throw new Exception($"key already registered _| {name} |_");
			}
			_services.Add(name, serviceHandler);
		}
	}
}
