using System.Text.Json.Serialization;
using BoardGamesTrackerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BoardGamesTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardGameController : ControllerBase
{
    private readonly DatabaseContext _dbContext;
    private readonly HttpClient _httpClient;

    public BoardGameController(DatabaseContext dbContext, HttpClient httpClient)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BoardGame>> GetBoardGame(Guid id)
    {
        var boardGame = await _dbContext.BoardGames
            .Include(e => e.BoardGameCategories)
            .ThenInclude(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (boardGame is null)
        {
            return NotFound();
        }

        return Ok(boardGame);
    }

    [HttpPost]
    public async Task<ActionResult> CreateBoardGame(BoardGameRequest boardGameRequest)
    {
        var boardGameId = Guid.NewGuid();
        var categories = await _dbContext.Categories.Where(e => boardGameRequest.Categories.Contains(e.Name))
            .ToListAsync();

        var boardGameCategories = boardGameRequest.BoardGameCategories.Select(e =>
        {
            var category = categories.FirstOrDefault(f => f.Name == e);
            if (category is null)
            {
                return new BoardGameCategory
                {
                    Id = Guid.NewGuid(), BoardGameId = boardGameId,
                    Category = new Category { Id = Guid.NewGuid(), Name = e }
                };
            }

            return new BoardGameCategory
                { Id = Guid.NewGuid(), BoardGameId = boardGameId, CategoryId = category.Id };
        }).ToList();
        var boardGame = new BoardGame(boardGameId, boardGameRequest.Name, boardGameRequest.ReleaseDate,
            boardGameRequest.AveragePlayingTimeInMinutes, boardGameRequest.MinNumberOfPlayers,
            boardGameRequest.MaxNumberOfPlayers, boardGameRequest.MinimumAge, boardGameRequest.Difficulty,
            boardGameCategories);

        await _dbContext.AddAsync(boardGame);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBoardGame(Guid id, BoardGameRequest boardGameRequest)
    {
        if (id != boardGameRequest.Id)
        {
            return BadRequest("Ids don't match.");
        }
        
        var boardGameDb = await _dbContext.BoardGames.Include(e => e.BoardGameCategories).ThenInclude(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (boardGameDb is null)
        {
            return NotFound();
        }
        
        var categories = await _dbContext.Categories.AsNoTracking().Where(e => boardGameRequest.Categories.Contains(e.Name))
            .ToListAsync();

        var boardGameCategories = boardGameRequest.BoardGameCategories.Select(e =>
        {
            var category = categories.FirstOrDefault(f => f.Name == e);
            if (category is null)
            {
                return new BoardGameCategory
                {
                    Id = Guid.NewGuid(), BoardGameId = id,
                    Category = new Category { Id = Guid.NewGuid(), Name = e }
                };
            }

            return new BoardGameCategory
                { Id = Guid.NewGuid(), BoardGameId = id, CategoryId = category.Id };
        }).ToList();

        boardGameDb.MinimumAge = boardGameRequest.MinimumAge;
        boardGameDb.Difficulty = boardGameRequest.Difficulty;
        boardGameDb.ReleaseDate = boardGameRequest.ReleaseDate;
        boardGameDb.MaxNumberOfPlayers = boardGameRequest.MaxNumberOfPlayers;
        boardGameDb.MinNumberOfPlayers = boardGameRequest.MinNumberOfPlayers;

        var addedBgc = boardGameCategories.Where(e => !boardGameDb.BoardGameCategories.Select(f => f.CategoryId).Contains(e.CategoryId)).ToList();
        var deletedBgc = boardGameDb.BoardGameCategories.Where(e => !boardGameCategories.Select(f => f.CategoryId).Contains(e.CategoryId)).ToList();

        foreach (var bgc in deletedBgc)
        {
            _dbContext.BoardGameCategories.Remove(bgc);
        }
        _dbContext.BoardGameCategories.AddRange(addedBgc);
        _dbContext.Update(boardGameDb);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBoardGame(Guid id)
    {
        var boardGame = await _dbContext.BoardGames.FirstOrDefaultAsync(e => e.Id == id);

        if (boardGame is null)
        {
            return NotFound();
        }

        _dbContext.Remove(boardGame);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BoardGame>>> GetAllBoardGames()
    {
        var boardGames = await _dbContext.BoardGames.Include(e => e.BoardGameCategories).ThenInclude(e => e.Category)
            .ToListAsync();
        return boardGames;
    }
    
    [HttpGet("query")]
    public async Task<ActionResult<IEnumerable<BoardGame>>> QueryBoardGames(string name)
    {
        var boardGames = await _dbContext.BoardGames.Where(e => e.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
        return boardGames;
    }

    [HttpGet("getBggBoardGames")]
    public async Task<List<string>> GetBggBoardGames(string name)
    {
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://boardgamegeek.com/search/boardgame?q={name}&nosession=1&showcount=10")
            //RequestUri = new Uri("http://localhost:5009/data") 
            // TODO: This Uri would be set through variable group in pipeline, depending on type of tests
        };
        httpRequestMessage.Headers.Add("Accept", "application/json");

        var gamesResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        var bggResponseString = await gamesResponseMessage.Content.ReadAsStringAsync();
        var bggResponse = JsonConvert.DeserializeObject<BggBoardGameResponse>(bggResponseString);

        return bggResponse.Items.Select(e => e.Name).ToList();
    }
}