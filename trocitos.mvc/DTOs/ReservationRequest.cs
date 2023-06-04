using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace trocitos.mvc.DTOs
{
    public class ReservationRequest
    {

        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public int ReqPartySize { get; set; }
        public DateTime ReqDate { get; set; }
        public TimeOnly ReqRsvStart { get; set; }
        public bool ReqOutside { get; set; }

    }
}