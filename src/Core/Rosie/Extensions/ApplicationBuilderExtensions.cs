using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Rosie.Server;
using System.Linq;
using Rosie.Server.Routes;
using System.Threading.Tasks;
using Rosie.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Rosie.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static void UseRosie(this IApplicationBuilder app, IList<Route> extraRoutes = null)
		{
			var provider = app.ApplicationServices;

			var router = (IRouter)provider.GetService(typeof(IRouter));
			RegisterRoutes(router, extraRoutes);

			app.ApplicationServices = router.GetProvider();
			app.ApplicationServices.StartRosieServices();

			app.Run(async (context) => await HandleRoute(context, router));
		}

		static void RegisterRoutes(IRouter router, IList<Route> extraRoutes = null)
		{
			router.AddRoute<DevicesRoute>();
			router.AddRoute<DeviceStateRoute>();
			router.AddRoute<ServicesRoute>();
			if(extraRoutes != null)
			{
				foreach (var route in extraRoutes)
				{
					router.AddRoute(route);
				}
			}
		}

		static void StartRosieServices(this IServiceProvider builder)
		{
			var deviceManager = builder.GetService<IDeviceManager>();
			foreach (var service in builder.GetServices<IRosieService>())
			{
				deviceManager.RegisterHandler(service);
				service.Start(); //or setup
			}
		}

		static async Task HandleRoute(HttpContext context, IRouter router)
		{
			var request = context.Request;
			var path = request?.Path.Value;

			path = path?.TrimStart('/');
			//Log($"Request from: {request.RemoteEndPoint.Address} Path: {path}");
			if (string.IsNullOrEmpty(path))
			{
				await context.Response.WriteAsync("Hello from Rosie!");
				return;
			}
			var route = router.GetRoute(path);
			if (route == null)
			{
				Console.WriteLine($"Route not found: {path}");
				context.Response.StatusCode = 404;
				return;
			}
			if (!route.GetSupportedMethods().Contains(new HttpMethod(context.Request.Method)))
			{
				context.Response.StatusCode = 405;
				return;
			}
			context.Response.ContentType = route.ContentType;
			await route.ProcessReponse(context);
		}


	}
}
