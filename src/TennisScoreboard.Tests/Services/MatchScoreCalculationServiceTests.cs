using TennisScoreboard.Models;
using TennisScoreboard.Services;

namespace TennisScoreboard.Tests.Services;

[TestFixture]
public class MatchScoreCalculationServiceTests {
    [Test]
    public void UpdateMatchScore_WhenPlayerWinsPoint_ShouldIncrementPointsInCurrentGame() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame, Is.EqualTo(1));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerLosePoint_ShouldNotIncrementPointsInCurrentGame() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[secondPlayerId].PointsInCurrentGame, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsStandard_ShouldIncrementGamesInCurrentSet() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame = OngoingMatch.MinPointsToWin - 1;
        match.PlayersMatchStates[secondPlayerId].PointsInCurrentGame = 0;
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[firstPlayerId].GamesInCurrentSet, Is.EqualTo(1));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsStandard_ShouldResetPointsInCurrentGame() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame = OngoingMatch.MinPointsToWin - 1;
        match.PlayersMatchStates[secondPlayerId].PointsInCurrentGame = 0;
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMatchScore_WhenDeuceIsAchieved_GameShouldBeContinued() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame = OngoingMatch.MinPointsToWin - 1;
        match.PlayersMatchStates[secondPlayerId].PointsInCurrentGame = OngoingMatch.MinPointsToWin - 1;
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[firstPlayerId].GamesInCurrentSet, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsWithAdvantage_GameShouldBeFinished() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        var pointsAfterDeuce = OngoingMatch.MinPointsToWin + new Random().Next(1, 10);
        match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame = pointsAfterDeuce;
        match.PlayersMatchStates[secondPlayerId].PointsInCurrentGame = pointsAfterDeuce - 1;
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[firstPlayerId].GamesInCurrentSet, Is.EqualTo(1));
    }

    [Test]
    public void UpdateMatchScore_WhenLoserRecoupsAdvantage_GameShouldBeContinued() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatch(firstPlayerId, secondPlayerId);
        var pointsAfterDeuce = OngoingMatch.MinPointsToWin + new Random().Next(1, 10);
        match.PlayersMatchStates[firstPlayerId].PointsInCurrentGame = pointsAfterDeuce;
        match.PlayersMatchStates[secondPlayerId].PointsInCurrentGame = pointsAfterDeuce - 1;
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, secondPlayerId);
        
        // Assert
        Assert.That(match.PlayersMatchStates[secondPlayerId].GamesInCurrentSet, Is.EqualTo(0));
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerHitsMinGamesToWinWithMinDifference_SetShouldBeFinished() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithPointsInCurrentGame(firstPlayerId, OngoingMatch.MinPointsToWin - 1)
            .WithPointsInCurrentGame(
                secondPlayerId, OngoingMatch.MinPointsToWin - OngoingMatch.MinPointsDifferenceToWin)
            .WithGamesInCurrentSet(firstPlayerId, OngoingMatch.MinGamesToWin - 1)
            .WithGamesInCurrentSet(
                secondPlayerId, OngoingMatch.MinGamesToWin - OngoingMatch.MinGamesDifferenceToWin)
            .Build();
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.Multiple(() => {
            Assert.That(match.PlayersMatchStates[firstPlayerId].GamesInCurrentSet, Is.Zero);
            Assert.That(match.PlayersMatchStates[secondPlayerId].GamesInCurrentSet, Is.Zero);
            Assert.That(
                match.PlayersMatchStates[firstPlayerId].FinishedGames,
                Is.EqualTo(new List<int> { OngoingMatch.MinGamesToWin })
            );
            Assert.That(
                match.PlayersMatchStates[secondPlayerId].FinishedGames,
                Is.EqualTo(new List<int> { OngoingMatch.MinGamesToWin - OngoingMatch.MinGamesDifferenceToWin })
            );
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoGamesAfterGamesDeuce_SetShouldBeFinished() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithPointsInCurrentGame(firstPlayerId, OngoingMatch.MinPointsToWin - 1)
            .WithPointsInCurrentGame(
                secondPlayerId, OngoingMatch.MinPointsToWin - OngoingMatch.MinPointsDifferenceToWin)
            .WithGamesInCurrentSet(firstPlayerId, OngoingMatch.MinGamesToWin)
            .WithGamesInCurrentSet(secondPlayerId, OngoingMatch.MinGamesToWin - 1)
            .Build();
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.Multiple(() => {
            Assert.That(
                match.PlayersMatchStates[firstPlayerId].FinishedGames,
                Is.EqualTo(new List<int> { OngoingMatch.MinGamesToWin + 1 }));
            Assert.That(
                match.PlayersMatchStates[secondPlayerId].FinishedGames,
                Is.EqualTo(new List<int> { OngoingMatch.MinGamesToWin - 1 }));
        });
    }

    [Test]
    public void UpdateMatchScore_WhenBothPlayersReachMinGamesToWin_TieBreakShouldStart() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithPointsInCurrentGame(firstPlayerId, OngoingMatch.MinPointsToWin - OngoingMatch.MinPointsDifferenceToWin)
            .WithPointsInCurrentGame(secondPlayerId, OngoingMatch.MinPointsToWin - 1)
            .WithGamesInCurrentSet(firstPlayerId, OngoingMatch.MinGamesToWin)
            .WithGamesInCurrentSet(secondPlayerId, OngoingMatch.MinGamesToWin - 1)
            .Build();
        var service = new MatchScoreCalculationService();

        // Precondition
        Assert.That(match.IsTieBreak(), Is.False);
        
        // Act
        service.UpdateMatchScore(match, secondPlayerId);
        
        // Assert
        Assert.Multiple(() => {
            Assert.That(match.PlayersMatchStates[firstPlayerId].FinishedGames, Is.Empty);
            Assert.That(match.PlayersMatchStates[secondPlayerId].FinishedGames, Is.Empty);
            Assert.That(match.IsTieBreak(), Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_PlayerGetsMinTieBreakPointsToWinWithMinDifference_PlayerShouldWinSet() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithPointsInCurrentGame(firstPlayerId, OngoingMatch.MinTieBreakPointsToWin - 1)
            .WithPointsInCurrentGame(secondPlayerId, OngoingMatch.MinTieBreakPointsToWin - OngoingMatch.MinPointsDifferenceToWin)
            .WithGamesInCurrentSet(firstPlayerId, OngoingMatch.MinGamesToWin)
            .WithGamesInCurrentSet(secondPlayerId, OngoingMatch.MinGamesToWin)
            .Build();
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.Multiple(() => {
            Assert.That(
                match.PlayersMatchStates[firstPlayerId].FinishedGames,
                Is.EqualTo(new List<int> { OngoingMatch.MinGamesToWin + 1 }));
            Assert.That(
                match.PlayersMatchStates[secondPlayerId].FinishedGames,
                Is.EqualTo(new List<int> { OngoingMatch.MinGamesToWin }));
        });
    }

    [Test]
    public void UpdateMatchScore_PlayerGetsMinTieBreakPointsWithoutMinDifference_SetShouldNotBeFinished() {
        // Arrange
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithPointsInCurrentGame(firstPlayerId, OngoingMatch.MinTieBreakPointsToWin - 1)
            .WithPointsInCurrentGame(
                secondPlayerId, OngoingMatch.MinTieBreakPointsToWin - OngoingMatch.MinPointsDifferenceToWin + 1)
            .WithGamesInCurrentSet(firstPlayerId, OngoingMatch.MinGamesToWin)
            .WithGamesInCurrentSet(secondPlayerId, OngoingMatch.MinGamesToWin)
            .Build();
        var service = new MatchScoreCalculationService();
        
        // Act
        service.UpdateMatchScore(match, firstPlayerId);
        
        // Assert
        Assert.Multiple(() => {
            Assert.That(
                match.PlayersMatchStates[firstPlayerId].GamesInCurrentSet,
                Is.EqualTo(OngoingMatch.MinGamesToWin));
            Assert.That(
                match.PlayersMatchStates[secondPlayerId].GamesInCurrentSet,
                Is.EqualTo(OngoingMatch.MinGamesToWin));
            Assert.That(match.PlayersMatchStates[firstPlayerId].FinishedGames, Is.Empty);
            Assert.That(match.PlayersMatchStates[secondPlayerId].FinishedGames, Is.Empty);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoOutOfTwoSets_MatchShouldBeEnded() {
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithFinishedGames(firstPlayerId, [6])
            .WithFinishedGames(secondPlayerId, [4])
            .WithGamesInCurrentSet(firstPlayerId, 5)
            .WithGamesInCurrentSet(secondPlayerId, 4)
            .WithPointsInCurrentGame(firstPlayerId, 3)
            .WithPointsInCurrentGame(secondPlayerId, 2)
            .Build();
        
        var service = new MatchScoreCalculationService();

        // Act
        service.UpdateMatchScore(match, firstPlayerId);

        // Assert
        Assert.Multiple(() => {
            Assert.That(match.PlayersMatchStates[firstPlayerId].FinishedGames, Is.EqualTo(new List<int> { 6, 6 }));
            Assert.That(match.PlayersMatchStates[secondPlayerId].FinishedGames, Is.EqualTo(new List<int> { 4, 4 }));
            Assert.That(match.IsMatchFinished(), Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenPlayerWinsTwoOutOfThreeSets_MatchShouldBeEnded() {
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithFinishedGames(firstPlayerId, [6, 4])
            .WithFinishedGames(secondPlayerId, [4, 6])
            .WithGamesInCurrentSet(firstPlayerId, 5)
            .WithGamesInCurrentSet(secondPlayerId, 4)
            .WithPointsInCurrentGame(firstPlayerId, 3)
            .WithPointsInCurrentGame(secondPlayerId, 2)
            .Build();
        
        var service = new MatchScoreCalculationService();

        // Act
        service.UpdateMatchScore(match, firstPlayerId);

        // Assert
        Assert.Multiple(() => {
            Assert.That(match.PlayersMatchStates[firstPlayerId].FinishedGames, Is.EqualTo(new List<int> { 6, 4, 6 }));
            Assert.That(match.PlayersMatchStates[secondPlayerId].FinishedGames, Is.EqualTo(new List<int> { 4, 6, 4 }));
            Assert.That(match.IsMatchFinished(), Is.True);
        });
    }

    [Test]
    public void UpdateMatchScore_WhenBothPlayersWinSet_MatchShouldBeContinued() {
        var firstPlayerId = 1;
        var secondPlayerId = 2;
        var match = new OngoingMatchBuilder(firstPlayerId, secondPlayerId)
            .WithFinishedGames(firstPlayerId, [6])
            .WithFinishedGames(secondPlayerId, [4])
            .WithGamesInCurrentSet(firstPlayerId, 4)
            .WithGamesInCurrentSet(secondPlayerId, 5)
            .WithPointsInCurrentGame(firstPlayerId, 2)
            .WithPointsInCurrentGame(secondPlayerId, 3)
            .Build();
        
        var service = new MatchScoreCalculationService();

        // Act
        service.UpdateMatchScore(match, secondPlayerId);

        // Assert
        Assert.Multiple(() => {
            Assert.That(match.PlayersMatchStates[firstPlayerId].FinishedGames, Is.EqualTo(new List<int> { 6, 4 }));
            Assert.That(match.PlayersMatchStates[secondPlayerId].FinishedGames, Is.EqualTo(new List<int> { 4, 6 }));
            Assert.That(match.IsMatchFinished(), Is.False);
        });
    }
}