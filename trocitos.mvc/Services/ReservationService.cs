
using Microsoft.EntityFrameworkCore;
using trocitos.mvc.DTOs;
using trocitos.mvc.Models;

namespace trocitos.mvc.Services
{
    public class ReservationService : IReservationService
    {
        private readonly TrocitosDbContext _trocitosDbContext;

        public ReservationService(TrocitosDbContext trocitosDbContext)
        {
            _trocitosDbContext = trocitosDbContext;
        }

        public async Task<List<Table>> CheckTableAvailabilityAsync(AvailabilityCheck availabilityRequest)
        {

            var availableTables = await _trocitosDbContext.TableCatalogue
                .Where(t => !t.Booked
                            && t.Capacity >= availabilityRequest.ReqPartySize
                            && ((availabilityRequest.ReqOutside && t.Location == "outside") || (!availabilityRequest.ReqOutside && t.Location != "outside")))
                .ToListAsync();

            return availableTables;
        }

        public async Task<Reservation> BookReservationAsync(ReservationRequest reservationRequest)
        {

            var tables = await CheckTableAvailabilityAsync(new AvailabilityCheck
            {
                ReqPartySize = reservationRequest.ReqPartySize,
                ReqDate = reservationRequest.ReqDate,
                ReqStartTime = reservationRequest.ReqRsvStart,
                ReqOutside = reservationRequest.ReqOutside
            });

            if (!tables.Any())
            {
                throw new Exception("No table available");
            }


            var reservation = new Reservation
            {
                FirstName = reservationRequest.FirstName,
                Surname = reservationRequest.Surname,
                PhoneNo = reservationRequest.PhoneNo,
                Email = reservationRequest.Email,
                PartySize = reservationRequest.ReqPartySize,
                ReservationDate = reservationRequest.ReqDate,
                RsvStart = reservationRequest.ReqRsvStart,
                TableNo = tables.First().TableNo
            };

            _trocitosDbContext.Reservations.Add(reservation);
            await _trocitosDbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _trocitosDbContext.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            if (reservation == null)
                return false;

            reservation.Cancellation = true;
            await _trocitosDbContext.SaveChangesAsync();
            return true;
        }
    }
}