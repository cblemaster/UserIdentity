using FluentValidation;
using UserIdentity.Core.DataTransferObjects;

namespace UserIdentity.Core.Validation;

public class UpdateRoleDTOValidator : AbstractValidator<UpdateRoleDTO>
{
    public UpdateRoleDTOValidator()
    {
        RuleFor(v => v.RoleId).GreaterThan(0);
        RuleFor(v => v.Role.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.ROLE_ROLE_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.Role.Role1).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.ROLE_ROLE_MAX_LENGTH));
        // TODO: Check for unique role
    }
}
