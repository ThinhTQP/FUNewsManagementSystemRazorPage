using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class CreatePartialModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = new();

        public CreatePartialModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingAccounts = await _accountService.GetAllSystemAccountsAsync();

            if (existingAccounts.Any(a => a.AccountId == SystemAccount.AccountId))
            {
                TempData["ErrorMessage"] = "❌ Account ID đã tồn tại. Vui lòng nhập ID khác."; 
                return RedirectToPage("./Index");
            }

            await _accountService.AddSystemAccountAsync(SystemAccount);
            return RedirectToPage("./Index");
        }



    }
}
