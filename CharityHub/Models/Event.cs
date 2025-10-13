using System.ComponentModel.DataAnnotations.Schema;

namespace CharityHub.Models
{
    public class Event
    {
        public long? EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Location { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TargetAmount { get; set; } // Ціль збору коштів
    }
}

