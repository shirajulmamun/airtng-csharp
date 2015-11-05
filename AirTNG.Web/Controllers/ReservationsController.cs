﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirTNG.Web.Models;
using AirTNG.Web.Models.Repository;
using AirTNG.Web.ViewModels;

namespace AirTNG.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IVacationPropertiesRepository _vacationPropertiesRepository;
        private readonly IReservationsRepository _reservationsRepository;

        public ReservationsController() : this(
            new VacationPropertiesRepository(), new ReservationsRepository()) { }

        public ReservationsController(
            IVacationPropertiesRepository vacationPropertiesRepository,
            IReservationsRepository reservationsRepository)
        {
            _vacationPropertiesRepository = vacationPropertiesRepository;
            _reservationsRepository = reservationsRepository;
        }

        // GET: Reservations/Create
        public async Task<ActionResult> Create(int id)
        {
            var vacationProperty = await _vacationPropertiesRepository.FindAsync(id);
            var reservation = new ReservationViewModel
            {
                ImageUrl = vacationProperty.ImageUrl,
                Description = vacationProperty.Description,
                VacationPropertyId = vacationProperty.Id,
                UserName = vacationProperty.User.Name,
                UserPhoneNumber = vacationProperty.User.PhoneNumber,
            };

            return View(reservation);
        }


        // POST: Reservations/Create
        [HttpPost]
        public async Task<ActionResult> Create(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = new Reservation
                {
                    Message = model.Message,
                    PhoneNumber = model.UserPhoneNumber,
                    Name = model.UserName,
                    VactionPropertyId = model.VacationPropertyId,
                    Status = ReservationStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                await _reservationsRepository.CreateAsync(reservation);

                return RedirectToAction("Index", "VacationProperties");
            }

            return View();
        }
    }
}