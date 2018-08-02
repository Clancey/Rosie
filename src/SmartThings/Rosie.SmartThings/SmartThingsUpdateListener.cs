using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;

namespace Rosie.SmartThings
{
	public class SmartThingsUpdateListener
	{
		SmartThingsApi api;
		public SmartThingsUpdateListener (SmartThingsApi api)
		{
			this.api = api;
		}
		public bool IsConnected => client?.Connected ?? false;
		public event Action<SmartThingsUpdate> UpdateReceived;
		public UserLocation Location { get; set; }
		public async Task StartListening ()
		{
			if (IsConnected)
				return;
			Location = Location ?? await api.GetDefaultLocation ();
			var clientId = await api.GetSocketClientID (Location);
			int port = 443;
			int.TryParse (Location.Shard.Client.Port, out port);
			Task.Run (() => { RunClient (clientId, Location.Shard.Client.Host, port);});
		}
		public void Close ()
		{
			sslStream?.Dispose ();
			sslStream = null;
			client?.Close ();
			client = null;
		}

		SslStream sslStream;
		TcpClient client;

		void RunClient (string clientId,string server, int port)
		{
			try {
				client = new TcpClient (server, port);
				Console.WriteLine ("Client connected.");

				sslStream = new SslStream (
					client.GetStream (),
					false,
					new RemoteCertificateValidationCallback (ValidateServerCertificate),
					null
					);
				// The server name must match the name on the server certificate.
				try {
					sslStream.AuthenticateAsClient (server);
				} catch (AuthenticationException e) {
					Console.WriteLine ("Exception: {0}", e.Message);
					if (e.InnerException != null) {
						Console.WriteLine ("Inner exception: {0}", e.InnerException.Message);
					}
					Console.WriteLine ("Authentication failed - closing the connection.");
					client.Close ();
					return;
				}
				//Send the register command
				SendMessage ($"register {clientId}");
				//Send a ping every 30 seconds
				Task.Run (async () => {
					while (IsConnected) {
						await Task.Delay (TimeSpan.FromSeconds (300));
						SendMessage ("ping");
					}
				});
				while (IsConnected) {
					try {
						// Read message from the server.
						ReadMessage ();
					} catch (Exception ex) {
						Console.WriteLine (ex);

					}
				}
				// Close the client connection.
				client.Close ();
				Console.WriteLine ("Client closed.");
			} catch (Exception ex) {
				Console.WriteLine (ex);
			} 
		}
		void SendMessage (string message)
		{
			sslStream.Write (Encoding.UTF8.GetBytes (message));
			sslStream.Flush ();
		}
		StringBuilder messageData = new StringBuilder ();
		void ReadMessage ()
		{
			// Read the  message sent by the server.
			// The end of the message is signaled using the
			// "<EOF>" marker.
			byte [] buffer = new byte [2048];
			int bytes = -1;
			do {
				bytes = sslStream.Read (buffer, 0, buffer.Length);


				// Use Decoder class to convert from bytes to UTF8
				// in case a character spans two buffers.
				Decoder decoder = Encoding.UTF8.GetDecoder ();
				char [] chars = new char [decoder.GetCharCount (buffer, 0, bytes)];
				decoder.GetChars (buffer, 0, bytes, chars, 0);
				messageData.Append (chars);

				var message = messageData.ToString ();
				if (!message.Contains ("\n"))
					continue;
				messageData.Clear ();
				var messages = message.Split (new [] { '\n' }, StringSplitOptions.None);
				foreach (var m in messages) {
					if (!HandleMessage (m)) {
						messageData.Append (m);
					}
				}
				
			} while (bytes != 0);
		}
		bool HandleMessage (string message)
		{
			if (string.IsNullOrWhiteSpace(message) || message.StartsWith ("register") || message.StartsWith ("echo"))
				return true;

			try {
				var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartThingsUpdate> (message);
				SendMessage ($"ack {resp.Event.Id}");
				UpdateReceived?.Invoke (resp);
				Console.WriteLine ("Server says: {0}", message);
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}
		public static bool ValidateServerCertificate (
			 object sender,
			 X509Certificate certificate,
			 X509Chain chain,
			 SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
				return true;

			Console.WriteLine ("Certificate error: {0}", sslPolicyErrors);

			// Do not allow this client to communicate with unauthenticated servers.
			return false;
		}

	}
}

