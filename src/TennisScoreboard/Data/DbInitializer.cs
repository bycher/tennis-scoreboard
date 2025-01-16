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
        await InitializeMatches(players);
    }

    private async Task<IEnumerable<Player>> InitializePlayers() =>
        await Task.WhenAll(_playerNames.Select(async pn => await _playersService.AddPlayer(pn)));

    private async Task InitializeMatches(IEnumerable<Player> players)
    {
        var playerIds = players.Select(p => p.Id).ToList();

        // Seed with all pairs of players from _players and random winner
        for (int firstPlayerId = 1; firstPlayerId < playerIds.Count; firstPlayerId++)
        {
            for (int secondPlayerId = firstPlayerId + 1; secondPlayerId < playerIds.Count + 1; secondPlayerId++)
            {   
                var matchScore = new MatchScoreDto(firstPlayerId, secondPlayerId);
                var context = new MatchScoreUpdateContextDto(
                    matchScore, GetRandomWinner(firstPlayerId, secondPlayerId));
                
                await _matchesHistoryService.AddToHistory(context);
            }
        }
    }

    private int GetRandomWinner(int firstPlayerId, int secondPlayerId) =>
        _rand.Next(2) == 0 ? firstPlayerId : secondPlayerId;
}