using System.ComponentModel.DataAnnotations;

namespace TennisScoreboard.Models;

public class NewMatch : IValidatableObject
{
    [Required(ErrorMessage="Missing player's name")]
    public string? FirstPlayerName { get; set; }

    [Required(ErrorMessage="Missing player's name")]
    public string? SecondPlayerName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstPlayerName == SecondPlayerName)
            yield return new ValidationResult("Player's names must be different");
    }
}