namespace Vidly.Dtos
{
	using Flurl;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Manages "Hypermedia As The Engine of Application State" (HATEOAS) links
	/// </summary>
	public abstract class HateoasDtoBase
	{
		public string Href { get; private set; }
		public List<LinkDto> Links { get; private set; } = new List<LinkDto>();

		/// <summary>
		/// Generates HATEOAS links
		/// </summary>
		public T GenerateLinks<T>(Uri uri, string id = null)
			where T : HateoasDtoBase
		{
			this.Href = Url.Combine(uri.ToString(), (!string.IsNullOrWhiteSpace(id) ? $"{id}" : string.Empty));

			this.Links.Add(new LinkDto(this.Href, "self", "GET"));
			this.Links.Add(new LinkDto(RemoveId(this.Href), "create", "POST"));
			this.Links.Add(new LinkDto(this.Href, "update", "PUT"));
			this.Links.Add(new LinkDto(this.Href, "delete", "DELETE"));

			return (T)this;
		}

		public static string RemoveId(string href)
		{
			int lastIndex = href.LastIndexOf('/');
			return href.Substring(0, lastIndex);
		}
	}
}