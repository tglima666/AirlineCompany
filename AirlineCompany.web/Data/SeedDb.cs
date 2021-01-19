using AirlineCompany.web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if(!_context.Flights.Any())
            {
                this.AddFlight("FirstFlight");
                await _context.SaveChangesAsync();
            }
        }

        private void AddFlight(string name)
        {
            _context.Flights.Add(new Flight
            {
                FlightNumber = name                
            });
        }
    }
}
