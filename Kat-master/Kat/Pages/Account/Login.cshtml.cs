using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Kat.Infrastructure.Domain;
using Kat.Infrastructure.Domain.Models;

namespace Kat.Pages.Account
{
    public class Login : PageModel
    {
        private ILogger<Index> _logger;
        private DefaultDbContext _context;

        [BindProperty]
        public ViewModel View { get; set; }

        public Login(DefaultDbContext context, ILogger<Index> logger)
        {
            _logger = logger;
            _context = context;
            View = View ?? new ViewModel();
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(View.EmailAddress))
            {
                ModelState.AddModelError("", "Login failed");
                return Page();
            }

            if (string.IsNullOrEmpty(View.Password))
            {
                ModelState.AddModelError("", "Login failed");
                return Page();
            }

            var user = _context?.Users?.FirstOrDefault(a => a.EmailAddress.ToLower() == View.EmailAddress.ToLower());

            if (user == null)
            {
                ModelState.AddModelError("", "Login failed");
                return Page();
            }
            else
            {
                var passwordInfo = _context?.UserLogins?.FirstOrDefault(a => a.UserId == user.Id && a.Key.ToLower() == "password");

                if (passwordInfo != null)
                {
                    if (BCrypt.Net.BCrypt.EnhancedVerify(View.Password, passwordInfo.Value))
                    {
                        var loginRetries = _context?.UserLogins?.FirstOrDefault(a => a.UserId == user.Id && a.Key.ToLower() == "loginretries");
                        if (loginRetries == null)
                        {
                            loginRetries = new UserLogin()
                            {
                                Id = Guid.NewGuid(),
                                UserId = user.Id,
                                Type = "General",
                                Key = "LoginRetries",
                                Value = "0"
                            };

                            _context?.UserLogins?.Add(loginRetries);
                            _context?.SaveChanges();
                        }

                        loginRetries.Value = "0";

                        _context?.UserLogins?.Update(loginRetries);
                        _context?.SaveChanges();


                        var isActive = _context?.UserLogins?.FirstOrDefault(a => a.UserId == user.Id && a.Key.ToLower() == "isactive");

                        if (isActive == null)
                        {
                            ModelState.AddModelError("", "Your account is inactive. Please talk to your administrator.");
                            return Page();
                        }
                        else
                        {

                            if (isActive.Value.ToLower() != "true")
                            {
                                ModelState.AddModelError("", "Your account is inactive. Please talk to your administrator.");
                                return Page();
                            }
                            else
                            {
                                var userRole = _context?.UserRoles?.Include(a => a.Role)!.FirstOrDefault(a => a.UserId == user.Id);


                                List<Claim> claims = new()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, (user.Id ?? Guid.NewGuid()).ToString()),
                                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                                    new Claim(ClaimTypes.Role, userRole!.Role!.Name)
                                };

                                ClaimsPrincipal principal = new(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                                await HttpContext.SignInAsync(principal, new AuthenticationProperties()
                                {
                                    IsPersistent = true,
                                    ExpiresUtc = DateTime.Now.AddMinutes(30)
                                });

                                if (userRole!.Role!.Name.ToLower() == "admin")
                                {
                                    return RedirectPermanent("/manage/roles");
                                }
                                else
                                {
                                    return RedirectPermanent("/manage/users");
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }


            return Page();
        }

        public class ViewModel
        {
            public string? EmailAddress { get; set; }
            public string? Password { get; set; }
        }
    }
}
