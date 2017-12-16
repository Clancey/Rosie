using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Rosie.Extensions;
using Rosie.Server;

namespace Rosie.Services
{
	public static class ServiceRegistry
	{
		static Type _iRosieServiceType = typeof(IRosieService);
		static IEnumerable<Type> _serviceTypes;
		static IRouter router;

		internal static RosieServiceProvider Register(IServiceCollection services)
		{
			AddDefaultServices(services);
			ConfigureServices(services);
			var rosieProvider = new RosieServiceProvider(services);
			Start(rosieProvider);
			return rosieProvider;
		}

		static void Start(IServiceProvider builder)
		{
			foreach (var service in builder.GetServices<IRosieService>())
				service.Start(); //or setup
		}

		static void AddDefaultServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddLogging();
			serviceCollection.AddSingleton<IRouter>((IServiceProvider arg) => new Router(serviceCollection));
			serviceCollection.AddSingleton<IDeviceManager, DeviceManager>();		
		}

		static void ConfigureServices(IServiceCollection serviceCollection)
		{
			var assembly = Assembly.GetEntryAssembly();

			if (_serviceTypes == null)
				_serviceTypes = GetIRosieServices(assembly);

			foreach (var serviceType in _serviceTypes)
			{
				serviceCollection.AddTransient(_iRosieServiceType, serviceType);
			}
		}

		static IEnumerable<Type> GetIRosieServices(Assembly assembly)
		{
			var assemblies = assembly.GetReferencedAssemblies();

			foreach (var assemblyName in assemblies)
			{
				assembly = Assembly.Load(assemblyName);

				foreach (var type in assembly.DefinedTypes)
				{
					if (type.ImplementedInterfaces.Contains(_iRosieServiceType))
					{
						yield return type;
					}
				}
			}
		}
	}
}
