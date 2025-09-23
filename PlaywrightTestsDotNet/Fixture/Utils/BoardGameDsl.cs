using PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;
using PlaywrightTestsDotNet.Fixture.Drivers.GameLogDrivers;

namespace PlaywrightTestsDotNet.Fixture.Utils;

public class BoardGameDsl
{
    private readonly IBoardGameDriver _boardGameDriver;
    private readonly IGameLogDriver _gameLogDriver;
    
    public BoardGameDsl(string channel)
    {
        _boardGameDriver = channel switch
        {
            "UI" => new UiBoardGameDriver(),
            "API" => new ApiBoardGameDriver(),
            _ => throw new NotImplementedException()
        };
        
        _gameLogDriver = channel switch
        {
            "UI" => new UiGameLogDriver(),
            "API" => new ApiGameLogDriver(),
            _ => throw new NotImplementedException()
        };
    }

    public async Task InitializeAsync()
    {
        await _boardGameDriver.InitializeAsync();
    }

    public async Task AddNewBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        await _boardGameDriver.SetupEntryPoint();
        await _boardGameDriver.CreateBoardGame(name, releaseDate, minNumberOfPlayers, maxNumberOfPlayers, minimumAge,
            difficulty, averagePlayingTime, categories);
        await _boardGameDriver.SaveBoardGame();
    }

    public async Task UpdateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        await _boardGameDriver.SetupEntryPoint();
        await _boardGameDriver.RefreshBoardGames();
        await _boardGameDriver.UpdateBoardGame(name, releaseDate, minNumberOfPlayers, maxNumberOfPlayers, minimumAge,
            difficulty, averagePlayingTime, categories);
        await _boardGameDriver.SaveBoardGame();
    }

    public async Task AssertBoardGameUpdated(string name, int releaseDate)
    {
        await _boardGameDriver.RefreshBoardGames();

        await _boardGameDriver.AssertBoardGameUpdated(name, releaseDate);
    }
    
    public async Task AssertBoardGameExists(string text)
    {
        await _boardGameDriver.RefreshBoardGames();
        
        await _boardGameDriver.AssertBoardGameExists(text);
    }

    public async Task AssertBoardGameDoesNotExist(string text)
    {
        await _boardGameDriver.RefreshBoardGames();
        
        await _boardGameDriver.AssertBoardGameDoesNotExist(text);
    }

    public async Task DeleteBoardGame(string name)
    {
        await _boardGameDriver.RefreshBoardGames();

        await _boardGameDriver.DeleteBoardGame(name);
    }

    public async Task AddBoardGameForDeletion(string name)
    {
        await AddNewBoardGame(name, 2020, 1, 4, 10, 2.5m, 60, "Family");
    }

    public async Task AddBoardGameWithFullDetails(string name, int releaseDate, int minNumberOfPlayers,
        int maxNumberOfPlayers, int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        await AddNewBoardGame(name, releaseDate, minNumberOfPlayers, maxNumberOfPlayers,
            minimumAge, difficulty, averagePlayingTime, categories);
    }

    public async Task AssertFieldIsNotEditable(string boardGame, string fieldName)
    {
        await _boardGameDriver.RefreshBoardGames();
        await _boardGameDriver.AssertFieldIsNotEditable(boardGame, fieldName);
    }

    public async Task CancelUpdate()
    {
        await _boardGameDriver.CancelUpdate();
    }
}