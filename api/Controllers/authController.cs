using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.User;
using api.Interface;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{

    [Route("api/user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager; 
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
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
                if(!validRoles.Contains(registerDto.Role, StringComparer.OrdinalIgnoreCase))
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
                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if(user == null){
                return Unauthorized("Invalid email");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded) return Unauthorized("Invalid password");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            try{
                var user = await _userManager.Users.FirstOrDefaultAsync( x => x.Id == id);

                if(user == null)
                {
                    return NotFound("User not found");
                }

                var userDto = new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                };

                return Ok(userDto);
            }
            catch(Exception ex){
                return StatusCode(500, ex.Message);
            }
    
        }


    }
}

