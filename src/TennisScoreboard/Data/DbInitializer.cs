using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Models.Entities;
using TennisScoreboard.Services;

namespace TennisScoreboard.Data;

public class DbInitializer(
    TennisMatchesContext context, MatchesHistoryService matchesHistoryService, PlayersService playersService
) {
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

    public async Task InitializeAsync() {
        await context.Database.EnsureCreatedAsync();        

        var players = await InitializePlayers();
        await InitializeMatches(players);
    }

    private async Task<IEnumerable<Player>> InitializePlayers() =>
        await Task.WhenAll(_playerNames.Select(async pn => await playersService.AddPlayer(pn)));

    private async Task InitializeMatches(IEnumerable<Player> players) {
        var playersList = players.ToList();

        for (int i = 0; i < playersList.Count; i++) {
            for (int j = i+1; j < playersList.Count; j++) {   
                var matchScore = new MatchScoreDto(playersList[i], playersList[j]);
                var winnerId = GetRandomWinnerId(playersList[i], playersList[j]);
                
                var context = new MatchScoreUpdateContextDto(matchScore, winnerId);
                await matchesHistoryService.AddToHistory(context);
            }
        }
    }

    private int GetRandomWinnerId(Player firstPlayer, Player secondPlayer) =>
        _rand.Next(2) == 0 ? firstPlayer.Id : secondPlayer.Id;
}