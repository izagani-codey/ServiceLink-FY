using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceLink.ViewModels
{
    public class BookingCreateViewModel
    {
        public int ServiceId { get; set; }

        public string ServiceTitle { get; set; } = null!; // for display only

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequestedFor { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
