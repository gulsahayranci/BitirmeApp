using System.ComponentModel.DataAnnotations;

namespace BitirmeApp3.ViewModels
{
    public class AdApplicationViewModel
    {
        [Required]
        public string? AdvertisementId { get; set; }
        [Required]
        public string? UserId { get; set; }
         public string? ApplicantName { get; set; }
   
        public string? City { get; set; }
       
        // Application form data
        [Required]
        public string Name { get; set; } = string.Empty;
      
        public string? LastName { get; set; } 
        [Required]
               
        public string Email { get; set; } = string.Empty;
         [Required]
        public int Age { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Experience { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        public string Tool { get; set; } = string.Empty;
        public string Tractor { get; set; } = string.Empty;
    }
}
