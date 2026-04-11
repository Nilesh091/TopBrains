# 🔨 Implementation Guide - What to Build/Fix

## Summary of Fixes Required

### ✅ IMMEDIATE ACTIONS (This Week)

#### 1. **Remove Placeholder Files** (30 minutes)

```bash
# Delete these files across all projects:
find . -name "Class1.cs" -delete
find . -name "UnitTest1.cs" -delete
```

Files to delete:

```
ProductService.Application/Class1.cs
ProductService.Domain/Class1.cs
ProductService.Infrastructure/Class1.cs
OrderService.Application/Class1.cs
OrderService.Domain/Class1.cs
OrderService.Infrastructure/Class1.cs
PaymentService.Application/Class1.cs
PaymentService.Domain/Class1.cs
PaymentService.Infrastructure/Class1.cs
[All]/[Service].Tests/UnitTest1.cs
```

#### 2. **Remove WeatherForecast Endpoints** (1 hour)

Delete from:

- ProductService.API/Controllers/WeatherForecastController.cs
- OrderService.API/Controllers/WeatherForecastController.cs
- PaymentService.API/Controllers/WeatherForecastController.cs

Replace with actual service controllers

---

### 🟠 HIGH PRIORITY (Week 1-2)

#### 1. **Add OAuth to UserService**

Files to Create/Modify:

**New Entity:**

```csharp
// UserService.Domain/Entities/OAuthProvider.cs
public class OAuthProvider
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Provider { get; set; }  // "Google", "Facebook", "Microsoft"
    public string ExternalId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
}
```

**New DTOs:**

```csharp
// UserService.Application/DTOs/OAuthLoginDTO.cs
public class OAuthLoginDTO
{
    public string Provider { get; set; }
    public string Token { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
}

// UserService.Application/DTOs/OAuthCallbackDTO.cs
public class OAuthCallbackDTO
{
    public string Code { get; set; }
    public string State { get; set; }
}
```

**New Service Method in UserService:**

```csharp
Task<LoginResponseDTO> LoginWithOAuthAsync(OAuthLoginDTO dto);
Task LinkOAuthAsync(Guid userId, string provider, string externalId);
Task UnlinkOAuthAsync(Guid userId, string provider);
```

**New Controller Endpoint:**

```csharp
// UserService.API/Controllers/UserController.cs
[HttpPost("oauth/login")]
public async Task<IActionResult> OAuthLogin([FromBody] OAuthLoginDTO dto)
{
    // Verify token with provider (Google, Facebook, etc.)
    // Get user info from provider
    // Find or create user
    // Return JWT token
}

[HttpPost("oauth/callback/{provider}")]
[Authorize]
public async Task<IActionResult> OAuthCallback(string provider, [FromBody] OAuthCallbackDTO dto)
{
    // Handle OAuth callback
}

[HttpPost("oauth/unlink/{provider}")]
[Authorize]
public async Task<IActionResult> UnlinkOAuth(string provider)
{
    // Remove provider link
}
```

---

#### 2. **Add Role-Based Access Control (RBAC)**

Files to Create/Modify:

**New Entity:**

```csharp
// UserService.Domain/Entities/Role.cs
public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }  // "Admin", "Customer", "StoreManager"
    public string Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}

// UserService.Domain/Entities/UserRole.cs
public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public User User { get; set; }
    public Role Role { get; set; }
}

// UserService.Domain/Entities/Permission.cs
public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; }  // "Create.Product", "Delete.Order", etc.
    public Guid RoleId { get; set; }

    public Role Role { get; set; }
}
```

**New DTOs:**

```csharp
// UserService.Application/DTOs/RoleDTO.cs
public class RoleDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Permissions { get; set; }
}

// UserService.Application/DTOs/AssignRoleDTO.cs
public class AssignRoleDTO
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
```

**Update User DTO:**

```csharp
// UserService.Application/DTOs/ProfileDTO.cs
public class ProfileDTO
{
    // ... existing fields ...
    public List<RoleDTO> Roles { get; set; }  // ADD THIS
}
```

**New Service Method:**

```csharp
// UserService.Application/Services/RoleService.cs
public interface IRoleService
{
    Task<RoleDTO> CreateRoleAsync(string name, string description);
    Task<RoleDTO> GetRoleAsync(Guid roleId);
    Task AssignRoleAsync(Guid userId, Guid roleId);
    Task RemoveRoleAsync(Guid userId, Guid roleId);
    Task<List<RoleDTO>> GetUserRolesAsync(Guid userId);
}
```

**Update JWT Token Generation:**

```csharp
// Add roles to JWT claims
var roles = await _roleService.GetUserRolesAsync(user.Id);
var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
// Add to token
```

---

#### 3. **Add MFA - Email OTP**

Files to Create/Modify:

**New Entity:**

```csharp
// UserService.Domain/Entities/MfaProvider.cs
public class MfaProvider
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; }  // "EmailOtp", "SmsOtp", "Totp"
    public bool IsEnabled { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
}

// UserService.Domain/Entities/OtpCode.cs
public class OtpCode
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Code { get; set; }  // 6-digit code
    public string Type { get; set; }  // "Login", "Setup", "Disable"
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public int Attempts { get; set; }

    public User User { get; set; }
}
```

**New DTOs:**

```csharp
// UserService.Application/DTOs/EnableMfaDTO.cs
public class EnableMfaDTO
{
    public string Type { get; set; }  // "EmailOtp", "SmsOtp"
    public string? PhoneNumber { get; set; }  // For SMS OTP
}

// UserService.Application/DTOs/VerifyMfaCodeDTO.cs
public class VerifyMfaCodeDTO
{
    public string Code { get; set; }
    public string Type { get; set; }
}

// UserService.Application/DTOs/MfaLoginDTO.cs
public class MfaLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? MfaCode { get; set; }  // If MFA is enabled
}

// UserService.Application/DTOs/MfaSetupResponseDTO.cs
public class MfaSetupResponseDTO
{
    public bool Enabled { get; set; }
    public string Message { get; set; }
    public List<string> BackupCodes { get; set; }  // For recovery
}
```

**New Service Method:**

```csharp
// UserService.Application/Services/MfaService.cs
public interface IMfaService
{
    Task<MfaSetupResponseDTO> EnableMfaAsync(Guid userId, EnableMfaDTO dto);
    Task<bool> VerifyMfaCodeAsync(Guid userId, VerifyMfaCodeDTO dto);
    Task<bool> DisableMfaAsync(Guid userId, string type);
    Task<List<string>> GenerateBackupCodesAsync(Guid userId);
    Task<string> GenerateAndSendOtpAsync(Guid userId, string type);
}
```

**Update Login Endpoint:**

```csharp
// UserService.API/Controllers/UserController.cs
[HttpPost("mfa-login")]
public async Task<IActionResult> MfaLogin([FromBody] MfaLoginDTO dto)
{
    // Step 1: Verify email + password
    // Step 2: If MFA enabled, send OTP and return temp token
    // Step 3: If OTP provided, verify and issue JWT
}
```

---

#### 4. **Add Profile Image Upload**

Files to Create/Modify:

**Update User Entity:**

```csharp
// UserService.Domain/Entities/User.cs
public class User
{
    // ... existing fields ...
    public string? ProfilePhotoUrl { get; set; }  // NEW
    public DateTime? ProfilePhotoUpdatedAt { get; set; }  // NEW
}
```

**New DTOs:**

```csharp
// UserService.Application/DTOs/UploadProfileImageDTO.cs
public class UploadProfileImageDTO
{
    [Required]
    public IFormFile Image { get; set; }  // Max 5MB, JPG/PNG only
}

// UserService.Application/DTOs/ProfileImageDTO.cs
public class ProfileImageDTO
{
    public string Url { get; set; }
    public DateTime UploadedAt { get; set; }
}
```

**New Service Method:**

```csharp
// UserService.Application/Services/FileService.cs
public interface IFileService
{
    Task<string> UploadProfileImageAsync(Guid userId, IFormFile file);
    Task DeleteProfileImageAsync(Guid userId);
    Task<string> GetProfileImageUrlAsync(Guid userId);
}

// Implementation with Azure Blob Storage or AWS S3
```

**New Controller Endpoint:**

```csharp
// UserService.API/Controllers/UserController.cs
[HttpPost("profile/image")]
[Authorize]
public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageDTO dto)
{
    var imageUrl = await _fileService.UploadProfileImageAsync(UserId, dto.Image);
    return Ok(new { ImageUrl = imageUrl });
}

[HttpDelete("profile/image")]
[Authorize]
public async Task<IActionResult> DeleteProfileImage()
{
    await _fileService.DeleteProfileImageAsync(UserId);
    return Ok();
}
```

---

### 🟡 MEDIUM PRIORITY (Week 2-3)

#### **NotificationService: Add WhatsApp**

Files to Create/Modify:

**New Entity:**

```csharp
// NotificationService.Domain/Entity/WhatsAppMessage.cs
public class WhatsAppMessage
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string? TemplateId { get; set; }
    public Dictionary<string, string>? TemplateVariables { get; set; }
    public string Status { get; set; }  // "Pending", "Sent", "Failed"
    public string? ExternalMessageId { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**New DTOs:**

```csharp
// NotificationService.Application/DTOs/SendWhatsAppDTO.cs
public class SendWhatsAppDTO
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string? TemplateId { get; set; }
    public Dictionary<string, string>? TemplateVariables { get; set; }
}
```

**New Service:**

```csharp
// NotificationService.Infrastructure/Services/WhatsAppSender.cs
public interface IWhatsAppSender
{
    Task SendAsync(WhatsAppMessage message);
}

// Implementation using Twilio or Gupshup API
```

**New Controller Endpoint:**

```csharp
[HttpPost("whatsapp/send")]
public async Task<IActionResult> SendWhatsApp([FromBody] SendWhatsAppDTO dto)
{
    // Send WhatsApp message
}
```

---

#### **NotificationService: Add SMS/OTP**

Files to Create/Modify:

**New Entity:**

```csharp
// NotificationService.Domain/Entity/SmsMessage.cs
public class SmsMessage
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string MessageType { get; set; }  // "OTP", "Notification", "Alert"
    public string Status { get; set; }  // "Pending", "Sent", "Failed"
    public string? ExternalMessageId { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**New DTOs:**

```csharp
// NotificationService.Application/DTOs/SendSmsDTO.cs
public class SendSmsDTO
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}

// NotificationService.Application/DTOs/SendOtpDTO.cs
public class SendOtpDTO
{
    public string PhoneNumber { get; set; }
}

// NotificationService.Application/DTOs/OtpResponseDTO.cs
public class OtpResponseDTO
{
    public string OtpId { get; set; }
    public string Message { get; set; }
}
```

**New Service:**

```csharp
// NotificationService.Infrastructure/Services/SmsSender.cs
public interface ISmsSender
{
    Task SendAsync(SmsMessage message);
    Task<string> SendOtpAsync(string phoneNumber);
    Task<bool> VerifyOtpAsync(string otpId, string code);
}

// Implementation using Twilio, AWS SNS, or similar
```

**New Controller Endpoints:**

```csharp
[HttpPost("sms/send")]
public async Task<IActionResult> SendSms([FromBody] SendSmsDTO dto)
{
    // Send SMS message
}

[HttpPost("otp/send")]
public async Task<IActionResult> SendOtp([FromBody] SendOtpDTO dto)
{
    // Send OTP
}

[HttpPost("otp/verify")]
public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO dto)
{
    // Verify OTP
}
```

---

### 🔴 CRITICAL (Week 3-5)

#### **CartService - Create Entire Service**

Folder Structure:

```
CartService/
├── CartService.sln
├── CartService.API/
│   ├── Program.cs
│   ├── CartService.API.csproj
│   ├── Controllers/
│   │   └── CartController.cs
│   ├── Middleware/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── CartService.API.http
│   ├── Properties/
│   └── ...
├── CartService.Application/
│   ├── CartService.Application.csproj
│   ├── Services/
│   │   ├── ICartService.cs
│   │   └── CartService.cs
│   ├── DTOs/
│   │   ├── CartItemDTO.cs
│   │   ├── CartDTO.cs
│   │   ├── AddCartItemDTO.cs
│   │   ├── UpdateCartItemDTO.cs
│   │   └── ApplyCouponDTO.cs
│   └── Validators/
│       └── CartItemValidator.cs
├── CartService.Domain/
│   ├── CartService.Domain.csproj
│   ├── Entities/
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   └── Coupon.cs
│   └── Repositories/
│       └── ICartRepository.cs
├── CartService.Infrastructure/
│   ├── CartService.Infrastructure.csproj
│   ├── Persistence/
│   │   └── CartDbContext.cs
│   ├── Repositories/
│   │   └── CartRepository.cs
│   └── Cache/
│       └── RedisCartCache.cs
└── CartService.Tests/
    ├── CartService.Tests.csproj
    └── CartServiceTests.cs
```

**Key Entities:**

```csharp
// CartService.Domain/Entities/Cart.cs
public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();
    public string? AppliedCouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Total => UnitPrice * Quantity;
}

public class Coupon
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal? MaxDiscount { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public int MaxUses { get; set; }
    public int UsageCount { get; set; }
    public bool IsActive { get; set; }
}
```

**Key Endpoints:**

```
GET    /api/cart                   - Get user's cart
POST   /api/cart/items             - Add item to cart
PUT    /api/cart/items/{itemId}    - Update item quantity
DELETE /api/cart/items/{itemId}    - Remove item from cart
DELETE /api/cart                   - Clear entire cart
POST   /api/cart/coupons/{code}    - Apply coupon/discount
GET    /api/cart/coupons/validate/{code}  - Validate coupon
DELETE /api/cart/coupons           - Remove coupon
GET    /api/cart/summary           - Get cart totals with tax
```

---

#### **ProductService - Implement All Features**

[Similar structure to CartService]

**Key Endpoints:**

```
GET    /api/products                      - List products (paginated, filtered)
GET    /api/products/{id}                 - Get product details
POST   /api/products                      - Create product [Admin]
PUT    /api/products/{id}                 - Update product [Admin]
DELETE /api/products/{id}                 - Delete product [Admin]
GET    /api/products/search?query=        - Search products
GET    /api/categories                    - List categories
POST   /api/categories                    - Create category [Admin]
PUT    /api/categories/{id}               - Update category [Admin]
DELETE /api/categories/{id}               - Delete category [Admin]
GET    /api/products/category/{catId}     - Filter by category
POST   /api/products/{id}/images          - Upload product images [Admin]
DELETE /api/products/{id}/images/{imgId}  - Delete product image [Admin]
GET    /api/products/{id}/reviews         - Get product reviews
POST   /api/products/{id}/reviews         - Add product review
PUT    /api/inventory/{productId}         - Update stock [Admin]
GET    /api/inventory/{productId}         - Get stock level
```

---

#### **OrderService - Implement All Features**

**Key Endpoints:**

```
POST   /api/orders                        - Create new order
GET    /api/orders/{orderId}              - Get order details
GET    /api/orders                        - List user's orders (paginated)
PUT    /api/orders/{orderId}/status       - Update order status [Admin]
GET    /api/orders/{orderId}/invoice      - Get/download invoice
GET    /api/orders/{orderId}/tracking     - Get order tracking info
POST   /api/orders/{orderId}/cancel       - Cancel order
POST   /api/orders/{orderId}/return       - Request return [Customer]
```

---

#### **PaymentService - Implement All Features**

**Key Endpoints:**

```
POST   /api/payments/initiate             - Initiate payment
POST   /api/payments/{paymentId}/verify   - Verify payment
GET    /api/payments/{paymentId}          - Get payment status
POST   /api/payments/{paymentId}/refund   - Process refund
GET    /api/payment-methods               - List available methods
POST   /api/webhooks/payment              - Payment gateway webhook
```

---

### 🏗️ INFRASTRUCTURE (Week 5)

#### **APIGateway - Create Service**

Use **Ocelot** NuGet package:

```csharp
// APIGateway/Program.cs
builder.Services.AddOcelot();

// Ocelot routes configuration
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/user/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [{ "Host": "localhost", "Port": 5190 }],
      "UpstreamPathTemplate": "/api/user/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Put", "Delete"],
      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "1m",
        "PeriodTimespan": 60,
        "Limit": 100
      }
    },
    // Similar routes for other services...
  ]
}
```

---

## Summary Checklist

### Week 1

- [ ] Delete Class1.cs files
- [ ] Delete WeatherForecast endpoints
- [ ] Add RBAC to UserService
- [ ] Add OAuth basics to UserService
- [ ] Setup unit test framework

### Week 2

- [ ] Complete MFA implementation
- [ ] Add profile image upload
- [ ] Add WhatsApp to NotificationService
- [ ] Add SMS/OTP to NotificationService
- [ ] Start CartService

### Week 3-4

- [ ] Complete CartService
- [ ] Complete ProductService
- [ ] Complete OrderService

### Week 5

- [ ] Complete PaymentService
- [ ] Setup APIGateway
- [ ] Setup Event Bus

### Week 6

- [ ] Integration testing
- [ ] Performance optimization
- [ ] Security audit
- [ ] Documentation
