using System;
using Microsoft.Extensions.DependencyInjection;
using Rosie.Server;
using Rosie.Server.Routes;
using Rosie.Services;

namespace Rosie.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRosie(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			//first we register all the services, the defaults ones used by Rosie
			//loggers, iRouter etcc
			//We also scan and register for plugins that implement iRosieServices
			ServiceRegistry.Register(services);

		
			return services;
		}
	}
}
