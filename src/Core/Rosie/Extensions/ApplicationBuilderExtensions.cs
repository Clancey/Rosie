using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Rosie.Server;
using System.Linq;
using Rosie.Server.Routes;

namespace Rosie.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static void UseRosie(this IApplicationBuilder app)
		{
			var router = (IRouter)app.ApplicationServices.GetService(typeof(IRouter));
			RegisterRoutes(app.ApplicationServices);
			app.Run(async (context) =>
			{
				var request = context.Request;
				var path = request?.Path.Value;

				path = path?.TrimStart('/');
				//Log($"Request from: {request.RemoteEndPoint.Address} Path: {path}");
				if(string.IsNullOrEmpty(path))
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

			});
		}

		static void RegisterRoutes(IServiceProvider builder)
		{
			IRouter router = (IRouter)builder.GetService(typeof(IRouter));
			router.AddRoute<DevicesRoute>();
		}
	}
}
