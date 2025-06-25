using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.BoardGames.EditComponent;
using PlaywrightTestsDotNet.Fixture.Pages.BoardGames.ListComponent;
using PlaywrightTestsDotNet.Fixture.Pages.Home;

namespace PlaywrightTestsDotNet.Fixture.Drivers.BoardGameDrivers;

public class UiBoardGameDriver : IBoardGameDriver
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;
    
    private BoardGameListPage _boardGameListPage;
    private BoardGameEditPage _boardGameEditPage;
    private HomePage _homePage;

    public async Task VisitPage()
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
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();

        _homePage = new HomePage(_page);
    }

    public async Task DisposeAsync()
    {
        await _browser.DisposeAsync();
        _playwright.Dispose();
    }
}