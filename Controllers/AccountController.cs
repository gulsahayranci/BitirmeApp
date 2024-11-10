using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BitirmeApp3.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IdentityContext _context;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger, IdentityContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("Login POST action called.");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        // Set user details in session
                        HttpContext.Session.SetString("UserEmail", user.Email ?? string.Empty);
                        HttpContext.Session.SetString("UserId", user.Id ?? string.Empty);
                        HttpContext.Session.SetString("UserName", user.UserName ?? string.Empty);
                        HttpContext.Session.SetString("UserLastName", user.LastName ?? string.Empty);
                        HttpContext.Session.SetString("UserCity", user.CityId ?? string.Empty);

                        // Fetch and set roles in session
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Any())
                        {
                            HttpContext.Session.SetString("UserRole", string.Join(",", roles));

                            if (roles.Contains("Yatırımcı") || roles.Contains("Çiftçi"))
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else if (roles.Contains("Admin") || roles.Contains("il Müdürlüğü"))
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "User has no valid roles.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User has no valid roles.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel
            {
                Cities = _context.Cities.ToList(),
                Roles = _roleManager.Roles.Where(r => r.Name == "Çiftçi" || r.Name == "Yatırımcı")
                                  .Select(r => r.Name)
                                  .ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    LastName = model.LastName,
                    CityId = model.SelectedCityId
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);

                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("CityId", user.CityId ?? string.Empty);
                    HttpContext.Session.SetString("Role", model.SelectedRole);

                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Cities = _context.Cities.ToList();
            model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var viewModel = new UserProfileViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                LastName = user.LastName,
                City = _context.Cities.FirstOrDefault(c => c.Id == user.CityId)?.Name,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(viewModel);
        }
    }
}
