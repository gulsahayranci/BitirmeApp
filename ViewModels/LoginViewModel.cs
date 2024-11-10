using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BitirmeApp3.ViewModels{
    public class LoginViewModel{
        public string Email { get; set; }=null!;
        [DataType(DataType.Password)]
        public string Password { get; set; }=null!;
        public bool RememberMe { get; set; }=true;
    }
}