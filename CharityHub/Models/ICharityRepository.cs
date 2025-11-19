namespace CharityHub.Models
{
    public interface ICharityRepository
    {
        IQueryable<Event> Events { get; }
        IQueryable<Donation> Donations { get; }

        // CRUD операції для Event
        void CreateEvent(Event e);
        void UpdateEvent(Event e);
        void DeleteEvent(Event e);
        Event? GetEventById(long? id);

        // CRUD операції для Donation
        void CreateDonation(Donation d);
        void UpdateDonation(Donation d);
        void DeleteDonation(Donation d);
        Donation? GetDonationById(long? id);
    }
}
