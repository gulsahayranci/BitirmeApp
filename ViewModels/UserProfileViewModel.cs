using System.Collections.Generic;

namespace BitirmeApp3.ViewModels
{
    public class UserProfileViewModel
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
