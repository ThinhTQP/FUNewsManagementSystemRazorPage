using BusinessObjects.Entities;
using FUNews.BLL.Services;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;


namespace FUNewsManagementSystem.Pages.NewsArticles
{


    public class CreatePartialModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<SignalrServer> _hubContext;

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> TagIds { get; set; } = new();

        public SelectList CategoryList { get; set; }
        public MultiSelectList TagList { get; set; }

        public CreatePartialModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IHubContext<SignalrServer> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;

        }

        [Authorize(Policy = "StaffOnly")]
        public async Task<IActionResult> OnGetAsync()
        {
            CategoryList = new SelectList(await _categoryService.GetActiveCategoriesAsync(), "CategoryId", "CategoryName");
            var tags = await _tagService.GetAllTagsAsync();
            TagList = new MultiSelectList(tags, "TagId", "TagName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
        

            if (string.IsNullOrEmpty(NewsArticle.NewsArticleId))
            {
                NewsArticle.NewsArticleId = Guid.NewGuid().ToString("N").Substring(0, 20);
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdValue) && short.TryParse(userIdValue, out short staffId))
            {
                NewsArticle.CreatedById = staffId;
                NewsArticle.UpdatedById = staffId;
            }

            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.ModifiedDate = DateTime.Now;


            await _newsArticleService.AddNewsArticleAsync(NewsArticle, TagIds);
            await _hubContext.Clients.All.SendAsync("LoadAllArticles");
            return RedirectToPage("/NewsArticles/Index");
        }


    }
}