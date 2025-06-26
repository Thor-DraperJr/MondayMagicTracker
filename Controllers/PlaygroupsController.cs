using Microsoft.AspNetCore.Mvc;
using MondayMagicTracker.Models.DTOs;
using MondayMagicTracker.Services;

namespace MondayMagicTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaygroupsController : AuthorizedController
{
    private readonly IPlaygroupService _playgroupService;

    public PlaygroupsController(IPlaygroupService playgroupService)
    {
        _playgroupService = playgroupService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaygroupDto>>> GetMyPlaygroups()
    {
        try
        {
            var userId = GetCurrentUserId();
            var playgroups = await _playgroupService.GetUserPlaygroupsAsync(userId);
            return Ok(playgroups);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlaygroupDto>> GetPlaygroup(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var playgroup = await _playgroupService.GetPlaygroupByIdAsync(id, userId);
            
            if (playgroup == null)
            {
                return NotFound("Playgroup not found or you don't have access");
            }

            return Ok(playgroup);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<PlaygroupDto>> CreatePlaygroup([FromBody] CreatePlaygroupDto createDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var playgroup = await _playgroupService.CreatePlaygroupAsync(createDto, userId);
            return CreatedAtAction(nameof(GetPlaygroup), new { id = playgroup.Id }, playgroup);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("{id}/members")]
    public async Task<ActionResult<IEnumerable<PlaygroupMemberDto>>> GetPlaygroupMembers(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var members = await _playgroupService.GetPlaygroupMembersAsync(id, userId);
            return Ok(members);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost("{id}/members/{memberId}")]
    public async Task<ActionResult> AddMember(int id, string memberId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _playgroupService.AddMemberToPlaygroupAsync(id, memberId, userId);
            
            if (!success)
            {
                return BadRequest("Unable to add member. You may not be the owner or the user may already be a member.");
            }

            return Ok(new { message = "Member added successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpDelete("{id}/members/{memberId}")]
    public async Task<ActionResult> RemoveMember(int id, string memberId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _playgroupService.RemoveMemberFromPlaygroupAsync(id, memberId, userId);
            
            if (!success)
            {
                return BadRequest("Unable to remove member. You may not have permission or the user may not be a member.");
            }

            return Ok(new { message = "Member removed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
