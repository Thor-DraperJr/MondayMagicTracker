using System.ComponentModel.DataAnnotations;

namespace MondayMagicTracker.Models;

public class Playgroup
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public string OwnerId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual User Owner { get; set; } = null!;
    public virtual ICollection<PlaygroupMember> Members { get; set; } = new List<PlaygroupMember>();
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
