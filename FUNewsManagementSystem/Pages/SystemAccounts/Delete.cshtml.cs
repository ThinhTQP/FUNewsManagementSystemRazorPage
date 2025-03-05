using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using FUNews.DAL;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public DeleteModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

       

        public async Task<IActionResult> OnPostAsync(short id)
        {
            await _accountService.DeleteSystemAccountAsync(id);
            return RedirectToPage("/SystemAccounts/Index");
        }
    }
}
