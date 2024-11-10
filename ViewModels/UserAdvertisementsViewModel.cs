using BitirmeApp3.Models;

namespace BitirmeApp3.ViewModels
{
    public class UserAdvertisementsViewModel
    {
        public AppUser User { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}
