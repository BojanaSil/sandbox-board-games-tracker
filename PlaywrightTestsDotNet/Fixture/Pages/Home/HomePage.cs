using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.BoardGames.ListComponent;

namespace PlaywrightTestsDotNet.Fixture.Pages.Home;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    public async Task OpenPage()
    {
        // TODO: Configure this in variable group in pipeline
        await _page.GotoAsync("http://localhost:4200/home");
    }

    public async Task AssertTitleMatches(string title)
    {
        await Assertions.Expect(_page).ToHaveTitleAsync(title);
    }

    public async Task AssertButtonExists(string button)
    {
        await Assertions.Expect(_page.GetByRole(AriaRole.Button, new() { Name = button })).ToBeVisibleAsync();
    }

    public async Task<BoardGameListPage> ClickBoardGamesButton()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Board games" }).ClickAsync();
        return new BoardGameListPage(_page);
    }
    
    public async Task ClickHomeButton()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Home" }).ClickAsync();
    }

    public async Task RefreshBoardGamesPage()
    {
        await ClickHomeButton();
        await ClickBoardGamesButton();
    }
}