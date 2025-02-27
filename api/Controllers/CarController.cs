using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Mapper;
using api.Dtos.Car;


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase  //dzieki temu potem mozna latwiej dodawac atrybuty
    {
        private readonly ApplicationDBContext _context;
        public CarController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
       
        public async Task<IActionResult> GetAllCars(){
            var cars = await _context.Cars.ToListAsync();
            var carDto = cars.Select( s=> s.ToCarDto()).ToList();
            return Ok(carDto);
        }

        //metoda dostępna dla admina i klienta(widok szczegolow auta)
        // [Authorize(Roles = "Admin,Client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById([FromRoute] int id )
        {
            var car = await _context.Cars.FindAsync(id);

            if(car == null)
            {
                return NotFound();
            }

            return Ok(car.ToCarDto());
        }

        //metoda dostępna tylko dla admina
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarRequestDto carDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carModel = carDto.ToCarFromCreateDto();
            await  _context.Cars.AddAsync(carModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarById), new {id = carModel.CarId}, carModel.ToCarDto());
        }

        //metoda dostępna tylko dla admina
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]

        public async Task<IActionResult> UpdateCar([FromRoute] int id, [FromBody] UpdateCarRequestDto updateCarDto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carModel = await _context.Cars.FirstOrDefaultAsync( x => x.CarId == id);

            if(carModel == null)
            {
                return NotFound();
            }

            carModel.Brand = updateCarDto.Brand;
            carModel.Model = updateCarDto.Model;
            carModel.Year = updateCarDto.Year;
            carModel.BodyType = updateCarDto.BodyType;
            carModel.Seats = updateCarDto.Seats;
            carModel.FuelType = updateCarDto.FuelType;
            carModel.Color = updateCarDto.Color;
            carModel.PricePerDay = updateCarDto.PricePerDay;
            // carModel.Status = updateCarDto.Status;
            carModel.ImageUrl = updateCarDto.ImageUrl;

            await _context.SaveChangesAsync();

            return Ok(carModel.ToCarDto());

        }

        //metoda dostępna tylko dla admina

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCar([FromRoute] int id)
        {
            var carModel = await _context.Cars.FirstOrDefaultAsync( x=> x.CarId == id);

            if(carModel == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(carModel);
            await _context.SaveChangesAsync();

            return NoContent();

          
        }

    }
}