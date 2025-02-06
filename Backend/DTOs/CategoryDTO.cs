using System.ComponentModel.DataAnnotations;

namespace IDS_App.DTOs
{
    public class CategoryDTO
    {
        [StringLength(100)]
        public required string Name { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
