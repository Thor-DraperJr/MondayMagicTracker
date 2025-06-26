# Monday Magic Tracker

A comprehensive web application for tracking Magic: The Gathering games played with your playgroup every Monday (or any day)!

## 🚀 Current Deployment Status (June 26, 2025)

### ✅ **COMPLETED - Azure Infrastructure:**
- **Resource Group:** `MondayMagicTracker-rg` (East US)
- **Azure SQL Server:** `monday-magic-sql-2025.database.windows.net`
- **Azure SQL Database:** `MondayMagicTracker` (Basic tier, ready)
- **Azure Web App:** `monday-magic-tracker-app.azurewebsites.net` (Free tier, West US 2)
- **Azure Container Registry:** `mondaymagicacr2025` (Basic tier)
- **App Service Plan:** `MondayMagicTracker-plan` (Free tier)

### ✅ **COMPLETED - Code & Repository:**
- **GitHub Repository:** `https://github.com/Thor-DraperJr/MondayMagicTracker`
- **Full Application Code:** ASP.NET Core 8 + React 18 with TypeScript
- **GitHub Actions:** Configured for CI/CD deployment
- **Environment Variables:** Database connection string and JWT keys configured

### ✅ **COMPLETED - Application Features:**
- User authentication with JWT tokens
- Playgroup management system
- Game tracking with commander selection
- Statistics and analytics
- RESTful API with Swagger documentation
- Modern React frontend with responsive design

### 🔧 **IN PROGRESS - Deployment Issues:**
- **Issue:** Application showing Azure welcome page instead of React app
- **Root Cause:** App startup/configuration issue (not infrastructure)
- **Status:** Infrastructure is solid, debugging final app deployment

### 📋 **NEXT SESSION TODO:**
1. **Debug App Startup:** Check GitHub Actions deployment logs
2. **Test Health Endpoint:** `https://monday-magic-tracker-app.azurewebsites.net/health`
3. **Verify API Endpoints:** Test `/api/commanders` and `/swagger`
4. **Fix React Frontend:** Ensure SPA routing works correctly
5. **Test Full Application:** User registration, game tracking, etc.

### 🔑 **Important URLs:**
- **Live App:** `https://monday-magic-tracker-app.azurewebsites.net`
- **GitHub Repo:** `https://github.com/Thor-DraperJr/MondayMagicTracker`
- **GitHub Actions:** `https://github.com/Thor-DraperJr/MondayMagicTracker/actions`
- **Azure Portal:** Resource Group `MondayMagicTracker-rg`

### 💡 **Key Insights:**
- All Azure resources are successfully provisioned and running
- Database connection string and environment variables are configured
- The application builds successfully in GitHub Actions
- Issue is likely related to ASP.NET Core startup or React SPA configuration
- Infrastructure costs: ~$0-5/month (using free/basic tiers)

---

## Features

- **User Authentication**: Secure JWT-based authentication with ASP.NET Core Identity
- **Playgroup Management**: Create and manage playgroups with your friends
- **Game Tracking**: Record games with detailed information including:
  - Commander played by each player
  - Game results and positions
  - Additional notes and life totals
- **Statistics & Analytics**: 
  - Win/loss records for players
  - Commander performance tracking
  - Playgroup statistics
- **Modern UI**: React-based frontend with responsive design

## Technology Stack

### Backend
- **ASP.NET Core 8.0** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **ASP.NET Core Identity** - User authentication and authorization
- **JWT Authentication** - Secure token-based authentication
- **SQL Server** - Database (Azure SQL Database for production)
- **Swagger/OpenAPI** - API documentation

### Frontend
- **React 18** - Frontend framework
- **TypeScript** - Type-safe JavaScript
- **Modern CSS** - Responsive design

### Cloud & Deployment
- **Azure App Service** - Web application hosting
- **Azure SQL Database** - Managed database service
- **Azure DevOps** - CI/CD pipelines (optional)

## Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v16 or later)
- [SQL Server](https://www.microsoft.com/sql-server/) or SQL Server LocalDB

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Thor-DraperJr/MondayMagicTracker.git
   cd MondayMagicTracker
   ```

2. **Backend Setup**
   ```bash
   # Restore packages
   dotnet restore
   
   # Build the project
   dotnet build
   
   # Run the API (will be available at https://localhost:7000)
   dotnet run
   ```

3. **Frontend Setup**
   ```bash
   # Navigate to the React app
   cd ClientApp
   
   # Install dependencies
   npm install
   
   # Start the development server (will be available at http://localhost:3000)
   npm start
   ```

4. **Database Setup**
   The application will automatically create the database and seed it with sample commanders on first run.

### Configuration

#### Development
- Update `appsettings.Development.json` for local database connection
- JWT settings are configured for development use

#### Production
Set the following environment variables:
- `AZURE_SQL_CONNECTIONSTRING` - Azure SQL Database connection string
- `JWT_KEY` - Secure JWT signing key (32+ characters)

## API Documentation

Once the application is running, visit:
- **Swagger UI**: `https://localhost:7000/swagger`
- **API Base URL**: `https://localhost:7000/api`

### Key Endpoints

#### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user info

#### Playgroups
- `GET /api/playgroups` - Get user's playgroups
- `POST /api/playgroups` - Create new playgroup
- `POST /api/playgroups/{id}/members/{userId}` - Add member
- `DELETE /api/playgroups/{id}/members/{userId}` - Remove member

#### Games
- `GET /api/games/playgroup/{id}` - Get playgroup games
- `POST /api/games` - Record new game
- `GET /api/games/stats` - Get player statistics

#### Commanders
- `GET /api/commanders` - Get all commanders

## Project Structure

```
MondayMagicTracker/
├── Controllers/          # API Controllers
├── Models/              # Entity models and DTOs
│   ├── DTOs/           # Data Transfer Objects
│   ├── User.cs         # User entity
│   ├── Game.cs         # Game entity
│   ├── Playgroup.cs    # Playgroup entity
│   └── ...
├── Services/            # Business logic services
├── ClientApp/           # React frontend application
├── Pages/              # Razor pages (minimal use)
├── Program.cs          # Application entry point
└── appsettings.json    # Configuration
```

## Deployment to Azure

### Option 1: Azure App Service (Recommended)

1. **Create Azure Resources**
   ```bash
   # Create resource group
   az group create --name MondayMagicTracker-rg --location eastus
   
   # Create SQL Server and Database
   az sql server create --name <your-server-name> --resource-group MondayMagicTracker-rg --location eastus --admin-user <admin-username> --admin-password <admin-password>
   az sql db create --resource-group MondayMagicTracker-rg --server <your-server-name> --name MondayMagicTracker
   
   # Create App Service Plan
   az appservice plan create --name MondayMagicTracker-plan --resource-group MondayMagicTracker-rg --sku B1
   
   # Create Web App
   az webapp create --resource-group MondayMagicTracker-rg --plan MondayMagicTracker-plan --name <your-app-name> --runtime "DOTNET|8.0"
   ```

2. **Configure Application Settings**
   ```bash
   az webapp config appsettings set --resource-group MondayMagicTracker-rg --name <your-app-name> --settings \
     AZURE_SQL_CONNECTIONSTRING="Server=tcp:<your-server-name>.database.windows.net,1433;Initial Catalog=MondayMagicTracker;Persist Security Info=False;User ID=<admin-username>;Password=<admin-password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" \
     JWT_KEY="<your-secure-jwt-key-32-characters-or-more>"
   ```

3. **Deploy the Application**
   ```bash
   # Build and publish
   dotnet publish -c Release -o ./publish
   
   # Deploy (or use Visual Studio publish, GitHub Actions, etc.)
   az webapp deployment source config-zip --resource-group MondayMagicTracker-rg --name <your-app-name> --src ./publish.zip
   ```

### Option 2: Container Deployment
A Dockerfile is included for containerized deployment to Azure Container Instances or Azure Container Apps.

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Future Enhancements

- [ ] Mobile app with React Native
- [ ] Real-time game updates with SignalR
- [ ] Tournament bracket management
- [ ] Card price tracking integration
- [ ] Advanced analytics and reporting
- [ ] Social features (friend requests, challenges)
- [ ] Integration with MTG databases (Scryfall, EDHREC)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support, please open an issue in the GitHub repository or contact the development team.

---

**Happy Gaming!** 🎮⚔️