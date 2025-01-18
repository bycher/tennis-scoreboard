using System.ComponentModel.DataAnnotations;
using TennisScoreboard.Utils;

namespace TennisScoreboard.Models.Requests;

public class UpdateMatchScoreRequest {
    [Required(ErrorMessage = "UUID is required")]
    [ValidGuid]
    public Guid Uuid { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid or missing winner ID")]
    public int WinnerId { get; set; }
}