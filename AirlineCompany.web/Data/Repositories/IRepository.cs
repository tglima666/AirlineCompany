using AirlineCompany.web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Repositories
{
    public interface IRepository
    {
        void AddFlight(Flight flight);

        bool FlightExists(int id);

        Flight GetFlight(int id);

        IEnumerable<Flight> GetFlights();

        void RemoveFlight(Flight flight);

        Task<bool> SaveAllAsync();

        void UpdateFlight(Flight flight);
    }
}