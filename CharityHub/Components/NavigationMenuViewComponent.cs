using Microsoft.AspNetCore.Mvc;
using CharityHub.Models;

namespace CharityHub.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ICharityRepository repository;

        public NavigationMenuViewComponent(ICharityRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            var categories = repository.Events
                .Select(e => e.Location)
                .Distinct()
                .OrderBy(c => c);

            ViewBag.SelectedCategory = RouteData?.Values["category"]?.ToString()
                ?? HttpContext.Request.Query["category"].ToString();

            return View(categories);
        }
    }
}


