using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Rosie.Node
{
	public class NodeSession : IDisposable
	{
		const string DefaultBrokerHost = "localhost";
		const int DefaultConnectionRetries = 5;

		public string[] AllowedHosts { get; set; }
		public int ConnectionRetries { get; set; } = DefaultConnectionRetries;

		internal const int Port = 8080;
		Process process;
		string hapNodePath;

		public string BrokerHost { get; private set; }

		public bool Debug { get; internal set; }
		public bool Sudo { get; set; }

		public void CheckHost ()
		{
			//Is our device allowed to execute this host
			if (AllowedHosts != null) {
				if (!AllowedHosts.Contains(Environment.MachineName)) {
					throw new UnauthorizedAccessException("Your device is not allowed to execute this Host session.");
				}
			}
		}

		public void Start(string hapNodePath, string brokerHost = DefaultBrokerHost)
		{
			this.hapNodePath = hapNodePath;

			CheckHost ();

			//Kill current user node processes
			ProcessService.CleanProcessesInMemory ();

			//Launches HAP-NodeJS process
			StartNodeJs ();

			Console.WriteLine ($"[Net] Host started in port: {Port}");
		}


		void StartNodeJs ()
		{
			var filename = Sudo ? "sudo" : "node";
			var arguments = Sudo ? "node app.js" : "app.js";
			process = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = filename,
					Arguments = arguments,
					WorkingDirectory = hapNodePath,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
				}
			};

			if (Debug) {
				process.StartInfo.EnvironmentVariables.Add ("DEBUG", "*");
			}

			process.OutputDataReceived += (s, e) => {
				Console.WriteLine ($"[NodeJS]{e.Data}");
			};

			process.Start ();
			process.BeginOutputReadLine ();
		}


		internal void Stop ()
		{
			KillProcess (process);
		}

		void KillProcess (Process process)
		{
			try {
				process.Close ();
				process.Kill ();
				process.Dispose ();
			} catch {
			}
		}

		public void Dispose ()
		{
			Stop ();
		}
	}
}
