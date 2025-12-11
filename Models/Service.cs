using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLink.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        // FK to AspNetUsers.Id (ApplicationUser)
        [Required]
        public string ProviderId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [Required]
        [Precision(18, 2)]             // ensures decimal(18,2) in SQL
        [Range(0, 9999999.99)]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public Service()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
