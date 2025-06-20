namespace PlaywrightTestsDotNet.Fixture.Utils;

public class BoardGameModel
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int ReleaseDate { get; set; }
    public int AveragePlayingTimeInMinutes { get; set; }
    public int MinNumberOfPlayers { get; set; }
    public int MaxNumberOfPlayers { get; set; }
    public int MinimumAge { get; set; }
    public decimal Difficulty { get; set; }
    public string Categories { get; set; }
}