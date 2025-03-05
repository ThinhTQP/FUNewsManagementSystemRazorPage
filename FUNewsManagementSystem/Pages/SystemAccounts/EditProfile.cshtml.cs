using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; }

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public EditProfileModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out short userId))
            {
                return Unauthorized();
            }

            var account = await _accountService.GetSystemAccountByIdAsync(userId);
            if (account == null) return NotFound();

            SystemAccount = account;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out short userId))
            {
                return Unauthorized();
            }

            if (userId != SystemAccount.AccountId) return Forbid();

            var existingAccount = await _accountService.GetSystemAccountByIdAsync(userId);
            if (existingAccount == null) return NotFound();

            // Kiểm tra nếu người dùng nhập mật khẩu cũ và mới
            if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                if (existingAccount.AccountPassword != OldPassword)
                {
                    TempData["ErrorMessage"] = "Old password is incorrect!";
                    return Page();
                }
                existingAccount.AccountPassword = NewPassword;
            }

            existingAccount.AccountName = SystemAccount.AccountName;

            await _accountService.UpdateSystemAccountAsync(existingAccount);
            TempData["SuccessMessage"] = "Profile updated successfully!";

            return RedirectToPage("/SystemAccounts/EditProfile");
        }
    }
}
