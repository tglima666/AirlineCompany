using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Entities
{
    public class OrderDetailTemp : IEntity
    {
        public int ID { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Flight Flight {get; set;}

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        public decimal Value { get { return this.Price * (decimal)this.Quantity; } }
    }
}
