using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CharityHub.Models
{
    public class Event
    {
        public long? EventId { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть назву події")]
        [StringLength(200, ErrorMessage = "Назва не може перевищувати 200 символів")]
        [Display(Name = "Назва")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, введіть опис")]
        [StringLength(1000, ErrorMessage = "Опис не може перевищувати 1000 символів")]
        [Display(Name = "Опис")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, вкажіть дату")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Будь ласка, вкажіть місце проведення")]
        [StringLength(100, ErrorMessage = "Місце не може перевищувати 100 символів")]
        [Display(Name = "Місце")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, введіть цільову суму")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Будь ласка, введіть додатну суму")]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Цільова сума")]
        public decimal TargetAmount { get; set; }

        // Навігаційна властивість для зв'язку один-до-багатьох
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}

