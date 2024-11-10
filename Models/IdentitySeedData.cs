using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BitirmeApp3.Models;
namespace BitirmeApp3.Models
{
    public static class IdentitySeedData{
        private const string adminUser="Admin";
        private const string adminPassword="Admin_123";
        public static async void IdentityTestUser(IApplicationBuilder app){
            var context=app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();
            if(context.Database.GetAppliedMigrations().Any()){
                context.Database.Migrate();
            }
            var userManager=app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(adminUser);
            if(user==null){
                user=new AppUser{
                    UserName=adminUser,
                    Email="admin@gulsah.com",
                    PhoneNumber="44444444",
                };
                await userManager.CreateAsync(user,adminPassword);
            }

 
        }
    }
}