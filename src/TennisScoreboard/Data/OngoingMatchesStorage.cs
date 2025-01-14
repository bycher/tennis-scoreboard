using TennisScoreboard.Models;

namespace TennisScoreboard.Data;

public class OngoingMatchesStorage {
    private readonly Dictionary<Guid, MatchScore> _storage = [];

    public MatchScore? Get(Guid key) => _storage.GetValueOrDefault(key);

    public Guid Add(Player firstPlayer, Player secondPlayer) => Add(new MatchScore(firstPlayer, secondPlayer));

    private Guid Add(MatchScore match)
    {
        var key = Guid.NewGuid();
        _storage.Add(key, match);
        return key;
    }

    public bool Remove(Guid key) => _storage.Remove(key);
}