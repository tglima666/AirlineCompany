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

        [Display(Name = "Image")]
        public string ImageURL { get; set; }        

        [Display(Name="Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Hour")]
        public DateTime Hour { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; }

        public User User { get; set; }
    }
}
