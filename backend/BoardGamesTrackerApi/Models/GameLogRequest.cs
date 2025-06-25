namespace BoardGamesTrackerApi.Models;

public class GameLogDto
{
    public Guid? Id { get; set; }
    public string BoardGameName { get; set; }
    public DateTime DateOfPlay { get; set; }
    public int TimesPlayed { get; set; }
    public int NumberOfPlayers { get; set; }
    public string Winner { get; set; }
    public int AverageDuration { get; set; }
}