using System;
using System.ComponentModel.DataAnnotations;

namespace BitirmeApp3.Models
{
    public class AdApplication
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? AdvertisementId { get; set; }
        public Advertisement? Advertisement { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Experience { get; set; }
        public string? Message { get; set; }
        public string? Tool { get; set; }
        public string? Tractor { get; set; }
        public bool IsApproved { get; set; } = false; // Default value
        public bool IsRejected { get; set; } = false; // Default value
        public string? Email { get; set; }
    }
}
