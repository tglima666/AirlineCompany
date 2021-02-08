using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Flight")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a flight.")]
        public int FlightID { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public double Quantity { get; set; }

        public IEnumerable<SelectListItem> Flights { get; set; }
    }
}
