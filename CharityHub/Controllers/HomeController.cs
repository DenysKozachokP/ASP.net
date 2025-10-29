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

        private const string SelectedCategorySessionKey = "SelectedCategory";

        public IActionResult Index(string? category, int page = 1, bool reset = false)
        {
            if (reset)
            {
                HttpContext.Session.Remove(SelectedCategorySessionKey);
                category = null;
            }

            if (!string.IsNullOrEmpty(category))
            {
                HttpContext.Session.SetString(SelectedCategorySessionKey, category);
            }
            else
            {
                category = HttpContext.Session.GetString(SelectedCategorySessionKey);
            }

            var filtered = repository.Events
                .Where(e => category == null || e.Location == category);

            var eventsList = new EventsListViewModel
            {
                Events = filtered
                    .OrderBy(e => e.EventId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = filtered.Count()
                },
                CurrentCategory = category
            };

            return View(eventsList);
        }
    }
}
