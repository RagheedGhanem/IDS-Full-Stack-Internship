using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IDS_App.DTOs
{
    public class NotificationDTO
    {
        [StringLength(255)]
        public required string Message { get; set; } = null!;
    }
}
