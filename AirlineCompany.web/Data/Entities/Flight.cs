using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Entities
{
    public class Flight : IEntity
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain a maximum {1} characters")]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Display(Name="Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Hour")]
        public DateTime Hour { get; set; }

        public User User { get; set; }
    }
}
