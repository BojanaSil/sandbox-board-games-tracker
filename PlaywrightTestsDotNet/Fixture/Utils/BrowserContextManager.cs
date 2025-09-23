using Microsoft.Playwright;
using PlaywrightTestsDotNet.Fixture.Pages.Home;

namespace PlaywrightTestsDotNet.Fixture.Utils;

public static class BrowserContextManager
{
    private static IPlaywright _playwright;
    private static IBrowser _browser;
    private static IBrowserContext _context;
    private static IPage _page;
    private static HomePage _homePage;
    private static readonly object _lock = new object();

    public static async Task<HomePage> GetOrCreateContextAsync()
    {
        lock (_lock)
        {
            if (_homePage != null)
            {
                return _homePage;
            }
        }

        // Initialize if not already done
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
        _homePage = new HomePage(_page);

        return _homePage;
    }

    public static async Task DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.DisposeAsync();
            _browser = null;
        }

        if (_playwright != null)
        {
            _playwright.Dispose();
            _playwright = null;
        }

        _context = null;
        _page = null;
        _homePage = null;
    }

    public static void Reset()
    {
        // This method can be used to force recreation of browser context
        // without disposing the browser
        _context = null;
        _page = null;
        _homePage = null;
    }
}