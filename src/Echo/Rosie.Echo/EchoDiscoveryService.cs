using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Net.NetworkInformation;
using Rosie.Server.Echo;

namespace Rosie.Echo
{
	public class EchoDiscoveryService
	{

		public const int DiscoveryPort = 1900;
		public const string MultiCastAddress = "239.255.255.250";

		static EchoDiscoveryService _shared;
		public static EchoDiscoveryService Shared { get { return _shared ?? (_shared = new EchoDiscoveryService ());} }

		static readonly IPAddress ipaddress = IPAddress.Parse (MultiCastAddress);
		public bool IsListening { get; set; }

		public void StopListening ()
		{
			if (!IsListening)
				return;
			IsListening = false;
			if (listener != null)
				listener.Close ();
		}

		UdpClient listener;
		public void StartListening ()
		{
			if (IsListening)
				return;

			Task.Run (() => {
				try {
					listener = null;
					listener = new UdpClient (DiscoveryPort);

					listener.Client.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
					listener.Client.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
					listener.Client.SetSocketOption (SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 4);
					listener.Client.SetSocketOption (SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption (ipaddress, 0));
					IPEndPoint groupEP = new IPEndPoint (IPAddress.Any, DiscoveryPort);
					IsListening = true;
					while (IsListening) {
						try {
							Console.WriteLine ("Waiting for discovery ping");
							byte [] bytes = listener.Receive (ref groupEP);
							Console.WriteLine ("Recieved from: " + groupEP.Address.ToString ());

							var json = Encoding.ASCII.GetString (bytes, 0, bytes.Length);
							//Console.WriteLine (json);
							if (!IsListening)
								return;
							MessageReceived (json, (IPEndPoint)listener.Client.LocalEndPoint, groupEP);
						} catch (Exception e) {
							Console.WriteLine (e.ToString ());
						}
					}
					listener.Close ();
				} catch (Exception ex) {
					Console.WriteLine (ex);
				}
			});
		}

		void MessageReceived (string message,IPEndPoint local, IPEndPoint address)
		{
			if (!(isSSDPDiscovery (message)))
			   return;
			
			var discoveryMessage = MessageTemplates.GetDiscoveryTemplate (GetAddress().ToString (), AmazonEchoWebServer.EchoWebServerPort, GetDeviceId());
			SendMessage (discoveryMessage,address);

		}

		public static string GetDeviceId ()
		{
			return "4061666B-8AEE-488C-BB37-877828E8992F";
		}

		public static IPAddress GetAddress ()
		{
			foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces ()) {
				if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
					netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
					foreach (var addrInfo in netInterface.GetIPProperties ().UnicastAddresses) {
						if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork) {
							return addrInfo.Address;
						}
					}
				}
			}
			return IPAddress.Any;
		}

		private static NetworkInterface GetNetworkInterface ()
		{
			IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties ();
			if (computerProperties == null)
				return null;

			NetworkInterface [] nics = NetworkInterface.GetAllNetworkInterfaces ();
			if (nics == null || nics.Length < 1)
				return null;

			NetworkInterface best = null;
			foreach (NetworkInterface adapter in nics) {
				if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback || adapter.NetworkInterfaceType == NetworkInterfaceType.Unknown)
					continue;
				if (!adapter.Supports (NetworkInterfaceComponent.IPv4))
					continue;
				if (best == null)
					best = adapter;
				if (adapter.OperationalStatus != OperationalStatus.Up)
					continue;

				// make sure this adapter has any ipv4 addresses
				IPInterfaceProperties properties = adapter.GetIPProperties ();
				foreach (UnicastIPAddressInformation unicastAddress in properties.UnicastAddresses) {
					if (unicastAddress != null && unicastAddress.Address != null && unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork) {
						// Yes it does, return this network interface.
						return adapter;
					}
				}
			}
			return best;
		}

		bool isSSDPDiscovery (string body)
		{
			if (body != null && body.StartsWith ("M-SEARCH * HTTP/1.1") && body.Contains ("MAN: \"ssdp:discover\"")) {
				return true;
			}
			return false;
		}

		public void SendMessage (string message, IPEndPoint source)
		{
			var data = Encoding.UTF8.GetBytes (message);
			listener.Send (data, data.Length, source);
		}

	}
}

