using BoardGamesTrackerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGamesTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameLogController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public GameLogController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameLogDto>> GetGameLog(Guid id)
    {
        var gameLog = await _dbContext.GameLogs
            .Include(e => e.BoardGame)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (gameLog is null)
        {
            return NotFound();
        }

        return Ok(new GameLogDto
        {
            Id = gameLog.Id,
            BoardGameName = gameLog.BoardGame.Name,
            AverageDuration = gameLog.AverageDuration,
            DateOfPlay = gameLog.DateOfPlay,
            NumberOfPlayers = gameLog.NumberOfPlayers,
            TimesPlayed = gameLog.TimesPlayed,
            Winner = gameLog.Winner
        });
    }

    [HttpPost]
    public async Task<ActionResult> CreateGameLog(GameLogDto gameLogDto)
    {
        var gameLogId = Guid.NewGuid();
        
        var boardGame = await _dbContext.BoardGames.FirstOrDefaultAsync(e => e.Name == gameLogDto.BoardGameName);

        if (boardGame is null)
        {
            return NotFound($"Board game with name {gameLogDto.BoardGameName} is not found");
        }

        var gameLog = new GameLog(gameLogId, boardGame.Id, gameLogDto.DateOfPlay,
            gameLogDto.TimesPlayed, gameLogDto.NumberOfPlayers, gameLogDto.Winner, gameLogDto.AverageDuration);

        await _dbContext.AddAsync(gameLog);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGameLog(Guid id, GameLogDto gameLogDto)
    {
        if (id != gameLogDto.Id)
        {
            return BadRequest("Ids don't match.");
        }
        
        var gameLogDb = await _dbContext.GameLogs.FirstOrDefaultAsync(e => e.Id == id);

        if (gameLogDb is null)
        {
            return NotFound();
        }

        var boardGame = await _dbContext.BoardGames.FirstOrDefaultAsync(e => e.Name == gameLogDto.BoardGameName);

        if (boardGame is null)
        {
            return NotFound($"Board game with name {gameLogDto.BoardGameName} is not found");
        }
        
        gameLogDb.BoardGameId = boardGame.Id;
        gameLogDb.DateOfPlay = gameLogDto.DateOfPlay;
        gameLogDb.TimesPlayed = gameLogDto.TimesPlayed;
        gameLogDb.NumberOfPlayers = gameLogDto.NumberOfPlayers;
        gameLogDb.Winner = gameLogDto.Winner;
        gameLogDb.AverageDuration = gameLogDto.AverageDuration;

        _dbContext.Update(gameLogDb);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGameLog(Guid id)
    {
        var gameLog = await _dbContext.GameLogs.FirstOrDefaultAsync(e => e.Id == id);

        if (gameLog is null)
        {
            return NotFound();
        }

        _dbContext.Remove(gameLog);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameLogDto>>> GetAllGameLogs()
    {
        var gameLogs = await _dbContext.GameLogs.Include(e => e.BoardGame).ToListAsync();
        return Ok(gameLogs.Select(e => new GameLogDto
        {
            Id = e.Id,
            BoardGameName = e.BoardGame.Name,
            AverageDuration = e.AverageDuration,
            DateOfPlay = e.DateOfPlay,
            NumberOfPlayers = e.NumberOfPlayers,
            TimesPlayed = e.TimesPlayed,
            Winner = e.Winner
        }));
    }
}