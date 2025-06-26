# Azure Deployment Guide

## Quick Deploy to Azure

### Option 1: One-Click Deploy
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FThor-DraperJr%2FMondayMagicTracker%2Fmain%2Fazure%2Fazuredeploy.json)

### Option 2: Azure CLI
```bash
# Create resource group
az group create --name MondayMagicTracker-rg --location eastus

# Deploy infrastructure
az deployment group create \
  --resource-group MondayMagicTracker-rg \
  --template-file azure/azuredeploy.json \
  --parameters azure/azuredeploy.parameters.json

# Get the web app name
WEBAPP_NAME=$(az deployment group show --resource-group MondayMagicTracker-rg --name azuredeploy --query properties.outputs.webAppUrl.value --output tsv | sed 's/https:\/\///' | sed 's/\.azurewebsites\.net//')

# Deploy the application
dotnet publish -c Release
cd bin/Release/net8.0/publish
zip -r ../../../../../deploy.zip .
cd ../../../../../

az webapp deployment source config-zip \
  --resource-group MondayMagicTracker-rg \
  --name $WEBAPP_NAME \
  --src deploy.zip
```

### Option 3: Azure Developer CLI (azd)
```bash
# Initialize azd
azd init --template https://github.com/Thor-DraperJr/MondayMagicTracker

# Deploy to Azure
azd up
```

## Configuration

### Required Environment Variables
- `AZURE_SQL_CONNECTIONSTRING` - Database connection string
- `JWT_KEY` - JWT signing key (32+ characters)

### Optional Environment Variables
- `ASPNETCORE_ENVIRONMENT` - Set to "Production" for production deployment

## Security Considerations

1. **Change default passwords** in `azuredeploy.parameters.json`
2. **Generate a secure JWT key** (32+ characters)
3. **Configure SQL Server firewall rules** as needed
4. **Enable Azure AD authentication** for additional security
5. **Use Azure Key Vault** for sensitive configuration in production

## Monitoring

- **Application Insights** - Add for application monitoring
- **Log Analytics** - For centralized logging
- **Azure Monitor** - For infrastructure monitoring

## Scaling

- **App Service Plan** - Scale up/out as needed
- **Azure SQL Database** - Consider higher tiers for production
- **Azure CDN** - For static content delivery
