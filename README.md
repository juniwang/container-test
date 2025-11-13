# container-test

A simple .NET 8.0 Hello World web application that can be containerized and deployed to Azure.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

## Running Locally

```bash
cd src
dotnet run
```

The app will be available at `http://localhost:5000` (or the port shown in the console).

## Building and Pushing to Azure Container Registry

### 1. Create an Azure Container Registry (if you don't have one)

```bash
# Login to Azure
az login

# Set variables
$RESOURCE_GROUP="myResourceGroup"
$ACR_NAME="mycontainerregistry"  # Must be globally unique, lowercase alphanumeric only
$LOCATION="eastus"

# Create resource group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create Azure Container Registry
az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic
```

### 2. Build and Push the Docker Image

```bash
# Login to Azure Container Registry
az acr login --name $ACR_NAME

# Build the Docker image (run from the project root, not the src directory)
cd container-test
docker build -t "${ACR_NAME}.azurecr.io/hello-world:latest" -f src/Dockerfile src/

# Push the image to ACR
docker push "${ACR_NAME}.azurecr.io/hello-world:latest"
```

### 3. Verify the Image

```bash
# List repositories in ACR
az acr repository list --name $ACR_NAME --output table

# Show tags for the hello-world repository
az acr repository show-tags --name $ACR_NAME --repository hello-world --output table
```

### 4. Run the Container Locally (Optional)

```bash
# Pull and run the image from ACR
docker run -d -p 8080:8080 ${ACR_NAME}.azurecr.io/hello-world:latest

# Test the application
curl http://localhost:8080
```

## Quick Build and Push Script

```powershell
# Set your ACR name
$ACR_NAME="mycontainerregistry"

# Login and push
az acr login --name $ACR_NAME
docker build -t ${ACR_NAME}.azurecr.io/hello-world:latest -f src/Dockerfile src/
docker push ${ACR_NAME}.azurecr.io/hello-world:latest
```

## Deploy to Azure Container Apps or AKS

After pushing to ACR, you can deploy the image to:
- Azure Container Apps
- Azure Kubernetes Service (AKS)
- Azure Container Instances (ACI)

Refer to Azure documentation for deployment instructions specific to your chosen service.
