namespace NetCoreApi
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

		public static Uri CombineUri(string basePath, string relativePath)
		{
			// Putting slash in the end if missing...
			string path = basePath.EndsWith("/") ? basePath : $"{basePath}/";
			Uri baseUri = new Uri(path);

			// ... because if the path do not ends with slash,
			// combine constructor will trim the last segment.
			return new Uri(baseUri, relativePath);
		}

		public static Uri CombineRequestPath(HttpRequest request, string relativePath)
		{
			var requestPath = GetHttpRequestPath(request);
			Uri uri = CombineUri(requestPath, relativePath);

			return uri;
		}

		public static string GetRuntimeInformation()
		{
			var framework = Assembly.GetEntryAssembly()?
				.GetCustomAttribute<TargetFrameworkAttribute>()?
				.FrameworkName;

			return $"{framework} ({RuntimeInformation.FrameworkDescription}) @ OS {RuntimeInformation.OSArchitecture} " +
				$"[{RuntimeInformation.OSDescription.Trim()}] Process {RuntimeInformation.ProcessArchitecture}";
		}
	}
}
