using System.ComponentModel.DataAnnotations;

namespace TennisScoreboard.Models.Requests;

public class NewMatchRequest : IValidatableObject {
    private const string MissingNameMessage = "Missing player's name";

    [Required(ErrorMessage = MissingNameMessage)]
    public required string FirstPlayerName { get; set; }

    [Required(ErrorMessage = MissingNameMessage)]
    public required string SecondPlayerName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (FirstPlayerName == SecondPlayerName)
            yield return new ValidationResult("Player's names must be different");
    }
}