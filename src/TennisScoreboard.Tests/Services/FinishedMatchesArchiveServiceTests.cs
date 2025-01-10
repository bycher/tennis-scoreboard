using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Tests.Services;

[TestFixture]
public class FinishedMatchesArchiveServiceTests {
    [Test]
    public void ArchiveMatch_ShouldAddMatchToDatabase() {
        // Arrange
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<TennisMatchesContext>()
            .UseSqlite(connection)
            .Options;
        using var context = new TennisMatchesContext(options);
        var service = new FinishedMatchesArchiveService(context);

        var player1 = new Player { Id = 1, Name = "Player 1" };
        var player2 = new Player { Id = 2, Name = "Player 2" };
        context.Players.AddRangeAsync(player1, player2);
        context.SaveChangesAsync();

        var match = new Match {
            Id = 1,
            FirstPlayerId = 1,
            SecondPlayerId = 2
        };

        // Act
        service.ArchiveMatch(match, 1);

        // Assert
        var archivedMatch = context.Matches.Find(match.Id);
        Assert.Multiple(() => {
            Assert.That(archivedMatch, Is.Not.Null);
            Assert.That(archivedMatch, Is.EqualTo(match));
        });
    }
}