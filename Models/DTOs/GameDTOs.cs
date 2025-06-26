using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models.DTOs;

public class CreateGameDto
{
    [Required]
    public int PlaygroupId { get; set; }

    public DateTime GameDate { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? Notes { get; set; }

    public List<CreateGamePlayerDto> Players { get; set; } = new();
}

public class CreateGamePlayerDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    public int? CommanderId { get; set; }

    [Range(1, 10)]
    public int Position { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? LifeTotal { get; set; }
}

public class GameDto
{
    public int Id { get; set; }
    public int PlaygroupId { get; set; }
    public string PlaygroupName { get; set; } = string.Empty;
    public DateTime GameDate { get; set; }
    public string? Notes { get; set; }
    public TimeSpan? Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public List<GamePlayerDto> Players { get; set; } = new();
}

public class GamePlayerDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int? CommanderId { get; set; }
    public string? CommanderName { get; set; }
    public string? CommanderColors { get; set; }
    public int Position { get; set; }
    public string? Notes { get; set; }
    public int? LifeTotal { get; set; }
    public bool IsWinner { get; set; }
}

public class PlayerStatsDto
{
    public string UserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int TotalGames { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public decimal WinRate { get; set; }
    public List<CommanderStatsDto> CommanderStats { get; set; } = new();
}

public class CommanderStatsDto
{
    public int CommanderId { get; set; }
    public string CommanderName { get; set; } = string.Empty;
    public string? CommanderColors { get; set; }
    public int GamesPlayed { get; set; }
    public int Wins { get; set; }
    public decimal WinRate { get; set; }
}
