namespace CharityHub.Models.ViewModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }      // Загальна кількість елементів
        public int ItemsPerPage { get; set; }    // Кількість елементів на сторінці
        public int CurrentPage { get; set; }     // Поточна сторінка

        public int TotalPages =>
            (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
