using AirlineCompany.web.Data.Repositories;
using AirlineCompany.web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFlightRepository _flightRepository;

        public OrdersController(IOrderRepository orderRepository, IFlightRepository flightRepository)
        {
            _orderRepository = orderRepository;
            _flightRepository = flightRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);
            return View(model);
        }


        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddFlight()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Flights = _flightRepository.GetComboFlights()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(AddItemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return this.RedirectToAction("Create");
            }

            return this.View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteDetailTempAsync(id.Value);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                return this.RedirectToAction("Index");
            }

            return this.RedirectToAction("Create");
        }
    }
}
