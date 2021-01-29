using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Entities
{
    public class Country : IEntity
    {
        public int ID { get ; set ; }

        public string Name { get; set; }
    }
}
