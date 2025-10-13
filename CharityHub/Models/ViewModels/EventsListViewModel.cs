namespace CharityHub.Models.ViewModels
{
    public class EventsListViewModel
    {
        public IEnumerable<Event> Events { get; set; } = Enumerable.Empty<Event>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
