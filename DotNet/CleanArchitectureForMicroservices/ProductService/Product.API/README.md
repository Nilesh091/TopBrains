# ProductService Microservice

A fully functional **Product Service** microservice built with **.NET 10**, featuring JWT authentication, Entity Framework Core, and comprehensive Swagger documentation.

## 📋 Overview

The ProductService is a RESTful API that manages product catalog operations. It follows a clean architecture pattern with clear separation of concerns across Domain, Application, and Infrastructure layers.

### Features

✅ **Complete CRUD Operations** - Create, Read, Update, Delete products  
✅ **JWT Authentication** - Secure API endpoints with JWT tokens  
✅ **Role-Based Access Control** - Admin-only operations for sensitive actions  
✅ **Database Persistence** - SQL Server with Entity Framework Core  
✅ **Swagger Documentation** - Interactive API documentation  
✅ **Error Handling** - Comprehensive error responses  
✅ **Soft Delete** - Products are marked inactive instead of hard deleted  

---

## 🏗️ Project Structure

```
ProductService/
├── Product.Domain/
│   ├── Entities/
│   │   └── ProductEntity.cs          # Product domain entity
│   └── Interfaces/
│       └── IProductRepository.cs     # Repository interface
│
├── Product.Application/
│   ├── DTOs/
│   │   ├── CreateProductDto.cs       # DTO for creating products
│   │   ├── UpdateProductDto.cs       # DTO for updating products
│   │   ├── ProductDto.cs             # DTO for reading products
│   │   └── ApiResponseDto.cs         # Generic API response wrapper
│   ├── Interfaces/
│   │   └── IProductService.cs        # Service interface
│   └── Services/
│       └── ProductService.cs         # Business logic implementation
│
├── Product.Infrastructure/
│   ├── Data/
│   │   └── ProductDbContext.cs       # EF Core database context
│   ├── Repositories/
│   │   └── ProductRepository.cs      # Data access layer
│   └── Migrations/
│       ├── 20260328080300_InitialCreate.cs
│       └── ProductDbContextModelSnapshot.cs
│
└── Product.API/
    ├── Controllers/
    │   └── ProductController.cs      # API endpoints
    ├── Program.cs                    # Configuration & startup
    └── appsettings.json              # Configuration file
```

---

## 🔧 Technology Stack

- **.NET 10** - Latest framework
- **ASP.NET Core** - Web framework
- **Entity Framework Core 10** - ORM
- **SQL Server** - Database
- **Swagger/Swashbuckle** - API documentation
- **JWT Bearer** - Authentication scheme

---

## 📦 Dependencies

All NuGet packages are configured in `Product.API.csproj`:

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.5" />
<PackageReference Include="Microsoft.IdentityModel.Tokens" Version="8.17.0" />
<PackageReference Include="Microsoft.OpenApi" Version="1.6.14" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.17.0" />
```

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=PARADOX\\SQLEXPRESS;Database=ProductDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
  },
  "Jwt": {
    "Key": "THIS_IS_MY_SUPER_SECRET_KEY_123456789",
    "Issuer": "ProductServiceAPI",
    "Audience": "ProductServiceClient",
    "DurationInMinutes": 60
  }
}
```

**Important**: Update the connection string and JWT key for your environment.

---

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Update Connection String**
   ```json
   "DefaultConnection": "Your_Connection_String_Here"
   ```

2. **Apply Database Migrations**
   ```bash
   dotnet ef database update -p Product.Infrastructure -s Product.API
   ```

3. **Run the Application**
   ```bash
   cd Product.API
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to: `https://localhost:7xxx/`
   - Swagger will be available at the root URL in development mode

---

## 📚 API Endpoints

All endpoints return standardized `ApiResponseDto<T>` format:

```json
{
  "success": true,
  "data": { /* response data */ },
  "message": "Operation successful"
}
```

### Public Endpoints

#### Get All Products
```http
GET /api/products
```
**Response**: List of all active products

#### Get Product by ID
```http
GET /api/products/{id}
```
**Response**: Single product details

### Protected Endpoints (Admin Only)

#### Create Product
```http
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Product Name",
  "description": "Product Description",
  "price": 99.99,
  "stock": 100,
  "category": "Electronics"
}
```

#### Update Product
```http
PUT /api/products/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": "uuid",
  "name": "Updated Name",
  "description": "Updated Description",
  "price": 149.99,
  "stock": 50,
  "category": "Electronics"
}
```

#### Delete Product
```http
DELETE /api/products/{id}
Authorization: Bearer {token}
```

---

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication.

### How to Get a Token

1. Login through the **UserService** API to get a JWT token
2. Copy the token from the response
3. Use it in the Authorization header:
   ```
   Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```

### Admin Role Requirement

Only users with the **"Admin"** role can:
- Create new products
- Update existing products
- Delete products

---

## 🗄️ Database Schema

### Products Table

| Column | Type | Constraints |
|--------|------|-------------|
| Id | uniqueidentifier | PRIMARY KEY |
| Name | nvarchar(200) | NOT NULL |
| Description | nvarchar(1000) | NULL |
| Price | decimal(10,2) | NOT NULL |
| Stock | int | NOT NULL |
| Category | nvarchar(100) | NOT NULL, Indexed |
| IsActive | bit | NOT NULL |
| CreatedAt | datetime2 | NOT NULL |
| UpdatedAt | datetime2 | NOT NULL |

**Indexes**: Category, IsActive

---

## 📝 Data Models

### ProductEntity (Domain)
```csharp
public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### ProductDto (Application)
Same structure as ProductEntity, used for API responses.

### CreateProductDto / UpdateProductDto
DTO models for request bodies.

---

## 🛡️ Error Handling

The API returns appropriate HTTP status codes:

| Status | Scenario |
|--------|----------|
| 200 OK | Successful GET, PUT, DELETE |
| 201 Created | Successful POST |
| 400 Bad Request | Invalid input or ID mismatch |
| 401 Unauthorized | Missing or invalid JWT token |
| 403 Forbidden | Insufficient permissions (non-Admin) |
| 404 Not Found | Product doesn't exist |
| 500 Internal Server Error | Server error |

---

## 🧪 Testing with Swagger

1. Open Swagger UI at application root
2. Click the **"Authorize"** button (lock icon)
3. Paste your JWT token in the format: `Bearer {token}`
4. Try out endpoints directly from the UI

---

## 🔄 Service Architecture

### Clean Architecture Pattern

```
API Layer (Controllers)
        ↓
Application Layer (Services, DTOs)
        ↓
Domain Layer (Entities, Interfaces)
        ↓
Infrastructure Layer (Database, Repositories)
```

### Dependency Injection

All services are registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(connectionString));
```

---

## 📝 Key Features Explanation

### Soft Delete
Products are not physically deleted from the database. Instead, `IsActive` is set to `false`. This allows:
- Data retention and auditing
- Historical tracking
- Easy recovery if needed

### Response Wrapper
All API responses follow a consistent format using `ApiResponseDto<T>`:
```json
{
  "success": true,
  "data": { /* ... */ },
  "message": "Descriptive message"
}
```

### Swagger Integration
- Automatic API documentation generation
- JWT authentication testing via UI
- Interactive endpoint testing
- Request/response schema visualization

---

## 🚨 Common Issues & Solutions

### Database Connection Error
**Issue**: "Cannot open database"  
**Solution**: Update `appsettings.json` with correct connection string and ensure SQL Server is running.

### JWT Token Expired
**Issue**: 401 Unauthorized after 60 minutes  
**Solution**: Get a new token from the UserService login endpoint.

### Migration Failures
**Issue**: "Migrations history table doesn't exist"  
**Solution**: 
```bash
dotnet ef database update -p Product.Infrastructure -s Product.API
```

---

## 📖 Related Services

This microservice is part of a larger microservices architecture:

- **UserService** - Authentication & user management (provides JWT tokens)
- **ProductService** - Product catalog management (this service)
- **OrderService** - Order processing
- **PaymentService** - Payment handling
- **NotificationService** - Email/notification delivery
- **CartService** - Shopping cart management

---

## 🔗 Integration Example

To integrate ProductService with other services:

```csharp
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", jwtToken);

var response = await httpClient.GetAsync("https://product-api/api/products");
var products = await response.Content.ReadAsAsync<ApiResponseDto<List<ProductDto>>>();
```

---

## 📞 Support

For questions or issues:
1. Check the Swagger documentation
2. Review error messages in the response
3. Check application logs in the console
4. Verify database connection and migrations

---

## 📄 License

This project is part of a microservices learning/demonstration system.

---

**Last Updated**: March 28, 2025  
**Framework**: .NET 10  
**Status**: ✅ Production Ready
