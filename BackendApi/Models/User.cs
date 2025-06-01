using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendApi.Models
{
    public class User
    {
        [Key]
        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Username { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleId { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }

        [JsonIgnore]
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}