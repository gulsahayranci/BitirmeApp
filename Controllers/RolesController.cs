using BitirmeApp3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeApp3.Controllers{
    public class RolesController:Controller{
        private RoleManager<AppRole> _roleManager;
        public RolesController(RoleManager<AppRole> roleManager)
        {
            _roleManager=roleManager;
        }
        public IActionResult Create(){
            return View();
        }
         public IActionResult Index(){
            return View(_roleManager.Roles);
        }
        
        [HttpPost]
         public async Task<IActionResult> Create(AppRole model){
            if(ModelState.IsValid){
                var result= await _roleManager.CreateAsync(model);
                if(result.Succeeded){
                    return RedirectToAction("Index");
                }
                foreach(var err in result.Errors ){
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);

        }
        public async Task<IActionResult> Edit(string id){
            var role=await _roleManager.FindByIdAsync(id);
            if(role!=null){
                return View(role);
            }
            return RedirectToAction("Index");

        }

    }
}