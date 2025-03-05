using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace FUNewsManagementSystem.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

        public async Task OnGetAsync()
        {
            Tags = await _tagService.GetAllTagsAsync();


        }
    }
}
