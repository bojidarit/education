namespace Vidly.Dtos
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Manages "Hypermedia As The Engine of Application State" (HATEOAS) links
	/// </summary>
	public abstract class HateoasDtoBase
	{
		List<LinkDto> Links { get; set; } = new List<LinkDto>();

		/// <summary>
		/// Generates HATEOAS links
		/// </summary>
		public T GenerateLinks<T>(Uri uri, string id)
			where T : HateoasDtoBase
		{
			string href = uri.ToString() + (!string.IsNullOrWhiteSpace(id) ? $"/{id}" : string.Empty);

			this.Links.Add(new LinkDto(href, "self", "GET"));
			this.Links.Add(new LinkDto(href, "create", "POST"));
			this.Links.Add(new LinkDto(href, "update", "PUT"));
			this.Links.Add(new LinkDto(href, "delete", "DELETE"));

			return (T)this;
		}
	}
}