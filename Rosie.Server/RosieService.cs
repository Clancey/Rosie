#define SmartThings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using Rosie.Echo;
using Rosie.Hue;
using Rosie.Server;
using Rosie.Services;

namespace Rosie
{
	public class RosieService : ServiceBase
	{
		public RosieService ()
		{
		}
#if DEBUG
		public void Start ()
		{
			try {
				new HueService(null,null);
				OnStart (new string [] { "" });
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}
		#endif
		protected override void OnStart (string [] args)
		{
			LocalWebServer.Shared.Start ();

			Init ();

			//AmazonEchoWebServer.Shared.Start ();
			//EchoDiscoveryService.Shared.StartListening ();
			base.OnStart (args);
		}

		void Init ()
		{
			//LocalWebServer.Registry.GetService<IDeviceManager>().RegisterDeviceLogHandler<SqliteDeviceLogger>();

#if SmartThings
	//		await Rosie.SmartThings.SmartThingsManager.Shared.Init ();
#endif
#if Azure
			await Rosie.AzureIoT.AzureDeviceManager.Shared.Init ();
#endif
#if Node
			await Rosie.Node.NodeManager.Shared.Init ();
#endif

		}
		protected override void OnStop ()
		{
			EchoDiscoveryService.Shared.StopListening ();
			LocalWebServer.Shared.Stop ();
			AmazonEchoWebServer.Shared.Stop ();
			Console.WriteLine ("Amazon Echo Service Stoped");
			base.OnStop ();
		}
	}
}

