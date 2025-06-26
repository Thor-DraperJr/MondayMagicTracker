using Microsoft.EntityFrameworkCore;
using MondayMagicTracker.Models;
using MondayMagicTracker.Models.DTOs;

namespace MondayMagicTracker.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetPlaygroupGamesAsync(int playgroupId, string userId);
    Task<GameDto?> GetGameByIdAsync(int gameId, string userId);
    Task<GameDto> CreateGameAsync(CreateGameDto createDto, string userId);
    Task<bool> CompleteGameAsync(int gameId, string userId);
    Task<PlayerStatsDto> GetPlayerStatsAsync(string userId, int? playgroupId = null);
    Task<IEnumerable<Commander>> GetCommandersAsync();
}

public class GameService : IGameService
{
    private readonly DatabaseContext _context;

    public GameService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GameDto>> GetPlaygroupGamesAsync(int playgroupId, string userId)
    {
        // Verify user is member of playgroup
        var isMember = await _context.PlaygroupMembers
            .AnyAsync(pm => pm.PlaygroupId == playgroupId && pm.UserId == userId && pm.IsActive);

        if (!isMember)
            return Enumerable.Empty<GameDto>();

        var games = await _context.Games
            .Where(g => g.PlaygroupId == playgroupId)
            .Include(g => g.Playgroup)
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.User)
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.Commander)
            .OrderByDescending(g => g.GameDate)
            .Select(g => new GameDto
            {
                Id = g.Id,
                PlaygroupId = g.PlaygroupId,
                PlaygroupName = g.Playgroup.Name,
                GameDate = g.GameDate,
                Notes = g.Notes,
                Duration = g.Duration,
                CreatedAt = g.CreatedAt,
                IsCompleted = g.IsCompleted,
                Players = g.GamePlayers.Select(gp => new GamePlayerDto
                {
                    Id = gp.Id,
                    UserId = gp.UserId,
                    UserName = gp.User.UserName ?? "",
                    DisplayName = gp.User.DisplayName,
                    CommanderId = gp.CommanderId,
                    CommanderName = gp.Commander != null ? gp.Commander.Name : null,
                    CommanderColors = gp.Commander != null ? gp.Commander.Colors : null,
                    Position = gp.Position,
                    Notes = gp.Notes,
                    LifeTotal = gp.LifeTotal,
                    IsWinner = gp.IsWinner
                }).OrderBy(gp => gp.Position).ToList()
            })
            .ToListAsync();

        return games;
    }

    public async Task<GameDto?> GetGameByIdAsync(int gameId, string userId)
    {
        var game = await _context.Games
            .Include(g => g.Playgroup)
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.User)
            .Include(g => g.GamePlayers)
            .ThenInclude(gp => gp.Commander)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null)
            return null;

        // Verify user is member of playgroup
        var isMember = await _context.PlaygroupMembers
            .AnyAsync(pm => pm.PlaygroupId == game.PlaygroupId && pm.UserId == userId && pm.IsActive);

        if (!isMember)
            return null;

        return new GameDto
        {
            Id = game.Id,
            PlaygroupId = game.PlaygroupId,
            PlaygroupName = game.Playgroup.Name,
            GameDate = game.GameDate,
            Notes = game.Notes,
            Duration = game.Duration,
            CreatedAt = game.CreatedAt,
            IsCompleted = game.IsCompleted,
            Players = game.GamePlayers.Select(gp => new GamePlayerDto
            {
                Id = gp.Id,
                UserId = gp.UserId,
                UserName = gp.User.UserName ?? "",
                DisplayName = gp.User.DisplayName,
                CommanderId = gp.CommanderId,
                CommanderName = gp.Commander?.Name,
                CommanderColors = gp.Commander?.Colors,
                Position = gp.Position,
                Notes = gp.Notes,
                LifeTotal = gp.LifeTotal,
                IsWinner = gp.IsWinner
            }).OrderBy(gp => gp.Position).ToList()
        };
    }

    public async Task<GameDto> CreateGameAsync(CreateGameDto createDto, string userId)
    {
        // Verify user is member of playgroup
        var isMember = await _context.PlaygroupMembers
            .AnyAsync(pm => pm.PlaygroupId == createDto.PlaygroupId && pm.UserId == userId && pm.IsActive);

        if (!isMember)
            throw new UnauthorizedAccessException("User is not a member of this playgroup");

        var game = new Game
        {
            PlaygroupId = createDto.PlaygroupId,
            GameDate = createDto.GameDate,
            Notes = createDto.Notes,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = true // Mark as completed since we're adding results
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        // Add players
        var gamePlayers = new List<GamePlayer>();
        foreach (var playerDto in createDto.Players)
        {
            var gamePlayer = new GamePlayer
            {
                GameId = game.Id,
                UserId = playerDto.UserId,
                CommanderId = playerDto.CommanderId,
                Position = playerDto.Position,
                Notes = playerDto.Notes,
                LifeTotal = playerDto.LifeTotal,
                CreatedAt = DateTime.UtcNow
            };

            gamePlayers.Add(gamePlayer);
            _context.GamePlayers.Add(gamePlayer);
        }

        await _context.SaveChangesAsync();

        // Return the created game
        return await GetGameByIdAsync(game.Id, userId) ?? throw new InvalidOperationException("Failed to retrieve created game");
    }

    public async Task<bool> CompleteGameAsync(int gameId, string userId)
    {
        var game = await _context.Games
            .Include(g => g.Playgroup)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null)
            return false;

        // Verify user is member of playgroup
        var isMember = await _context.PlaygroupMembers
            .AnyAsync(pm => pm.PlaygroupId == game.PlaygroupId && pm.UserId == userId && pm.IsActive);

        if (!isMember)
            return false;

        game.IsCompleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PlayerStatsDto> GetPlayerStatsAsync(string userId, int? playgroupId = null)
    {
        var query = _context.GamePlayers
            .Include(gp => gp.Game)
            .Include(gp => gp.User)
            .Include(gp => gp.Commander)
            .Where(gp => gp.UserId == userId && gp.Game.IsCompleted);

        if (playgroupId.HasValue)
        {
            query = query.Where(gp => gp.Game.PlaygroupId == playgroupId.Value);
        }

        var gamePlayers = await query.ToListAsync();
        var user = await _context.Users.FindAsync(userId);

        var totalGames = gamePlayers.Count;
        var wins = gamePlayers.Count(gp => gp.IsWinner);
        var losses = totalGames - wins;
        var winRate = totalGames > 0 ? (decimal)wins / totalGames * 100 : 0;

        var commanderStats = gamePlayers
            .Where(gp => gp.Commander != null)
            .GroupBy(gp => new { gp.CommanderId, gp.Commander!.Name, gp.Commander.Colors })
            .Select(g => new CommanderStatsDto
            {
                CommanderId = g.Key.CommanderId!.Value,
                CommanderName = g.Key.Name,
                CommanderColors = g.Key.Colors,
                GamesPlayed = g.Count(),
                Wins = g.Count(gp => gp.IsWinner),
                WinRate = g.Count() > 0 ? (decimal)g.Count(gp => gp.IsWinner) / g.Count() * 100 : 0
            })
            .OrderByDescending(cs => cs.GamesPlayed)
            .ToList();

        return new PlayerStatsDto
        {
            UserId = userId,
            DisplayName = user?.DisplayName ?? "",
            TotalGames = totalGames,
            Wins = wins,
            Losses = losses,
            WinRate = winRate,
            CommanderStats = commanderStats
        };
    }

    public async Task<IEnumerable<Commander>> GetCommandersAsync()
    {
        return await _context.Commanders
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
