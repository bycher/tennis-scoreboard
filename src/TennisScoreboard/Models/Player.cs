using Microsoft.EntityFrameworkCore;

namespace TennisScoreboard.Models;

[Index(nameof(Name), IsUnique=true)]
public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Match> Matches { get; set; } = null!;
}