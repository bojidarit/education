namespace SQLiteDapperApi.Dtos
{
	using Microsoft.AspNetCore.Hosting;
	using System.Net;

	public class DebugInfoDto
	{
		public DebugInfoDto()
		{
			PoweredBy = Helper.GetRuntimeInformation();
			string conf = "Release";
#if DEBUG
			conf = "Debug";
#endif
			BuildConfiguration = conf;
		}

		public DebugInfoDto(IPAddress remoteIpAddress, IHostingEnvironment hostingEnvironment)
			: this()
		{
			RemoteIpAddress = remoteIpAddress.ToString();
			ApplicationName = hostingEnvironment.ApplicationName;
			EnvironmentName = hostingEnvironment.EnvironmentName;
		}

		/// <summary>
		/// From HttpContext
		/// </summary>
		public string RemoteIpAddress { get; set; }

		/// <summary>
		/// From Header: Host
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// From Header: User-Agent
		/// </summary>
		public string UserAgent { get; set; }

		/// <summary>
		/// From Header: Accept
		/// </summary>
		public string Accept { get; set; }

		/// <summary>
		/// From Header: Accept-Language
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// From Header: Accept-Encoding
		/// </summary>
		public string Encoding { get; set; }

		/// <summary>
		/// Framework: Runtime Information
		/// </summary>
		public string PoweredBy { get; set; }

		// From HostEnvironment
		public string EnvironmentName { get; private set; }
		public string ApplicationName { get; private set; }

		public string BuildConfiguration { get; set; }
	}
}
