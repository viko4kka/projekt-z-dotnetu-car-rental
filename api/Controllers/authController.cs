using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.User;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager; 
        private readonly SignInManager<User> _signInManager;
        
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
      
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try{

                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }

                //sprawdzene czy przeslana rola jest prawidlowa
                var validRoles = new List<string> {"Admin", "Client"};
                if(!validRoles.Contains(registerDto.Role))
                {
                    return BadRequest("Invalid role");
                }

                var user = new User
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if(createdUser.Succeeded){

                    var roleResult = await _userManager.AddToRoleAsync(user, "Client");
                    
                    if(roleResult.Succeeded){
                        return Ok("User created successfully");
                    } 
                    else 
                    {
                        return StatusCode(500, "Failed to create user");
                    }

                } 
                else{
                    return StatusCode(500,"Failed to create user");
                }

            } catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
       
    }
}

//UserManager class is used to manage user information