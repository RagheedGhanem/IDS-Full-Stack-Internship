using System.ComponentModel.DataAnnotations.Schema;

namespace IDS_App.DTOs
{
    public class CommentDTO
    {
        public required string Content { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }

        public int? UserId { get; set; }
    }
}