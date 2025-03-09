using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using BusinessObjects.Entities;
using FUNews.BLL.Services;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;

        public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService, ISystemAccountService systemAccountService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _systemAccountService = systemAccountService;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<NewsArticle> NewsArticles { get; set; }
        public int CountArticles { get; set; }

        public Dictionary<string, int> CategoryStats { get; set; }
        public Dictionary<string, int> StaffStats { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate ??= startDate;
            EndDate ??= endDate;
            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            var articles = await _newsArticleService.GetAllNewsArticlesAsync();

            if (StartDate.HasValue)
                articles = articles.Where(a => a.CreatedDate >= StartDate.Value).ToList();
            if (EndDate.HasValue)
                articles = articles.Where(a => a.CreatedDate <= EndDate.Value).ToList();

            NewsArticles = articles.OrderByDescending(a => a.CreatedDate).ToList();
            CountArticles = NewsArticles.Count;

            var categories = await _categoryService.GetAllCategoriesAsync();
            CategoryStats = categories.ToDictionary(
                c => c.CategoryName,
                c => articles.Count(a => a.CategoryId == c.CategoryId)
            );

            var staffList = await _systemAccountService.GetAllSystemAccountsAsync();
            StaffStats = staffList.ToDictionary(
                s => s.AccountName,
                s => articles.Count(a => a.CreatedById == s.AccountId)
            );
        }
    }

}