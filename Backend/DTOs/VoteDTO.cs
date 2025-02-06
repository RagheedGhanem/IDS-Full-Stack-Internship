using System.ComponentModel.DataAnnotations;

namespace IDS_App.DTOs
{
    public class VoteDTO
    {
        [Required]
        [RegularExpression("like|dislike", ErrorMessage = "Type must be 'like' or 'dislike'.")]
        public required string Type { get; set; }

        [Required]
        public int PostId { get; set; }

        public int? UserId { get; set; }
    }
}