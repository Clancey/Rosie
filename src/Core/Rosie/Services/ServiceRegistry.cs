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

		public IEnumerable<IRosieService> Services => _services;
		public IConfiguration Configuration { get; }

		public ServiceRegistry()
		{
			var services = new ServiceCollection();
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true);

			Configuration = builder.Build();
			Register();
			AddDefaultServices(services);
			ConfigureServices(services);
		}

		void AddDefaultServices(ServiceCollection serviceCollection)
		{
			serviceCollection.AddLogging();
		}

		public virtual void ConfigureServices(IServiceCollection serviceCollection)
		{
			ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
			foreach (var s in Services)
			{
				var type = s.GetType();
				s.Setup(serviceProvider);
				serviceCollection.AddSingleton(s);
			}

		}

		public async void Start()
		{
			foreach (var service in Services)
			{
				await service.Start();
			}
		}

		public async void Stop()
		{
			foreach (var service in Services)
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
