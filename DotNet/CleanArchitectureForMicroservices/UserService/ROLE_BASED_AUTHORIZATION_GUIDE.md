# Role-Based Authorization Implementation Guide

## Overview

This guide documents the role-based authentication and authorization system implemented across the microservices architecture. The system uses JWT tokens with role claims and ASP.NET Core's Authorization framework.

## Architecture

### Roles Defined

1. **Admin** - Full system access
2. **Seller** - Can list and manage their own products
3. **Buyer** - Can add items to cart and place orders
4. **Customer** - Legacy role for shopping
5. **Vendor** - Legacy role for product management

### JWT Token Flow

```
1. User registers with selected role (Seller or Buyer)
2. User logs in
3. UserService generates JWT token with role claims
4. JWT token includes:
   - NameIdentifier (UserId)
   - Email
   - Name (Username)
   - Role (Seller, Buyer, Customer, etc.)
   - client_id
5. Token is sent to ProductService and OrderService
6. Services validate token and check role claims
```

## Implementation Details

### 1. UserService (Authentication)

#### RegisterDTO.cs

- Added `Role` property for role selection
- Default role: "Buyer"
- Supported roles: "Seller" or "Buyer"

```csharp
[Required(ErrorMessage = "Role is required.")]
[StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
public string Role { get; set; } = "Buyer";
```

#### UserService.cs - RegisterAsync()

- Uses the selected role from RegisterDTO
- Assigns role during user creation

```csharp
var role = !string.IsNullOrWhiteSpace(dto.Role) ? dto.Role : "Buyer";
await _userRepository.AddUserToRoleAsync(user, role);
```

#### JWT Token Generation

- Includes role claims in the token
- Used by downstream services for authorization

```csharp
claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
```

#### LoginResponseDTO.cs

- Added `Roles` property to return roles to frontend
- Helps frontend implement client-side role-based UI

```csharp
public List<string>? Roles { get; set; }
```

#### User Database Seeding

```csharp
// Seller Role - Can create and manage products
new ApplicationRole
{
    Id = Guid.Parse("7d5c4e8a-1234-4b5c-8f2e-9a8b7c6d5e4f"),
    Name = "Seller",
    NormalizedName = "SELLER",
    Description = "Seller who can list and manage products"
}

// Buyer Role - Can purchase products and place orders
new ApplicationRole
{
    Id = Guid.Parse("8e6d5f9b-2345-4c6d-9g3f-0b9c8d7e6f5g"),
    Name = "Buyer",
    NormalizedName = "BUYER",
    Description = "Buyer who can add items to cart and place orders"
}
```

### 2. ProductService (Seller Authorization)

#### Program.cs Configuration

```csharp
// JWT Authentication setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "UserService.API",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();
```

#### appsettings.json

```json
"JwtSettings": {
    "Issuer": "UserService.API",
    "Audience": "ProductService",
    "SecretKey": "fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4="
}
```

#### ProductController.cs - Authorization

```csharp
[ApiController]
[Authorize] // All endpoints require authentication
public class ProductController : ControllerBase
{
    /// Lists all products - any authenticated user
    [HttpGet("all")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllProducts()

    /// Lists seller's own products - Seller only
    [HttpGet("my-products")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> GetMyProducts()

    /// Create new product - Seller only
    [HttpPost]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)

    /// Update product - Seller only
    [HttpPut("{productId}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> UpdateProduct(Guid productId, UpdateProductDto dto)

    /// Delete product - Seller only
    [HttpDelete("{productId}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
}
```

### 3. OrderService (Buyer Authorization)

#### Program.cs Configuration

- Same JWT setup as ProductService
- Validates tokens from UserService.API

#### appsettings.json

```json
"JwtSettings": {
    "Issuer": "UserService.API",
    "Audience": "OrderService",
    "SecretKey": "fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4="
}
```

#### OrderController.cs - Authorization

```csharp
[ApiController]
[Authorize] // All endpoints require authentication
public class OrderController : ControllerBase
{
    /// Add item to cart - Buyer only
    [HttpPost("cart/add")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> AddToCart(AddToCartRequest request)

    /// View cart - Buyer only
    [HttpGet("cart")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetCart()

    /// Remove from cart - Buyer only
    [HttpDelete("cart/remove/{itemId}")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> RemoveFromCart(Guid itemId)

    /// Place order - Buyer only
    [HttpPost("place-order")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)

    /// View order history - Buyer only
    [HttpGet("orders")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetOrderHistory()

    /// View order details - Buyer only
    [HttpGet("orders/{orderId}")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetOrderDetails(Guid orderId)

    /// Cancel order - Buyer only
    [HttpPut("orders/{orderId}/cancel")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
}
```

## API Usage Examples

### 1. Register as Seller

```http
POST /api/user/register
Content-Type: application/json

{
  "username": "seller123",
  "email": "seller@example.com",
  "password": "Password123!",
  "fullName": "John Seller",
  "role": "Seller"
}

Response:
{
  "success": true,
  "message": "User registered successfully"
}
```

### 2. Register as Buyer

```http
POST /api/user/register
Content-Type: application/json

{
  "username": "buyer456",
  "email": "buyer@example.com",
  "password": "Password456!",
  "fullName": "Jane Buyer",
  "role": "Buyer"
}
```

### 3. Login

```http
POST /api/user/login
Content-Type: application/json

{
  "emailOrUserName": "seller123",
  "password": "Password123!",
  "clientId": "web"
}

Response:
{
  "succeeded": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "roles": ["Seller"]
}
```

### 4. Seller - Create Product

```http
POST /api/product
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "quantity": 10
}

Response (Seller can create):
{
  "success": true,
  "data": {
    "id": "product-uuid",
    "name": "Laptop",
    "sellerId": "seller-uuid"
  }
}

Response (Buyer cannot create):
{
  "statusCode": 403,
  "message": "Access Denied"
}
```

### 5. Buyer - Add to Cart

```http
POST /api/order/cart/add
Authorization: Bearer {BUYER_JWT_TOKEN}
Content-Type: application/json

{
  "productId": "product-uuid",
  "productName": "Laptop",
  "price": 999.99,
  "quantity": 1
}

Response:
{
  "success": true,
  "message": "Item added to cart successfully"
}
```

### 6. Buyer - Place Order

```http
POST /api/order/place-order
Authorization: Bearer {BUYER_JWT_TOKEN}
Content-Type: application/json

{
  "shippingAddress": "123 Main St, City, State, 12345",
  "billingAddress": "123 Main St, City, State, 12345"
}

Response:
{
  "success": true,
  "message": "Order placed successfully",
  "data": {
    "id": "order-uuid",
    "totalPrice": 999.99,
    "status": "Pending"
  }
}
```

## Best Practices

### 1. Token Validation

- All microservices validate the JWT token signature
- Verify issuer matches UserService
- Check token expiration
- Validate role claims

### 2. Role-Based Access Control

- Always use `[Authorize(Roles = "RoleName")]` on endpoints
- Check roles in code for complex authorization logic
- Log authorization failures

### 3. Data Isolation

- Sellers can only see and modify their own products
- Buyers can only see and modify their own cart and orders
- Use userId from token claims for data filtering

### 4. Security Considerations

- Keep JWT secret key secure (use appsettings for dev, environment variables for prod)
- Use HTTPS for all communication
- Implement rate limiting on login/registration
- Log all authorization failures
- Consider implementing refresh token rotation

### 5. Future Enhancements

- Implement authorization policies for more granular control
- Add role-based API versioning
- Implement audit logging for all role-based actions
- Add UI-based role management
- Implement permission-based (not just role-based) authorization

## Troubleshooting

### Issue: "Access Denied" for authorized user

**Solution:**

- Check JWT token includes correct role claim
- Verify`Authorize(Roles = "RoleName")` matches actual role
- Check token is not expired
- Verify service has same JWT secret key

### Issue: Token validation fails

**Solution:**

- Ensure Issuer in appsettings matches token issuer
- Verify SecretKey is identical across all services
- Check ValidateLifetime setting in token validation
- Verify service is using correct authentication scheme

### Issue: ClaimTypes.Role not populated

**Solution:**

- Check JWT generation includes role claims
- Verify user has role assigned in database
- Check `GetUserRolesAsync` returns correct values
- Verify role exists in database

## Database Queries

### Check user roles

```sql
SELECT u.Email, r.Name
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY u.Email;
```

### Add role to user manually

```sql
DECLARE @UserId UNIQUEIDENTIFIER = (SELECT Id FROM AspNetUsers WHERE Email = 'user@example.com');
DECLARE @RoleId UNIQUEIDENTIFIER = (SELECT Id FROM AspNetRoles WHERE Name = 'Seller');
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);
```

## References

- [ASP.NET Core Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization)
- [JWT Bearer Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt)
- [Role-Based Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles)
- [Claims-Based Authorization](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims)
