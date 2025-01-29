using System;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Reservation;
using api.Mapper;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ReservationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDto reservationDto)
        {
            
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == reservationDto.CarId);

           
            if (car == null || car.Status != "Available")
                return BadRequest("Car is not available for reservation.");

            
            var overlappingReservation = await _context.Reservations
                .Where(r => r.CarId == reservationDto.CarId && 
                            ((reservationDto.StartDate >= r.StartDate && reservationDto.StartDate <= r.EndDate) ||
                            (reservationDto.EndDate >= r.StartDate && reservationDto.EndDate <= r.EndDate)))
                .FirstOrDefaultAsync();

            if (overlappingReservation != null)
                return BadRequest("Car is already reserved during this period.");

          
            var totalDays = (reservationDto.EndDate - reservationDto.StartDate).Days;
            if (totalDays <= 0)
                return BadRequest("Invalid reservation dates.");

            
            var totalPrice = totalDays * car.PricePerDay;

            
            var reservation = new Reservation
            {
                Id = reservationDto.Id,
                CarId = reservationDto.CarId,
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                TotalPrice = totalPrice 
            };

           
            _context.Reservations.Add(reservation);

           
            car.CarStatus = "Reserved"; 
            await _context.SaveChangesAsync();

            
            return Ok(reservation.ToReservationDto());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            
            if (reservation == null)
                return NotFound();

            
            return Ok(reservation.ToReservationDto());
        }

       [HttpPost("{reservationId}/return")]
        public async Task<IActionResult> ReturnCar(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null)
                return NotFound("Reservation not found.");

            // Zmieniamy czas zwrotu na UTC
            reservation.EndDate = DateTime.UtcNow;

            // Zmieniamy status samochodu na "Available"
            reservation.Car.CarStatus = "Available";

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Car returned successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while updating the reservation: " + ex.Message);
            }
        }

    }
}
