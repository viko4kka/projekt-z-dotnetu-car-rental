using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Car;
// using api.Migrations;
using api.Models;


namespace api.Mapper
{
    public static class CarMapper
    {
        public static CarDto ToCarDto(this Car carModel)
        {
            return new CarDto
            {
                CarId = carModel.CarId,
                Brand = carModel.Brand,
                Model = carModel.Model,
                Year = carModel.Year,
                BodyType = carModel.BodyType,
                Seats = carModel.Seats,
                FuelType = carModel.FuelType,
                Color = carModel.Color,
                PricePerDay = carModel.PricePerDay,
                Status = carModel.Status,
                ImageUrl = carModel.ImageUrl
            };

        }

        public static Car ToCarFromCreateDto(this CreateCarRequestDto carDto)
        {
            return new Car
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                Year = carDto.Year,
                BodyType = carDto.BodyType,
                Seats = carDto.Seats,
                FuelType = carDto.FuelType,
                Color = carDto.Color,
                PricePerDay = carDto.PricePerDay,
                Status = carDto.Status,
                ImageUrl = carDto.ImageUrl

            };
        }
    }
}