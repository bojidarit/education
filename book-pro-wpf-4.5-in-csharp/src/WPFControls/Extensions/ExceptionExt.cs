namespace WPFControls.Extensions
{
	using System;

	public static class ExceptionExt
	{
		public static string Format(this Exception exception)
		{
			var message = $"Exception of type: {exception.GetType().FullName}" +
				$"{Environment.NewLine}Message: '{exception.Message}'" +
				$"{Environment.NewLine}Stack Trace:{Environment.NewLine}{exception.StackTrace}";

			if (exception.InnerException != null)
			{
				message += $"{Environment.NewLine}{Environment.NewLine}Inner Exception: " +
					$"{exception.InnerException.Message} ({exception.InnerException.GetType().Name})";
			}

			return message;
		}
	}
}
