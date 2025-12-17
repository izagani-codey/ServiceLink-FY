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

    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public Service? Service { get; set; }

        [Required]
        public string CustomerId { get; set; } = null!; // AspNetUsers.Id

        [Required]
        public string ProviderId { get; set; } = null!; // copied from Service.ProviderId

        [Required]
        public DateTime RequestedFor { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(24)")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Add this property if it does not exist
        public string ServiceTitle { get; set; }
    }
}
