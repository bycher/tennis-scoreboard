using System.ComponentModel.DataAnnotations;

namespace TennisScoreboard.Models.Requests;

public class NewMatchRequest : IValidatableObject
{
    [Required(ErrorMessage="Missing player's name")]
    public string FirstPlayerName { get; set; } = null!;

    [Required(ErrorMessage="Missing player's name")]
    public string SecondPlayerName { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FirstPlayerName == SecondPlayerName)
            yield return new ValidationResult("Player's names must be different");
    }
}