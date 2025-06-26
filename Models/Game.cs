using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models;

public class Game
{
    public int Id { get; set; }

    [Required]
    public int PlaygroupId { get; set; }

    public DateTime GameDate { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? Notes { get; set; }

    public TimeSpan? Duration { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;

    // Navigation properties
    public virtual Playgroup Playgroup { get; set; } = null!;
    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
}
