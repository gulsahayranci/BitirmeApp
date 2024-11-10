using System.ComponentModel.DataAnnotations;

namespace BitirmeApp3.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Lütfen isminizi giriniz.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen konuyu giriniz.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Lütfen mesajınızı giriniz.")]
        public string Message { get; set; }
    }
}
