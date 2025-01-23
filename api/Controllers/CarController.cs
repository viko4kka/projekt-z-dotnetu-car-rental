using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Mapper;
using api.Dtos.Car;


using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllCars(){
            var cars = _context.Cars.ToList()
            .Select( s=> s.ToCarDto());

            return Ok(cars);
        }

        [HttpGet("{id}")]
        public IActionResult GetCarById([FromRoute] int id )
        {
            var car = _context.Cars.Find(id);

            if(car == null)
            {
                return NotFound();
            }

            return Ok(car.ToCarDto());
        }

        [HttpPost]
        public IActionResult CreateCar([FromBody] CreateCarRequestDto carDto)
        {
            var carModel = carDto.ToCarFromCreateDto();

            _context.Cars.Add(carModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCarById), new {id = carModel.CarId}, carModel.ToCarDto());
        }

        [HttpPut]
        [Route("{id}")]

        public IActionResult UpdateCar([FromRoute] int id, [FromBody] UpdateCarRequestDto updateCarDto)
        {
            var carModel = _context.Cars.FirstOrDefault( x=> x.CarId == id);

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
            carModel.Status = updateCarDto.Status;
            carModel.ImageUrl = updateCarDto.ImageUrl;

            _context.SaveChanges();

            return Ok(carModel.ToCarDto());

        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteCar([FromRoute] int id)
        {
            var carModel = _context.Cars.FirstOrDefault( x=> x.CarId == id);

            if(carModel == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(carModel);
            _context.SaveChanges();

            return NoContent();

          
        }

    }
}