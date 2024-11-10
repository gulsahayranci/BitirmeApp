using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BitirmeApp3.Models
{
    public class IdentityContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdApplication> AdApplications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define relationships
            builder.Entity<AppUser>()
                .HasOne(u => u.City)
                .WithMany()
                .HasForeignKey(u => u.CityId)
                .IsRequired(false);
                
            builder.Entity<Advertisement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Advertisements)
                .HasForeignKey(a => a.UserId);

            builder.Entity<Advertisement>()
                .HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId);

            builder.Entity<AdApplication>()
                .HasOne(aa => aa.Advertisement)
                .WithMany(a => a.AdApplications)
                .HasForeignKey(aa => aa.AdvertisementId);

            builder.Entity<AdApplication>()
                .HasOne(aa => aa.User)
                .WithMany(u => u.AdApplications)
                .HasForeignKey(aa => aa.UserId);
            builder.Entity<AdApplication>()
                .Property(a => a.Email)
                .IsRequired(); 
            builder.Entity<Announcement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Announcements)
                .HasForeignKey(a => a.UserId);

            builder.Entity<AppUser>()
                .HasMany(u => u.Advertisements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(u => u.AdApplications)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(u => u.Announcements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
             builder.Entity<SiteSettings>(entity =>
            {
                entity.Property(e => e.PhoneNumber).IsRequired(false);  // Bu satır nullable olarak günceller
                entity.Property(e => e.Address).IsRequired(false);      // Bu satır nullable olarak günceller
                entity.Property(e => e.Email).IsRequired(false);        // Bu satır nullable olarak günceller
            });

        }
    }
}
