# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/out .

# Force app to run on port 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "jwtAuth.dll"]
