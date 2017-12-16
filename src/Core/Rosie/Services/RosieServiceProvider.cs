using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Rosie.Services
{
	public class RosieServiceProvider : IServiceProvider
	{
		IServiceCollection _serviceCollection;
		ServiceProvider _serviceProvider;
		IConfiguration _configuration;

		public RosieServiceProvider(IServiceCollection serviceCollection)
		{
			_serviceCollection = serviceCollection;
			_serviceProvider = serviceCollection.BuildServiceProvider();

			//maybe try to read something from here to configure services 
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true);

			_configuration = builder.Build();
		}

		public object GetService(Type serviceType)
		{
			return _serviceProvider.GetService(serviceType);
		}

		public object GetServices(Type serviceType)
		{
			return _serviceProvider.GetServices(serviceType);
		}
	}
}
