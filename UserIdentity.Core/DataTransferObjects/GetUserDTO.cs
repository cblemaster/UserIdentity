using UserIdentity.Core.Entities;

namespace UserIdentity.Core.DataTransferObjects;

public record GetUserDTO(int UserId, string Username, string FirstName, 
    string LastName, string Email, string Phone, DateTime CreateDate,
    DateTime? UpdateDate, IEnumerable<RoleDTO> Roles);
