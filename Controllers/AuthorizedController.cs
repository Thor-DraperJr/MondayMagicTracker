using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MondayMagicTracker.Controllers;

[Authorize]
public abstract class AuthorizedController : ControllerBase
{
    // Protected helper method to get the JWT claim related to the user Id
    protected string GetCurrentUserId()
    {
        // Get the User Id from the claim
        return User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException("User ID not found in token");
    }

    // Helper method to get the current user's name
    protected string GetCurrentUserName()
    {
        return User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value ?? "";
    }

    // Helper method to get the current user's display name
    protected string GetCurrentUserDisplayName()
    {
        return User.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value ?? "";
    }
}
