# Order Service - Production-Ready Microservice

A clean architecture microservice for managing orders in an e-commerce system built with .NET 10, Entity Framework Core, and JWT Authentication.

## 📋 Features

### ✅ Cart Management

- Add products to cart
- Update cart item quantities
- Remove items from cart
- Clear entire cart
- View cart with total calculation

### 📦 Order Management

- Create orders from cart
- Validate product availability via Product Service
- Track order status and payment status
- View order history
- Retrieve individual order details

### 💳 Payment Integration

- Initiate payment process (stub implementation)
- Confirm payment and mark order as paid
- Generate invoices after successful payment
- Track payment status

### 🧾 Invoice Management

- Auto-generate invoices after payment
- View invoices by order
- View all user invoices
- Invoice number generation and tracking

### 🔐 Security

- JWT token-based authentication
- Role-based authorization (Buyer role required)
- UserID extracted from JWT claims
- Secure API endpoints

## 🏗 Architecture

```
OrderService/
├── OrderService.API/              # Presentation Layer (Web API)
│   ├── Controllers/
│   ├── Program.cs                 # DI & Configuration
│   └── appsettings.json          # Configuration
├── OrderService.Application/      # Business Logic Layer
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Interfaces/                # Service Contracts
│   └── Services/                  # Business Logic
├── OrderService.Domain/           # Domain Layer
│   ├── Entities/                  # Domain Entities
│   ├── Enums/                     # Value Objects
│   └── [No dependencies]
├── OrderService.Infrastructure/   # Data Access & External Services
│   ├── Data/                      # DbContext
│   ├── Repositories/              # Data Access
│   └── Services/                  # External Service Clients
└── OrderService.Tests/            # Unit Tests
```

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (local or remote)
- Visual Studio 2022+ or VS Code

### Installation

1. **Clone the repository**

   ```bash
   cd OrderService
   ```

2. **Update Database Connection**
   Edit `appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=OrderServiceDb_Dev;Trusted_Connection=true;"
   }
   ```

3. **Install Dependencies**

   ```bash
   dotnet restore
   ```

4. **Create and Seed Database**

   ```bash
   cd OrderService.API
   dotnet ef database update -s OrderService.API.csproj -p ../OrderService.Infrastructure/OrderService.Infrastructure.csproj
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

The API will be available at: `https://localhost:7000`
Swagger UI: `https://localhost:7000/swagger`

## 🔐 Authentication & Authorization

### JWT Token Claims

The system expects JWT tokens with the following claims:

- `sub` or `nameid`: User ID (string)
- `role`: User role (must be "Buyer" for cart/order operations)

### Example JWT Header

```
Authorization: Bearer <jwt-token>
```

### Roles

- **Buyer**: Can create cart, add items, place orders, view invoices
- **Admin** (future): Can manage orders, view reports

## 📡 API Endpoints

### Cart Endpoints

#### 1. Get User's Cart

```http
GET /api/cart
Authorization: Bearer <token>
```

**Response:**

```json
{
  "success": true,
  "message": "Cart retrieved successfully",
  "data": {
    "id": "guid",
    "userId": "user-id",
    "items": [
      {
        "id": "guid",
        "productId": "prod-1",
        "productName": "Product Name",
        "price": 99.99,
        "quantity": 2,
        "lineTotal": 199.98,
        "addedAt": "2026-03-31T10:00:00Z"
      }
    ],
    "total": 199.98,
    "itemCount": 1,
    "createdAt": "2026-03-31T09:00:00Z",
    "updatedAt": "2026-03-31T10:00:00Z"
  },
  "timestamp": "2026-03-31T10:00:00Z"
}
```

#### 2. Add Product to Cart

```http
POST /api/cart/add
Authorization: Bearer <token>
Content-Type: application/json

{
  "productId": "prod-1",
  "productName": "New Product",
  "price": 99.99,
  "quantity": 1
}
```

#### 3. Update Cart Item Quantity

```http
PUT /api/cart/update
Authorization: Bearer <token>
Content-Type: application/json

{
  "cartItemId": "guid",
  "quantity": 3
}
```

#### 4. Remove from Cart

```http
DELETE /api/cart/remove/{cartItemId}
Authorization: Bearer <token>
```

#### 5. Clear Cart

```http
DELETE /api/cart/clear
Authorization: Bearer <token>
```

---

### Order Endpoints

#### 1. Create Order

```http
POST /api/order/create
Authorization: Bearer <token>
Content-Type: application/json

{
  "shippingAddress": "123 Main Street, City, State 12345",
  "notes": "Handle with care"
}
```

**Response:**

```json
{
  "success": true,
  "message": "Order created successfully",
  "data": {
    "orderId": "guid",
    "orderNumber": "ORD-20260331-ABC123DE",
    "totalAmount": 299.97,
    "paymentUrl": "https://payment-service.local/pay?id=payment-guid",
    "message": "Order created successfully. Please proceed to payment."
  },
  "timestamp": "2026-03-31T10:00:00Z"
}
```

#### 2. Get Order Details

```http
GET /api/order/{orderId}
Authorization: Bearer <token>
```

#### 3. Get User's Orders

```http
GET /api/order/user/all
Authorization: Bearer <token>
```

#### 4. Confirm Payment

```http
POST /api/order/{orderId}/confirm-payment?paymentId=payment-123
Authorization: Bearer <token>
```

#### 5. Get Order Invoice

```http
GET /api/order/{orderId}/invoice
Authorization: Bearer <token>
```

#### 6. Get User's Invoices

```http
GET /api/order/invoices/all
Authorization: Bearer <token>
```

---

## 📊 Data Models

### Cart

```csharp
public class Cart
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public ICollection<CartItem> Items { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### CartItem

```csharp
public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}
```

### Order

```csharp
public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string UserId { get; set; }
    public ICollection<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? PaymentId { get; set; }
    public string? ShippingAddress { get; set; }
    public Guid? InvoiceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### Order Status

- `Pending` - Order created, awaiting payment
- `Paid` - Payment successful
- `Failed` - Payment failed
- `Shipped` - Order shipped
- `Delivered` - Order delivered
- `Cancelled` - Order cancelled

### Payment Status

- `Pending` - Payment awaiting
- `Success` - Payment successful
- `Failed` - Payment failed
- `Refunded` - Payment refunded

### Invoice

```csharp
public class Invoice
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; }
    public Guid OrderId { get; set; }
    public string UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public string PaymentId { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public InvoiceStatus Status { get; set; }
}
```

---

## 🔄 Service Flow

### Order Creation Flow

```
1. Customer adds items to cart
   ↓
2. Customer creates order (POST /api/order/create)
   ↓
3. Order Service:
   - Gets user's cart
   - Validates products via Product Service
   - Creates order entity
   - Initiates payment
   - Clears user's cart
   ↓
4. Returns payment URL
   ↓
5. Customer completes payment
   ↓
6. Customer confirms payment (POST /api/order/{id}/confirm-payment)
   ↓
7. Order Service:
   - Verifies payment
   - Marks order as paid
   - Generates invoice
   - Stores invoice
   ↓
8. Returns order with invoice details
```

---

## 🌐 External Service Integration

### Product Service

The Order Service calls the Product Service to:

- Validate product availability
- Check stock levels

**Expected Base URL:** Configured in `appsettings.json`

```json
"ExternalServices": {
  "ProductServiceUrl": "https://localhost:7001/api/"
}
```

### Payment Service

**Status:** Currently a stub implementation (no actual payment processing)

To integrate with real Payment Service:

1. Replace `PaymentServiceStub` with actual client
2. Implement payment gateway integration (Stripe, PayPal, etc.)
3. Update `IPaymentService` implementation

---

## 🗄️ Database Setup

### First-Time Setup

1. **Create Initial Migration**

   ```bash
   Add-Migration InitialCreate
   ```

2. **Apply Migration**
   ```bash
   Update-Database
   ```

### Modify Database Schema

1. **Add Migration**

   ```bash
   Add-Migration <MigrationName>
   ```

2. **Update Database**

   ```bash
   Update-Database
   ```

3. **Rollback**
   ```bash
   Update-Database <PreviousMigrationName>
   ```

---

## 📝 Environment Variables

### Development

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OrderServiceDb_Dev;..."
  },
  "ExternalServices": {
    "ProductServiceUrl": "https://localhost:7001/api/",
    "PaymentServiceUrl": "https://localhost:7002/api/"
  }
}
```

### Production

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=OrderServiceDb;..."
  },
  "ExternalServices": {
    "ProductServiceUrl": "https://product-service.prod/api/",
    "PaymentServiceUrl": "https://payment-service.prod/api/"
  }
}
```

---

## ✅ SOLID Principles Implementation

- **S (Single Responsibility)**: Each service has one reason to change
- **O (Open/Closed)**: Open for extension, closed for modification (interfaces)
- **L (Liskov Substitution)**: Repository implementations are interchangeable
- **I (Interface Segregation)**: Small, focused interfaces (ICartService, IOrderService, etc.)
- **D (Dependency Inversion)**: Depends on abstractions, not concrete classes

---

## 🧪 Unit Testing

```bash
cd OrderService.Tests
dotnet test
```

Example test structure:

```csharp
[TestClass]
public class CartServiceTests
{
    private CartService _cartService;
    private Mock<IUnitOfWork> _unitOfWorkMock;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cartService = new CartService(_unitOfWorkMock.Object, ...);
    }

    [TestMethod]
    public async Task AddToCart_ShouldAddNewItem()
    {
        // Arrange
        var userId = "user-1";
        var dto = new AddToCartDto { ... };

        // Act
        var result = await _cartService.AddToCartAsync(userId, dto);

        // Assert
        Assert.IsNotNull(result);
    }
}
```

---

## 🚨 Error Handling

All endpoints return consistent error responses:

```json
{
  "success": false,
  "message": "Detailed error message",
  "errorCode": "ERROR_CODE",
  "timestamp": "2026-03-31T10:00:00Z"
}
```

### Common Error Codes

- `VALIDATION_ERROR` - Request validation failed
- `INVALID_TOKEN` - JWT token is invalid
- `UNAUTHORIZED` - User not authenticated
- `FORBIDDEN` - User lacks required role
- `NOT_FOUND` - Resource not found
- `INVALID_OPERATION` - Operation cannot be performed
- `ERROR` - Generic server error

---

## 📊 Logging

Configured through `appsettings.json`:

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning",
    "Microsoft.EntityFrameworkCore": "Information"
  }
}
```

---

## 🔒 Security Considerations

1. **JWT Validation**
   - Signature validation enabled
   - Issuer validation enabled
   - Lifetime validation enabled

2. **Role-based Authorization**
   - Only "Buyer" role can access cart/order endpoints
   - User can only view their own carts and orders

3. **Connection Security**
   - HTTPS enforced
   - CORS policy configured

4. **Input Validation**
   - DTOs use required properties
   - Model state validation on all endpoints

---

## 📈 Future Enhancements

- [ ] Implement actual payment gateway integration
- [ ] Add order status webhook notifications
- [ ] Implement order cancellation with refunds
- [ ] Add order filters (date range, status)
- [ ] Email notifications integration
- [ ] Analytics and reporting endpoints
- [ ] Caching layer (Redis)
- [ ] Message queue integration (RabbitMQ)
- [ ] API versioning
- [ ] Rate limiting

---

## 📞 Support

For issues or questions:

1. Check the API documentation in Swagger UI
2. Review error codes and messages
3. Check application logs for detailed errors

---

## 📄 License

[Your License Here]

---

## 👥 Contributors

[Your Name/Team]
