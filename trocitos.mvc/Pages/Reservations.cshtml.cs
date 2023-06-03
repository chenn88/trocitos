using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace trocitos.Pages;

public class ReservationsModel : PageModel
{
    private readonly ILogger<ReservationsModel> _logger;

    public ReservationsModel(ILogger<ReservationsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}