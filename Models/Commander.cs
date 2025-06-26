using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models;

public class Commander
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Colors { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
}
