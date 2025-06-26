using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models;

public class User : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Bio { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
    public virtual ICollection<PlaygroupMember> PlaygroupMemberships { get; set; } = new List<PlaygroupMember>();
    public virtual ICollection<Playgroup> OwnedPlaygroups { get; set; } = new List<Playgroup>();
}
