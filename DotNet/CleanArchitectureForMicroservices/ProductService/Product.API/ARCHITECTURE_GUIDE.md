# ProductService Architecture & Flow Diagrams

## 🏗️ Layered Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                    API LAYER (Presentation)                 │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           ProductController                            │ │
│  │  • GET /api/products (public)                          │ │
│  │  • GET /api/products/{id} (public)                     │ │
│  │  • POST /api/products (admin only)                     │ │
│  │  • PUT /api/products/{id} (admin only)                 │ │
│  │  • DELETE /api/products/{id} (admin only)              │ │
│  └────────────────────────────────────────────────────────┘ │
└───────────────────────┬──────────────────────────────────────┘
                        │ Dependency Injection
                        ▼
┌─────────────────────────────────────────────────────────────┐
│               APPLICATION LAYER (Business Logic)            │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           IProductService / ProductService            │ │
│  │  • GetAllProductsAsync()                              │ │
│  │  • GetProductByIdAsync(id)                            │ │
│  │  • CreateProductAsync(dto)                            │ │
│  │  • UpdateProductAsync(dto)                            │ │
│  │  • DeleteProductAsync(id)                             │ │
│  └────────────────────────────────────────────────────────┘ │
│                        ▲                                      │
│                        │ Uses                                 │
│  ┌────────────────────┴────────────────────────────────────┐ │
│  │ DTOs: CreateProductDto, UpdateProductDto, ProductDto │ │
│  │ Response: ApiResponseDto<T>                           │ │
│  └────────────────────────────────────────────────────────┘ │
└───────────────────────┬──────────────────────────────────────┘
                        │ Interface Implementation
                        ▼
┌─────────────────────────────────────────────────────────────┐
│                DOMAIN LAYER (Entities & Rules)              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           ProductEntity                                │ │
│  │  • Id (Guid)                                          │ │
│  │  • Name (string)                                      │ │
│  │  • Description (string)                               │ │
│  │  • Price (decimal)                                    │ │
│  │  • Stock (int)                                        │ │
│  │  • Category (string)                                  │ │
│  │  • IsActive (bool)                                    │ │
│  │  • CreatedAt (DateTime)                               │ │
│  │  • UpdatedAt (DateTime)                               │ │
│  └────────────────────────────────────────────────────────┘ │
│                        ▲                                      │
│                        │ Interface Definition                 │
│  ┌────────────────────┴────────────────────────────────────┐ │
│  │      IProductRepository                                │ │
│  │  • GetAllAsync()                                       │ │
│  │  • GetByIdAsync(id)                                    │ │
│  │  • CreateAsync(entity)                                │ │
│  │  • UpdateAsync(entity)                                │ │
│  │  • DeleteAsync(id)                                    │ │
│  │  • ProductExistsAsync(id)                             │ │
│  └────────────────────────────────────────────────────────┘ │
└───────────────────────┬──────────────────────────────────────┘
                        │ Implementation
                        ▼
┌─────────────────────────────────────────────────────────────┐
│          INFRASTRUCTURE LAYER (Data Access & DB)            │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           ProductRepository                            │ │
│  │  • Implements IProductRepository                       │ │
│  │  • Uses ProductDbContext (EF Core)                    │ │
│  └────────────────────────────────────────────────────────┘ │
│                        ▲                                      │
│                        │ DbSet Mapping                        │
│  ┌────────────────────┴────────────────────────────────────┐ │
│  │      ProductDbContext (DbContext)                      │ │
│  │  • DbSet<ProductEntity> Products                       │ │
│  │  • OnModelCreating() - Schema Configuration           │ │
│  │  • Indexes: Category, IsActive                        │ │
│  └────────────────────────────────────────────────────────┘ │
└───────────────────────┬──────────────────────────────────────┘
                        │ SQL Server Provider
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              DATABASE LAYER (SQL Server)                    │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           Products Table                               │ │
│  │  • PK: Id (uniqueidentifier)                          │ │
│  │  • Columns: Name, Description, Price, Stock...        │ │
│  │  • Indexes: Category, IsActive                        │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔄 Request/Response Flow

```
CLIENT REQUEST
    │
    ▼
┌─────────────────────────────────────────────┐
│ HTTP Request to /api/products               │
│ GET, POST, PUT, DELETE                      │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ JWT Authentication Middleware               │
│ ✓ Validate token                            │
│ ✓ Extract claims (UserId, Role)            │
│ ✓ Set User Principal                        │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ Authorization Middleware (if [Authorize])   │
│ ✓ Check if authenticated                    │
│ ✓ Check role (if role required)            │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ ProductController Route Selection           │
│ Matches method & path                       │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ Model Binding & Validation                  │
│ ✓ Bind JSON to DTO                          │
│ ✓ Validate ModelState                       │
└─────────────────────────────────────────────┘
    │
    ├─── Invalid? ──→ 400 Bad Request
    │
    ▼
┌─────────────────────────────────────────────┐
│ ProductService.Operation()                  │
│ ✓ Business logic                            │
│ ✓ Error handling (try-catch)               │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ ProductRepository.Operation()               │
│ ✓ Database query (async)                    │
│ ✓ EF Core execution                         │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ ProductDbContext                            │
│ ✓ SQL generation                            │
│ ✓ Parameter mapping                         │
│ ✓ Connection pooling                        │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ SQL Server Execution                        │
│ ✓ Query parsing                             │
│ ✓ Index usage                               │
│ ✓ Results returned                          │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ Entity Mapping                              │
│ ProductEntity → ProductDto                  │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ Response Wrapping                           │
│ ApiResponseDto<T> {                         │
│   Success: true,                            │
│   Data: { ... },                            │
│   Message: "..."                            │
│ }                                           │
└─────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────┐
│ HTTP Response                               │
│ Status Code (200, 201, 400, 404, etc.)     │
│ Content-Type: application/json              │
│ Body: JSON response                         │
└─────────────────────────────────────────────┘
    │
    ▼
CLIENT RESPONSE
```

---

## 🔐 Authentication & Authorization Flow

```
┌──────────────────────────────────────────────────────┐
│ 1. User Calls UserService Login Endpoint             │
│    POST /api/auth/login                              │
│    { email, password }                               │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│ 2. UserService Validates & Generates JWT Token       │
│    • Verify email & password                         │
│    • Create claims (UserId, Email, Role)            │
│    • Sign with secret key                            │
│    • Return: { token, message }                      │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│ 3. Client Stores JWT Token                           │
│    • localStorage, sessionStorage, cookie, etc.      │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│ 4. Client Calls ProductService Protected Endpoint    │
│    POST /api/products                                │
│    Authorization: Bearer {token}                     │
│    { product data }                                  │
└──────────────────────────────────────────────────────┘
                        │
                        ▼
┌──────────────────────────────────────────────────────┐
│ 5. ProductService JWT Middleware                     │
│    • Extract token from Authorization header         │
│    • Parse JWT                                       │
│    • Validate signature (using secret key)           │
│    • Check expiration                                │
│    • Extract claims                                  │
└──────────────────────────────────────────────────────┘
                        │
            ┌───────────┴───────────┐
            │                       │
        VALID?                  INVALID?
            │                       │
            ▼                       ▼
      Token Accepted         401 Unauthorized
            │                       │
            ▼                       └─→ Return Error
    ┌──────────────────┐
    │ 6. Check [Authorize]
    │ Has [Authorize]?
    │ Authenticated? ✓
    └──────────────────┘
            │
            ▼
    ┌──────────────────┐
    │ 7. Check Role
    │ Has [Authorize(Roles)]?
    │ User.Role == "Admin"?
    └──────────────────┘
            │
    ┌───────┴──────┐
    │              │
  YES (Admin)    NO (Non-Admin)
    │              │
    ▼              ▼
  Proceed    403 Forbidden
    │         Return Error
    ▼
┌──────────────────────────────────────────────────────┐
│ 8. Execute Controller Action                         │
│    Call ProductService method                        │
└──────────────────────────────────────────────────────┘
    │
    ▼
┌──────────────────────────────────────────────────────┐
│ 9. Return Response                                   │
│    200 OK / 201 Created / Error Response             │
└──────────────────────────────────────────────────────┘
```

---

## 🗄️ Database Schema Diagram

```
┌─────────────────────────────────┐
│         Products Table          │
├─────────────────────────────────┤
│ PK │ Id (uniqueidentifier)      │
├────┼─────────────────────────────┤
│    │ Name (nvarchar(200))       │ ← NOT NULL, Unique
│    │ Description (nvarchar(1000))│ ← NULLABLE
│    │ Price (decimal(10,2))       │ ← NOT NULL
│    │ Stock (int)                 │ ← NOT NULL
│    │ Category (nvarchar(100))    │ ← NOT NULL, INDEXED
│    │ IsActive (bit)              │ ← NOT NULL, INDEXED
│    │ CreatedAt (datetime2)       │ ← NOT NULL
│    │ UpdatedAt (datetime2)       │ ← NOT NULL
├────┴─────────────────────────────┤
│ Indexes:                        │
│ • IX_Products_Category          │
│ • IX_Products_IsActive          │
└─────────────────────────────────┘
```

---

## 📊 Dependency Injection Flow

```
Application Startup (Program.cs)
│
├─ builder.Services.AddDbContext<ProductDbContext>()
│  │
│  └─→ Registers DbContext in DI Container
│
├─ builder.Services.AddScoped<IProductRepository, ProductRepository>()
│  │
│  └─→ When IProductRepository is requested:
│      Creates ProductRepository instance
│      Injects ProductDbContext dependency
│
├─ builder.Services.AddScoped<IProductService, ProductService>()
│  │
│  └─→ When IProductService is requested:
│      Creates ProductService instance
│      Injects IProductRepository dependency
│
└─ app.MapControllers()
   │
   └─→ When HTTP request arrives:
       Creates ProductController instance
       Injects IProductService dependency
       Executes action method
       Disposes resources (DbContext, etc.)

Timeline:
Request #1 → New DbContext instance
             ↓ (Query/Update)
           Dispose
Request #2 → New DbContext instance (different)
             ↓ (Query/Update)
           Dispose
```

---

## 🔄 CRUD Operations Flow

```
CREATE (POST)
├─ Client sends: CreateProductDto
├─ Controller validates ModelState
├─ Service creates ProductEntity
├─ Repository calls context.Products.Add()
├─ SaveChangesAsync() persists to DB
└─ Return: 201 Created with ProductDto

READ (GET)
├─ Controller receives Id (optional)
├─ Service calls repository
├─ Repository queries:
│  ├─ GetAllAsync() → WHERE IsActive = true
│  └─ GetByIdAsync(id) → WHERE Id = @id
├─ Maps ProductEntity → ProductDto
└─ Return: 200 OK with data

UPDATE (PUT)
├─ Client sends: UpdateProductDto
├─ Controller validates Id match
├─ Service checks product exists
├─ Repository finds and updates properties:
│  ├─ Name
│  ├─ Description
│  ├─ Price
│  ├─ Stock
│  ├─ Category
│  └─ UpdatedAt = DateTime.UtcNow
├─ SaveChangesAsync() persists to DB
└─ Return: 200 OK with updated ProductDto

DELETE (DELETE)
├─ Controller receives Id
├─ Service checks product exists
├─ Repository sets:
│  ├─ IsActive = false (Soft Delete)
│  └─ UpdatedAt = DateTime.UtcNow
├─ SaveChangesAsync() persists to DB
└─ Return: 200 OK with success flag
```

---

## 🧪 Test Flow Example

```
1. Swagger UI Opens
   └─→ Loads from /swagger/v1/swagger.json
       Contains all endpoint definitions

2. Authorize Button Clicked
   └─→ Stores JWT token in memory
       Applied to all subsequent requests

3. POST /api/products Endpoint
   ├─ Input: CreateProductDto
   │  {
   │    "name": "Laptop",
   │    "description": "High performance",
   │    "price": 999.99,
   │    "stock": 10,
   │    "category": "Electronics"
   │  }
   │
   ├─ Request Headers:
   │  Authorization: Bearer eyJhbGc...
   │  Content-Type: application/json
   │
   ├─ Server Processing:
   │  ✓ JWT Validation
   │  ✓ Authorization (Roles = "Admin")
   │  ✓ ModelState Validation
   │  ✓ Business Logic Execution
   │  ✓ Database Insert
   │
   └─ Response:
      Status: 201 Created
      Body: {
        "success": true,
        "data": {
          "id": "550e8400-e29b-41d4-a716-446655440000",
          "name": "Laptop",
          ...
        },
        "message": "Product created successfully"
      }
```

---

## 🔍 Error Handling Flow

```
Request Processing
│
└─→ Try {
      ├─ Authentication?
      │  └─ No → 401 Unauthorized
      │
      ├─ Authorization?
      │  └─ No → 403 Forbidden
      │
      ├─ ModelState Valid?
      │  └─ No → 400 Bad Request
      │
      ├─ Business Logic Execution
      │  ├─ Product Exists?
      │  │  └─ No → Catch Exception → 404 Not Found
      │  │
      │  └─ Database Operation
      │     └─ Exception? → Catch → 500 Server Error
      │
      └─ Success
         └─ Return ApiResponseDto { Success: true }
   }
   Catch (Exception ex) {
      └─ Return ApiResponseDto { 
           Success: false, 
           Message: ex.Message 
         }
   }
```

---

**This architecture ensures:**
- ✅ Clear separation of concerns
- ✅ Testability at each layer
- ✅ Scalability for future features
- ✅ Maintainability and readability
- ✅ Security through authentication & authorization
- ✅ Consistent API responses
- ✅ Proper error handling
