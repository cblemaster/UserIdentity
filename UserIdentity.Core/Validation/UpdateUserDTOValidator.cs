using FluentValidation;
using UserIdentity.Core.DataTransferObjects;

namespace UserIdentity.Core.Validation;

public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        RuleFor(v => v.UserId).GreaterThan(0);
        RuleFor(v => v.Username.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_USERNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Username).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_USERNAME_MAX_LENGTH));
        // TODO: Check for unique username
        RuleFor(v => v.FirstName.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_FIRSTNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.FirstName).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_FIRSTNAME_MAX_LENGTH));
                nameof(Core.Entities.User.FirstName), DataSchemaConstants.USER_FIRSTNAME_MAX_LENGTH));
        RuleFor(v => v.LastName.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_LASTNAME_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.LastName).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_LASTNAME_MAX_LENGTH));
                nameof(Core.Entities.User.LastName), DataSchemaConstants.USER_LASTNAME_MAX_LENGTH));
        RuleFor(v => v.Email.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_EMAIL_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Email).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_EMAIL_MAX_LENGTH));
                nameof(Core.Entities.User.Email), DataSchemaConstants.USER_EMAIL_MAX_LENGTH));
        //RuleFor(v => v.Email)
        //    .Must(u => IsEmailUnique(u))
        //    .WithMessage("{PropertyName} {PropertyValue} is already in use.");
        RuleFor(v => v.Phone.Length)
            .InclusiveBetween(DataSchemaConstants.REQUIRED_STRING_MIN_LENGTH,
                DataSchemaConstants.USER_PHONE_MAX_LENGTH)
            .WithMessage(nameof(Core.Entities.User.Username).GetErrorMessageForIsRequiredAndHasMaxLength(DataSchemaConstants.USER_PHONE_MAX_LENGTH));
                nameof(Core.Entities.User.Phone), DataSchemaConstants.USER_PHONE_MAX_LENGTH));
        //RuleFor(v => v.Phone)
        //    .Must(u => IsPhoneUnique(u))
        //    .WithMessage("{PropertyName} {PropertyValue} is already in use.");
        RuleFor(v => v.Roles)
            .NotEmpty()
            .WithMessage("User must have at least one (1) role.");
    }

    private static string GetErrorMessageForIsRequiredAndHasMaxLength
        (string propName, int maxLength) =>
            $"{propName.ToUpper()} is required and must be {maxLength} " +
            "characters or fewer.";

    // TODO: replace this with api calls
    //private static bool IsUsernameUnique(string username) =>
    //    !new AppDbContext().Users.Select(u => u.Username).Contains(username);

    //private static bool IsEmailUnique(string email) =>
    //    !new AppDbContext().Users.Select(u => u.Email).Contains(email);

    //private static bool IsPhoneUnique(string phone) =>
    //    !new AppDbContext().Users.Select(u => u.Phone).Contains(phone);
}
