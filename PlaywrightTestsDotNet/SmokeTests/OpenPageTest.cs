using System.Text.RegularExpressions;
using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture;
using PlaywrightTestsDotNet.Fixture.Pages.Home;

namespace PlaywrightTestsDotNet.SmokeTests;

public class OpenPageTest : TestFixture
{
    private HomePage _homePage;
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        
        _homePage = new HomePage(page);
    }

    [Fact]
    public async Task HasTitle()
    {
        await _homePage.OpenPage();

        await _homePage.AssertTitleMatches("BoardGameTracker");
    }

    [Fact]
    public async Task HasButtons()
    {
        await _homePage.OpenPage();

        await _homePage.AssertButtonExists("Board games");
        await _homePage.AssertButtonExists("Game logs");
        await _homePage.AssertButtonExists("Home");
    }
} 