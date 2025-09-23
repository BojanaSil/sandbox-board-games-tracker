namespace PlaywrightTestsDotNet.Fixture.Drivers.GameLogDrivers;

public interface IGameLogDriver
{
    Task InitializeAsync();
    
    Task SetupEntryPoint();

    Task CreateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner);

    Task UpdateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, 
        int averageDuration, string winner);
    
    Task SaveGameLog();

    Task RefreshGameLogs();

    Task AssertGameLogUpdated(string boardGameName, int timesPlayed);

    Task AssertGameLogExists(string text);
    
    Task AssertGameLogDoesNotExist(string text);

    Task DeleteGameLog(string boardGameName, DateTime dateOfPlay);
}