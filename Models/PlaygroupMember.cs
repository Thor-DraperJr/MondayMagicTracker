using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models;

public class PlaygroupMember
{
    public int Id { get; set; }

    [Required]
    public int PlaygroupId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Playgroup Playgroup { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
