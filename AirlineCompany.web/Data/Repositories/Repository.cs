using AirlineCompany.web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        //Método que vai buscar os voos todos
        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights.OrderBy(p => p.FlightNumber);
        }

        //Método que vai buscar um voo pelo ID
        public Flight GetFlight(int id)
        {
            return _context.Flights.Find(id);
        }

        //Método que adiciona um voo à tabela
        public void AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
        }

        //Método que actualiza (update) um voo
        public void UpdateFlight(Flight flight)
        {
            _context.Update(flight);
        }

        //Método que remove um voo
        public void RemoveFlight(Flight flight)
        {
            _context.Flights.Remove(flight);
        }

        //Método que actualiza a BD
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //Verifica se o voo existe
        public bool FlightExists(int id)
        {
            return _context.Flights.Any(p => p.ID == id);
        }
    }
}
