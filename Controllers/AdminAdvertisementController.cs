using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace BitirmeApp3.Controllers
{
    public class AdminAdvertisementController : Controller
    {
        private readonly IdentityContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AdminAdvertisementController> _logger;

        public AdminAdvertisementController(IdentityContext context, UserManager<AppUser> userManager, ILogger<AdminAdvertisementController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
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
                    Alan=model.Alan,
                    Price=model.Price
                };

                _context.Advertisements.Add(advertisement);
                await _context.SaveChangesAsync();

                return RedirectToAction("Hesabım", "Admin");
            }

            model.Cities = _context.Cities.ToList();
            return View(model);
        }

        public IActionResult MyAdd()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var advertisements = _context.Advertisements
                    .Where(ad => ad.UserId == userId)
                    .ToList();

                return View(advertisements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading advertisements.");
                return View(new List<Advertisement>());
            }
        }

       
    [HttpGet]
    public async Task<IActionResult> ViewApplications(string applicationId)
    {
        _logger.LogInformation("ViewApplications called with applicationId: {ApplicationId}", applicationId);

        if (string.IsNullOrEmpty(applicationId))
        {
            _logger.LogWarning("ApplicationId is null or empty");
            return NotFound();
        }

        var application = await _context.AdApplications
            .Include(app => app.Advertisement)
            .FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application == null)
        {
            _logger.LogWarning("Application not found with id: {ApplicationId}", applicationId);
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

        _logger.LogInformation("ViewApplications view model created successfully");
        return View(viewModel);
    }

        [HttpPost]
        public async Task<IActionResult> EvaluateApplication(string applicationId, bool isAccepted)
        {
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
                application.IsRejected = false;
                application.Advertisement.Status = "Passive";
            }
            else
            {
                application.IsRejected = true;
                application.IsApproved = false;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("InApp");
        }

public async Task<IActionResult> InApp()
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

    var userAds = await _context.Advertisements.Where(ad => ad.UserId == userId).ToListAsync();

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
        IsRejected = app.IsRejected
    }).ToList();

    ViewData["ApprovedCount"] = approvedCount;
    ViewData["RejectedCount"] = rejectedCount;
    ViewData["PendingCount"] = pendingCount;

    return View(model);
}

       public async Task<IActionResult> ViewMessages()
{
    var userId = HttpContext.Session.GetString("UserId");

    if (string.IsNullOrEmpty(userId))
    {
        return RedirectToAction("Login", "Account");
    }

    var messages = await _context.Messages
        .Where(m => m.ReceiverId == userId)
        .Include(m => m.Sender)
        .Select(m => new ReceivedMessageViewModel
        {
            Id = m.Id,
            SenderUserName = m.Sender.Email,
            Content = m.Content,
            SentAt = m.SentAt
        })
        .ToListAsync();

    return View(messages);
}


    }
}
