using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLink.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        // optional navigation property
        public Service Service { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string ProviderId { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ScheduledFor { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected, Completed

        public string Notes { get; set; }
    }
}
