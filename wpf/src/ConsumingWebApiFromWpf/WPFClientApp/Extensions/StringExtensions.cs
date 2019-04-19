namespace WPFClientApp.Extensions
{
	using System;

	public static class StringExtensions
	{
		public static bool ValidateUrl(this string uriString)
		{
			Uri uriResult = null;
			bool result = Uri.TryCreate(uriString, UriKind.Absolute, out uriResult);

			if (result)
			{
				result = (uriResult.Scheme == Uri.UriSchemeHttp) ||
					(uriResult.Scheme == Uri.UriSchemeHttps);
			}

			return result;
		}
	}
}
