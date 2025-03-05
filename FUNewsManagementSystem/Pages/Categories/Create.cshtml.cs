using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using FUNews.BLL.Services;

namespace FUNewsManagementSystem.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var activeCategories = await _categoryService.GetActiveCategoriesAsync();
            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var parentCategories = await _categoryService.GetAllCategoriesAsync();
                ViewData["ParentCategoryId"] = new SelectList(parentCategories, "CategoryId", "CategoryName", Category.ParentCategoryId);
                return Page();
            }

            await _categoryService.AddCategoryAsync(Category);
            return RedirectToPage("/Categories/Index");
        }

    }
}
