using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using FUNews.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.Categories
{
    [Authorize(Policy = "StaffOnly")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IEnumerable<Category> Category { get; set; } = Enumerable.Empty<Category>();

        public async Task OnGetAsync()
        {
            Category = await _categoryService.GetAllCategoriesAsync();

           
        }
    }
}
