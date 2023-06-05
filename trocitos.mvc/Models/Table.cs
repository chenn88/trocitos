using System.ComponentModel.DataAnnotations;


namespace trocitos.mvc.Models
{
    public class Table
    {
        [Key]
        public int TableNo { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }

        public List<Reservation> Reservations { get; set; }

        public Table()
        {
            Reservations = new List<Reservation>();
        }

    }
}