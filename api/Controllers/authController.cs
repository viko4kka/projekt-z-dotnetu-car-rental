using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/user")]
    [ApiController]
    public class authController : ControllerBase
    {

        private readonly ApplicationDBContext _context;
        public authController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("user")]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUserById([FromRoute] int id){
            var user = _context.Users.Find(id);

            if(user == null){
                return NotFound();
            }

            return Ok(user);
        }
    }
}