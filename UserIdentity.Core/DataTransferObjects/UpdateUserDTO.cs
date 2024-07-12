namespace UserIdentity.Core.DataTransferObjects;

public class UpdateUserDTO
{
    public required int UserId { get; init; }

    public string Username { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime UpdateDate { get; set; }

    public ICollection<GetRoleDTO> Roles { get; set; } = [];
}
