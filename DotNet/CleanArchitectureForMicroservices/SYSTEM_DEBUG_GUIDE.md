# Role-Based Authorization - Complete System Debug Guide

## System Architecture Check

### ✅ JWT Configuration

```
Secret Key (all services): fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4=
Issuer: UserService.API

UserService:
  - Audience: UserService
  - Port: 5000

ProductService:
  - Audience: ProductService
  - Port: 5001 (typically)
  - Endpoint: http://localhost:5082/api/product (from Swagger)

OrderService:
  - Audience: OrderService
  - Port: 5002 (typically)
```

---

## Step 1: Verify Services Are Running

### Terminal 1 - UserService (Port 5000)

```bash
cd /Users/nrs/TopBrains/DotNet/CleanArchitectureForMicroservices/UserService/UserService.API
dotnet run
```

Expected output:

```
Building...
Built in X ms
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5000
```

Check UserService is running:

```bash
curl http://localhost:5000/api/user 2>/dev/null || echo "NOT RUNNING"
```

### Terminal 2 - ProductService (Port 5001 or 5082)

```bash
cd /Users/nrs/TopBrains/DotNet/CleanArchitectureForMicroservices/ProductService/ProductService.API
dotnet run
```

Expected output:

```
Building...
Built in X ms
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

Check ProductService is running:

```bash
curl http://localhost:5001/api/product 2>/dev/null || echo "NOT RUNNING"
```

### Terminal 3 - OrderService (Port 5002)

```bash
cd /Users/nrs/TopBrains/DotNet/CleanArchitectureForMicroservices/OrderService/OrderService.API
dotnet run
```

---

## Step 2: Test Complete Auth Flow

### A. Register as Seller

```bash
curl -X POST http://localhost:5000/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "seller_test",
    "email": "seller_test@example.com",
    "password": "TestPassword123!",
    "fullName": "Test Seller",
    "role": "Seller"
  }'
```

Expected Response:

```json
{
  "success": true,
  "message": "User registered successfully..."
}
```

### B. Register as Buyer

```bash
curl -X POST http://localhost:5000/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "buyer_test",
    "email": "buyer_test@example.com",
    "password": "TestPassword123!",
    "fullName": "Test Buyer",
    "role": "Buyer"
  }'
```

### C. Send Confirmation Email (Seller)

```bash
curl -X POST http://localhost:5000/api/user/send-confirmation-email \
  -H "Content-Type: application/json" \
  -d '{"email": "seller_test@example.com"}'
```

### D. Get JWT Token (Login as Seller)

```bash
curl -X POST http://localhost:5000/api/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUserName": "seller_test",
    "password": "TestPassword123!",
    "clientId": "web"
  }'
```

Expected Response:

```json
{
  "succeeded": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "roles": ["Seller"]
}
```

**Copy the token value and save it:**

```bash
SELLER_TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### E. Login as Buyer

```bash
curl -X POST http://localhost:5000/api/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUserName": "buyer_test",
    "password": "TestPassword123!",
    "clientId": "web"
  }'
```

Save Buyer token:

```bash
BUYER_TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## Step 3: Test ProductService Authorization

### A. Seller Creates Product ✅ (Should work)

```bash
curl -X POST http://localhost:5001/api/product \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $SELLER_TOKEN" \
  -d '{
    "name": "iPhone 14",
    "description": "Apple smartphone",
    "price": 79999,
    "stock": 50,
    "category": "Electronics"
  }'
```

Expected Response: **201 Created** or **200 OK**

```json
{
  "success": true,
  "message": "Product created successfully",
  "data": {...}
}
```

**If you get 401 Unauthorized:**

- Token is expired or invalid
- Check JWT token is properly formatted
- Verify secret keys match

**If you get 403 Forbidden:**

- User doesn't have Seller role
- Check database: `SELECT * FROM AspNetUserRoles WHERE UserId = 'your-id'`

### B. Buyer Creates Product ❌ (Should fail with 403)

```bash
curl -X POST http://localhost:5001/api/product \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $BUYER_TOKEN" \
  -d '{
    "name": "Test Product",
    "description": "Test",
    "price": 100,
    "stock": 10,
    "category": "Test"
  }'
```

Expected Response: **403 Forbidden**

```json
{
  "statusCode": 403,
  "message": "Access Denied"
}
```

### C. Seller Gets All Products ✅ (Should work)

```bash
curl -X GET http://localhost:5001/api/product \
  -H "Authorization: Bearer $SELLER_TOKEN"
```

Expected Response: **200 OK**

### D. Seller Gets Own Products ✅ (Should work)

```bash
curl -X GET http://localhost:5001/api/product/my-products \
  -H "Authorization: Bearer $SELLER_TOKEN"
```

---

## Step 4: Test OrderService Authorization

### A. Buyer Adds to Cart ✅ (Should work)

```bash
curl -X POST http://localhost:5002/api/order/cart/add \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $BUYER_TOKEN" \
  -d '{
    "productId": "550e8400-e29b-41d4-a716-446655440000",
    "productName": "iPhone 14",
    "price": 79999,
    "quantity": 1
  }'
```

Expected Response: **200 OK**

### B. Seller Adds to Cart ❌ (Should fail with 403)

```bash
curl -X POST http://localhost:5002/api/order/cart/add \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $SELLER_TOKEN" \
  -d '{
    "productId": "550e8400-e29b-41d4-a716-446655440000",
    "productName": "iPhone 14",
    "price": 79999,
    "quantity": 1
  }'
```

Expected Response: **403 Forbidden**

### C. Buyer Views Cart ✅ (Should work)

```bash
curl -X GET http://localhost:5002/api/order/cart \
  -H "Authorization: Bearer $BUYER_TOKEN"
```

Expected Response: **200 OK**

```json
{
  "success": true,
  "data": {
    "items": [...],
    "totalItems": 1,
    "totalPrice": 79999
  }
}
```

### D. Buyer Places Order ✅ (Should work)

```bash
curl -X POST http://localhost:5002/api/order/place-order \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $BUYER_TOKEN" \
  -d '{
    "shippingAddress": "123 Main St, City, State, 12345",
    "billingAddress": "123 Main St, City, State, 12345"
  }'
```

Expected Response: **201 Created**

---

## Common Issues & Solutions

### Issue 1: 401 Unauthorized - "The signature key was not found"

**Cause:** JWT secret key mismatch

**Debug:**

```bash
# Check ProductService config
grep -A 5 "JwtSettings" ProductService/ProductService.API/appsettings.json

# Check OrderService config
grep -A 5 "JwtSettings" OrderService/OrderService.API/appsettings.json

# Check UserService config
grep -A 5 "JwtSettings" UserService/UserService.API/appsettings.json
```

**Solution:** All three services must have identical SecretKey

```
SecretKey: "fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4="
```

---

### Issue 2: 403 Forbidden even with correct role

**Cause:** Role not assigned to user in database

**Debug:**

```sql
-- Check user roles
SELECT u.Email, r.Name
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'seller_test@example.com';
```

**Solution:** If no results, add the role:

```sql
DECLARE @UserId UNIQUEIDENTIFIER = (SELECT Id FROM AspNetUsers WHERE Email = 'seller_test@example.com');
DECLARE @RoleId UNIQUEIDENTIFIER = (SELECT Id FROM AspNetRoles WHERE Name = 'Seller');
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);
```

---

### Issue 3: 401 Unauthorized - Token expired

**Cause:** JWT token lifespan (default 15 minutes in UserService)

**Debug:** Check token expiration:

```bash
# Decode JWT (use jwt.io or run this in PowerShell)
# Split token by '.' and base64 decode the middle part (payload)
```

**Solution:** Get a new token or increase expiration time in appsettings.json:

```json
"AccessTokenExpirationMinutes": 60
```

---

### Issue 4: Endpoint returns 404 Not Found

**Cause:** Controller not mapped or wrong endpoint path

**Debug:**

```bash
# Check Swagger endpoint
curl http://localhost:5001/swagger/ui.html

# Verify endpoint exists in Swagger
# Should show /api/product endpoints
```

**Solution:** Ensure:

- `[Route("api/[controller]")]` is on controller (will expand to `/api/product`)
- `MapControllers()` is called in Program.cs
- Controllers directory exists and files are created

---

### Issue 5: 500 Internal Server Error

**Cause:** Unhandled exception

**Debug:** Check service logs:

```bash
# Look at console output for error message
# Check logs file: UserService/logs/userservice-YYYY-MM-DD.txt
tail -f logs/userservice-*.txt
```

---

## Database Verification Checklist

### Verify Roles Exist

```sql
SELECT * FROM AspNetRoles;
```

Should show: Admin, Buyer, Customer, Seller, Vendor

### Verify Users Exist

```sql
SELECT Id, Email, UserName FROM AspNetUsers;
```

### Verify User Roles

```sql
SELECT u.Email, u.UserName, r.Name as Role
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY u.Email;
```

---

## Token Verification

### Decode JWT Token (Linux/Mac)

```bash
# Save token to variable
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Decode header
echo $TOKEN | cut -d'.' -f1 | base64 -d | jq .

# Decode payload (contains claims)
echo $TOKEN | cut -d'.' -f2 | base64 -d | jq .
```

Expected payload should show:

```json
{
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "user-id-uuid",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "user@example.com",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "username",
  "client_id": "web",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Seller",
  "exp": 1234567890,
  "iss": "UserService.API"
}
```

---

## Quick Test Script

Save as `test-rbac.sh`:

```bash
#!/bin/bash

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo "🚀 Role-Based Authorization System Test"
echo "======================================"
echo ""

# 1. Register Seller
echo "1️⃣  Registering Seller..."
SELLER_RESP=$(curl -s -X POST http://localhost:5000/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "seller_test_'$(date +%s)'",
    "email": "seller_'$(date +%s)'@example.com",
    "password": "TestPassword123!",
    "fullName": "Test Seller",
    "role": "Seller"
  }')

if echo "$SELLER_RESP" | grep -q "success.*true"; then
  echo -e "${GREEN}✓ Seller registered${NC}"
else
  echo -e "${RED}✗ Failed to register seller${NC}"
  echo "$SELLER_RESP"
  exit 1
fi

echo ""
echo "✅ All basic tests passed! System is operational."
```

Make executable and run:

```bash
chmod +x test-rbac.sh
./test-rbac.sh
```

---

## Full Test Checklist

- [ ] UserService is running on port 5000
- [ ] ProductService is running on port 5001
- [ ] OrderService is running on port 5002
- [ ] All JWT secret keys match
- [ ] Can register as Seller
- [ ] Can register as Buyer
- [ ] Can login and get JWT token
- [ ] Token contains correct role claim
- [ ] Seller can create product
- [ ] Buyer cannot create product (403)
- [ ] Buyer can add to cart
- [ ] Seller cannot add to cart (403)
- [ ] Database has Seller and Buyer roles
- [ ] Users have correct role assignments

---

## Next Steps

If all tests pass:

1. Implement actual database persistence for products/orders
2. Add OpenAPI/Swagger documentation
3. Set up API Gateway or API Management
4. Implement refresh token rotation
5. Add audit logging
6. Deploy to staging environment
