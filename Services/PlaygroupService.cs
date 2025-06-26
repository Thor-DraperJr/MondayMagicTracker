using Microsoft.EntityFrameworkCore;
using MondayMagicTracker.Models;
using MondayMagicTracker.Models.DTOs;

namespace MondayMagicTracker.Services;

public interface IPlaygroupService
{
    Task<IEnumerable<PlaygroupDto>> GetUserPlaygroupsAsync(string userId);
    Task<PlaygroupDto?> GetPlaygroupByIdAsync(int playgroupId, string userId);
    Task<PlaygroupDto> CreatePlaygroupAsync(CreatePlaygroupDto createDto, string ownerId);
    Task<bool> AddMemberToPlaygroupAsync(int playgroupId, string userId, string requestingUserId);
    Task<bool> RemoveMemberFromPlaygroupAsync(int playgroupId, string userId, string requestingUserId);
    Task<IEnumerable<PlaygroupMemberDto>> GetPlaygroupMembersAsync(int playgroupId, string requestingUserId);
}

public class PlaygroupService : IPlaygroupService
{
    private readonly DatabaseContext _context;

    public PlaygroupService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlaygroupDto>> GetUserPlaygroupsAsync(string userId)
    {
        var playgroups = await _context.PlaygroupMembers
            .Where(pm => pm.UserId == userId && pm.IsActive)
            .Include(pm => pm.Playgroup)
            .ThenInclude(p => p.Owner)
            .Select(pm => new PlaygroupDto
            {
                Id = pm.Playgroup.Id,
                Name = pm.Playgroup.Name,
                Description = pm.Playgroup.Description,
                OwnerId = pm.Playgroup.OwnerId,
                OwnerName = pm.Playgroup.Owner.DisplayName,
                CreatedAt = pm.Playgroup.CreatedAt,
                IsActive = pm.Playgroup.IsActive,
                MemberCount = pm.Playgroup.Members.Count(m => m.IsActive),
                GameCount = pm.Playgroup.Games.Count
            })
            .ToListAsync();

        return playgroups;
    }

    public async Task<PlaygroupDto?> GetPlaygroupByIdAsync(int playgroupId, string userId)
    {
        var playgroup = await _context.Playgroups
            .Include(p => p.Owner)
            .Include(p => p.Members.Where(m => m.IsActive))
            .Include(p => p.Games)
            .FirstOrDefaultAsync(p => p.Id == playgroupId);

        if (playgroup == null || !playgroup.Members.Any(m => m.UserId == userId))
            return null;

        return new PlaygroupDto
        {
            Id = playgroup.Id,
            Name = playgroup.Name,
            Description = playgroup.Description,
            OwnerId = playgroup.OwnerId,
            OwnerName = playgroup.Owner.DisplayName,
            CreatedAt = playgroup.CreatedAt,
            IsActive = playgroup.IsActive,
            MemberCount = playgroup.Members.Count,
            GameCount = playgroup.Games.Count
        };
    }

    public async Task<PlaygroupDto> CreatePlaygroupAsync(CreatePlaygroupDto createDto, string ownerId)
    {
        var playgroup = new Playgroup
        {
            Name = createDto.Name,
            Description = createDto.Description,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Playgroups.Add(playgroup);
        await _context.SaveChangesAsync();

        // Add the owner as a member
        var member = new PlaygroupMember
        {
            PlaygroupId = playgroup.Id,
            UserId = ownerId,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.PlaygroupMembers.Add(member);
        await _context.SaveChangesAsync();

        // Load the owner information
        var owner = await _context.Users.FindAsync(ownerId);

        return new PlaygroupDto
        {
            Id = playgroup.Id,
            Name = playgroup.Name,
            Description = playgroup.Description,
            OwnerId = playgroup.OwnerId,
            OwnerName = owner?.DisplayName ?? "",
            CreatedAt = playgroup.CreatedAt,
            IsActive = playgroup.IsActive,
            MemberCount = 1,
            GameCount = 0
        };
    }

    public async Task<bool> AddMemberToPlaygroupAsync(int playgroupId, string userId, string requestingUserId)
    {
        var playgroup = await _context.Playgroups
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == playgroupId);

        if (playgroup == null || playgroup.OwnerId != requestingUserId)
            return false;

        var existingMember = playgroup.Members.FirstOrDefault(m => m.UserId == userId);
        if (existingMember != null)
        {
            if (!existingMember.IsActive)
            {
                existingMember.IsActive = true;
                existingMember.JoinedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        var member = new PlaygroupMember
        {
            PlaygroupId = playgroupId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.PlaygroupMembers.Add(member);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveMemberFromPlaygroupAsync(int playgroupId, string userId, string requestingUserId)
    {
        var playgroup = await _context.Playgroups
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == playgroupId);

        if (playgroup == null || (playgroup.OwnerId != requestingUserId && userId != requestingUserId))
            return false;

        var member = playgroup.Members.FirstOrDefault(m => m.UserId == userId && m.IsActive);
        if (member == null)
            return false;

        member.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PlaygroupMemberDto>> GetPlaygroupMembersAsync(int playgroupId, string requestingUserId)
    {
        var playgroup = await _context.Playgroups
            .Include(p => p.Members.Where(m => m.IsActive))
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(p => p.Id == playgroupId);

        if (playgroup == null || !playgroup.Members.Any(m => m.UserId == requestingUserId))
            return Enumerable.Empty<PlaygroupMemberDto>();

        return playgroup.Members.Select(m => new PlaygroupMemberDto
        {
            Id = m.Id,
            UserId = m.UserId,
            UserName = m.User.UserName ?? "",
            DisplayName = m.User.DisplayName,
            JoinedAt = m.JoinedAt,
            IsActive = m.IsActive
        });
    }
}
