using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using trocitos.mvc.Models;

namespace trocitos.mvc.Controllers
{
    public class ReservationController : Controller
    {

        private readonly TrocitosDbContext _context;

        public ReservationController(TrocitosDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cancellation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckAvailability(string date, string startTime, int capacity, string location)
        {
            try
            {
                DateTime dateObj;
                TimeOnly startTimeObj;

                try
                {
                    dateObj = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    startTimeObj = TimeOnly.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    return StatusCode(400, "Invalid date or time format. Expected formats are date: yyyy-MM-dd, time: HH:mm:ss.");
                }

                TimeOnly endTimeObj = startTimeObj.Add(TimeSpan.FromHours(2.5));

                var availableTables = _context.TableCatalogue
                    .Where(t => t.Capacity >= capacity && (t.Location == null || t.Location.ToLower() == location.ToLower()))
                    .Select(t => t.TableNo)
                    .ToList();

                if (!availableTables.Any())
                {
                    return Json(new { success = false, message = "No tables available for the given capacity and location." });
                }

                var overlappingReservation = _context.Reservations
                    .Where(r => r.ReservationDate.Date == dateObj.Date && availableTables.Contains(r.TableNo)
                                && ((r.RsvStart < endTimeObj) && (r.RsvEnd > startTimeObj)) && r.Cancellation == false)
                    .Select(r => r.TableNo)
                    .ToList();


                availableTables = availableTables.Except(overlappingReservation).ToList();

                if (!availableTables.Any())
                {
                    return Json(new { success = false, message = "There is a reservation already in the requested time slot." });
                }

                return Json(new { success = true, message = "Table is available." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPost]
        public IActionResult Book(string firstName, string surname, string phoneNo, string email, DateTime date, TimeSpan? startTime, TimeSpan? endTime, int capacity, string location)
        {
            if (startTime == null)
            {
                return Json(new { success = false, message = "Please provide valid start and end times." });
            }

            TimeOnly rsvStart = TimeOnly.FromTimeSpan(startTime.Value);
            TimeOnly rsvEnd = rsvStart.Add(TimeSpan.FromHours(2.5));

            var availableTables = _context.TableCatalogue
                .Where(t => t.Capacity >= capacity && (t.Location == null || t.Location.ToLower() == location.ToLower()))
                .OrderBy(t => Math.Abs(t.Capacity - capacity))
                .Select(t => t.TableNo)
                .ToList();

            if (!availableTables.Any())
            {
                return Json(new { success = false, message = "No tables available for the given capacity and location." });
            }

            var overlappingReservations = _context.Reservations
                .Where(r => r.ReservationDate == date && availableTables.Contains(r.TableNo) && ((r.RsvStart < rsvEnd) && (r.RsvEnd > rsvStart)) && r.Cancellation == false)
                .Select(r => r.TableNo)
                .ToList();

            if (overlappingReservations.Any())
            {
                availableTables = availableTables.Except(overlappingReservations).ToList();

                if (!availableTables.Any())
                {
                    return Json(new { success = false, message = "There is a reservation already in the requested time slot." });
                }
            }
            int tableToBook = availableTables.First();

            var newReservation = new Reservation
            {
                FirstName = firstName,
                Surname = surname,
                PhoneNo = phoneNo,
                Email = email,
                ReservationDate = date,
                RsvStart = rsvStart,
                RsvEnd = rsvEnd,
                PartySize = capacity,
                TableNo = tableToBook,
                Cancellation = false,
                Table = _context.TableCatalogue.FirstOrDefault(t => t.TableNo == tableToBook)
            };

            _context.Reservations.Add(newReservation);
            _context.SaveChanges();

            return Json(new { success = true, message = "Reservation has been successfully booked." });
        }

        [HttpGet]
        public IActionResult ReservationExists(int reservationId, string contactInfo)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId && (r.PhoneNo == contactInfo || r.Email == contactInfo));

            if (reservation != null)
            {
                return Json(new { exists = true });
            }
            else
            {
                return Json(new { exists = false });
            }
        }


        [HttpPut]
        public IActionResult CancelReservation(int reservationId, string contactInfo)
        {
            try
            {
                var reservationToCancel = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId && (r.PhoneNo == contactInfo || r.Email == contactInfo));

                if (reservationToCancel == null)
                {
                    return Json(new { success = false, message = "No reservation found with provided Reservation ID and contact information." });
                }

                if (reservationToCancel.Cancellation)
                {
                    return Json(new { success = false, message = "This reservation has already been cancelled." });
                }

                reservationToCancel.Cancellation = true;

                _context.Reservations.Update(reservationToCancel);
                _context.SaveChanges();

                return Json(new { success = true, message = "Reservation has been successfully cancelled." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }
    }

}
