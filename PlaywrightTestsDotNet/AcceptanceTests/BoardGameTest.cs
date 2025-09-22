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

    private static async Task<BoardGameDsl> CreateAndInitializeBoardGame(string channel)
    {
        var boardGame = new BoardGameDsl(channel);

        await boardGame.InitializeAsync();
        return boardGame;
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
}