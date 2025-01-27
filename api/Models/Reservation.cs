using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [ForeignKey("User")]
        public string? Id { get; set; }
        public User? User { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car? Car { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}