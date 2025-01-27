using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Car
{
    public class UpdateCarRequestDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? BodyType { get; set; }
        public int Seats { get; set; }
        public string? FuelType { get; set; }
        public string? Color { get; set; }
        public decimal PricePerDay { get; set; }
        // public string? Status { get; set; }
        public string? ImageUrl { get; set; }
    }
}