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
    }
}
