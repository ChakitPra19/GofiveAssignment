public class EditUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public List<PermissionDto> Permission { get; set; }
}

public class PermissionDto
{
    public string PermissionId { get; set; }
    public bool IsReadable { get; set; }
    public bool IsWritable { get; set; }
    public bool IsDeletable { get; set; }
}
