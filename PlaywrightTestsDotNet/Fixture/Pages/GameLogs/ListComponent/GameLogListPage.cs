using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.GameLogs.EditComponent;

namespace PlaywrightTestsDotNet.Fixture.Pages.GameLogs.ListComponent;

public class GameLogListPage
{
    public GameLogListPage(IPage page)
    {
        Page = page;
    }

    private IPage Page { get; set; }
    
    public async Task OpenPage()
    {
        await Page.GotoAsync("/game-logs");
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
    
    public async Task<GameLogEditPage> OpenEditPageForCreate()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();
        return new GameLogEditPage(Page);
    }
    
    public async Task<GameLogEditPage> OpenEditPageForUpdate(string name)
    {
        await Page.GetByRole(AriaRole.Row)
            .Filter(new LocatorFilterOptions { HasText = name }).First
            .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Edit" }).ClickAsync();
        return new GameLogEditPage(Page);
    }
}