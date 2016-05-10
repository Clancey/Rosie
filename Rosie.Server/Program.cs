using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace Rosie.Server
{
	class MainClass
	{
		public static void Main (string [] args)
		{
#if DEBUG
			//The debugger cannot launch a service, so we handle it oursleves
			if (Debugger.IsAttached) {
				Task.Run (() => {
					(new RosieService ()).Start ();
				});
				while (true) {
					Thread.Sleep (10000);
				}
			}
#endif

			var ServicesToRun = new System.ServiceProcess.ServiceBase []
			  { new RosieService() };
			System.ServiceProcess.ServiceBase.Run (ServicesToRun);

		}
	}
}
