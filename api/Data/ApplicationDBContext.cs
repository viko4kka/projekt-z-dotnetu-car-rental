using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
     public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
     {
        
     }

        //  public DbSet<User> Users { get; set; } 
        public DbSet<Car> Cars { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin", 
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Client", 
                    NormalizedName = "CLIENT"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }


    }
}

//dzieki domyslnej dodanej role, one beda dostepne od razu po uruchomieniu aplikacji 
