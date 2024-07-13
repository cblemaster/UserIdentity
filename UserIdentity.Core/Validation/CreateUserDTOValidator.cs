using FluentValidation;
using UserIdentity.Core.DataTransferObjects;

namespace UserIdentity.Core.Validation;

// TODO: A lot of code duplication with UpdateUserDTOValidator.cs
public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(v => v.Username.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_USERNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Username).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_USERNAME_MAX_LENGTH));
        // TODO: Check for unique username
        RuleFor(v => v.Password.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_PASSWORD_MAX_LENGTH)
            .WithMessage(nameof(Core.DataTransferObjects.CreateUserDTO.Password).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_PASSWORD_MAX_LENGTH));
        RuleFor(v => v.FirstName.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_FIRSTNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.FirstName).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_FIRSTNAME_MAX_LENGTH));
        RuleFor(v => v.LastName.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_LASTNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.LastName).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_LASTNAME_MAX_LENGTH));
        RuleFor(v => v.Email.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_EMAIL_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Email).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_EMAIL_MAX_LENGTH));
        // TODO: Check for unique email
        RuleFor(v => v.Phone.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_PHONE_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Phone).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_PHONE_MAX_LENGTH));
        // TODO: Check for unique phone
        RuleFor(v => v.Roles)
            .NotEmpty()
            .WithMessage("User must have at least one (1) role.");
    }
}
