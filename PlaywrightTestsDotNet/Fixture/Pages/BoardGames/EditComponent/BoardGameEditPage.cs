using System.Globalization;
using Microsoft.Playwright;

namespace PlaywrightTestsDotNet.Fixture.Pages.BoardGames.EditComponent;

public class BoardGameEditPage
{
    public BoardGameEditPage(IPage page)
    {
        Page = page;
    }

    private IPage Page { get; set; }

    public async Task OpenPageForCreate()
    {
        await Page.GotoAsync("/board-games");

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

    public async Task CreateBoardGame(string name, int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        await FillName(name);
        await FillReleaseDate(releaseDate);
        await FillMinNumberOfPlayers(minNumberOfPlayers);
        await FillMaxNumberOfPlayers(maxNumberOfPlayers);
        await FillMinimumAge(minimumAge);
        await FillDifficulty(difficulty);
        await FillAveragePlayingTime(averagePlayingTime);
        await FillCategories(categories);
    }

    public async Task UpdateBoardGame(int releaseDate, int minNumberOfPlayers, int maxNumberOfPlayers,
        int minimumAge, decimal difficulty, int averagePlayingTime, string categories)
    {
        await FillReleaseDate(releaseDate);
        await FillMinNumberOfPlayers(minNumberOfPlayers);
        await FillMaxNumberOfPlayers(maxNumberOfPlayers);
        await FillMinimumAge(minimumAge);
        await FillDifficulty(difficulty);
        await FillAveragePlayingTime(averagePlayingTime);
        await FillCategories(categories);
    }

    public async Task FillName(string name)
    {
        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Name" }).ClickAsync();
        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Name" }).ClearAsync();
        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Name" }).FillAsync(name);

        await Page.GetByRole(AriaRole.Option, new() { Name = name, Exact = true }).ClickAsync();
    }

    public async Task FillReleaseDate(int releaseDate)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Release date" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Release date" }).FillAsync(releaseDate.ToString());
    }

    public async Task FillMinNumberOfPlayers(int minNumberOfPlayers)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Min number of players" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Min number of players" })
            .FillAsync(minNumberOfPlayers.ToString());
    }

    public async Task FillMaxNumberOfPlayers(int maxNumberOfPlayers)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Max number of players" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Max number of players" })
            .FillAsync(maxNumberOfPlayers.ToString());
    }

    public async Task FillMinimumAge(int minimumAge)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Minimum age" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Minimum age" }).FillAsync(minimumAge.ToString());
    }

    public async Task FillDifficulty(decimal difficulty)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Difficulty" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Difficulty" })
            .FillAsync(difficulty.ToString(CultureInfo.InvariantCulture));
    }

    public async Task FillAveragePlayingTime(int averagePlayingTime)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Average playing time in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Average playing time in" })
            .FillAsync(averagePlayingTime.ToString());
    }

    public async Task FillCategories(string categories)
    {
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Categories" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Categories" }).FillAsync(categories);
    }

    public async Task  SaveBoardGame()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    }
}