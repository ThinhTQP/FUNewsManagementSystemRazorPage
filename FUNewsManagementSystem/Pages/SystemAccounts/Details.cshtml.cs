using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class DetailsModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public DetailsModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

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
    }
}
