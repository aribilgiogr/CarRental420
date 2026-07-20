namespace Core.Concretes.DTOs.MaintenanceRecord
{
    public class MaintenanceRecordResponseDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } 
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string? ServiceProvider { get; set; }
        public string? MaintenanceStatus { get; set; } 
        public DateTime? CompletionDate { get; set; }
        public int? Odometer { get; set; }
        public string? Notes { get; set; }
        public string? Attachments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}