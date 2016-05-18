using System;
using Rosie.Server;
using Rosie.Server.Routes.Echo;

namespace Rosie.Echo
{
	public class AmazonEchoWebServer : LocalServer
	{

		public static AmazonEchoWebServer SharedEcho { get; set; } = new AmazonEchoWebServer ();
		public const int EchoWebServerPort = 8082;

		public AmazonEchoWebServer (int webserverPort = EchoWebServerPort) : base(webserverPort)
		{
			
		}

		public override void RegisterRoutes ()
		{
			base.RegisterRoutes ();
			Router.AddRoute<SetupRoute> ();
			Router.AddRoute<ApiRootRoute> ();
			Router.AddRoute<ApiUserRoute> ();
			Router.AddRoute<GetLightRoute> ();
			Router.AddRoute<GetLightsRoute> ();
			Router.AddRoute<PutLightRoute> ();
		}
	}
}

