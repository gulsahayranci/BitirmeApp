using System.ComponentModel.DataAnnotations;
using BitirmeApp3.Models;
namespace BitirmeApp3.ViewModels
{
    public class CreateViewModel{
       
        [Required]
        public string UserName { get; set; }=string.Empty;
        [Required]
        public string LastName { get; set; }=string.Empty;
        
         [Required]
         [EmailAddress]
        public string  Email { get; set; }=string.Empty;

         [Required]
         [DataType(DataType.Password)]
        public string Password { get; set; }=string.Empty;

         [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage="Parola Eşleşmiyor!")]
        public string ConfirmPassword { get; set; }=string.Empty;
       public string SelectedCityId { get; set; }=string.Empty;
        public List<City>? Cities { get; set; }
    }
    
}