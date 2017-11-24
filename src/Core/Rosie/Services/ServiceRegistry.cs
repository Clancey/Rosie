using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rosie.Services
{
	public class ServiceRegistry
	{
		IEnumerable<IRosieService> _services;
		ServiceProvider _serviceProvider;
		ServiceCollection _serviceCollection;
		public IEnumerable<IRosieService> RosieServices => _services;
		public IConfiguration Configuration { get; }

		public ServiceRegistry()
		{
			_serviceCollection = new ServiceCollection();
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true);

			Configuration = builder.Build();
			Register();
			AddDefaultServices(_serviceCollection);
			ConfigureServices(_serviceCollection);
			_serviceProvider = _serviceCollection.BuildServiceProvider();
		}

		public T GetService<T>() where T : class => _serviceProvider.GetService<T>();

		void AddDefaultServices(ServiceCollection serviceCollection)
		{
			serviceCollection.AddLogging();
			serviceCollection.AddSingleton<IDeviceManager>(DeviceManager.Shared);
		}

		void ConfigureServices(IServiceCollection serviceCollection)
		{
			foreach (var s in RosieServices)
			{
				var type = s.GetType();
				s.Setup(serviceCollection.BuildServiceProvider());
				serviceCollection.AddSingleton(type, s);
				serviceCollection.AddSingleton<IRosieService>(s);
			}

		}

		public void Start()
		{
			try
			{
				var services = _serviceProvider.GetServices<IRosieService>();

				foreach (var service in services)
				{
					service.Start();
				}
			}
			catch (Exception ex)
			{

			}

		}

		public async void Stop()
		{
			foreach (var service in _serviceProvider.GetServices<IRosieService>())
			{
				await service.Stop();
			}
		}

		void Register()
		{
			if (_services == null)
			{
				var assembly = Assembly.GetEntryAssembly();
				var services = GetIRosieServices(assembly);
				_services = services;
			}
		}

		static IEnumerable<IRosieService> GetIRosieServices(Assembly assembly)
		{
			var assemblies = assembly.GetReferencedAssemblies();

			foreach (var assemblyName in assemblies)
			{
				assembly = Assembly.Load(assemblyName);

				foreach (var types in assembly.DefinedTypes)
				{
					if (types.ImplementedInterfaces.Contains(typeof(IRosieService)))
					{
						yield return (IRosieService)assembly.CreateInstance(types.FullName);
					}
				}
			}
		}


	}
}
