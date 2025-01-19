using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = "Client"; // Domyślna rola, jeśli nie zostanie podana
    }
}
