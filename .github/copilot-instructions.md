<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# Monday Magic Tracker - Copilot Instructions

**Repository**: https://github.com/Thor-DraperJr/MondayMagicTracker.git

This is a Magic the Gathering game tracking web application built with:
- **Backend**: ASP.NET Core 8.0 Web API with Entity Framework Core
- **Frontend**: React with TypeScript (in ClientApp folder)
- **Database**: SQL Server (Azure SQL Database for production)
- **Authentication**: JWT tokens with ASP.NET Core Identity

## Key Features
- User registration and authentication
- Playgroup management (create groups, add/remove members)
- Game tracking with commander information
- Win/loss statistics
- Player performance analytics

## Architecture Guidelines
- Use Repository pattern through DbContext
- Services handle business logic (AuthService, PlaygroupService, GameService)
- Controllers are thin and delegate to services
- DTOs for API communication
- Entity models for database mapping

## Code Patterns
- Use async/await for all database operations
- Follow RESTful API conventions
- Implement proper error handling and validation
- Use Authorization attributes for protected endpoints
- Return appropriate HTTP status codes

## Azure Integration
- Configured for Azure App Service deployment
- Azure SQL Database connection strings
- Environment-based configuration
- Ready for Azure AD B2C integration

## When generating code:
- Follow C# naming conventions (PascalCase for public members)
- Use nullable reference types appropriately
- Implement proper logging using ILogger
- Add XML documentation for public APIs
- Use Entity Framework migrations for schema changes
- Implement DTOs for all API endpoints
