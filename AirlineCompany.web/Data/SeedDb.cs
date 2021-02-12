using AirlineCompany.web.Data.Entities;
using AirlineCompany.web.Helpers;
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
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");

            var user = await _userHelper.GetUserByEmailAsync("tiago.sa.lima@formandos.cinel.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Tiago",
                    LastName = "Lima",
                    Email = "tiago.sa.lima@formandos.cinel.pt",
                    UserName = "tiago.sa.lima@formandos.cinel.pt"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Flights.Any())
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
                Price = _random.Next(1000),
                IsAvailable = true,
                User = user
            });
        }
    }
}
