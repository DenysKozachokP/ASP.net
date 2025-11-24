using System.ComponentModel.DataAnnotations;

namespace CharityHub.Client.Models
{
    public class EventDto
    {
        public long? EventId { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть назву події")]
        [StringLength(200, ErrorMessage = "Назва не може перевищувати 200 символів")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, введіть опис")]
        [StringLength(1000, ErrorMessage = "Опис не може перевищувати 1000 символів")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, вкажіть дату")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Будь ласка, вкажіть місце проведення")]
        [StringLength(100, ErrorMessage = "Місце не може перевищувати 100 символів")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, введіть цільову суму")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Будь ласка, введіть додатну суму")]
        public decimal TargetAmount { get; set; }
    }
}

