using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "LecturerOnly")]
    public class LecturerNewsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public LecturerNewsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;

        }


        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

      
        public async Task OnGetAsync()
        {
            NewsArticles = await _newsArticleService.GetActiveNewsForLecturerAsync();

        }

    }
}
