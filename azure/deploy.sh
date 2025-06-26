#!/bin/bash

# Monday Magic Tracker - Azure Deployment Script
# This script deploys the Monday Magic Tracker application to Azure

set -e

# Configuration
RESOURCE_GROUP="MondayMagicTracker-rg"
LOCATION="eastus"
SITE_NAME="monday-magic-tracker-$(date +%s)"
SQL_SERVER_NAME="monday-magic-sql-$(date +%s)"
SQL_ADMIN_LOGIN="mtgadmin"
SQL_ADMIN_PASSWORD="$(openssl rand -base64 32 | tr -d "=+/" | cut -c1-25)!"
JWT_KEY="$(openssl rand -base64 32 | tr -d "=+/")"

echo "🎮 Monday Magic Tracker - Azure Deployment"
echo "==========================================="
echo ""
echo "Configuration:"
echo "  Resource Group: $RESOURCE_GROUP"
echo "  Location: $LOCATION"
echo "  Site Name: $SITE_NAME"
echo "  SQL Server: $SQL_SERVER_NAME"
echo "  SQL Admin: $SQL_ADMIN_LOGIN"
echo ""

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    echo "❌ Azure CLI is not installed. Please install it first."
    echo "   Visit: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli"
    exit 1
fi

# Check if user is logged in
if ! az account show &> /dev/null; then
    echo "🔐 Please log in to Azure CLI..."
    az login
fi

echo "📦 Creating resource group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "🚀 Deploying Azure resources..."
az deployment group create \
    --resource-group $RESOURCE_GROUP \
    --template-file azure/deploy.json \
    --parameters \
        siteName=$SITE_NAME \
        sqlServerName=$SQL_SERVER_NAME \
        sqlAdminLogin=$SQL_ADMIN_LOGIN \
        sqlAdminPassword=$SQL_ADMIN_PASSWORD \
        jwtKey=$JWT_KEY

echo "📱 Building and publishing application..."
dotnet publish -c Release -o ./publish

echo "📦 Creating deployment package..."
cd publish
zip -r ../deploy.zip .
cd ..

echo "🚀 Deploying application to Azure App Service..."
az webapp deployment source config-zip \
    --resource-group $RESOURCE_GROUP \
    --name $SITE_NAME \
    --src deploy.zip

echo ""
echo "✅ Deployment completed successfully!"
echo ""
echo "🌐 Your application is available at: https://$SITE_NAME.azurewebsites.net"
echo "📊 SQL Server: $SQL_SERVER_NAME.database.windows.net"
echo ""
echo "🔐 Save these credentials securely:"
echo "   SQL Admin Username: $SQL_ADMIN_LOGIN"
echo "   SQL Admin Password: $SQL_ADMIN_PASSWORD"
echo "   JWT Key: $JWT_KEY"
echo ""
echo "📝 To manage your resources:"
echo "   az group show --name $RESOURCE_GROUP"
echo "   az webapp show --resource-group $RESOURCE_GROUP --name $SITE_NAME"
echo ""
echo "🗑️  To delete all resources:"
echo "   az group delete --name $RESOURCE_GROUP --yes --no-wait"
echo ""
echo "🎉 Happy gaming!"
