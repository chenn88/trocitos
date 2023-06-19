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

        //Creating view for reservations index page
        public IActionResult Index()
        {
            return View();
        }

        //Creating view for cancellation page
        public IActionResult Cancellation()
        {
            return View();
        }

        // GET method for checking table availability
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

                //Creating variable to store available tables.
                // Available table will be stored if requested capacity and location are available

                var availableTables = _context.TableCatalogue
                    .Where(t => t.Capacity >= capacity && (t.Location == null || t.Location.ToLower() == location.ToLower()))
                    .Select(t => t.TableNo)
                    .ToList();


                if (!availableTables.Any())
                {
                    return Json(new { success = false, message = "No tables available for the given capacity and location." });
                }

                //Checking if there are any over-lapping reservations slots for the available tables. If the database returns an over-lapping reservation the table number won't be included

                var overlappingReservation = _context.Reservations
                    .Where(r => r.ReservationDate.Date == dateObj.Date && availableTables.Contains(r.TableNo)
                                && ((r.RsvStart < endTimeObj) && (r.RsvEnd > startTimeObj)) && r.Cancellation == false)
                    .Select(r => r.TableNo)
                    .ToList();


                availableTables = availableTables.Except(overlappingReservation).ToList();

                //Message to advise there is a conflicting reservation

                if (!availableTables.Any())
                {
                    return Json(new { success = false, message = "There is a reservation already in the requested time slot. Please try again" });
                }

                //Message to advise there is a table available

                return Json(new { success = true, message = "A table is available! Please fill in details below to reserve." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        // POST method for completing a reservation

        [HttpPost]
        public IActionResult Book(string firstName, string surname, string phoneNo, string email, DateTime date, TimeSpan? startTime, TimeSpan? endTime, int capacity, string location)
        {
            if (startTime == null)
            {
                return Json(new { success = false, message = "Please provide valid start and end times." });
            }

            //setting reservation start time
            TimeOnly rsvStart = TimeOnly.FromTimeSpan(startTime.Value);

            //setting reservation end time to 2.5 hours after start time

            TimeOnly rsvEnd = rsvStart.Add(TimeSpan.FromHours(2.5));

            // second check to see if table is still available - this is to address cases where the checked availability has been booked by someone else prior to submitting the reservation

            var availableTables = _context.TableCatalogue
                .Where(t => t.Capacity >= capacity && (t.Location == null || t.Location.ToLower() == location.ToLower()))
                .OrderBy(t => Math.Abs(t.Capacity - capacity))
                .Select(t => t.TableNo)
                .ToList();

            if (!availableTables.Any())
            {
                return Json(new { success = false, message = "No tables available for the given capacity and location." });
            }

            //second check for overlapping reservations

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

            //selecting the first available table from the list of available tables

            int tableToBook = availableTables.First();

            //creating the reservation 
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

            //adding to DB context and saving

            _context.Reservations.Add(newReservation);
            _context.SaveChanges();


            //creating object to show reservation details

            return Json(new
            {
                success = true,
                message = "Reservation has been successfully booked:",
                reservationId = newReservation.ReservationId,
                bookingDate = date.ToString("yyyy-MM-dd"),
                startTime = rsvStart.ToString(),
                partySize = capacity
            });
        }

        // GET method to check if the reservation exists (for cancellation form)
        [HttpGet]
        public IActionResult ReservationExists(int reservationId, string contactInfo)
        {
            //if reservation id exits in DB and contact information (either phone number OR email) matches, confirm reservation exists - otherwise reservation is "not found"
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

        // PUT method to change cancellation boolean to true
        [HttpPut]
        public IActionResult CancelReservation(int reservationId, string contactInfo)
        {
            try
            {
                //checking for matching reservation id and either phone number or email

                var reservationToCancel = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId && (r.PhoneNo == contactInfo || r.Email == contactInfo));

                if (reservationToCancel == null)
                {
                    return Json(new { success = false, message = "No reservation found with provided Reservation ID and contact information." });
                }

                if (reservationToCancel.Cancellation)
                {
                    return Json(new { success = false, message = "This reservation has already been cancelled." });
                }

                //setting the Cancellation boolean to true
                reservationToCancel.Cancellation = true;

                //updating and saving in DB context

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
