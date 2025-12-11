using System;
using ServiceLink.Models;

namespace ServiceLink.ViewModels
{
    public class BookingListItemViewModel
    {
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; } = null!;
        public DateTime RequestedFor { get; set; }
        public BookingStatus Status { get; set; }
        public string CustomerId { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
    }
}
