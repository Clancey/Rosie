#define Azure
#define Node

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
			Init ();
			LocalServer.Shared.Start ();
			EchoDiscoveryService.Shared.StartListening ();
			base.OnStart (args);
		}

		async void Init ()
		{


#if Azure
			await Rosie.AzureIoT.AzureDeviceManager.Shared.Init ();
#endif
#if Node
			await Rosie.Node.NodeManager.Shared.Init ();
#endif
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

