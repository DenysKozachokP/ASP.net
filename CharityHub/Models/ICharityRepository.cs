namespace CharityHub.Models
{
    public interface ICharityRepository
    {
        IQueryable<Event> Events { get; }
    }
}
