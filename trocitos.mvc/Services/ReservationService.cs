
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trocitos.mvc.Models;

namespace trocitos.mvc.Services
{
    public class ReservationService : IReservationService
    {
        private readonly TrocitosDbContext _context;

        public ReservationService(TrocitosDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> MakeReservationAsync(Reservation reservation)
        {
            var availableTables = await GetAvailableTables(reservation.ReservationDate, reservation.PartySize, reservation.Table?.Location ?? string.Empty);

            if (availableTables.Any())
            {
                reservation.TableNo = availableTables.First().TableNo;
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return reservation;
            }

            return null;
        }

        public async Task<List<Table>> GetAvailableTables(DateTime dateTime, int partySize, string location)
        {
            var reservations = await _context.Reservations
                .Where(r => r.ReservationDate.Date == dateTime.Date && r.Table.Location == location)
                .ToListAsync();

            var allTables = await _context.TableCatalogue
                .Where(t => t.Capacity >= partySize && t.Location == location)
                .ToListAsync();

            return allTables.Except(allTables.Where(t => reservations.Any(r => r.TableNo == t.TableNo))).ToList();
        }
    }
}