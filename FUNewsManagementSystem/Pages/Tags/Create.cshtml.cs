using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace FUNewsManagementSystem.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

        private readonly IHubContext<SignalrServer> _hubContext;
        public CreateModel(ITagService tagService, IHubContext<SignalrServer> hubContext)
        {
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Lấy TagId lớn nhất và +1
                var maxId = (await _tagService.GetAllTagsAsync()).Max(t => (int?)t.TagId) ?? 0;
                Tag.TagId = maxId + 1;

                await _tagService.AddTagAsync(Tag);
                await _hubContext.Clients.All.SendAsync("LoadAllItems");

                return RedirectToPage("./Index");
            }

            return Page();
          
        }
    }
}
