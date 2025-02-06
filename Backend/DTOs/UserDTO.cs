using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IDS_App.DTOs
{
    public class UserDTO
    {

        [StringLength(50)]
        public required string Username { get; set; } = null!;

        [StringLength(100)]
        public required string Email { get; set; } = null!;

        [StringLength(255)]
        public required string Password { get; set; } = null!;
    }
}
