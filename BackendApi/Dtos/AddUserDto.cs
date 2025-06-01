using System.ComponentModel.DataAnnotations;

namespace BackendApi.Dtos
{
    public class AddUserDto
    {
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleId { get; set; }
    }
} 