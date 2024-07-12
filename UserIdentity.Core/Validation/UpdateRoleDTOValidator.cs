using FluentValidation;
using UserIdentity.Core.DataTransferObjects;
using UserIdentity.Core.Interfaces;

namespace UserIdentity.Core.Validation;

public class UpdateRoleDTOValidator : AbstractValidator<UpdateRoleDTO>, IUpdateRoleDTOValidator
{
    public UpdateRoleDTOValidator()
    {
        RuleFor(v => v.RoleId).GreaterThan(0);
        RuleFor(v => v.Role.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.ROLE_ROLE_MAX_LENGTH)
            .WithMessage(GetErrorMessageForIsRequiredAndHasMaxLength(
                nameof(Core.Entities.Role.Role1), DataSchemaConstants.ROLE_ROLE_MAX_LENGTH));
        //RuleFor(v => v.Username)
        //    .Must(u => IsUsernameUnique(u))
        //    .WithMessage("{PropertyName} {PropertyValue} is already in use.");
    }

    private static string GetErrorMessageForIsRequiredAndHasMaxLength
        (string propName, int maxLength) =>
            $"{propName.ToUpper()} is required and must be {maxLength} " +
            "characters or fewer.";

    // TODO: Check for unique role with api call
}
