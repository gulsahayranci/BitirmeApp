using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace BitirmeApp3.Models{
    public class AppUser:IdentityUser
    {
    
    
        public string? LastName { get; set; }
        [ForeignKey("City")]
        public string? CityId { get; set; }
        public City? City { get; set; }
    
        public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
        public ICollection<AdApplication> AdApplications { get; set; } = new List<AdApplication>();
         public ICollection<Announcement> Announcements { get; set; }
      

    }
    
}
