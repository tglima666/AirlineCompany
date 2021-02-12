using AirlineCompany.web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        private readonly DataContext _context;

        public FlightRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        //Método para ir á tabela buscar todos os produtos
        //e colocar numa SelectListItem
        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = _context.Flights.Select(p => new SelectListItem
            {
                Text = p.FlightNumber,
                Value = p.ID.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a flight...)",
                Value = "0"
            });

            return list;
        }
    }
}
