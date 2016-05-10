using System;
using System.Collections.Generic;
using System.Linq;

namespace Rosie.Server
{
	public static class Router
	{
		static Router()
		{
			//Register Default Routes
//			routes = new Dictionary<string, NativeRoute>{
//				{SessionRoute.RouteKey, new SessionRoute ()},
//			};
		}

		static Dictionary<string,Route> routes = new Dictionary<string, Route>();
		static Dictionary<string, Route> matchedRoutes = new Dictionary<string, Route> ();
		public static void AddRoute(string path,Route route)
		{
			route.Path = path;
			routes [path.ToLower ()] = route;
			var parts = path.Split ('/');
			for (var i = 0; i < parts.Length; i++) {
				var part = parts [i];
				if (!part.StartsWith ("{") || !part.EndsWith ("}"))
					continue;
				parts [i] = "*";
			}
			if (!parts.Contains ("*"))
				return;
			path = string.Join ("/", parts);
			matchedRoutes [path] = route;
		}

		public static Route GetRoute (string path)
		{
			Route route;
			if (!routes.TryGetValue (path.ToLower (), out route)) {
				var matches = matchedRoutes.Where (x => path.IsMatch (x.Key)).ToList ();
				if (matches.Count == 1) {
					route = matches.First ().Value;
				}
				else {
					//Sometimes there are two matching paterns check what one matches perfectly;
					var pathSplit = path.Split (new char [] { '/' }, StringSplitOptions.RemoveEmptyEntries);
					var closestMatch = matches.Where (x => {
						var matchParts = x.Key.Split (new char [] { '/' }, StringSplitOptions.RemoveEmptyEntries);
						if (matchParts.Length != pathSplit.Length)
							return false;
						//Check the matching patern
						var pathPartCopy = pathSplit.ToArray ();
						for (var i = 0; i < matchParts.Length; i++) {
							if (matchParts [i] == "*")
								pathPartCopy [i] = "*";
						}
						return string.Join("/",matchParts) == string.Join ("/", pathPartCopy);
					}).ToList();
					route = closestMatch.FirstOrDefault ().Value;
				}
			}
			return route;
		}

		public static void AddRoute<T>() where T : Route
		{
			var type = typeof(T);
			var path = type.GetCustomAttributes(true).OfType<PathAttribute>().FirstOrDefault();
			if (path == null)
				throw new Exception("Cannot automatically regiseter Route without Path attribute");
			var route = (Route)Activator.CreateInstance(type);

			AddRoute(path.Path.TrimStart('/'), route); 
		}

	}
}

