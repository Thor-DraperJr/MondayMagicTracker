using Microsoft.AspNetCore.Mvc;
using MondayMagicTracker.Models;
using MondayMagicTracker.Models.DTOs;
using MondayMagicTracker.Services;

namespace MondayMagicTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : AuthorizedController
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("playgroup/{playgroupId}")]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetPlaygroupGames(int playgroupId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var games = await _gameService.GetPlaygroupGamesAsync(playgroupId, userId);
            return Ok(games);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var game = await _gameService.GetGameByIdAsync(id, userId);
            
            if (game == null)
            {
                return NotFound("Game not found or you don't have access");
            }

            return Ok(game);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> CreateGame([FromBody] CreateGameDto createDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var game = await _gameService.CreateGameAsync(createDto, userId);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPut("{id}/complete")]
    public async Task<ActionResult> CompleteGame(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _gameService.CompleteGameAsync(id, userId);
            
            if (!success)
            {
                return BadRequest("Unable to complete game. Game may not exist or you may not have access.");
            }

            return Ok(new { message = "Game completed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("stats")]
    public async Task<ActionResult<PlayerStatsDto>> GetMyStats([FromQuery] int? playgroupId = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _gameService.GetPlayerStatsAsync(userId, playgroupId);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("stats/{userId}")]
    public async Task<ActionResult<PlayerStatsDto>> GetPlayerStats(string userId, [FromQuery] int? playgroupId = null)
    {
        try
        {
            var stats = await _gameService.GetPlayerStatsAsync(userId, playgroupId);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class CommandersController : AuthorizedController
{
    private readonly IGameService _gameService;

    public CommandersController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Commander>>> GetCommanders()
    {
        try
        {
            var commanders = await _gameService.GetCommandersAsync();
            return Ok(commanders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
