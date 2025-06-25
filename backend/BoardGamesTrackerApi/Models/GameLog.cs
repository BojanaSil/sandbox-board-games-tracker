namespace BoardGamesTrackerApi.Models;

public class GameLog
{
    public GameLog()
    {
    }
    
    public GameLog(Guid id, Guid boardGameId, DateTime dateOfPlay, int timesPlayed, int numberOfPlayers, string winner,
        int averageDuration)
    {
        Id = id;
        BoardGameId = boardGameId;
        DateOfPlay = dateOfPlay;
        TimesPlayed = timesPlayed;
        NumberOfPlayers = numberOfPlayers;
        Winner = winner;
        AverageDuration = averageDuration;
    }

    public Guid Id { get; set; }
    public Guid BoardGameId { get; set; }
    public DateTime DateOfPlay { get; set; }
    public int TimesPlayed { get; set; }
    public int NumberOfPlayers { get; set; }
    public string Winner { get; set; }
    public int AverageDuration { get; set; }

    public BoardGame BoardGame { get; set; }
}