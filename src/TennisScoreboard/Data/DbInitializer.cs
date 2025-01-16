using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;
using TennisScoreboard.Services;

namespace TennisScoreboard.Data;

public class DbInitializer(
    TennisMatchesContext context, MatchesHistoryService matchesHistoryService, PlayersService playersService)
{
    private readonly List<string> _playerNames = [
        "Roger Federer",
        "Rafael Nadal",
        "Novak Djokovic",
        "Andy Murray",
        "Stan Wawrinka",
        "Marin Cilic",
        "Dominic Thiem",
        "Alexander Zverev",
        "Stefanos Tsitsipas",
        "Daniil Medvedev"
    ];
    private readonly Random _rand = new();

    private readonly TennisMatchesContext _context = context;
    private readonly MatchesHistoryService _matchesHistoryService = matchesHistoryService;
    private readonly PlayersService _playersService = playersService;

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();        

        var players = await InitializePlayers();
        await InitializeMatches(players.ToList());
    }

    private async Task<IEnumerable<Player>> InitializePlayers() =>
        await Task.WhenAll(_playerNames.Select(async pn => await _playersService.AddPlayer(pn)));

    private async Task InitializeMatches(List<Player> players)
    {
        // Seed with all pairs of players from _players and random winner
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = i + 1; j < players.Count; j++)
            {   
                var matchScore = new MatchScoreDto(players[i], players[j]);
                var winnerId = GetRandomWinnerId(players[i], players[j]);
                var context = new MatchScoreUpdateContextDto(matchScore, winnerId);
                
                await _matchesHistoryService.AddToHistory(context);
            }
        }
    }

    private int GetRandomWinnerId(Player firstPlayer, Player secondPlayer) =>
        _rand.Next(2) == 0 ? firstPlayer.Id : secondPlayer.Id;
}