using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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

			RestorePackages();
			//Launches HAP-NodeJS process
			StartNodeJs ();

			Console.WriteLine ($"[Net] Host started in port: {Port}");
		}

		void RestorePackages()
		{
			var filename = "npm";
			var arguments = "install";
			var p = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = filename,
					Arguments = arguments,
					WorkingDirectory = hapNodePath,
					UseShellExecute = true,
					CreateNoWindow = true,

				}
			};

			if (Debug)
			{
				p.StartInfo.EnvironmentVariables.Add("DEBUG", "*");
			}
			p.Start();
			p.WaitForExit();
		}
		void StartNodeJs ()
		{
			var tcs = new TaskCompletionSource<bool>();
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
				if (e?.Data?.StartsWith("Magic happens on") ?? false)
					tcs.TrySetResult(true);
				//The server is still running, we will call it good!
				if (e?.Data?.Contains("listen EADDRINUSE :::8080") ?? false)
					tcs.TrySetResult(true);
				Console.WriteLine ($"[NodeJS]{e.Data}");
			};

			process.Start ();
			process.BeginOutputReadLine ();
			var result = Task.WhenAny(tcs.Task, Task.Delay(5000) ).Result;
			if (result != tcs.Task)
				throw new Exception("Error loading the server");
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
