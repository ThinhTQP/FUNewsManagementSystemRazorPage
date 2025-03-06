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

            if (ModelState.IsValid)
            {
                var maxId = (await _accountService.GetAllSystemAccountsAsync())
                        .Max(t => (short?)t.AccountId) ?? 0;
                SystemAccount.AccountId = (short)(maxId + 1);
                await _accountService.AddSystemAccountAsync(SystemAccount);
                return RedirectToPage("/SystemAccounts/Index"); 
            }

            return Page(); 

        }
    }
}
