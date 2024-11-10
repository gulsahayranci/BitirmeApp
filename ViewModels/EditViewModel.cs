using System.ComponentModel.DataAnnotations;
using BitirmeApp3.Models;
namespace BitirmeApp3.ViewModels
{
    public class EditViewModel{
       
         public string? Id { get; set; }
       
        public string? UserName { get; set; }
       
        public string? LastName { get; set; }
        
         [EmailAddress]
        public string?  Email { get; set; }

      
         [DataType(DataType.Password)]
        public string? Password { get; set; }

      
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Parola Eşleşmiyor!")]
        public string? ConfirmPassword { get; set; }
        public IList<string>? SelectedRoles {get; set;}
     
    }
    
}