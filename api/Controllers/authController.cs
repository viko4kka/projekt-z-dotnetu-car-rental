using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
                    Email = registerDto.Email,
                    Role = registerDto.Role
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if(createdUser.Succeeded){

                    var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role);


                    
                    if(roleResult.Succeeded){

                        user.Role = registerDto.Role;

                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Role = user.Role,
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

            var role = await _userManager.GetRolesAsync(user);
            var userRole = role.FirstOrDefault() ?? "Client";

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

       
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

            Console.WriteLine($"token: {token}");
 
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token not found");
            }

            try
            {

                
                var handler = new JwtSecurityTokenHandler();

                Console.WriteLine($"handelr{handler}");

                var jwtToken = handler.ReadJwtToken(token);

                Console.WriteLine($"jwt{jwtToken}");

                
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                  
        
            Console.WriteLine($"userid{userId}");
        

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Invalid token");
                }

                
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return Unauthorized("Invalid token");
                }

                
                return Ok(new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    Token = _tokenService.CreateToken(user)
                });
            }
            catch (Exception)
            {
                return Unauthorized("Invalid token");
            }
        }

       

    }
}

