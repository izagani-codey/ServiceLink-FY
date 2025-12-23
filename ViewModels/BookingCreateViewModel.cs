using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceLink.ViewModels
{
    public class BookingCreateViewModel
    {
        public int ServiceId { get; set; }

        // ❌ NOT REQUIRED — display only
        public string? ServiceTitle { get; set; }

        [Required]
        public DateTime RequestedFor { get; set; }

        public string? Notes { get; set; }
    }


}
