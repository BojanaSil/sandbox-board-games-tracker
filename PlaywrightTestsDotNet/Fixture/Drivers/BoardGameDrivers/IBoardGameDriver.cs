namespace PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;

public interface IBoardGameDriver
{
    Task InitializeAsync();
    
    Task SetupEntryPoint();

    Task CreateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories);

    Task UpdateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories);
    
    Task SaveBoardGame();

    Task CancelUpdate();

    Task RefreshBoardGames();

    Task AssertBoardGameUpdated(string name, int releaseDate);

    Task AssertBoardGameExists(string text);
    
    Task AssertBoardGameDoesNotExist(string text);

    Task DeleteBoardGame(string name);

    Task AssertFieldIsNotEditable(string boardGame, string fieldName);
}