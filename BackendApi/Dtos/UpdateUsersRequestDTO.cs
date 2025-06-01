using System.ComponentModel.DataAnnotations;

namespace BackendApi.Dtos
{
    public class UpdateUsersRequestDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
} 