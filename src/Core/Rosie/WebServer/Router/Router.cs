using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Rosie.Services;

namespace Rosie.Server
{
	public class Router : IRouter
	{
		IServiceCollection _serviceCollection;
		IServiceProvider _provider;

		public Router(IServiceCollection col)
		{
			_serviceCollection = col;
		}

		Dictionary<string, (Type RouteType, string Path)> routes = new Dictionary<string, (Type RouteType, string Path)>();
		Dictionary<string, (Type RouteType, string Path)> matchedRoutes = new Dictionary<string, (Type RouteType, string Path)>();

		public IServiceProvider GetProvider()
		{
			if(_provider == null)
			{
				_provider = _serviceCollection.BuildServiceProvider(false);
			}
			return _provider;
		}

		public void AddRoute(string path, Type route)
		{
			_serviceCollection.AddTransient(route);
			var orgPath = path;
			routes[path.ToLower()] = (route, orgPath);
			var parts = path.Split('/');
			for (var i = 0; i < parts.Length; i++)
			{
				var part = parts[i];
				if (!part.StartsWith("{", StringComparison.Ordinal) || !part.EndsWith("}", StringComparison.Ordinal))
					continue;
				parts[i] = "*";
			}
			if (!parts.Contains("*"))
				return;
			path = string.Join("/", parts);
			matchedRoutes[path] = (route, orgPath);
		}

		public Route GetRoute(string path)
		{
			(Type RouteType, string Path) routeInformation;
			if (!routes.TryGetValue(path.Trim('/').ToLower(), out routeInformation))
			{
				var matches = matchedRoutes.Where(x => path.ToLower().IsMatch(x.Key.ToLower())).ToList();
				if (matches.Count == 0)
					return null;
				if (matches.Count == 1)
				{
					routeInformation = matches.First().Value;
				}
				else
				{
					//Sometimes there are two matching paterns check what one matches perfectly;
					var pathSplit = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
					var closestMatch = matches.Where(x => {
						var matchParts = x.Key.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
						if (matchParts.Length != pathSplit.Length)
							return false;
						//Check the matching patern
						var pathPartCopy = pathSplit.ToArray();
						for (var i = 0; i < matchParts.Length; i++)
						{
							if (matchParts[i] == "*")
								pathPartCopy[i] = "*";
						}
						return string.Join("/", matchParts) == string.Join("/", pathPartCopy);
					}).ToList();
					routeInformation = closestMatch.FirstOrDefault().Value;
				}
			}
			var route = _provider.GetRequiredService(routeInformation.RouteType) as Route;
			route.Path = routeInformation.Path;
			return route;
		}


		public void AddRoute(Route route)
		{
			var type = route.GetType();
			AddRouteWithAttribute(type);
		}


		public void AddRoute<T>() where T : Route
		{
			var type = typeof(T);
			AddRouteWithAttribute(type);
		}

		private void AddRouteWithAttribute(Type type)
		{
			var path = type.GetCustomAttributes(true).OfType<PathAttribute>().FirstOrDefault();
			if (path == null)
				throw new Exception("Cannot automatically regiseter Route without Path attribute");
			AddRoute(path.Path.Trim('/'), type);
		}

	}
}

