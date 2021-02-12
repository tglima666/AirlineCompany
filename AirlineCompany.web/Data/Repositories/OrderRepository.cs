using AirlineCompany.web.Data.Entities;
using AirlineCompany.web.Helpers;
using AirlineCompany.web.Models;
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

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var flight = await _context.Flights.FindAsync(model.FlightID);
            if (flight == null)
            {
                return;
            }

            var orderDetailTemp = await _context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Flight == flight)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = flight.Price,
                    Flight = flight,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await _context.OrderDetailTemps
                .Include(o => o.Flight)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Flight = o.Flight,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            _context.Orders.Add(order);
            _context.OrderDetailTemps.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await _context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailTemps.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
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
                .Include(o => o.User)
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

        //Existe no carrinho e só vai modificar a quantidade
        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailTemps.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
        }
    }
}
