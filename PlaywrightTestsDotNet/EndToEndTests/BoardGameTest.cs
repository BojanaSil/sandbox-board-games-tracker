using PlaywrightTestsDotNet.Fixture;
using PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;

namespace PlaywrightTestsDotNet.EndToEndTests;

public class BoardGameTest
{
    [Theory, MemberData(nameof(Data.Channels), MemberType = typeof(Data))]
    public async Task CreateBoardGame(string channel)
    {
        var boardGameDsl = new BoardGameDsl(channel);

        await boardGameDsl.InitializeAsync();

        await boardGameDsl.AddNewBoardGame("Everdell", 2020, 1, 4, 10, 2.3m, 60, "Family");

        await boardGameDsl.AssertBoardGameExists("Everdell");

        await boardGameDsl.DeleteBoardGame("Everdell");
    }

    [Theory, MemberData(nameof(Data.Channels), MemberType = typeof(Data))]
    public async Task UpdateBoardGame(string channel)
    {
        var boardGameDsl = new BoardGameDsl(channel);

        await boardGameDsl.InitializeAsync();

        await boardGameDsl.AddNewBoardGame("Everdell", 2020, 1, 4, 10, 2.3m, 60, "Family");

        await boardGameDsl.UpdateBoardGame("Everdell", 2055, 2, 5, 12, 2.2m, 75, "Family,Fun");

        await boardGameDsl.AssertBoardGameUpdated("Everdell", 2055);
        
        await boardGameDsl.DeleteBoardGame("Everdell");
    }

    [Theory, MemberData(nameof(Data.Channels), MemberType = typeof(Data))]
    public async Task DeleteBoardGame(string channel)
    {
        var boardGameDsl = new BoardGameDsl(channel);

        await boardGameDsl.InitializeAsync();

        await boardGameDsl.AddNewBoardGame("Everdell", 2020, 1, 4, 10, 2.3m, 60, "Family");

        await boardGameDsl.DeleteBoardGame("Everdell");

        await boardGameDsl.AssertBoardGameDoesNotExist("Everdell");
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