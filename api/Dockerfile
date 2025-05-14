# Use the official .NET image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY api.csproj .

# Restore dependencies
RUN dotnet restore api.csproj

# Copy the rest of the application code
COPY . .
WORKDIR /src/.

# Build the application
RUN dotnet build "./api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "./api.csproj" -c Release -o /app/publish

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "api.dll"]
