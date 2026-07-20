using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class MaintenanceRecord : BaseEntity
    {
        public int VehicleId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } // Rutin, Acil, Tamir vb.
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string? ServiceProvider { get; set; }
        public string? MaintenanceStatus { get; set; } // Tamamlandı, Beklemede, İptal
        public DateTime? CompletionDate { get; set; }
        public int? Odometer { get; set; }
        public string? Notes { get; set; }
        public string? Attachments { get; set; } // Fatura, resim vb. dosya yolları

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; }
    }
}
