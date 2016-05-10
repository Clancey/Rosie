using System;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Rosie.Echo;
using Rosie.Server;
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
				OnStart (new string [] { "" });
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}
		#endif
		protected override void OnStart (string [] args)
		{
			Console.WriteLine ("Amazon Echo Service Started");

			LocalServer.Shared.Start ();
			EchoDiscoveryService.Shared.StartListening ();

#if Azure
			Rosie.AzureIoT.AzureDeviceManager.Shared.Init ();
#endif

			base.OnStart (args);
		}
		protected override void OnStop ()
		{
			LocalServer.Shared.Stop ();
			EchoDiscoveryService.Shared.StopListening ();
			Console.WriteLine ("Amazon Echo Service Stoped");
			base.OnStop ();
		}
	}
}

