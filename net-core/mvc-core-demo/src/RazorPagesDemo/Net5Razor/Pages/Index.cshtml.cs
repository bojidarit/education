namespace Net5Razor.Pages
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;

	public class IndexModel : PageModel
	{
		[BindProperty]
		public string Name { get; set; }

		public void OnGet()
		{

		}
	}
}
