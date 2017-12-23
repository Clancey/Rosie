using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Rosie.Services
{
	public delegate void ServiceHandler(Data data);

	public interface IServicesManager
	{
		void RegisterService(string domain, string name, ServiceHandler serviceHandler, string description = "");
		Dictionary<string, ServiceHandler> GetAvailableServices();
		void CallService(string senderDomain, string domain, string name, ExpandoObject data = null);
	}
}
