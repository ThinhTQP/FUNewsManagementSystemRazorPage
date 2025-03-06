using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FUNews.BLL.Services;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<SignalrServer> _hubContext;
        public EditPartialModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IHubContext<SignalrServer> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public SelectList Categories { get; set; }
        public List<SelectListItem> Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            NewsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (NewsArticle == null)
            {
                return NotFound();
            }

            SelectedTagIds = NewsArticle.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();

            Categories = new SelectList(
                await _categoryService.GetActiveCategoriesAsync(),
                "CategoryId",
                "CategoryName",
                NewsArticle.CategoryId);

            var allTags = await _tagService.GetAllTagsAsync();
            Tags = allTags
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Console.WriteLine($"[DEBUG] ID: {id}");
            Console.WriteLine($"[DEBUG] ID: {NewsArticle.NewsArticleId}");
            Console.WriteLine($"TagIds count: {SelectedTagIds?.Count ?? 0}");
            if (id != NewsArticle.NewsArticleId)
            {
                return NotFound();
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdValue) && short.TryParse(userIdValue, out short staffId))
            {
                NewsArticle.UpdatedById = staffId;
            }

            NewsArticle.ModifiedDate = DateTime.Now;
            Console.WriteLine($"[DEBUG] TagIds Count: {SelectedTagIds?.Count ?? 0}");
            Console.WriteLine($"[DEBUG] TagIds: {string.Join(", ", SelectedTagIds ?? new List<int>())}");
            await _newsArticleService.UpdateNewsArticleAsync(NewsArticle, SelectedTagIds);
            await _hubContext.Clients.All.SendAsync("LoadAllArticles");

            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
