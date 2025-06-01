using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendApi.Models
{
    public class UserPermission
    {
        [Key]
        [StringLength(50)]
        public string UserId { get; set; }

        [Key]
        [StringLength(50)]
        public string PermissionId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Permission Permission { get; set; }
    }
} 