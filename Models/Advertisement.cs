using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BitirmeApp3.Models
{
    public class Advertisement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";

        public string? UserId { get; set; }

        public string? CityId { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public int?  Price { get; set; }
        public string? Alan { get; set; }

        // Form data for applicants
       
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }

        public ICollection<AdApplication> AdApplications { get; set; } = new List<AdApplication>();
         public string ImageUrl { get; set; } = string.Empty;
    }
}
