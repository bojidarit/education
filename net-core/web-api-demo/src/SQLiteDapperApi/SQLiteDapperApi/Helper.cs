namespace SQLiteDapperApi
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Extensions;
	using System;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Runtime.Versioning;

	public static class Helper
	{
		public static string GetHttpRequestPath(HttpRequest request)
		{
			// Assembly: Microsoft.AspNetCore.Http.Extensions
			return request.GetEncodedUrl();
		}

		public static Uri CombineUri(string basePath, object relativePath)
		{
			// Putting slash in the end if missing...
			string path = basePath.EndsWith("/") ? basePath : $"{basePath}/";

			return new Uri($"{path}{relativePath}");
		}

		public static Uri CombineRequestPath(HttpRequest request, object relativePath)
		{
			var requestPath = GetHttpRequestPath(request);
			Uri uri = CombineUri(requestPath, relativePath.ToString());

			return uri;
		}

		public static string GetRuntimeInformation()
		{
			var framework = Assembly.GetEntryAssembly()?
				.GetCustomAttribute<TargetFrameworkAttribute>()?
				.FrameworkName;

			return $"{framework} ({RuntimeInformation.FrameworkDescription}) " +
				$"@ OS {RuntimeInformation.OSArchitecture} [{RuntimeInformation.OSDescription.Trim()}] " +
				$"Process {RuntimeInformation.ProcessArchitecture}";
		}
	}
}
