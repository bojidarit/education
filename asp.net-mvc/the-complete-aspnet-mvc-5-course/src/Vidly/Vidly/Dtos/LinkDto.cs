namespace Vidly.Dtos
{
	public class LinkDto
	{
		public LinkDto(string href, string rel, string method)
		{
			this.Href = href;
			this.Rel = rel;
			this.Method = method;
		}

		public string Href { get; private set; }
		public string Rel { get; private set; }
		public string Method { get; private set; }

		public override string ToString() =>
			$"<a href='{this.Href}' rel='{this.Rel}' method='{this.Method}'/>";
	}
}