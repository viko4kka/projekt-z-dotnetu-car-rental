using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Reservation;
using api.Models;

namespace api.Mapper
{
    public static class ReservationMapper
    {
        public static ReservationDto ToReservationDto(this Reservation reservation)
        {
            return new ReservationDto
            {
                ReservationId = reservation.ReservationId,
                Id = reservation.Id,
                CarId = reservation.CarId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                TotalPrice = reservation.TotalPrice
            };
        }

        public static Reservation ToReservationFromCreateDto(this CreateReservationRequestDto reservationDto)
        {
            return new Reservation
            {
                Id = reservationDto.Id,
                CarId = reservationDto.CarId,
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                TotalPrice = reservationDto.TotalPrice
            };
        }
    }
}