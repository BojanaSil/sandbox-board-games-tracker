using PlaywrightTestsDotNet.Fixture;
using PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;
using PlaywrightTestsDotNet.Fixture.Utils;

namespace PlaywrightTestsDotNet.AcceptanceTests;

[Collection("AcceptanceBoardGameTests")]
public class BoardGameTest : TestFixture
{
    [Theory, MemberData(nameof(Data.Channels), MemberType = typeof(Data))]
    public async Task CreateBoardGame(string channel)
    {
        var boardGame = await CreateAndInitializeBoardGame(channel);

        await boardGame.AddBoardGameWithFullDetails("Everdell", 2020, 1, 4, 10, 2.3m, 60, "Family");

        await boardGame.AssertBoardGameExists("Everdell");

        await boardGame.DeleteBoardGame("Everdell");
    }

    [Theory, MemberData(nameof(Data.Channels), MemberType = typeof(Data))]
    public async Task UpdateBoardGame(string channel)
    {
        var boardGame = await CreateAndInitializeBoardGame(channel);

        await boardGame.AddBoardGameWithFullDetails("Everdell", 2020, 1, 4, 10, 2.3m, 60, "Family");

        await boardGame.UpdateBoardGame("Everdell", 2055, 2, 5, 12, 2.2m, 75, "Family,Fun");

        await boardGame.AssertBoardGameUpdated("Everdell", 2055);

        await boardGame.DeleteBoardGame("Everdell");
    }

    [Theory, MemberData(nameof(Data.Channels), MemberType = typeof(Data))]
    public async Task DeleteBoardGame(string channel)
    {
        var boardGame = await CreateAndInitializeBoardGame(channel);

        await boardGame.AddBoardGameForDeletion("Everdell");

        await boardGame.DeleteBoardGame("Everdell");

        await boardGame.AssertBoardGameDoesNotExist("Everdell");
    }

    [Theory, MemberData(nameof(Data.UiOnlyChannel), MemberType = typeof(Data))]
    public async Task BoardGameWithGameLogs_ShouldHavePlayerFieldsNotEditable(string channel)
    {
        var boardGame = await CreateAndInitializeBoardGame(channel);
        
        await boardGame.AddBoardGameWithFullDetails("Everdell", 2020, 1, 4, 10, 2.3m, 60, "Family");

        var gameLog = await CreateAndInitializeGameLog(channel);
        await gameLog.AddGameLogWithFullDetails("Everdell", DateTime.Today, 1, 2, 50, "Jane Doe");

        await boardGame.AssertFieldIsNotEditable("Everdell", "Min number of players");
        await boardGame.AssertFieldIsNotEditable("Everdell", "Max number of players");

        await boardGame.DeleteBoardGame("Everdell");
    }

    private static async Task<BoardGameDsl> CreateAndInitializeBoardGame(string channel)
    {
        var boardGame = new BoardGameDsl(channel);

        await boardGame.InitializeAsync();
        return boardGame;
    }

    private static async Task<GameLogDsl> CreateAndInitializeGameLog(string channel)
    {
        var gameLog = new GameLogDsl(channel);

        await gameLog.InitializeAsync();
        return gameLog;
    }
}

public static class Data
{
    public static IEnumerable<object[]> Channels =>
        new List<object[]>
        {
            new object[] { "UI" }, 
            new object[] { "API" }
        };

    public static IEnumerable<object[]> UiOnlyChannel =>
        new List<object[]>
        {
            new object[] { "UI" }
        };
}