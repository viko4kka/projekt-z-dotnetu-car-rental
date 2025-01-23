using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Car
    {
        public int CarId{ get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year {get; set;}
        public string? BodyType { get; set; }
        public int Seats { get; set; }
        public string? FuelType { get; set; }
        public string? Color { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerDay { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }

    }
}