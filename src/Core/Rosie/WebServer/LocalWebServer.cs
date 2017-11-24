using System;
using Rosie.Server;
using Rosie.Server.Routes;

namespace Rosie
{
	public class LocalWebServer : WebServer
	{

		public static LocalWebServer Shared { get; set; } = new LocalWebServer ();

		public const int DefaultWebServerPort = 8081;

		public LocalWebServer (int webServerPort = DefaultWebServerPort) : base ("Rosie",webServerPort)
		{
			
		}
		public override void RegisterRoutes ()
		{
			Router.AddRoute<DevicesRoute> ();
		}

	}
}

