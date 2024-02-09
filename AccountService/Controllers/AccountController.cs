using AccountService.Dtos;
using AccountService.Entities;
//using AccountService.Entities;
//using IdentityConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AccountService.Controllers
{
    
   
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            _roleManager = roleManager;
            this.config = config;
        }
      
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginDto { ReturnUrl = returnUrl });
        }
      
        
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            var user1 = await userManager.FindByEmailAsync(loginDto.Email);
            var userRole = await userManager.GetRolesAsync(user1);
            if(result.Succeeded)
            return Redirect(loginDto.ReturnUrl);
            else
            {
                return View();
            }
         
        }
     
    }
}
