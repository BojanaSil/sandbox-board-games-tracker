using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.BoardGames.EditComponent;
using PlaywrightTestsDotNet.Fixture.Pages.BoardGames.ListComponent;
using PlaywrightTestsDotNet.Fixture.Pages.Home;
using PlaywrightTestsDotNet.Fixture.Utils;

namespace PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;

public class UiBoardGameDriver : IBoardGameDriver
{
    private BoardGameListPage _boardGameListPage;
    private BoardGameEditPage _boardGameEditPage;
    private HomePage _homePage;

    public async Task SetupEntryPoint()
    {
        await _homePage.OpenPage();
        _boardGameListPage = await _homePage.ClickBoardGamesButton();
    }

    public async Task CreateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        _boardGameEditPage = await _boardGameListPage.OpenEditPageForCreate();
        await _boardGameEditPage.CreateBoardGame(name, releaseDate, minNumberOfPlayers, maxNumberOfPlayers, minimumAge,
            difficulty, averagePlayingTime, categories);
    }

    public async Task UpdateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers, int minimumAge,
        decimal difficulty, int averagePlayingTime, string categories)
    {
        _boardGameEditPage = await _boardGameListPage.OpenEditPageForUpdate(name);
        await _boardGameEditPage.UpdateBoardGame(releaseDate, minNumberOfPlayers, maxNumberOfPlayers, minimumAge,
            difficulty, averagePlayingTime, categories);
    }

    public async Task SaveBoardGame()
    {
        await _boardGameEditPage.SaveBoardGame();
    }

    public async Task CancelUpdate()
    {
        await _boardGameEditPage.CancelBoardGameUpdate();
    }

    public async Task RefreshBoardGames()
    {
        await _homePage.RefreshBoardGamesPage();
    }

    public async Task AssertBoardGameUpdated(string name, int releaseDate)
    {
        await Assertions.Expect(_boardGameListPage.GetTableElement(releaseDate.ToString())).ToHaveCountAsync(1);
    }

    public async Task AssertBoardGameExists(string text)
    {
        await Assertions.Expect(_boardGameListPage.GetTableElement(text)).ToHaveCountAsync(1);
    }

    public async Task AssertBoardGameDoesNotExist(string text)
    {
        await Assertions.Expect(_boardGameListPage.GetTableElement(text)).ToHaveCountAsync(0);
    }

    public async Task DeleteBoardGame(string name)
    {
        await _boardGameEditPage.DeleteRow(name);
    }

    public async Task InitializeAsync()
    {
        _homePage = await BrowserContextManager.GetOrCreateContextAsync();
    }

    public async Task DisposeAsync()
    {
        await BrowserContextManager.DisposeAsync();
    }

    public async Task AssertFieldIsNotEditable(string boardGame, string fieldName)
    {
        _boardGameEditPage = await _boardGameListPage.OpenEditPageForUpdate(boardGame);
        var locator = _boardGameEditPage.GetTextField(fieldName);
        await Assertions.Expect(locator).ToBeDisabledAsync();
        await CancelUpdate();
    }
}