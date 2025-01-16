using TennisScoreboard.Models.Dtos;

namespace TennisScoreboard.Services;

public class OngoingMatchesService {
    private readonly Dictionary<Guid, MatchScoreDto> _storage = [];

    public MatchScoreDto? Get(Guid key) => _storage.GetValueOrDefault(key);

    public Guid Add(MatchScoreDto match)
    {
        var key = Guid.NewGuid();
        _storage.Add(key, match);
        return key;
    }

    public bool Remove(Guid key) => _storage.Remove(key);
}