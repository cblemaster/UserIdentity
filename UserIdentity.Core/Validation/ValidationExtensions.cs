namespace UserIdentity.Core.Validation;

internal static class ValidationExtensions
{
    internal static string GetErrorMessageForIsRequiredAndHasMaxLength
        (this string propName, int maxLength) =>
            $"{propName} is required and must be {maxLength} " +
            "characters or fewer.";
}
