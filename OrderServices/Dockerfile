# Use the official Microsoft .NET image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
# Install curl for debugging purposes
RUN apt-get update && apt-get install -y curl && apt-get clean

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# Copy the csproj and restore any dependencies (via dotnet restore)
COPY OrderServices/OrderServices.csproj .
RUN dotnet restore "OrderServices.csproj"

# Publish the application
FROM build AS publish
# Copy all contents to current directory
COPY . .
# Navigate to the API directory
WORKDIR "/src/OrderServices"
# Build and publish the application to /app/publish directory in the image
RUN dotnet publish "OrderServices.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create a final image for running the application
FROM base AS final
WORKDIR /app
# Copy the publish folder to our final image
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderServices.dll"]
