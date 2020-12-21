using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Entities
{
    public class Flight
    {
        public int ID { get; set; }

        [Display(Name = "Flight Number")]
        public int FlightNumber { get; set; }

        [Display(Name="Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Hour")]
        public DateTime Hour { get; set; }
    }
}
