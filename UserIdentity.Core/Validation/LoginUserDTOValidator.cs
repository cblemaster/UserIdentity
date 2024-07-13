using FluentValidation;
using UserIdentity.Core.DataTransferObjects;

namespace UserIdentity.Core.Validation;

public class LoginUserDTOValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDTOValidator()
    {
        RuleFor(v => v.Username.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH, DataSchemaConstants.USER_USERNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Username).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_USERNAME_MAX_LENGTH));
        RuleFor(v => v.Password.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_PASSWORD_MAX_LENGTH)
            .WithMessage(nameof(Core.DataTransferObjects.CreateUserDTO.Username).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_PASSWORD_MAX_LENGTH));
    }
}
