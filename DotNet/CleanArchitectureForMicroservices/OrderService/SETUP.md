# Quick Setup Guide

## Windows Setup (Visual Studio 2022)

### Step 1: Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 Community/Professional
- SQL Server (local or Docker)

### Step 2: Open Project

```bash
cd OrderService
# Open with Visual Studio
OrderService.sln
```

### Step 3: Update Connection String

Edit `OrderService.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=OrderServiceDb_Dev;Trusted_Connection=true;TrustServerCertificate=true;Encrypt=false;"
}
```

### Step 4: Create Database

In Package Manager Console:

```powershell
Add-Migration InitialCreate -Project OrderService.Infrastructure
Update-Database
```

### Step 5: Run Application

- Press `F5` or click `Run` button
- API will open in browser at `https://localhost:7000`
- Swagger UI at `https://localhost:7000/swagger`

---

## Linux/Mac Setup (.NET CLI)

### Step 1: Prerequisites

```bash
# Install .NET 10 (if not already installed)
# For macOS, use Homebrew:
brew install dotnet

# Verify installation
dotnet --version
```

### Step 2: Restore and Build

```bash
cd OrderService
dotnet restore
dotnet build
```

### Step 3: Update Connection String

Edit `OrderService.API/appsettings.Development.json` with your SQL Server connection details.

### Step 4: Create Database

```bash
cd OrderService.API
dotnet ef database update -s OrderService.API.csproj -p ../OrderService.Infrastructure/OrderService.Infrastructure.csproj
cd ..
```

### Step 5: Run Application

```bash
cd OrderService.API
dotnet run
```

The API will be available at `https://localhost:7000`

---

## Docker Setup

### Step 1: Create Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10 AS build
WORKDIR /src
COPY ["OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["OrderService.Application/OrderService.Application.csproj", "OrderService.Application/"]
COPY ["OrderService.Domain/OrderService.Domain.csproj", "OrderService.Domain/"]
COPY ["OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService.Infrastructure/"]
RUN dotnet restore "OrderService.API/OrderService.API.csproj"
COPY . .
RUN dotnet build "OrderService.API/OrderService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.API/OrderService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
```

### Step 2: Build and Run

```bash
docker build -t order-service .
docker run -p 7000:80 -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=OrderServiceDb;..." order-service
```

---

## Troubleshooting

### Issue: Connection String Error

**Solution:** Verify SQL Server is running and connection string is correct

```bash
# On Windows
sqlcmd -S . -U sa -P YourPassword

# Verify database exists
SELECT name FROM sys.databases;
```

### Issue: EF Migration Error

**Solution:** Ensure Infrastructure project is set as startup project for migrations

```bash
# From Package Manager Console
Set-DefaultProject OrderService.Infrastructure
Update-Database
```

### Issue: JWT Token Error

**Solution:** Ensure token contains correct claims:

- Subject (`sub`) = User ID
- Role = "Buyer"

Generate test token using online JWT tools or your User Service.

### Issue: Port Already in Use

**Solution:** Change port in `launchSettings.json`:

```json
"applicationUrl": "https://localhost:YOUR_PORT"
```

---

## Testing the API

### Using cURL

#### 1. Get Cart (requires valid JWT token)

```bash
curl -X GET https://localhost:7000/api/cart \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json"
```

#### 2. Add to Cart

```bash
curl -X POST https://localhost:7000/api/cart/add \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "prod-1",
    "productName": "Sample Product",
    "price": 99.99,
    "quantity": 1
  }'
```

### Using Swagger UI

1. Navigate to `https://localhost:7000/swagger`
2. Click "Authorize" button
3. Paste your JWT token
4. Use the "Try it out" button on each endpoint

### Using Postman

1. Import the collection from Swagger (`https://localhost:7000/swagger/v1/swagger.json`)
2. Set Authorization header with Bearer token
3. Test endpoints

---

## Database Migrations

### Create New Migration

```bash
Add-Migration DescriptiveNameHere
```

### Apply Pending Migrations

```bash
Update-Database
```

### Rollback to Previous Migration

```bash
Update-Database PreviousMigrationName
```

### Remove Last Migration

```bash
Remove-Migration
```

### Generate SQL Script

```bash
Script-Migration -From <PrevMigration> -To <NewMigration>
```

---

## Project Structure Reference

```
OrderService/
├── OrderService.Domain/              ← Domain Entities
│   ├── Entities/
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   └── Invoice.cs
│   └── Enums/
│       ├── OrderStatus.cs
│       └── PaymentStatus.cs
│
├── OrderService.Application/          ← Business Logic
│   ├── DTOs/
│   │   ├── Cart/
│   │   ├── Order/
│   │   ├── Invoice/
│   │   ├── Payment/
│   │   └── Common/
│   ├── Interfaces/
│   │   ├── ICartService.cs
│   │   ├── IOrderService.cs
│   │   ├── IInvoiceService.cs
│   │   ├── IPaymentService.cs
│   │   ├── IProductServiceClient.cs
│   │   └── Repository/
│   └── Services/
│       ├── CartService.cs
│       ├── OrderService.cs
│       └── InvoiceService.cs
│
├── OrderService.Infrastructure/       ← Data Access & External
│   ├── Data/
│   │   └── OrderServiceDbContext.cs
│   ├── Repositories/
│   │   ├── Repository.cs (Generic)
│   │   ├── CartRepository.cs
│   │   ├── OrderRepository.cs
│   │   ├── InvoiceRepository.cs
│   │   └── UnitOfWork.cs
│   └── Services/
│       ├── ProductServiceClient.cs
│       └── PaymentServiceStub.cs
│
├── OrderService.API/                  ← Presentation Layer
│   ├── Controllers/
│   │   ├── CartController.cs
│   │   └── OrderController.cs
│   ├── Program.cs                    ← DI Setup
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── OrderService.API.csproj
│
└── OrderService.Tests/                ← Unit Tests
    └── [Test classes]
```

---

## Key Configuration Files

### appsettings.json (Production)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=OrderServiceDb;..."
  },
  "JwtSettings": {
    "Issuer": "UserService.API",
    "SecretKey": "base64-encoded-secret"
  },
  "ExternalServices": {
    "ProductServiceUrl": "https://product-service.prod/api/",
    "PaymentServiceUrl": "https://payment-service.prod/api/"
  }
}
```

### appsettings.Development.json (Local)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OrderServiceDb_Dev;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

---

## Useful Commands

### Build Project

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Publish for Production

```bash
dotnet publish -c Release -o ./publish
```

### Clean Build

```bash
dotnet clean
dotnet build
```

### Format Code

```bash
dotnet format
```

---

## Security Checklist

- [ ] Change JWT SecretKey in production
- [ ] Use HTTPS only in production
- [ ] Configure CORS properly for specific domains
- [ ] Use environment variables for sensitive data
- [ ] Update NuGet packages regularly
- [ ] Implement rate limiting
- [ ] Add audit logging
- [ ] Use SQL parameterized queries (EF Core does this)
- [ ] Validate all input data
- [ ] Implement request validation with FluentValidation (optional)

---

## Performance Tips

- [ ] Enable query result caching for frequently accessed data
- [ ] Use async/await throughout
- [ ] Implement pagination for large datasets
- [ ] Use indexes on frequently queried columns
- [ ] Lazy load related entities when needed
- [ ] Monitor query execution time
- [ ] Consider implementing Redis caching

---

## Common Endpoints Quick Reference

| Method | Endpoint                          | Description         |
| ------ | --------------------------------- | ------------------- |
| GET    | `/api/cart`                       | Get user's cart     |
| POST   | `/api/cart/add`                   | Add item to cart    |
| PUT    | `/api/cart/update`                | Update cart item    |
| DELETE | `/api/cart/remove/{id}`           | Remove from cart    |
| DELETE | `/api/cart/clear`                 | Clear entire cart   |
| POST   | `/api/order/create`               | Create order        |
| GET    | `/api/order/{id}`                 | Get order details   |
| GET    | `/api/order/user/all`             | Get all user orders |
| POST   | `/api/order/{id}/confirm-payment` | Confirm payment     |
| GET    | `/api/order/{id}/invoice`         | Get invoice         |
| GET    | `/api/order/invoices/all`         | Get all invoices    |

---

For complete API documentation, see [README.md](README.md)
