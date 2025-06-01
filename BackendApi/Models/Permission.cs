using System.ComponentModel.DataAnnotations;

namespace BackendApi.Models
{
    public class Permission
    {
        [Key]
        [StringLength(50)]
        public string PermissionId { get; set; }

        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}