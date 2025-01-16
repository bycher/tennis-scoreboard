using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard.Models.Requests;

public class GetMatchesRequest
{
    public int Page { get; set; } = 1; 

    [FromQuery(Name="filter_by_player_name")]
    public string? FilterByPlayerName { get; set; } = null;
}