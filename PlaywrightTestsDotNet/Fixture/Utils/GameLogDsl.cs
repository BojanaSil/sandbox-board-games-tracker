using PlaywrightTestsDotNet.Fixture.Drivers.GameLogDrivers;

namespace PlaywrightTestsDotNet.Fixture.Utils;

public class GameLogDsl
{
    private readonly IGameLogDriver _gameLogDriver;
    
    public GameLogDsl(string channel)
    {
        _gameLogDriver = channel switch
        {
            "UI" => new UiGameLogDriver(),
            "API" => new ApiGameLogDriver(),
            _ => throw new NotImplementedException()
        };
    }

    public async Task InitializeAsync()
    {
        await _gameLogDriver.InitializeAsync();
    }

    public async Task AddNewGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner)
    {
        await _gameLogDriver.SetupEntryPoint();
        await _gameLogDriver.CreateGameLog(boardGameName, dateOfPlay, timesPlayed, numberOfPlayers, averageDuration, winner);
        await _gameLogDriver.SaveGameLog();
    }

    public async Task UpdateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner)
    {
        await _gameLogDriver.SetupEntryPoint();
        await _gameLogDriver.RefreshGameLogs();
        await _gameLogDriver.UpdateGameLog(boardGameName, dateOfPlay, timesPlayed, numberOfPlayers, averageDuration, winner);
        await _gameLogDriver.SaveGameLog();
    }

    public async Task AssertGameLogUpdated(string boardGameName, int timesPlayed)
    {
        await _gameLogDriver.RefreshGameLogs();

        await _gameLogDriver.AssertGameLogUpdated(boardGameName, timesPlayed);
    }
    
    public async Task AssertGameLogExists(string text)
    {
        await _gameLogDriver.RefreshGameLogs();
        
        await _gameLogDriver.AssertGameLogExists(text);
    }

    public async Task AssertGameLogDoesNotExist(string text)
    {
        await _gameLogDriver.RefreshGameLogs();
        
        await _gameLogDriver.AssertGameLogDoesNotExist(text);
    }

    public async Task DeleteGameLog(string boardGameName, DateTime dateOfPlay)
    {
        await _gameLogDriver.RefreshGameLogs();

        await _gameLogDriver.DeleteGameLog(boardGameName, dateOfPlay);
    }

    public async Task AddGameLogForDeletion(string boardGameName, DateTime dateOfPlay)
    {
        await AddNewGameLog(boardGameName, dateOfPlay, 1, 4, 60, "Jane Doe");
    }

    public async Task AddGameLogWithFullDetails(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner)
    {
        await AddNewGameLog(boardGameName, dateOfPlay, timesPlayed, numberOfPlayers, averageDuration, winner);
    }
}