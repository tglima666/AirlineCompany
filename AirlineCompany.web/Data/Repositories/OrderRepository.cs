using AirlineCompany.web.Data.Entities;
using AirlineCompany.web.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            return _context.OrderDetailTemps
                .Include(o => o.Flight)
                .Where(o => o.User == user)
                .OrderBy(o => o.Flight.FlightNumber);
        }

        public async Task<IQueryable<Order>> GetOrderAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //se o user for o admin
                return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Flight)
                .OrderByDescending(o => o.OrderDate);

            }

            //os users normais
            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Flight)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }
    }
}
