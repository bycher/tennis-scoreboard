using System.ComponentModel.DataAnnotations;

namespace TennisScoreboard.Utils;

public class ValidGuidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && value is Guid guid && guid != Guid.Empty)
            return ValidationResult.Success;

        return new ValidationResult("Invalid GUID");
    }
}