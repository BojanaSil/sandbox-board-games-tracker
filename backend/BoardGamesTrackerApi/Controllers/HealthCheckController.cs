using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly DatabaseContext _context;

    public HealthCheckController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("api")]
    public ActionResult HealthCheck()
    {
        return Ok("API Works!");
    }

    [HttpGet("db")]
    public ActionResult DbHealthCheck()
    {
        var canConnectToDb = _context.Database.CanConnect();
        if (canConnectToDb)
        {
            return Ok("Database works!");
        }

        return UnprocessableEntity("Database not working!");
    }
}