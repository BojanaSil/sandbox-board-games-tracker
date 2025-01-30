using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGamesTrackerApi.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [NotMapped]
    public List<BoardGameCategory> BoardGameCategories { get; set; }
}