using System;
namespace Rosie.Server
{
	public interface IRouter
	{
		IServiceProvider GetProvider();
		void AddRoute(string path, Type route);
		Route GetRoute(string path);
		void AddRoute<T>() where T : Route;
		void AddRoute(Route route);
	}
}
