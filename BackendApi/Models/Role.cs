using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendApi.Models
{
    public class Role
    {
        [Key]
        [StringLength(50)]
        public string RoleId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}