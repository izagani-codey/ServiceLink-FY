using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceLink.ViewModels
{
    public class BookingCreateViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }
        public DateTime RequestedFor { get; set; }
        public string? Notes { get; set; }
    }

}
