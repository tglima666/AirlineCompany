using AirlineCompany.web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Repositories
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        IEnumerable<SelectListItem> GetComboFlights();
    }
}
