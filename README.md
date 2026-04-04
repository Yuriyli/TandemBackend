
# TandemBackend

.NET Web API for the [Tandem](https://github.com/AnnStarrySky/tandem) application.

## Deployment Links

- **Development environment**: http://45.12.130.140:1314/swagger/
- **Production environment**: http://45.12.130.140:1315/swagger/
- **Endpoint for custom openApi user**: `/openapi/v1.json`

## Local Build and Run

- Create a `/database` folder in the root of the project

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 10.0 or higher)
- [Docker](https://docker.com) (optional, for containerization)
- Install [secret](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows) (optional): `"AuthOptions:Key": "AnyRandomString12345"`

### Build and Run without Docker

```bash

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```
## Docker Containers

### Download Pre-built Images

```bash
# Development version
docker pull yuriyli/tandembackend:dev

# Production version
docker pull yuriyli/tandembackend:latest
```

### Create Volume for Data Storage
```bash
# Create volume for development
docker volume create tandem-dev

# Create volume for production
docker volume create tandem-latest
```

### Example Commands to Run Containers
```bash
# Run development container
docker run --mount type=volume,source=tandem-dev,target=/app/database -d -p 1314:8080 yuriyli/tandembackend:dev

# Run production container
docker run --mount type=volume,source=tandem-latest,target=/app/database -d -p 1315:8080 yuriyli/tandembackend:latest
```

### Useful Commands

```bash
# View running containers
docker ps

# View container logs
docker logs <container-id>

# Stop a container
docker stop <container-id>

# Remove a container
docker rm <container-id>

# View volume information
docker volume inspect tandem-dev

# Clean up unused containers and images
docker container prune
docker image prune
```