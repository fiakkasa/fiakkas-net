namespace app.Shared.Options.Extensions;

public static class IConfigurationExtensions
{
    private const string _validationMessagesSeparator = "; ";
    private const string _validationMemberNamesSeparator = ", ";

    public static T GetConfiguration<T>(this IConfiguration configuration, string? section = default) where T : class
    {
        var typeName = typeof(T).Name;
        var normalizedSection = section ?? typeName;
        var obj =
            configuration.GetSection(normalizedSection).Get<T>()
            ?? throw new ValidationException(
                $"Configuration for type '{typeName}' at section '{normalizedSection}' cannot be materialized."
            );
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), validationResults, true))
        {
            return obj;
        }

        throw new ValidationException(
            string.Join(
                _validationMessagesSeparator,
                validationResults.Select(x =>
                {
                    var memberNames = string.Join(_validationMemberNamesSeparator, x.MemberNames);

                    return $"Validation failed for type '{typeName}', at section '{normalizedSection}', and member(s) '{memberNames}' with message: '{x.ErrorMessage}'";
                })
            )
        );
    }
}
