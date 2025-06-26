# Monday Magic Tracker

A comprehensive web application for tracking Magic: The Gathering games played with your playgroup every Monday (or any day)!

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