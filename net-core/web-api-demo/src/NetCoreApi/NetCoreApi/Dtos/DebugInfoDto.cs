namespace NetCoreApi.Dtos
{
	using System.Net;

	public class DebugInfoDto
	{
		public DebugInfoDto() { }

		public DebugInfoDto(IPAddress remoteIpAddress)
		{
			this.RemoteIpAddress = remoteIpAddress.ToString();
		}

		public string RemoteIpAddress { get; set; }

		public string UserAgent { get; set; }

		public string Accept { get; set; }

		public string Language { get; set; }

		public string PoweredBy { get; set; }
	}
}
