using Microsoft.AspNetCore.Mvc;
using trocitos.mvc.DTOs;
using trocitos.mvc.Services;


[Route("[controller]")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost("book")]
    public async Task<IActionResult> BookReservationAsync(ReservationRequest reservationRequest)
    {
        var result = await _reservationService.BookReservationAsync(reservationRequest);
        if (result == null)
            return View("Error", new ErrorViewModel { Message = "Failed to book the reservation." });
        return View("Confirmation", result);
    }

    [HttpPost("availability")]
    public async Task<IActionResult> CheckTableAvailabilityAsync(AvailabilityCheck availabilityCheck)
    {
        var result = await _reservationService.CheckTableAvailabilityAsync(availabilityCheck);
        if (!result.Any())
            return View("Error", new ErrorViewModel { Message = "No tables available." });
        return View("AvailableTables", result);
    }

    [HttpPost("cancel/{reservationId}")]
    public async Task<IActionResult> CancelReservationAsync(int reservationId)
    {
        var result = await _reservationService.CancelReservationAsync(reservationId);
        if (!result)
            return View("Error", new ErrorViewModel { Message = "Reservation not found." });
        return View("Confirmation", new { Message = "Reservation cancelled successfully." });
    }
}