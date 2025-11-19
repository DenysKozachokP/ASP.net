using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CharityHub.Models
{
    public class Donation
    {
        public long? DonationId { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть ім'я донора")]
        [StringLength(100, ErrorMessage = "Ім'я донора не може перевищувати 100 символів")]
        [Display(Name = "Ім'я донора")]
        public string DonorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, введіть суму пожертви")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Будь ласка, введіть додатну суму")]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Сума")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Будь ласка, вкажіть дату пожертви")]
        [Display(Name = "Дата пожертви")]
        public DateTime DonationDate { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "Коментар не може перевищувати 500 символів")]
        [Display(Name = "Коментар")]
        public string? Comment { get; set; }

        [Display(Name = "Подія")]
        public long? EventId { get; set; }
        
        // Навігаційна властивість для зв'язку один-до-багатьох
        public Event? Event { get; set; }
    }
}

