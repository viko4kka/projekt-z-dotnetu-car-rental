using System.ComponentModel.DataAnnotations.Schema;
using api.Models;

public class Car
{
    public int CarId { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public string? BodyType { get; set; }
    public int Seats { get; set; }
    public string? FuelType { get; set; }
    public string? Color { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PricePerDay { get; set; }

    public string CarStatus { get; set; } = "Available"; // domyślnie available

    [NotMapped] 
    public string Status
    {
        get
        {
            
            return Reservations != null && Reservations.Any(r =>
                r.StartDate <= DateTime.Now && r.EndDate >= DateTime.Now
            ) ? "Reserved" : CarStatus; 
        }
    }

    public string? ImageUrl { get; set; }

    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
}
