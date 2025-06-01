using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models
{
    public class RolePermission
    {
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string RoleId { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string PermissionId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }
    }
}