# Use the .NET 7.0 SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ENV ASPNETCORE_ENVIRONMENT Development
WORKDIR /app
# Copy the solution file and restore dependencies
COPY *.sln .
COPY VIDconnectBackend/*.csproj ./VIDconnectBackend/
COPY VIDconnectSdk/*.csproj ./VIDconnectSdk/
RUN dotnet restore

# Copy the remaining source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "VIDconnectBackend.dll"]

