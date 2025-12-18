using System;
using ServiceLink.Models;

namespace ServiceLink.ViewModels
{
    public class BookingListItemViewModel
    {
        public int BookingId { get; set; }

        // Display fields from service 
        public string ServiceTitle { get; set; } = string.Empty;

        // Dates
        public DateTime RequestedFor { get; set; }
        public DateTime CreatedAt { get; set; }

        public BookingStatus Status { get; set; }

        // Optional: any other display-only fields you need (nullable if unsure)
        public string? Notes { get; set; }
        public string? CustomerEmail { get; set; }
    }
}

