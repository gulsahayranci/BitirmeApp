using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BitirmeApp3.Controllers
{
    public class AdminController : Controller
    {
        private readonly IdentityContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager, IdentityContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        public async  Task<IActionResult> Index()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
            
    var userId = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    var approvedCount = await _context.Advertisements.CountAsync(ad => ad.UserId == userId && ad.IsApproved == true);
    var rejectedCount = await _context.Advertisements.CountAsync(ad => ad.UserId == userId && ad.IsRejected == true);
    var pendingCount = await _context.Advertisements.CountAsync(ad => ad.UserId == userId && ad.IsApproved == null && ad.IsRejected == null);

    // Log the counts to debug any issues
  
    ViewData["ApprovedCount"] = approvedCount;
    ViewData["RejectedCount"] = rejectedCount;
    ViewData["PendingCount"] = pendingCount;

    return View();
            
            return View();
        }
  
        [HttpGet]
        public async Task<IActionResult> ProvinceAdvertisementStats()
        {
            var ads = await _context.Advertisements
                .Where(ad => ad.IsApproved == true)
                .Include(ad => ad.City)
                .ToListAsync();

            var stats = ads
                .GroupBy(ad => ad.City.Name)
                .Select(g => new ProvinceAdvertisementStatsViewModel
                {
                    Province = g.Key,
                    TotalPrice = g.Sum(ad => ad.Price) ?? 0,
                    TotalArea = g.Sum(ad => 
                    {
                        double alanValue;
                        return double.TryParse(ad.Alan, out alanValue) ? alanValue : 0;
                    })
                })
                .ToList();

            // Debugging: Output stats to console or log
            foreach (var stat in stats)
            {
                System.Diagnostics.Debug.WriteLine($"Province: {stat.Province}, TotalPrice: {stat.TotalPrice}, TotalArea: {stat.TotalArea}");
            }

            return View(stats);
        }

        [Authorize]
        public async Task<IActionResult> Hesabım()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
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

        public async Task<IActionResult> Advertisement()
        {
            var advertisements = await _context.Advertisements
                .Include(ad => ad.User) // Include the user who created the advertisement
                .Include(ad => ad.AdApplications) // Include the applications for each advertisement
                .Select(ad => new AppliedAdvertisementsViewModel
                {
                    AdvertisementId = ad.Id,
                    AdvertisementTitle = ad.Title,
                    ImageUrl = ad.ImageUrl,
                    OwnerName = ad.User.UserName, // Add owner information
                    Applicants = ad.AdApplications.Select(app => new ApplicantViewModel
                    {
                        Id = app.Id,
                        Name = app.Name,
                        Surname = app.LastName,
                        Age = app.Age,
                        PhoneNumber = app.PhoneNumber,
                        Experience = app.Experience,
                        Message = app.Message,
                        Tool = app.Tool,
                        Tractor = app.Tractor,
                        IsApproved = app.IsApproved,
                        IsRejected = app.IsRejected,
                        Email = app.Email
                    }).ToList(),
                    IsApproved = ad.IsApproved ?? false,
                    IsRejected = ad.IsRejected ?? false,
                    Status = ad.Status
                })
                .ToListAsync();

            return View(advertisements);
        }

        public async Task<IActionResult> GelenMesajlar()
        {
            var messages = await _context.ContactMessages.OrderByDescending(m => m.SentAt).ToListAsync();
            return View(messages);
        }
         [HttpGet]
        public IActionResult SiteSettings()
        {
            var settings = _context.SiteSettings.FirstOrDefault();
            if (settings == null)
            {
                settings = new SiteSettings();
                _context.SiteSettings.Add(settings);
                _context.SaveChanges();
            }

            var viewModel = new SiteSettingsViewModel
            {
                PhoneNumber = settings.PhoneNumber,
                Address = settings.Address,
                Email = settings.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SiteSettings(SiteSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = _context.SiteSettings.FirstOrDefault();
                if (settings != null)
                {
                    settings.PhoneNumber = model.PhoneNumber;
                    settings.Address = model.Address;
                    settings.Email = model.Email;
                    _context.SiteSettings.Update(settings);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("SiteSettings");
            }

            return View(model);
        }
    }
}

