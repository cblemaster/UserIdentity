using UserIdentity.Core.DataTransferObjects;
using UserIdentity.Core.Entities;

namespace UserIdentity.API.Extensions;

internal static class DTOEntityMappingExtensions
{
    internal static GetRoleDTO MapGetRoleToDTO(this Role role) =>
        new(role.RoleId, role.Role1);

    internal static GetUserDTO MapGetUserToDTO(this User user) =>
        new(user.UserId, user.Username, user.FirstName, user.LastName, user.Email,
            user.Phone, user.CreateDate, user.UpdateDate,
            user.Roles.MapGetRolesToDTO());

    internal static IEnumerable<GetRoleDTO> MapGetRolesToDTO(this IEnumerable<Role> roles)
    {
        List<GetRoleDTO> dtos = [];
        roles.ToList().ForEach(r => dtos.Add(MapGetRoleToDTO(r)));
        return dtos.AsEnumerable();
    }

    internal static IEnumerable<GetUserDTO> MapGetUsersToDTO(this IEnumerable<User> users)
    {
        List<GetUserDTO> dtos = [];
        users.ToList().ForEach(u => dtos.Add(MapGetUserToDTO(u)));
        return dtos.AsEnumerable();
    }

    internal static Role MapGetRoleDTOToEntity(this GetRoleDTO role) =>
        new() { RoleId = role.RoleId, Role1 = role.Role };

    internal static Role MapCreateRoleDTOToEntity(this CreateRoleDTO role) =>
        new() { Role1 = role.Role };

    internal static User MapCreateUserDTOToEntity(this CreateUserDTO user) =>
        new()
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            CreateDate = DateTime.Now,
            UpdateDate = null,
        };

    internal static Role MapUpdateRoleDTOToEntity(this UpdateRoleDTO dto, Role entity)
    {
        entity.Role1 = dto.Role;
        return entity;
    }

    internal static User MapUpdateUserDTOToEntity(this UpdateUserDTO dto, User entity)
    {
        entity.Username = dto.Username;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.UpdateDate = DateTime.Now;
        return entity;
    }
}
