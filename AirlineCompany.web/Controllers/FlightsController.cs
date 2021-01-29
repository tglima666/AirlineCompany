using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirlineCompany.web.Data;
using AirlineCompany.web.Data.Entities;
using AirlineCompany.web.Data.Repositories;
using AirlineCompany.web.Helpers;

namespace AirlineCompany.web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserHelper _userHelper;

        public FlightsController(IFlightRepository flightRepository, IUserHelper userHelper)
        {
            _flightRepository = flightRepository;
            _userHelper = userHelper;
        }

        // GET: Flights
        public IActionResult Index()
        {
            return View(_flightRepository.GetAll());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FlightNumber,Date,Hour")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                //TODO: Mudar para o user que depois estiver logado
                flight.User = await _userHelper.GetUserByEmailAsync("tiago.sa.lima@formandos.cinel.pt");
                await _flightRepository.CreateAsync(flight);
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FlightNumber,Date,Hour")] Flight flight)
        {
            if (id != flight.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: Mudar para o user que depois estiver logado
                    flight.User = await _userHelper.GetUserByEmailAsync("tiago.sa.lima@formandos.cinel.pt");
                    await _flightRepository.UpdateAsync(flight);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _flightRepository.ExistAsync(flight.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            await _flightRepository.DeleteAsync(flight);
            return RedirectToAction(nameof(Index));
        }
    }
}
