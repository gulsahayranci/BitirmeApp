using BitirmeApp3.Models;
namespace BitirmeApp3.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Announcement> Announcements { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}


