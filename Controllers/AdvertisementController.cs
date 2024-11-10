using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;

namespace BitirmeApp3.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IdentityContext _context;
        private readonly ILogger<AdvertisementController> _logger;

        public AdvertisementController(UserManager<AppUser> userManager, IdentityContext context, ILogger<AdvertisementController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
            var viewModel = new CreateAdvertisementViewModel
            {
                Cities = _context.Cities.ToList()
            };
            return View(viewModel);
        }

      [HttpPost]
public async Task<IActionResult> Create(CreateAdvertisementViewModel model)
{

    if (ModelState.IsValid)
    {
        string imageUrl = string.Empty;

        if (model.Image != null && model.Image.Length > 0)
        {
            var fileName = Path.GetFileName(model.Image.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var filePath = Path.Combine(imagePath, fileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

                imageUrl = $"/images/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the image.");
                ModelState.AddModelError(string.Empty, "An error occurred while uploading the image.");
                model.Cities = _context.Cities.ToList();
                return View(model);
            }
        }

        var advertisement = new Advertisement
        {
            Title = model.Title,
            Content = model.Content,
            Tag = model.Tag,
            Status = model.Status,
            UserId = HttpContext.Session.GetString("UserId"),
            CityId = model.SelectedCityId,
            ImageUrl = imageUrl,
            Price = model.Price,
            Alan = model.Alan
        };

        _context.Advertisements.Add(advertisement);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    model.Cities = _context.Cities.ToList();
    return View(model);
}

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            
            var advertisement = await _context.Advertisements
                .Include(a => a.City)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advertisement == null)
            {
                return NotFound();
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;

            return View(advertisement);
        }

        [HttpGet]
        [Authorize(Roles = "Çiftçi")]
        public IActionResult Apply(string id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userCityId = HttpContext.Session.GetString("UserCity");
            var userCity = _context.Cities.FirstOrDefault(c => c.Id == userCityId)?.Name;

            var viewModel = new AdApplicationViewModel
            {
                AdvertisementId = id,
                UserId = userId,
                Name = HttpContext.Session.GetString("UserName"),
            
                Email = HttpContext.Session.GetString("UserEmail"),
                City = userCity
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(AdApplicationViewModel model)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
            if (ModelState.IsValid)
            {
                var adApplication = new AdApplication
                {
                    Id = Guid.NewGuid().ToString(),
                    AdvertisementId = model.AdvertisementId,
                    UserId = model.UserId,
                    Name = model.Name,
                    
                    Age = model.Age,
                    PhoneNumber = model.PhoneNumber,
                    Experience = model.Experience,
                    Message = model.Message,
                    Tool = model.Tool,
                    Tractor = model.Tractor,
                    Email = model.Email
                };

                _context.AdApplications.Add(adApplication);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

   

        [HttpGet]
        public async Task<IActionResult> ViewApplications(string applicationId)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
            var application = await _context.AdApplications
                .Include(app => app.Advertisement)
                .FirstOrDefaultAsync(app => app.Id == applicationId);

            if (application == null)
            {
                return NotFound();
            }

            var viewModel = new ViewApplicationsViewModel
            {
                ApplicationId = application.Id,
                AdvertisementTitle = application.Advertisement.Title,
                ApplicantName = application.Name,
                ApplicantEmail = application.Email,
                Message = application.Message,
                Age = application.Age,
                PhoneNumber = application.PhoneNumber,
                Experience = application.Experience,
                Tool = application.Tool,
                Tractor = application.Tractor
            };

            return View(viewModel);
        }

[HttpPost]
public async Task<IActionResult> EvaluateApplication(string applicationId, bool isAccepted)
{
    var userEmail = HttpContext.Session.GetString("UserEmail");
    ViewData["UserEmail"] = userEmail;
    var application = await _context.AdApplications
        .Include(app => app.Advertisement)
        .FirstOrDefaultAsync(app => app.Id == applicationId);

    if (application == null)
    {
        return NotFound();
    }

    if (isAccepted)
    {
        application.IsApproved = true;
        application.IsRejected = false; // Ensure rejection flag is false
        application.Advertisement.Status = "Passive";
    }
    else
    {
        application.IsRejected = true;
        application.IsApproved = false; // Ensure approval flag is false
    }

    await _context.SaveChangesAsync();

    return RedirectToAction("InApp");
}

[HttpGet]
public async Task<IActionResult> InApp()
{
    var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
    var userId = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    var userAds = await _context.Advertisements
        .Where(ad => ad.UserId == userId)
        .ToListAsync();

    var adApplications = await _context.AdApplications
        .Where(app => userAds.Select(ad => ad.Id).Contains(app.AdvertisementId))
        .Include(app => app.Advertisement)
        .ThenInclude(ad => ad.User)
        .ToListAsync();

    var model = adApplications.Select(app => new AppliedAdvertisementsViewModel
    {
        ApplicationId = app.Id,
        AdvertisementTitle = app.Advertisement.Title,
        AdvertisementOwner = app.Advertisement.User.UserName,
        ImageUrl = app.Advertisement.ImageUrl,
        ApplicantName = app.Name,
        IsApproved = app.IsApproved,
        IsRejected = app.IsRejected // Add this line
    }).ToList();

    return View(model);
}

[HttpGet]
public async Task<IActionResult> Applied()
{
    var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
    var userId = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    var appliedAds = await _context.AdApplications
        .Where(app => app.UserId == userId)
        .Include(app => app.Advertisement)
        .ThenInclude(ad => ad.User)
        .ToListAsync();

    var model = appliedAds.Select(app => new AppliedAdvertisementsViewModel
    {
        ApplicationId = app.Id,
        AdvertisementId = app.AdvertisementId,
        AdvertisementTitle = app.Advertisement.Title,
        AdvertisementOwner = app.Advertisement.User.UserName,
        ImageUrl = app.Advertisement.ImageUrl,
        Tag = app.Advertisement.Tag,
        IsApproved = app.IsApproved, // Add this line
        IsRejected = app.IsRejected  // Add this line
    }).ToList();

    return View(model);
}
       public IActionResult Myadd()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            ViewData["UserEmail"] = userEmail;
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var advertisements = _context.Advertisements
                .Where(ad => ad.UserId == userId)
                .ToList();

            return View(advertisements);
        }
  [HttpPost]
        public async Task<IActionResult> Filter(string tag)
        {
            var advertisements = _context.Advertisements.AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                advertisements = advertisements.Where(a => a.Tag.Contains(tag));
            }

            var filteredAdvertisements = await advertisements
                .Include(a => a.City)
                .Include(a => a.User)
                .ToListAsync();

            return View("Index", filteredAdvertisements);
        }
    

    }
}
