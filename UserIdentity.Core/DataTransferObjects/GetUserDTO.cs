using UserIdentity.Core.Entities;

namespace UserIdentity.Core.DataTransferObjects;

public class GetUserDTO
{
    public required int UserId { get; init; }

    public required string Username { get; init; } = null!;

    public required string FirstName { get; init; } = null!;

    public required string LastName { get; init; } = null!;

    public required string Email { get; init; } = null!;

    public required string Phone { get; init; } = null!;

    public required DateTime CreateDate { get; init; }

    public required DateTime? UpdateDate { get; init; }

    public required ICollection<RoleDTO> Roles { get; init; } = [];
}
