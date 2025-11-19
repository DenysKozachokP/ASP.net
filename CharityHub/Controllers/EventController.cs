using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityHub.Models;

namespace CharityHub.Controllers
{
    public class EventController : Controller
    {
        private ICharityRepository repository;

        public EventController(ICharityRepository repo)
        {
            repository = repo;
        }

        // Read - список всіх подій
        public IActionResult Index()
        {
            return View(repository.Events.ToList());
        }

        // Read - деталі конкретної події
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = repository.GetEventById(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // Create - GET
        public IActionResult Create()
        {
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Description,Date,Location,TargetAmount")] Event eventItem)
        {
            if (ModelState.IsValid)
            {
                repository.CreateEvent(eventItem);
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        // Update - GET
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = repository.GetEventById(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // Update - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("EventId,Title,Description,Date,Location,TargetAmount")] Event eventItem)
        {
            if (id != eventItem.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repository.UpdateEvent(eventItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (repository.GetEventById(eventItem.EventId) == null)
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
            return View(eventItem);
        }

        // Delete - GET (підтвердження)
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = repository.GetEventById(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var eventItem = repository.GetEventById(id);
            if (eventItem != null)
            {
                repository.DeleteEvent(eventItem);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

