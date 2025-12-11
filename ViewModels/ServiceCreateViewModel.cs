using System.ComponentModel.DataAnnotations;

namespace ServiceLink.ViewModels
{
    public class ServiceCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [Required]
        [Range(0, 9999999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
