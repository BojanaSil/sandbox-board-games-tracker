namespace BoardGamesTrackerApi.Models;

public class BggBoardGameResponse
{
    public List<BggBoardGame> Items { get; set; }
}

public class BggBoardGame
{
    public string Name { get; set; }
    public int YearPublished { get; set; }
}