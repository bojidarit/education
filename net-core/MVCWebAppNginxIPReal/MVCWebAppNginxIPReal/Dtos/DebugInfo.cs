namespace MVCWebAppNginxIPReal.Dtos
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Extensions;
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;

	public class DebugInfo
	{
		#region Constructor
		public DebugInfo(HttpContext httpContext, IWebHostEnvironment env)
		{
			var connection = httpContext?.Connection;
			var request = httpContext?.Request;

			var hostInfo = Helpers.GetHostInfo();
			HostName = hostInfo.Item1;
			ActiveHostIp = Helpers.GetActiveHostIp();
			DhcpHostIp = Helpers.GetHostDhspIpAddress();
			HostIPs = hostInfo.Item2;

			if (connection != null)
			{
				RemoteIp = connection.RemoteIpAddress.ToString();
				RemotePort = connection.RemotePort;
				LocalIp = connection.LocalIpAddress.ToString();
				LocalPort = connection.LocalPort;
			}

			if (request != null)
			{
				DisplayUrl = request.GetDisplayUrl();
				BaseUrl = DisplayUrl.TrimEnd(request.Path.ToString().ToCharArray()) + "/";
			}

			if (env != null)
			{
				EnvironmentName = env.EnvironmentName;
				WebRootPath = env.WebRootPath;
				ContentRootPath = env.ContentRootPath;
			}

			TargetFramework = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
			EnvironmentVersion = Environment.Version.ToString();
			FrameworkDescription = RuntimeInformation.FrameworkDescription;
			OSDescription = RuntimeInformation.OSDescription;
			OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
			ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
		}

		#endregion


		#region Properties

		public string RemoteIp { get; set; }
		public int RemotePort { get; set; }
		public string LocalIp { get; set; }
		public int LocalPort { get; set; }
		public string HostName { get; set; }
		public string DhcpHostIp { get; set; }
		public string ActiveHostIp { get; set; }
		public IEnumerable<string> HostIPs { get; set; }
		public string DisplayUrl { get; set; }
		public string BaseUrl { get; set; }

		public string EnvironmentName { get; set; }
		public string ContentRootPath { get; set; }
		public string WebRootPath { get; set; }

		public string TargetFramework { get; set; }
		public string EnvironmentVersion { get; set; }
		public string FrameworkDescription { get; set; }
		public string OSDescription { get; set; }
		public string OSArchitecture { get; set; }
		public string ProcessArchitecture { get; set; }

		#endregion


		#region Overrides

		public override string ToString() => Helpers.ToJsonString(this);

		#endregion
	}
}
