namespace BoardGamesTrackerApi.Models;

public class BoardGame
{
    public BoardGame(){}
    public BoardGame(Guid id, string name, int releaseDate, int averagePlayingTimeInMinutes, int minNumberOfPlayers, 
        int maxNumberOfPlayers, int minimumAge, decimal difficulty, List<BoardGameCategory> boardGameCategories)
    {
        Id = id;
        Name = name;
        ReleaseDate = releaseDate;
        AveragePlayingTimeInMinutes = averagePlayingTimeInMinutes;
        MinNumberOfPlayers = minNumberOfPlayers;
        MaxNumberOfPlayers = maxNumberOfPlayers;
        MinimumAge = minimumAge;
        Difficulty = difficulty;
        BoardGameCategories = boardGameCategories;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public int ReleaseDate { get; set; }
    public int AveragePlayingTimeInMinutes { get; set; }
    public int MinNumberOfPlayers { get; set; }
    public int MaxNumberOfPlayers { get; set; }
    public int MinimumAge { get; set; }
    public decimal Difficulty { get; set; }

    public List<BoardGameCategory> BoardGameCategories { get; set; }
    public List<GameLog> GameLogs { get; set; }

    public string Categories => BoardGameCategories != null ? string.Join(",", BoardGameCategories.Select(e => e.Category.Name)) : null;
}