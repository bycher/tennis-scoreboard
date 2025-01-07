using TennisScoreboard.Models;

namespace TennisScoreboard.Data
{
    public class OngoingMatchesStorage
    {
        private readonly Dictionary<Guid, OngoingMatch> _storage = [];

        public OngoingMatch? Get(Guid key) {
            return _storage.GetValueOrDefault(key);
        }

        public Guid Add(OngoingMatch match) {
            var key = Guid.NewGuid();
            _storage.Add(key, match);
            return key;
        }
    }
}