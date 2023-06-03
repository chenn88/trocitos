using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trocitos.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeOnly RsvStart { get; set; }
        public TimeOnly RsvEnd { get; set; }
        public bool HighChairRequired { get; set; }
        public bool Outside { get; set; }
        public bool Cancellation { get; set; }

    }


}