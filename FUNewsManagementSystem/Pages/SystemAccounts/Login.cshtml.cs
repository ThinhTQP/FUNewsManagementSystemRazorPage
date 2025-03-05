using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNews.BLL.Services;
using BusinessObjects.Entities;

namespace FUNewsManagementSystem.Pages.SystemAccounts
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _config;

        public LoginModel(ISystemAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _config = config;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var defaultAdminEmail = _config["DefaultAccount:Email"];
            var defaultAdminPassword = _config["DefaultAccount:Password"];

            if (Input.Email == defaultAdminEmail && Input.Password == defaultAdminPassword)
            {
                var adminClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Default Admin"),
                    new Claim(ClaimTypes.Email, defaultAdminEmail),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(adminIdentity));

                return RedirectToPage("/SystemAccounts/Index");
            }

            var user = (await _accountService.GetAllSystemAccountsAsync())
                .FirstOrDefault(u => u.AccountEmail == Input.Email && u.AccountPassword == Input.Password);

            if (user == null)
            {
                ErrorMessage = "Invalid credentials!";
                return Page(); // Quay lại trang login
            }

            var role = user.AccountRole switch
            {
                1 => "Staff",
                2 => "Lecturer",
                _ => "Lecturer"
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.AccountName ?? ""),
                new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return role switch
            {
                "Staff" => RedirectToPage("/NewsArticles/Index"),
                "Lecturer" => RedirectToPage("/NewsArticles/LecturerNews"),
                _ => RedirectToPage("/NewsArticles/LecturerNews")
            };
        }

        public class LoginInputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
