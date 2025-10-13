using Microsoft.AspNetCore.Mvc;
using CharityHub.Models;
using CharityHub.Models.ViewModels;

namespace CharityHub.Controllers
{
    public class HomeController : Controller
    {
        private ICharityRepository repository;
        public int PageSize = 2; // Кількість записів на сторінку

        public HomeController(ICharityRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(int page = 1)
        {
            var eventsList = new EventsListViewModel
            {
                Events = repository.Events
                    .OrderBy(e => e.EventId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Events.Count()
                }
            };

            return View(eventsList);
        }
    }
}
