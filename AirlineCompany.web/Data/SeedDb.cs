using AirlineCompany.web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private Random _random;

        public SeedDb(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            var user = await _userManager.FindByEmailAsync("tiago.sa.lima@formandos.cinel.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Tiago",
                    LastName = "Lima",
                    Email = "tiago.sa.lima@formandos.cinel.pt",
                    UserName = "tiago.sa.lima@formandos.cinel.pt"
                };

                var result = await _userManager.CreateAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            if(!_context.Flights.Any())
            {
                this.AddFlight("FirstFlight", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddFlight(string name, User user)
        {
            _context.Flights.Add(new Flight
            {
                FlightNumber = name,
                User = user
            });
        }
    }
}
