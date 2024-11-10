using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BitirmeApp3.Controllers
{
    public class DuyuruController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IdentityContext _context;

        public DuyuruController(UserManager<AppUser> userManager, IdentityContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements.Include(a => a.User).ToListAsync();
            return View(announcements);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);

                var announcement = new Announcement
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreatedDate = DateTime.Now,
                    UserId = userId,
                    User = user
                };

                if (model.Image != null)
                {
                    var imagePath = Path.Combine("wwwroot/images", model.Image.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    announcement.ImagePath = $"/images/{model.Image.FileName}";
                }

                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
