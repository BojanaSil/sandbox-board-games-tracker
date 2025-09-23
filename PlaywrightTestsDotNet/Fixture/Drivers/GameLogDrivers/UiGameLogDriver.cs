using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.GameLogs.EditComponent;
using PlaywrightTestsDotNet.Fixture.Pages.GameLogs.ListComponent;
using PlaywrightTestsDotNet.Fixture.Pages.Home;
using PlaywrightTestsDotNet.Fixture.Utils;

namespace PlaywrightTestsDotNet.Fixture.Drivers.GameLogDrivers;

public class UiGameLogDriver : IGameLogDriver
{
    private GameLogListPage _gameLogListPage;
    private GameLogEditPage _gameLogEditPage;
    private HomePage _homePage;

    public async Task SetupEntryPoint()
    {
        await _homePage.OpenPage();
        _gameLogListPage = await _homePage.ClickGameLogsButton();
    }

    public async Task CreateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, int averageDuration,
        string winner)
    {
        _gameLogEditPage = await _gameLogListPage.OpenEditPageForCreate();
        await _gameLogEditPage.CreateGameLog(boardGameName, dateOfPlay, timesPlayed, numberOfPlayers, averageDuration,
            winner);
    }

    public async Task UpdateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, int averageDuration,
        string winner)
    {
        _gameLogEditPage = await _gameLogListPage.OpenEditPageForUpdate(boardGameName);
        await _gameLogEditPage.UpdateGameLog(dateOfPlay, timesPlayed, numberOfPlayers, averageDuration,
            winner);
    }

    public async Task SaveGameLog()
    {
        await _gameLogEditPage.SaveGameLog();
    }

    public async Task RefreshGameLogs()
    {
        await _homePage.RefreshGameLogsPage();
    }

    public async Task AssertGameLogUpdated(string boardGameName, int releaseDate)
    {
        await Assertions.Expect(_gameLogListPage.GetTableElement(releaseDate.ToString())).ToHaveCountAsync(1);
    }

    public async Task AssertGameLogExists(string text)
    {
        await Assertions.Expect(_gameLogListPage.GetTableElement(text)).ToHaveCountAsync(1);
    }

    public async Task AssertGameLogDoesNotExist(string text)
    {
        await Assertions.Expect(_gameLogListPage.GetTableElement(text)).ToHaveCountAsync(0);
    }

    public Task DeleteGameLog(string boardGameName, DateTime dateOfPlay)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteGameLog(string boardGameName)
    {
        await _gameLogEditPage.DeleteRow(boardGameName);
    }

    public async Task InitializeAsync()
    {
        _homePage = await BrowserContextManager.GetOrCreateContextAsync();
    }

    public async Task DisposeAsync()
    {
        await BrowserContextManager.DisposeAsync();
    }
}