namespace HelperLib.Dtos
{
	using System.Collections.Generic;

	public class DebugInfo
	{
		#region Constructors

		public DebugInfo() { }

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

		public override string ToString() => Util.ToJsonString(this);

		#endregion
	}
}
