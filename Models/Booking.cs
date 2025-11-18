using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceLink.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        // navigation property is optional during create
        public Service? Service { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public string ProviderId { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ScheduledFor { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // optional
        public string? Notes { get; set; }
    }
}
