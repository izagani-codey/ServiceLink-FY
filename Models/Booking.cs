using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLink.Models
{
    public enum BookingStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled
    }

    // Models/Booking.cs
    public class Booking
    {
        public int BookingId { get; set; }

        public int ServiceId { get; set; }
        [Required]
        public string CustomerId { get; set; } = null!;

        [Required]
        public string ProviderId { get; set; } = null!;

        public Service? Service { get; set; }

        public DateTime RequestedFor { get; set; }
        public DateTime CreatedAt { get; set; }

        public BookingStatus Status { get; set; }
        public string? Notes { get; set; }
    }

}
