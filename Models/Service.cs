using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceLink.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        // required: will always be set to AppUser.Id (string), so keep non-nullable
        [Required]
        public string ProviderId { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // optional, allow null
        public string? Description { get; set; }

        public string? Category { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // optional url or empty
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
