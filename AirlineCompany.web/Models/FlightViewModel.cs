using AirlineCompany.web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Models
{
    public class FlightViewModel : Flight
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
