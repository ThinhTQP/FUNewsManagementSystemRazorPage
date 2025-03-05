using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public IndexModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        // 🔹 Dùng IEnumerable thay vì List
        public IEnumerable<SystemAccount> Accounts { get; set; } = Enumerable.Empty<SystemAccount>();

        public async Task OnGetAsync()
        {
            Accounts = await _accountService.GetAllSystemAccountsAsync();
        }
    }
}
