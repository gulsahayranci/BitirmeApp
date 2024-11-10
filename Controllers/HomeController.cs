using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BitirmeApp3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IdentityContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IdentityContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

     public async Task<IActionResult> Index()
{
    var userEmail = HttpContext.Session.GetString("UserEmail");
    ViewData["UserEmail"] = userEmail;

    var announcements = await _context.Announcements
        .OrderByDescending(a => a.CreatedDate)
        .ToListAsync();

    var advertisements = await _context.Advertisements
        .Where(ad => ad.Status != "Passive")
        .Include(a => a.City)
        .Include(a => a.User)
        .OrderByDescending(a => a.Tag)
        .ToListAsync();

    var viewModel = new HomeViewModel
    {
        Announcements = announcements,
        Advertisements = advertisements
    };

    return View(viewModel);
}


        public IActionResult Hesabım()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            ViewData["UserEmail"] = userEmail;
            return View();
        }

  public async Task<IActionResult> Advertisement(string searchTag)
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        ViewData["UserEmail"] = userEmail;
        ViewData["SearchTag"] = searchTag;

        var advertisements = _context.Advertisements
            .Where(ad => ad.Status != "Passive")
            .Include(a => a.City)
            .Include(a => a.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTag))
        {
            advertisements = advertisements.Where(ad => ad.Tag.Contains(searchTag));
        }

        var adList = await advertisements.OrderByDescending(a => a.Tag).ToListAsync();
        return View(adList);
    }

        public async Task<IActionResult> Mudurluk()
        {
            var provinceDirectorates = await _userManager.GetUsersInRoleAsync("il Müdürlüğü");
            return View(provinceDirectorates);
        }
       public async Task<IActionResult> Yatırımcı()
{
    var investors = await _userManager.GetUsersInRoleAsync("Yatırımcı");
    return View(investors);
}
        
        public async Task<IActionResult> ViewYatırımcı(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var advertisements = await _context.Advertisements
                .Where(ad => ad.UserId == user.Id)
                .ToListAsync();

            var viewModel = new UserAdvertisementsViewModel
            {
                User = user,
                Advertisements = advertisements
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ViewUser(string id)
        {
             
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var advertisements = await _context.Advertisements
                .Where(ad => ad.UserId == user.Id)
                .ToListAsync();

            var viewModel = new UserAdvertisementsViewModel
            {
                User = user,
                Advertisements = advertisements
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string message)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(receiverId))
            {
                return BadRequest();
            }

            var senderId = _userManager.GetUserId(User);
            if (senderId == null)
            {
                  TempData["MessageSent"] = "Giriş Yapınız! Ve mesaj göndermeyi tekrar deneyiniz.";
                 return RedirectToAction("ViewUser", new { id = receiverId });
            }

            var newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.Now
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
             TempData["MessageSent"] = "Mesajınız başarıyla gönderildi!";

            return RedirectToAction("ViewUser", new { id = receiverId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Hakkımızda(){
            return View();
        }
        public IActionResult Iletisim()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Iletisim(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Save the message to the database
            var newContactMessage = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                SentAt = DateTime.Now
            };

            _context.ContactMessages.Add(newContactMessage);
            await _context.SaveChangesAsync();

            TempData["MessageSent"] = "Mesajınız başarıyla gönderildi!";
            return RedirectToAction("Iletisim");
        }

    }
}
