using trocitos.mvc.Models;
using trocitos.mvc.DTOs;
namespace trocitos.mvc.Services

{
    public interface IReservationService
    {

        Task<Reservation> BookReservationAsync(ReservationRequest reservationRequest);
        Task<List<Table>> CheckTableAvailabilityAsync(AvailabilityCheck availabilityRequest);
        Task<bool> CancelReservationAsync(int reservationId);

    }
}