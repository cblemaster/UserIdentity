using FluentValidation;
using UserIdentity.Core.DataTransferObjects;

namespace UserIdentity.Core.Validation;

public class CreateRoleDTOValidator : AbstractValidator<CreateRoleDTO>
{
    public CreateRoleDTOValidator() =>
        RuleFor(v => v.Role.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.ROLE_ROLE_MAX_LENGTH)
            .WithMessage(GetErrorMessageForIsRequiredAndHasMaxLength(
                nameof(Core.Entities.Role.Role1), DataSchemaConstants.ROLE_ROLE_MAX_LENGTH));

    private static string GetErrorMessageForIsRequiredAndHasMaxLength
        (string propName, int maxLength) =>
            $"{propName.ToUpper()} is required and must be {maxLength} " +
            "characters or fewer.";
}
