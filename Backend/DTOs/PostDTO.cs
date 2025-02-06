using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IDS_App.DTOs
{
    public class PostDTO
    {
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string? Tags { get; set; }

        // Add UserID property
        [Required]
        public int UserId { get; set; }
    }
}
