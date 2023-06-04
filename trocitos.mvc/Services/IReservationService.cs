using trocitos.mvc.Models;
namespace trocitos.mvc.Services

{
    public interface IReservationService
    {

        Task<Reservation?> MakeReservationAsync(Reservation reservation);
        Task<List<Table>> GetAvailableTables(DateTime dateTime, int partySize, string location);

    }
}