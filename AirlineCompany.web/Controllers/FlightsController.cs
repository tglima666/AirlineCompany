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
using Microsoft.AspNetCore.Authorization;
using AirlineCompany.web.Models;
using System.IO;

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
        [Authorize(Roles = "Admin")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FlightNumber,Price,ImageFile,Date,Hour")] FlightViewModel view)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Flights",
                        view.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Flights/{view.ImageFile.FileName}";
                }

                var flight = this.ToFlight(view, path);

                flight.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _flightRepository.CreateAsync(flight);
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        private Flight ToFlight(FlightViewModel view, string path)
        {
            return new Flight
            {
                ID = view.ID,
                ImageURL = path,
                IsAvailable = view.IsAvailable,
                FlightNumber = view.FlightNumber,
                Date = view.Date,
                Hour = view.Hour,
                Price = view.Price,
                User = view.User
            };
        }

        // GET: Flights/Edit/5
        [Authorize(Roles = "Admin")]
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

            var view = this.ToFlightViewModel(flight);
            return View(view);
        }

        private object ToFlightViewModel(Flight flight)
        {
            return new FlightViewModel
            {
                ID = flight.ID,
                IsAvailable = flight.IsAvailable,
                ImageURL = flight.ImageURL,
                FlightNumber = flight.FlightNumber,
                Price = flight.Price,
                User = flight.User
            };
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
                    var path = view.ImageUrl;

                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{file}";
                    }

                    var product = this.ToFlight(view, path);
                    flight.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
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
        [Authorize(Roles = "Admin")]
        [Authorize]
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
