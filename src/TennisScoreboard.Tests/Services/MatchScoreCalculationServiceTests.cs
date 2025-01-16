using TennisScoreboard.Models.Dtos;
using TennisScoreboard.Services;
using TennisScoreboard.Tests.Utils;

namespace TennisScoreboard.Tests.Services;

[TestFixture]
public class MatchScoreCalculationServiceTests
{
    private int _firstPlayerId;
    private int _secondPlayerId;

    private MatchScoreCalculationService _service = null!;
    private MatchScoreBuilder _builder = null!;
    private MatchScoreDto _matchScore = null!;

    [SetUp]
    public void SetUp()
    {
        _firstPlayerId = 1;
        _secondPlayerId = 2;

        _service = new MatchScoreCalculationService();
        _builder = new MatchScoreBuilder(_firstPlayerId, _secondPlayerId);
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsPoint_ShouldIncrementPoints()
    {
        // Arrange
        _matchScore = _builder.WithPoints("0", "0").Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        AssertPointsEqualTo(_firstPlayerId, 1);
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerLosePoint_ShouldNotIncrementPoints()
    {
        // Arrange
        _matchScore = _builder.WithPoints("0", "0").Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        AssertPointsEqualTo(_secondPlayerId, 0);
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsGameStandard_ShouldIncrementGames()
    {
        // Arrange
        _matchScore = _builder.WithPoints("40", "0").Build();

        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        AssertGamesEqualTo(_firstPlayerId, 1);
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsGame_ShouldResetPoints()
    {
        // Arrange
        _matchScore = _builder.WithPoints("40", "0").Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertPointsEqualTo(_firstPlayerId, 0);
            AssertPointsEqualTo(_secondPlayerId, 0);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenDeuceIsAchieved_GameShouldBeContinued()
    {
        // Arrange
        _matchScore = _builder.WithPoints("40", "40").Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertGamesEqualTo(_firstPlayerId, 0);
            AssertGamesEqualTo(_secondPlayerId, 0);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsFromAdvantage_ShouldIncrementGames()
    {
        // Arrange
        _matchScore = _builder.WithPoints("AD", "40").Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertGamesEqualTo(_firstPlayerId, 1);
            AssertGamesEqualTo(_secondPlayerId, 0);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenLoserRecoupsAdvantage_GameShouldBeContinued()
    {
        // Arrange
        _matchScore = _builder.WithPoints("AD", "40").Build();
        
        // Act
        UpdateScore(_secondPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertGamesEqualTo(_firstPlayerId, 0);
            AssertGamesEqualTo(_secondPlayerId, 0);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenLoserRecoupsAdvantage_WinnerShouldLosePoint()
    {
        // Arrange
        _matchScore = _builder.WithPoints("AD", "40").Build();
        _matchScore.TryGetScoreComponent(_firstPlayerId, nameof(PlayerScoresDto.Points), out int? winnerPoints);
        _matchScore.TryGetScoreComponent(_secondPlayerId, nameof(PlayerScoresDto.Points), out int? loserPoints);

        // Act
        UpdateScore(_secondPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertPointsEqualTo(_firstPlayerId, winnerPoints - 1);
            AssertPointsEqualTo(_secondPlayerId, loserPoints);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsSixGamesWithAtLeastTwoGamesLead_ShouldUpdateSets()
    {
        // Arrange
        _matchScore = _builder.WithGames(5, 4)
                              .WithPoints("40", "0")
                              .Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, [6]);
            AssertSetsEqualTo(_secondPlayerId, [4]);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsSevenGamesWithoutTieBreak_ShouldUpdateSets()
    {
        // Arrange
        _matchScore = _builder.WithGames(6, 5)
                              .WithPoints("40", "0")
                              .Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, [7]);
            AssertSetsEqualTo(_secondPlayerId, [5]);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenBothPlayersReachMinGamesToWin_CurrentSetShouldBeContinuedWithTieBreak() {
        // Arrange
        _matchScore = _builder.WithGames(6, 5)
                              .WithPoints("0", "40")
                              .Build();

        // Precondition
        Assert.That(_matchScore.IsTieBreak, Is.False);
        
        // Act
        UpdateScore(_secondPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, []);
            AssertSetsEqualTo(_secondPlayerId, []);
            Assert.That(_matchScore.IsTieBreak, Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenTieBreak_And_PlayerGetsSevenPointsWithTwoPointsLead_ShouldUpdateSets()
    {
        // Arrange
        _matchScore = _builder.WithTieBreak(6, 5).Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, [7]);
            AssertSetsEqualTo(_secondPlayerId, [6]);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenTieBreak_And_PlayerGetsSevenPointsWithLessThanTwoPointLead_TieBreakShouldBeContinued() {
        // Arrange
        _matchScore = _builder.WithTieBreak(6, 6).Build();
        
        // Act
        UpdateScore(_firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, []);
            AssertSetsEqualTo(_secondPlayerId, []);
            Assert.That(_matchScore.IsSetFinished, Is.False);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoOutOfTwoSets_MatchShouldBeEnded()
    {
        // Arrange
        _matchScore = _builder.WithSets([(6, 4)])
                              .WithGames(5, 4)
                              .WithPoints("40", "0")
                              .Build();

        // Act
        UpdateScore(_firstPlayerId);

        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, [6, 6]);
            AssertSetsEqualTo(_secondPlayerId, [4, 4]);
            Assert.That(_matchScore.IsMatchFinished, Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoOutOfThreeSets_MatchShouldBeEnded()
    {
        // Arrange
        _matchScore = _builder.WithSets([(6, 4), (4, 6)])
                              .WithGames(5, 4)
                              .WithPoints("40", "0")
                              .Build();

        // Act
        UpdateScore(_firstPlayerId);

        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, [6, 4, 6]);
            AssertSetsEqualTo(_secondPlayerId, [4, 6, 4]);
            Assert.That(_matchScore.IsMatchFinished, Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenBothPlayersWinSet_MatchShouldBeContinued()
    {
        // Arrange
        _matchScore = _builder.WithSets([(6, 4)])
                              .WithGames(4, 5)
                              .WithPoints("0", "40")
                              .Build();

        // Act
        UpdateScore(_secondPlayerId);

        // Assert
        Assert.Multiple(() =>
        {
            AssertSetsEqualTo(_firstPlayerId, [6, 4]);
            AssertSetsEqualTo(_secondPlayerId, [4, 6]);
            Assert.That(_matchScore.IsMatchFinished, Is.False);
        });
    }

    private void UpdateScore(int winnerId)
    {
        var context = new MatchScoreUpdateContextDto(_matchScore, winnerId);
        _service.UpdateMatchScore(context);
    }

    private void AssertPointsEqualTo(int playerId, int? expectedPoints) =>
        Assert.Multiple(() =>
        {
            Assert.That(
                _matchScore.TryGetScoreComponent(playerId, nameof(PlayerScoresDto.Points), out int? newPoints),
                Is.True
            );
            Assert.That(newPoints, Is.EqualTo(expectedPoints));
        });
    
    private void AssertGamesEqualTo(int playerId, int? expectedGames) =>
        Assert.Multiple(() =>
        {
            Assert.That(
                _matchScore.TryGetScoreComponent(playerId, nameof(PlayerScoresDto.Games), out int? newGames),
                Is.True
            );
            Assert.That(newGames, Is.EqualTo(expectedGames));
        });
    
    private void AssertSetsEqualTo(int playerId, List<int>? expectedSets) =>
        Assert.Multiple(() =>
        {
            Assert.That(
                _matchScore.TryGetScoreComponent(playerId, nameof(PlayerScoresDto.Sets), out List<int>? newSets),
                Is.True
            );
            Assert.That(newSets, Is.EqualTo(expectedSets));
        });
}