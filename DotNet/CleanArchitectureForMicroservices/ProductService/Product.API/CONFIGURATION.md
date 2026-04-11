# ProductService Configuration & Deployment Guide

## ⚙️ Development Configuration

### 1. appsettings.Development.json (Optional)
Create for development-specific settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=PARADOX\\SQLEXPRESS;Database=ProductDb_Dev;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
  },
  "Jwt": {
    "Key": "DEVELOPMENT_KEY_12345678901234567890",
    "Issuer": "ProductServiceAPI",
    "Audience": "ProductServiceClient",
    "DurationInMinutes": 1440
  }
}
```

### 2. appsettings.Production.json (Recommended)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=ProductDb;User Id=sa;Password=YourStrongPassword;"
  },
  "Jwt": {
    "Key": "YOUR_VERY_LONG_RANDOM_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "ProductServiceAPI",
    "Audience": "ProductServiceClient",
    "DurationInMinutes": 60
  }
}
```

---

## 🔐 Security Best Practices

### JWT Key Generation
```bash
# PowerShell
[Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes((New-Guid).ToString() + (New-Guid).ToString()))

# Linux/Mac
echo -n "$(uuidgen)$(uuidgen)" | base64
```

**Requirements:**
- Minimum 32 characters
- Avoid dictionary words
- Change in production
- Store securely (use Azure Key Vault, AWS Secrets Manager, etc.)

### CORS Configuration

**For Development** (Allow All):
```csharp
// In Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

**For Production** (Specific Domain):
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", policy =>
    {
        policy.WithOrigins("https://yourdomain.com", "https://api.yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

app.UseCors("AllowSpecific");
```

---

## 🗄️ Database Setup

### SQL Server Connection Strings

**Windows Authentication:**
```
Data Source=SERVER_NAME;Database=ProductDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True
```

**SQL Authentication:**
```
Server=SERVER_NAME;Database=ProductDb;User Id=sa;Password=YourPassword;Encrypt=True;
```

**Azure SQL Database:**
```
Server=servername.database.windows.net;Database=ProductDb;User Id=sqladmin;Password=YourPassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### Database Initialization

**Option 1: Code First with Migrations**
```bash
# Install EF Core Tools if not already installed
dotnet tool install -g dotnet-ef

# Navigate to solution directory
cd ProductService

# Create migration
dotnet ef migrations add InitialCreate -p Product.Infrastructure -s Product.API

# Apply migration
dotnet ef database update -p Product.Infrastructure -s Product.API
```

**Option 2: Database First (Existing Database)**
```bash
dotnet ef dbcontext scaffold "connection_string" Microsoft.EntityFrameworkCore.SqlServer -p Product.Infrastructure -o Models
```

### Initial Data Seeding

Add to `ProductDbContext.cs`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // ... existing configurations ...

    // Seed sample data
    modelBuilder.Entity<ProductEntity>().HasData(
        new ProductEntity
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name = "Sample Laptop",
            Description = "High-performance laptop",
            Price = 999.99m,
            Stock = 10,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    );
}
```

Then create migration:
```bash
dotnet ef migrations add SeedInitialData -p Product.Infrastructure -s Product.API
dotnet ef database update -p Product.Infrastructure -s Product.API
```

---

## 🐳 Docker Deployment

### Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution files
COPY ["Product.API/Product.API.csproj", "Product.API/"]
COPY ["Product.Application/Product.Application.csproj", "Product.Application/"]
COPY ["Product.Domain/Product.Domain.csproj", "Product.Domain/"]
COPY ["Product.Infrastructure/Product.Infrastructure.csproj", "Product.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Product.API/Product.API.csproj"

# Copy source code
COPY . .

# Build
RUN dotnet build "Product.API/Product.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Product.API/Product.API.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80 443
ENTRYPOINT ["dotnet", "Product.API.dll"]
```

### docker-compose.yml

```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourStrongPassword123!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

  productapi:
    build: .
    depends_on:
      - sqlserver
    ports:
      - "5001:80"
    environment:
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=ProductDb;User Id=sa;Password=YourStrongPassword123!;"
      Jwt__Key: "YOUR_JWT_KEY_HERE"
      ASPNETCORE_ENVIRONMENT: "Production"

volumes:
  sqlserver_data:
```

**Build and Run:**
```bash
docker-compose up -d
```

---

## ☁️ Cloud Deployment

### Azure App Service

1. **Create Resource Group**
```bash
az group create -n ProductServiceRG -l eastus
```

2. **Create App Service Plan**
```bash
az appservice plan create -n ProductServicePlan -g ProductServiceRG --sku B1
```

3. **Create Web App**
```bash
az webapp create -n productserviceapi -g ProductServiceRG --plan ProductServicePlan --runtime "dotnet:10"
```

4. **Deploy**
```bash
dotnet publish -c Release -o ./publish
az webapp deployment source config-zip -r productserviceapi -g ProductServiceRG --src publish.zip
```

### AWS Elastic Beanstalk

1. **Install EB CLI**
```bash
pip install awsebcli
```

2. **Initialize EB Application**
```bash
eb init -p "Windows Server 2022 running .NET 10" ProductService
```

3. **Create Environment**
```bash
eb create ProductServiceEnv
```

4. **Deploy**
```bash
eb deploy
```

---

## 📊 Logging & Monitoring

### Application Insights Integration

**Install NuGet Package:**
```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

**Update Program.cs:**
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

**appsettings.json:**
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "YOUR_INSTRUMENTATION_KEY"
  }
}
```

### Serilog Integration (Optional)

**Install Package:**
```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console
```

**Update Program.cs:**
```csharp
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/productservice-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
```

---

## 🔍 Performance Optimization

### Database Query Optimization

```csharp
// Bad: N+1 Query
var products = _context.Products.ToList();
foreach (var product in products)
{
    // This triggers a query for each product
}

// Good: Use Include for eager loading
var products = _context.Products
    .Include(p => p.Category)  // If you add categories later
    .ToList();

// Better: Use AsNoTracking for read-only queries
var products = _context.Products
    .AsNoTracking()
    .Where(p => p.IsActive)
    .ToList();
```

### Caching Implementation

**Install Redis:**
```bash
dotnet add package StackExchange.Redis
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
```

**Update Program.cs:**
```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
```

**Use in Service:**
```csharp
private readonly IMemoryCache _cache;

public ProductService(IProductRepository repository, IMemoryCache cache)
{
    _repository = repository;
    _cache = cache;
}

public async Task<List<ProductDto>> GetAllProducts()
{
    const string cacheKey = "all_products";
    
    if (_cache.TryGetValue(cacheKey, out List<ProductDto> products))
    {
        return products;
    }

    products = await FetchFromDatabase();
    _cache.Set(cacheKey, products, TimeSpan.FromMinutes(5));
    
    return products;
}
```

---

## 🧪 Health Check Endpoint

Add to Program.cs:
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ProductDbContext>();

app.MapHealthChecks("/health");
```

Check health:
```bash
curl https://localhost:7xxx/health
```

---

## 📋 Environment Variables

**Create .env file (for development):**
```
ConnectionString=Data Source=PARADOX\SQLEXPRESS;Database=ProductDb;...
JwtKey=YOUR_JWT_KEY
JwtIssuer=ProductServiceAPI
ASPNETCORE_ENVIRONMENT=Development
```

**Load in Program.cs:**
```csharp
var envFile = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envFile))
{
    var lines = File.ReadAllLines(envFile);
    foreach (var line in lines)
    {
        var parts = line.Split('=');
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}
```

---

## ✅ Pre-Deployment Checklist

- [ ] All unit tests pass
- [ ] Database migrations tested
- [ ] API endpoints tested in Swagger
- [ ] JWT authentication verified
- [ ] Error handling works correctly
- [ ] Logging configured
- [ ] CORS settings correct for target domain
- [ ] Connection strings updated for environment
- [ ] JWT secret key changed from default
- [ ] HTTPS enabled
- [ ] Health check endpoint working
- [ ] Database backups configured
- [ ] Monitoring/alerting set up
- [ ] Load testing completed
- [ ] Security audit passed

---

**Last Updated**: March 28, 2025  
**Version**: 1.0
