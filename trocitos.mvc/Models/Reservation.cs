namespace trocitos.mvc.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public int PartySize { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeOnly RsvStart { get; set; }
        public TimeOnly RsvEnd { get; set; }
        public int TableNo { get; set; }
        public bool Cancellation { get; set; }
        public Table? Table { get; set; }

    }

}