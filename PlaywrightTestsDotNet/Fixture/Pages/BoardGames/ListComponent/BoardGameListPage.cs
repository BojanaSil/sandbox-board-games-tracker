using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.BoardGames.EditComponent;

namespace PlaywrightTestsDotNet.Fixture.Pages.BoardGames.ListComponent;

public class BoardGameListPage
{
    public BoardGameListPage(IPage page)
    {
        Page = page;
    }

    private IPage Page { get; set; }
    
    public async Task OpenPage()
    {
        await Page.GotoAsync("/board-games");
    }

    public ILocator GetTable()
    {
        return Page.GetByRole(AriaRole.Table);
    }

    public ILocator GetTableElement(string text)
    {
        return Page.GetByRole(AriaRole.Row)
            .Filter(new LocatorFilterOptions { HasText = text });
    }
    
    public async Task<BoardGameEditPage> OpenEditPageForCreate()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();
        return new BoardGameEditPage(Page);
    }
    
    public async Task<BoardGameEditPage> OpenEditPageForUpdate(string name)
    {
        await Page.GetByRole(AriaRole.Row)
            .Filter(new LocatorFilterOptions { HasText = name }).First
            .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Edit" }).ClickAsync();
        return new BoardGameEditPage(Page);
    }
}