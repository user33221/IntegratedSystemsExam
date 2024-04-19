﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository;
using IntegratedSystems.Service.Interface;
using IntegratedSystems.Domain.DTO;

namespace IntegratedSystems.Web.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IVaccinationService _vaccineService;

        public PatientsController(IPatientService _patientService, IVaccinationService vaccineService)
        {
            this._patientService = _patientService;
            _vaccineService = vaccineService;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(_patientService.GetPatients());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PatientDetailsDTO dto = new PatientDetailsDTO();

            dto.patient = _patientService.GetPatientById(id);
            dto.vaccines = _vaccineService.GetVaccinesForPatient(id);

            if (dto.patient == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Embg,FirstName,LastName,PhoneNumber,Email,Id")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patientService.CreateNewPatient(patient);
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Embg,FirstName,LastName,PhoneNumber,Email,Id")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _patientService.UpdatePatient(patient);
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _patientService.DeletePatient(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(Guid id)
        {
            return _patientService.GetPatientById(id) != null;
        }
    }
}
