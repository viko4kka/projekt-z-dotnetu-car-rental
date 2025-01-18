using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models;

public enum Role {

    Admin, 
    Client
}
    public class User : IdentityUser
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public Role role { get; set; }
    }
