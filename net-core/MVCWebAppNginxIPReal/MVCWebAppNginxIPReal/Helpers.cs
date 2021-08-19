namespace MVCWebAppNginxIPReal
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.NetworkInformation;
	using System.Net.Sockets;
	using System.Text.Json;

	public static class Helpers
	{
		/// <summary>
		/// Source: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-core-3-1
		/// How to serialize and deserialize JSON in .NET
		/// </summary>
		public static string ToJsonString<T>(T obj)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				IgnoreReadOnlyProperties = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			var json = JsonSerializer.Serialize(obj, options);

			return json;
		}

		/// <summary>
		/// Source: https://stackoverflow.com/questions/6803073/get-local-ip-address
		/// </summary>
		public static Tuple<string, IEnumerable<string>> GetHostInfo()
		{
			var hostName = Dns.GetHostName();
			var host = Dns.GetHostEntry(hostName);

			// If the list is empty => No network adapters with an IPv4 address in the system!
			var ipV4Addresses = host.AddressList
				.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
				.Select(ip => ip.ToString());

			return Tuple.Create(hostName, ipV4Addresses.Any() ? ipV4Addresses : null);
		}

		/// <summary>
		/// Source: https://stackoverflow.com/questions/6803073/get-local-ip-address
		/// Without a network connection--you have no IP address, anyhow.
		/// When there are many network cards one can get the preferred IP address 
		/// depending on which connection was activated.
		/// </summary>
		public static string GetActiveHostIp()
		{
			try
			{
				using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
				{
					socket.Connect("8.8.8.8", 65530);
					var endPoint = socket.LocalEndPoint as IPEndPoint;
					var localIP = endPoint.Address.ToString();
					return localIP;
				}
			}
			catch (Exception ex)
			{
				return ex.FormatException();
			}
		}

		/// <summary>
		/// Source: https://stackoverflow.com/questions/6803073/get-local-ip-address
		/// Gets the most suitable IP from active network interfaces.
		/// The best IP is the IP got from DHCP server.
		/// </summary>
		public static string GetHostDhspIpAddress()
		{
			UnicastIPAddressInformation mostSuitableIp = null;

			try
			{
				var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

				foreach (var network in networkInterfaces)
				{
					if (network.OperationalStatus != OperationalStatus.Up)
						continue;

					var properties = network.GetIPProperties();

					if (properties.GatewayAddresses.Count == 0)
						continue;

					foreach (var address in properties.UnicastAddresses)
					{
						if (address.Address.AddressFamily != AddressFamily.InterNetwork)
							continue;

						if (IPAddress.IsLoopback(address.Address))
							continue;

						if (!address.IsDnsEligible)
						{
							if (mostSuitableIp == null)
								mostSuitableIp = address;
							continue;
						}

						// The best IP is the IP got from DHCP server
						if (address.PrefixOrigin != PrefixOrigin.Dhcp)
						{
							if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
								mostSuitableIp = address;
							continue;
						}

						return address.Address.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				return ex.FormatException();
			}

			return mostSuitableIp != null
				? mostSuitableIp.Address.ToString()
				: null;
		}

		public static string FormatException(this Exception ex) =>
			$"{ex.GetType().FullName}: {ex.Message}";
	}
}
