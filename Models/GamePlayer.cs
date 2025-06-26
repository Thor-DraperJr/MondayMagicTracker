using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models;

public class GamePlayer
{
    public int Id { get; set; }

    [Required]
    public int GameId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public int? CommanderId { get; set; }

    public int Position { get; set; } // 1 = Winner, 2 = Second, etc.

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? LifeTotal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Game Game { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Commander? Commander { get; set; }

    // Helper properties
    public bool IsWinner => Position == 1;
}
