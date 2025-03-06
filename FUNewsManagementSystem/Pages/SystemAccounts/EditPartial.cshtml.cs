using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public EditPartialModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {

            var systemaccount = await _accountService.GetSystemAccountByIdAsync(id);
            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                await _accountService.UpdateSystemAccountAsync(SystemAccount);
                return RedirectToPage("/SystemAccounts/Index");
            }

            return Page();
        }
    }
}
