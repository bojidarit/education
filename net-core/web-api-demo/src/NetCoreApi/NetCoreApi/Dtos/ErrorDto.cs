namespace NetCoreApi.Dtos
{
	public class ErrorDto
	{
		public ErrorDto()
		{
		}

		public ErrorDto(int id, string message)
		{
			ErrorId = id;
			ErrorMessage = message;
		}

		public int ErrorId { get; set; }

		public string ErrorMessage { get; set; }

		public static ErrorDto Create(string errorMessage) => new ErrorDto(-1, errorMessage);
	}
}
