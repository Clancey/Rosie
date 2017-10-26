using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using SimpleAuth;

namespace Rosie.Server
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			//DeviceDatabase.Shared.TestUpdates ();

			BasicAuthApi.ShowAuthenticator = async (auth) => {
				var authenticator = new BasicAuthController(auth);
				await authenticator.GetCredentials("SmartThings Login");
			};
#if DEBUG
			//The debugger cannot launch a service, so we handle it oursleves
			//if (Debugger.IsAttached) {
				Task.Run (() => {
					(new RosieService ()).Start ();
				}).Wait();
				while (true) {
					Thread.Sleep (10000);
				}
			//}
#endif

			var ServicesToRun = new System.ServiceProcess.ServiceBase []
			  { new RosieService() };
			System.ServiceProcess.ServiceBase.Run (ServicesToRun);

		}
	}
}
