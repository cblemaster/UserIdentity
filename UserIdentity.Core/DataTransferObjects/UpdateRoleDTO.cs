namespace UserIdentity.Core.DataTransferObjects;

public class UpdateRoleDTO
{
    public required int RoleId { get; set; }
    public string Role { get; set; } = string.Empty;
}
