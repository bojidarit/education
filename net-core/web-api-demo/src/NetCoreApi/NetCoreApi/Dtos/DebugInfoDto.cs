namespace NetCoreApi.Dtos
{
	using Microsoft.AspNetCore.Hosting;
	using System.Net;

	public class DebugInfoDto
	{
		public DebugInfoDto() { }

		public DebugInfoDto(IPAddress remoteIpAddress, IWebHostEnvironment env)
		{
			RemoteIpAddress = remoteIpAddress.ToString();
			EnvironmentName = env.EnvironmentName;
			ApplicationName = env.ApplicationName;
			ContentRootPath = env.ContentRootPath;
			WebRootPath = env.WebRootPath;

			SetBuildConfiguration();
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

		// From HostEnvironment

		public string EnvironmentName { get; private set; }
		public string ApplicationName { get; private set; }
		private string ContentRootPath { get; set; }
		private string WebRootPath { get; set; }

		public string BuildConfiguration { get; private set; }

		/// <summary>
		/// Framework: Runtime Information
		/// </summary>
		public string PoweredBy { get; set; }

		private void SetBuildConfiguration()
		{
			string conf = "Release";
#if DEBUG
			conf = "Debug";
#endif
			BuildConfiguration = conf;
		}
	}
}
