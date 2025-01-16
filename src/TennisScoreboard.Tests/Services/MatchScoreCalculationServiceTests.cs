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

    private void Act(MatchScoreDto matchScore, int winnerId)
    {
        var context = new MatchScoreUpdateContextDto(matchScore, winnerId);
        _service.UpdateMatchScore(context);
    }

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
        var matchScore = _builder.WithPoints("0:0").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.That(matchScore.FirstPlayerScores.Points, Is.EqualTo(1));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerLosePoint_ShouldNotIncrementPoints()
    {
        // Arrange
        var matchScore = _builder.WithPoints("0:0").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.That(matchScore.SecondPlayerScores.Points, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsGameStandard_ShouldIncrementGames()
    {
        // Arrange
        var matchScore = _builder.WithPoints("40:0").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.That(matchScore.FirstPlayerScores.Games, Is.EqualTo(1));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsGame_ShouldResetPoints()
    {
        // Arrange
        var matchScore = _builder.WithPoints("40:0").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.That(matchScore.FirstPlayerScores.Points, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMatchScore_WhenDeuceIsAchieved_GameShouldBeContinued()
    {
        // Arrange
        var matchScore = _builder.WithPoints("40:40").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Games, Is.EqualTo(0));
            Assert.That(matchScore.SecondPlayerScores.Games, Is.EqualTo(0));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsFromAdvantage_ShouldIncrementGames()
    {
        // Arrange
        var matchScore = _builder.WithPoints("AD:40").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Games, Is.EqualTo(1));
            Assert.That(matchScore.SecondPlayerScores.Games, Is.EqualTo(0));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenLoserRecoupsAdvantage_GameShouldBeContinued()
    {
        // Arrange
        var matchScore = _builder.WithPoints("AD:40").Build();
        
        // Act
        Act(matchScore, _secondPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Games, Is.EqualTo(0));
            Assert.That(matchScore.SecondPlayerScores.Games, Is.EqualTo(0));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsSixGamesWithAtLeastTwoGamesLead_ShouldUpdateSets()
    {
        // Arrange
        var matchScore = _builder.WithGames("5:4").WithPoints("40:0").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.EqualTo(new List<int> { 6 }));
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.EqualTo(new List<int> { 4 }));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsSevenGamesWithoutTieBreak_ShouldUpdateSets()
    {
        // Arrange
        var matchScore = _builder.WithGames("6:5").WithPoints("40:0").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.EqualTo(new List<int> { 7 }));
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.EqualTo(new List<int> { 5 }));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenBothPlayersReachMinGamesToWin_CurrentSetShouldBeContinuedWithTieBreak() {
        // Arrange
        var matchScore = _builder.WithGames("6:5").WithPoints("0:40").Build();

        // Precondition
        Assert.That(matchScore.IsTieBreak, Is.False);
        
        // Act
        Act(matchScore, _secondPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.Empty);
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.Empty);
            Assert.That(matchScore.IsTieBreak, Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenTieBreak_And_PlayerGetsSevenPointsWithTwoPointsLead_ShouldUpdateSets()
    {
        // Arrange
        var matchScore = _builder.WithTieBreak("6:5").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.EqualTo(new List<int> { 7 }));
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.EqualTo(new List<int> { 6 }));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenTieBreak_And_PlayerGetsSevenPointsWithLessThanTwoPointLead_TieBreakShouldBeContinued() {
        // Arrange
        var matchScore = _builder.WithTieBreak("6:6").Build();
        
        // Act
        Act(matchScore, _firstPlayerId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.Empty);
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.Empty);
            Assert.That(matchScore.IsSetFinished, Is.False);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoOutOfTwoSets_MatchShouldBeEnded()
    {
        // Arrange
        var matchScore = _builder.WithSets([new(6, 4)])
                                 .WithGames("5:4")
                                 .WithPoints("40:0")
                                 .Build();

        // Act
        Act(matchScore, _firstPlayerId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.EqualTo(new List<int> { 6, 6 }));
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.EqualTo(new List<int> { 4, 4 }));
            Assert.That(matchScore.IsMatchFinished, Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoOutOfThreeSets_MatchShouldBeEnded()
    {
        // Arrange
        var matchScore = _builder.WithSets([new(6, 4), new(4, 6)])
                                 .WithGames("5:4")
                                 .WithPoints("40:0")
                                 .Build();

        // Act
        Act(matchScore, _firstPlayerId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.EqualTo(new List<int> { 6, 4, 6 }));
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.EqualTo(new List<int> { 4, 6, 4 }));
            Assert.That(matchScore.IsMatchFinished, Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenBothPlayersWinSet_MatchShouldBeContinued()
    {
        // Arrange
        var matchScore = _builder.WithSets([new(6, 4)])
                                 .WithGames("4:5")
                                 .WithPoints("0:40")
                                 .Build();

        // Act
        Act(matchScore, _secondPlayerId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matchScore.FirstPlayerScores.Sets, Is.EqualTo(new List<int> { 6, 4 }));
            Assert.That(matchScore.SecondPlayerScores.Sets, Is.EqualTo(new List<int> { 4, 6 }));
            Assert.That(matchScore.IsMatchFinished, Is.False);
        });
    }
}