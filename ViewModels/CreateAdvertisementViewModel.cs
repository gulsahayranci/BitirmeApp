using System.ComponentModel.DataAnnotations;
using BitirmeApp3.Models;
namespace BitirmeApp3.ViewModels
{
    public class CreateAdvertisementViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        [Required]
        public string? SelectedCityId { get; set; }
        
        public int? Price { get; set; }

      
        public string? Alan { get; set; }

        // Form data
        public List<City>? Cities { get; set; }
         public IFormFile? Image { get; set; }  // Property for image upload
    }
}
