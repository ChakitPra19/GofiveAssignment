namespace BackendApi.Dtos
{
    public class GetUserByIdResponseDto
    {
        public Status Status { get; set; }
        public UserData Data { get; set; }
    }

    public class UserData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public RoleData Role { get; set; }
        public string Username { get; set; }
        public List<PermissionData> Permissions { get; set; }
    }

    public class RoleData
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class PermissionData
    {
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
} 