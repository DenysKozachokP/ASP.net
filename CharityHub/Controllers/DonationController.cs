using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CharityHub.Models;

namespace CharityHub.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        private ICharityRepository repository;

        public DonationController(ICharityRepository repo)
        {
            repository = repo;
        }

        // Read - список всіх пожертв
        public IActionResult Index()
        {
            var donations = repository.Donations.ToList();
            return View(donations);
        }

        // Read - деталі конкретної пожертви
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = repository.GetDonationById(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // Create - GET
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(repository.Events, "EventId", "Title");
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DonorName,Amount,DonationDate,Comment,EventId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                repository.CreateDonation(donation);
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(repository.Events, "EventId", "Title", donation.EventId);
            return View(donation);
        }

        // Update - GET
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = repository.GetDonationById(id);
            if (donation == null)
            {
                return NotFound();
            }

            ViewData["EventId"] = new SelectList(repository.Events, "EventId", "Title", donation.EventId);
            return View(donation);
        }

        // Update - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("DonationId,DonorName,Amount,DonationDate,Comment,EventId")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repository.UpdateDonation(donation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (repository.GetDonationById(donation.DonationId) == null)
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
            ViewData["EventId"] = new SelectList(repository.Events, "EventId", "Title", donation.EventId);
            return View(donation);
        }

        // Delete - GET (підтвердження)
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = repository.GetDonationById(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(long id)
        {
            var donation = repository.GetDonationById(id);
            if (donation != null)
            {
                repository.DeleteDonation(donation);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

