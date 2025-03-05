using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();

        //public async Task OnGetAsync(string searchString)
        //{
        //    NewsArticles = await _newsArticleService.GetAllNewsArticlesAsync();
        //}
        public async Task OnGetAsync(string searchString)
        {
            NewsArticles = await _newsArticleService.GetAllNewsArticlesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                NewsArticles = NewsArticles
                    .Where(a => a.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}
