using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGamesTrackerApi.Models;

public class BoardGameCategory
{
    public Guid Id { get; set; }
    public Guid BoardGameId { get; set; }
    public Guid CategoryId { get; set; }

    [NotMapped]
    public BoardGame BoardGame { get; set; }
    public Category Category { get; set; }
}