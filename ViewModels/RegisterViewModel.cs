using System.ComponentModel.DataAnnotations;
using BitirmeApp3.Models;

namespace BitirmeApp3.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }=null!;
        
         [Required]
        public string LastName { get; set; }=string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; }=null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }=null!;

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }=null!;

        [Required]
        [Display(Name = "City")]
        public string? SelectedCityId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string? SelectedRole { get; set; }

        public List<City>? Cities { get; set; }
        public List<string>? Roles { get; set; }
    }
}
