using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class NewDetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        public NewDetailsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            else
            {
                NewsArticle = article;
            }
            return Page();
        }
    }
}
