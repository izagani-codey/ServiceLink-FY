using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceLink.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        // ProviderId will match AspNetUsers.Id (string)
        [Required]
        public string ProviderId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
