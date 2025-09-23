using System.Globalization;
using Microsoft.Playwright;

namespace PlaywrightTestsDotNet.Fixture.Pages.GameLogs.EditComponent;

public class GameLogEditPage
{
    public GameLogEditPage(IPage page)
    {
        Page = page;
    }

    private IPage Page { get; set; }

    public async Task OpenPageForCreate()
    {
        await Page.GotoAsync("/game-logs");

        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Create" }).ClickAsync();
    }

    public async Task OpenPageForEdit(string name)
    {
        await Page.GetByRole(AriaRole.Row)
            .Filter(new LocatorFilterOptions { HasText = name }).First
            .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Edit" }).ClickAsync();
    }

    public async Task DeleteRow(string name)
    {
        await Page.GetByRole(AriaRole.Row)
            .Filter(new LocatorFilterOptions { HasText = name }).First
            .GetByRole(AriaRole.Button, new LocatorGetByRoleOptions { Name = "Delete" }).ClickAsync();
    }

    public async Task CreateGameLog(string boardGameName, DateTime dateOfPlay, int timesPlayed, 
        int numberOfPlayers, int averageDuration, string winner)
    {
        await FillBoardGame(boardGameName);
        await FillDateOfPlay(dateOfPlay);
        await FillTimesPlayed(timesPlayed);
        await FillNumberOfPlayers(numberOfPlayers);
        await FillAverageDuration(averageDuration);
        await FillWinner(winner);
    }

    public async Task UpdateGameLog(DateTime dateOfPlay, int timesPlayed, 
        int numberOfPlayers, int averageDuration, string winner)
    {
        await FillDateOfPlay(dateOfPlay);
        await FillTimesPlayed(timesPlayed);
        await FillNumberOfPlayers(numberOfPlayers);
        await FillAverageDuration(averageDuration);
        await FillWinner(winner);
    }

    public async Task FillBoardGame(string boardGameName)
    {
        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Game" }).ClickAsync();
        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Game" }).ClearAsync();
        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Game" }).FillAsync(boardGameName);

        // Wait for debounce (1000ms) plus API call and options to appear
        await Page.WaitForTimeoutAsync(1500);

        // Try different selectors for the autocomplete option
        await Page.GetByRole(AriaRole.Option, new() { Name = boardGameName, Exact = true }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 10000
        });

        await Page.GetByRole(AriaRole.Option, new() { Name = boardGameName, Exact = true }).ClickAsync();
    }

    public async Task FillDateOfPlay(DateTime dateOfPlay)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Date of play" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Date of play" }).FillAsync(dateOfPlay.ToString("yyyy-MM-dd"));
    }

    public async Task FillNumberOfPlayers(int numberOfPlayers)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Number of players" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Number of players" })
            .FillAsync(numberOfPlayers.ToString());
    }

    public async Task FillTimesPlayed(int timesPlayed)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Number of times played" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Number of times played" })
            .FillAsync(timesPlayed.ToString());
    }

    public async Task FillAverageDuration(int averageDuration)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Average duration" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Average duration" })
            .FillAsync(averageDuration.ToString());
    }

    public async Task FillWinner(string winner)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Winner" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Winner" }).FillAsync(winner);
    }

    public async Task  SaveGameLog()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    }
}