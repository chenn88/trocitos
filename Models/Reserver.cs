using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace trocitos.Models
{
    public class Reserver
    {

        [Key]
        public int ReserverId { get; set; }
        public int ReservationId { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }

    }
}