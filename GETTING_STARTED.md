# Monday Magic Tracker - Quick Start Guide

## 🎯 What You Have

You now have a fully functional Magic: The Gathering game tracking web application with:

### ✅ Features Implemented
- **User Authentication** - JWT-based secure login/registration
- **Playgroup Management** - Create and manage groups with friends
- **Game Tracking** - Record games with commanders and results
- **Statistics** - Win/loss tracking and commander performance
- **Modern API** - RESTful API with Swagger documentation
- **Azure Ready** - Configured for cloud deployment

### ✅ Technology Stack
- **Backend**: ASP.NET Core 8.0 Web API
- **Frontend**: React 18 with TypeScript (ready to customize)
- **Database**: SQL Server / Azure SQL Database
- **Authentication**: JWT tokens with ASP.NET Core Identity
- **Cloud**: Azure App Service ready with CI/CD pipeline

## 🚀 Next Steps

### 1. Test the API (Right Now!)
The API is running at: http://localhost:5000

**View API Documentation:**
```bash
# Open in browser
curl http://localhost:5000/swagger
```

**Test User Registration:**
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "testuser",
    "email": "test@example.com",
    "displayName": "Test User",
    "password": "TestPass123!",
    "bio": "MTG enthusiast"
  }'
```

### 2. Set Up Local Database (Optional)
For full functionality, set up SQL Server:

**Using Docker:**
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

**Update connection string in appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=MondayMagicTrackerDev;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;"
  }
}
```

### 3. Customize the React Frontend
```bash
cd ClientApp
npm install
npm start
```

The React app template is ready for customization with:
- User authentication pages
- Playgroup management UI
- Game tracking forms
- Statistics dashboards

### 4. Deploy to Azure

**Quick Deploy:**
1. Fork the repository: https://github.com/Thor-DraperJr/MondayMagicTracker.git
2. Use the one-click Azure deploy button in the README
3. Configure your GitHub Actions secrets for automated deployments

**Manual Deploy:**
```bash
# See azure/README.md for detailed instructions
az group create --name MondayMagicTracker-rg --location eastus
az deployment group create --resource-group MondayMagicTracker-rg --template-file azure/azuredeploy.json
```

## 📖 Key API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user

### Playgroups
- `GET /api/playgroups` - Get user's playgroups
- `POST /api/playgroups` - Create playgroup
- `POST /api/playgroups/{id}/members/{userId}` - Add member

### Games
- `GET /api/games/playgroup/{id}` - Get playgroup games
- `POST /api/games` - Record new game
- `GET /api/games/stats` - Get player statistics

### Commanders
- `GET /api/commanders` - Get all commanders (seeded with popular ones)

## 🛠️ Development Commands

```bash
# Build and run API
dotnet run

# Watch mode (auto-reload)
dotnet watch run

# Run tests
dotnet test

# Build for production
dotnet publish -c Release

# Frontend development
cd ClientApp && npm start
```

## 🎮 Sample Data

The application comes pre-seeded with popular commanders:
- Atraxa, Praetors' Voice
- Edgar Markov
- The Ur-Dragon
- Korvold, Fae-Cursed King
- Meren of Clan Nel Toth

## 🔧 Configuration

### Environment Variables
- `AZURE_SQL_CONNECTIONSTRING` - Database connection
- `JWT_KEY` - JWT signing key (32+ characters)
- `ASPNETCORE_ENVIRONMENT` - Development/Production

### Security Notes
- Change JWT key in production
- Use Azure Key Vault for secrets
- Configure CORS for your domain
- Set up SSL certificates

## 📝 What's Next?

1. **Customize the React Frontend** - Build the UI your friend wants
2. **Add More Features** - Tournament brackets, deck tracking, etc.
3. **Deploy to Azure** - Get it running in the cloud
4. **Set up CI/CD** - Automatic deployments from GitHub
5. **Monitor & Scale** - Add Application Insights, scale as needed

## 🤝 Support

- **Documentation**: Check the README.md and azure/README.md
- **API Docs**: Visit /swagger when running
- **Issues**: Create issues on the GitHub repository
- **Customization**: Use the .github/copilot-instructions.md for AI assistance

---

**Your Magic tracker is ready to roll! 🎲⚔️**

Stop the current application with `Ctrl+C` and start customizing the React frontend to match your friend's needs!
