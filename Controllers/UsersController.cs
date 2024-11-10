using BitirmeApp3.Models;
using BitirmeApp3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BitirmeApp3.Controllers{
    public class UsersController:Controller{
        private UserManager<AppUser> _userManager;
         private RoleManager<AppRole> _roleManager;
        private IdentityContext _context;
        public UsersController(UserManager<AppUser> userManager,IdentityContext context,RoleManager<AppRole> roleManager)
        {
            _userManager=userManager;
            _context=context;
            _roleManager=roleManager;
            
        }
        public IActionResult Index(){
            var users = _userManager.Users.Include(u => u.City);
            return View(users);
        }
          public IActionResult Create(){
                var viewModel = new CreateViewModel
                    {
                        Cities = _context.Cities.ToList()
                    };
                    return View(viewModel);
        }
        [HttpPost]
        
            public async Task<IActionResult> Create(CreateViewModel model){
                if(ModelState.IsValid){
                    var user= new AppUser {UserName=model.UserName,Email=model.Email,LastName=model.LastName, CityId = model.SelectedCityId};
                    IdentityResult result=  await _userManager.CreateAsync(user, model.Password);
                    if(result.Succeeded){
                        return RedirectToAction("Index");
                    }
                    foreach(IdentityError err in result.Errors ){
                        ModelState.AddModelError(" ",err.Description);
                    }
                }
            model.Cities = _context.Cities.ToList();
                return View(model);
            }
        public async Task<IActionResult> Edit(string id)
        {
            if(id==null){
                return RedirectToAction("Index");
            }
            var user= await _userManager.FindByIdAsync(id);
            if(user!=null){
                ViewBag.Roles=await _roleManager.Roles.Select(i=>i.Name).ToListAsync();
                return View(new EditViewModel{
                    Id=user.Id,
                    UserName=user.UserName,
                    LastName=user.LastName,
                    Email=user.Email,
                    SelectedRoles=await _userManager.GetRolesAsync(user)
                });
            }
           
        return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditViewModel model){
            if(id!=model.Id){
                return RedirectToAction("Index");

            }
            if(ModelState.IsValid){
                var user= await _userManager.FindByIdAsync(model.Id);
                if(user!=null){
                    user.Email=model.Email;
                    user.LastName=model.LastName;
                    user.UserName=model.UserName;
                    var result= await _userManager.UpdateAsync(user);
                    if(result.Succeeded && !string.IsNullOrEmpty(model.Password)){
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }
                    if(result.Succeeded){
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                        if(model.SelectedRoles != null){
                            await _userManager.AddToRolesAsync(user,model.SelectedRoles);
                        }
                        return RedirectToAction("Index");
                    }
                    foreach(IdentityError err in result.Errors){
                        ModelState.AddModelError("",err.Description);
                    }
                }

            }
            return View(model);

        }
         [HttpPost]
        public async Task<IActionResult> Delete(string id){
            var user= await _userManager.FindByIdAsync(id);
            if(user!=null){
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");

        }
    }
}