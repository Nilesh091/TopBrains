# Role-Based Authorization - Quick Testing Guide

## Prerequisites

- All services running locally:
  - UserService: http://localhost:5000
  - ProductService: http://localhost:5001
  - OrderService: http://localhost:5002
- Postman or similar API testing tool

## Step-by-Step Testing

### Step 1: Register a Seller Account

**Request:**

```
POST http://localhost:5000/api/user/register
Content-Type: application/json

{
  "userName": "seller_john",
  "email": "seller@example.com",
  "password": "TestPassword123!",
  "fullName": "John Seller",
  "phoneNumber": "1234567890",
  "role": "Seller"
}
```

**Expected Response:**

```json
{
  "success": true,
  "message": "User registered successfully. Please check your email for confirmation."
}
```

---

### Step 2: Register a Buyer Account

**Request:**

```
POST http://localhost:5000/api/user/register
Content-Type: application/json

{
  "userName": "buyer_jane",
  "email": "buyer@example.com",
  "password": "TestPassword123!",
  "fullName": "Jane Buyer",
  "phoneNumber": "0987654321",
  "role": "Buyer"
}
```

---

### Step 3: Verify Email for Both Accounts

You need to:

1. Get email confirmation token (or check email)
2. Call the verify email endpoint

**For Seller:**

```
POST http://localhost:5000/api/user/send-confirmation-email
Content-Type: application/json

{
  "email": "seller@example.com"
}
```

---

### Step 4: Login as Seller

**Request:**

```
POST http://localhost:5000/api/user/login
Content-Type: application/json

{
  "emailOrUserName": "seller_john",
  "password": "TestPassword123!",
  "clientId": "web"
}
```

**Expected Response:**

```json
{
  "succeeded": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "roles": ["Seller"]
}
```

**Save the token for Seller:**

```
SELLER_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### Step 5: Login as Buyer

**Request:**

```
POST http://localhost:5000/api/user/login
Content-Type: application/json

{
  "emailOrUserName": "buyer_jane",
  "password": "TestPassword123!",
  "clientId": "web"
}
```

**Save the token for Buyer:**

```
BUYER_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### Step 6: Test Seller Endpoints

#### Test 1: Seller Creates Product ✅ (Should succeed)

**Request:**

```
POST http://localhost:5001/api/product
Authorization: Bearer {SELLER_TOKEN}
Content-Type: application/json

{
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop with RTX 3080",
  "price": 1499.99,
  "quantity": 5
}
```

**Expected Response:** 201 Created with product data

#### Test 2: Buyer Creates Product ❌ (Should fail with 403 Forbidden)

**Request:**

```
POST http://localhost:5001/api/product
Authorization: Bearer {BUYER_TOKEN}
Content-Type: application/json

{
  "name": "Try to create",
  "description": "This should fail",
  "price": 99.99,
  "quantity": 10
}
```

**Expected Response:** 403 Forbidden - "Access Denied"

#### Test 3: Seller Views Their Products ✅ (Should succeed)

**Request:**

```
GET http://localhost:5001/api/product/my-products
Authorization: Bearer {SELLER_TOKEN}
```

**Expected Response:** 200 OK with seller's products

#### Test 4: Seller Views All Products ✅ (Should succeed)

**Request:**

```
GET http://localhost:5001/api/product/all
Authorization: Bearer {SELLER_TOKEN}
```

**Expected Response:** 200 OK with all products

---

### Step 7: Test Buyer Endpoints

#### Test 1: Buyer Adds Item to Cart ✅ (Should succeed)

**Request:**

```
POST http://localhost:5002/api/order/cart/add
Authorization: Bearer {BUYER_TOKEN}
Content-Type: application/json

{
  "productId": "product-id-from-seller",
  "productName": "Gaming Laptop",
  "price": 1499.99,
  "quantity": 1
}
```

**Expected Response:** 200 OK - Item added to cart

#### Test 2: Seller Adds Item to Cart ❌ (Should fail)

**Request:**

```
POST http://localhost:5002/api/order/cart/add
Authorization: Bearer {SELLER_TOKEN}
Content-Type: application/json

{
  "productId": "some-product-id",
  "productName": "Try to buy",
  "price": 99.99,
  "quantity": 1
}
```

**Expected Response:** 403 Forbidden

#### Test 3: Buyer Views Cart ✅ (Should succeed)

**Request:**

```
GET http://localhost:5002/api/order/cart
Authorization: Bearer {BUYER_TOKEN}
```

**Expected Response:** 200 OK with cart items

#### Test 4: Buyer Places Order ✅ (Should succeed)

**Request:**

```
POST http://localhost:5002/api/order/place-order
Authorization: Bearer {BUYER_TOKEN}
Content-Type: application/json

{
  "shippingAddress": "123 Main Street, Anytown, State, 12345",
  "billingAddress": "123 Main Street, Anytown, State, 12345"
}
```

**Expected Response:** 201 Created with order details

#### Test 5: Buyer Views Orders ✅ (Should succeed)

**Request:**

```
GET http://localhost:5002/api/order/orders
Authorization: Bearer {BUYER_TOKEN}
```

**Expected Response:** 200 OK with order list

---

### Step 8: Test Authorization Edge Cases

#### Test 1: Access without token ❌ (Should fail)

**Request:**

```
GET http://localhost:5001/api/product/my-products
(NO Authorization header)
```

**Expected Response:** 401 Unauthorized

#### Test 2: Access with invalid token ❌ (Should fail)

**Request:**

```
GET http://localhost:5001/api/product/my-products
Authorization: Bearer invalid_token_123
```

**Expected Response:** 401 Unauthorized

#### Test 3: Access after token expiration ❌ (Should fail)

Wait for token to expire (default 15 minutes) or modify the expiration time in appsettings for testing.

**Expected Response:** 401 Unauthorized

---

## Summary Table

| Endpoint                     | Seller | Buyer | Anonymous |
| ---------------------------- | ------ | ----- | --------- |
| GET /api/product/all         | ✅     | ✅    | ❌        |
| GET /api/product/my-products | ✅     | ❌    | ❌        |
| POST /api/product            | ✅     | ❌    | ❌        |
| PUT /api/product/{id}        | ✅     | ❌    | ❌        |
| DELETE /api/product/{id}     | ✅     | ❌    | ❌        |
| POST /api/order/cart/add     | ❌     | ✅    | ❌        |
| GET /api/order/cart          | ❌     | ✅    | ❌        |
| POST /api/order/place-order  | ❌     | ✅    | ❌        |
| GET /api/order/orders        | ❌     | ✅    | ❌        |
| GET /api/order/orders/{id}   | ❌     | ✅    | ❌        |

---

## Common Issues and Solutions

### Issue: "Token validation failed" in ProductService/OrderService

**Cause:** JWT secret key mismatch

**Solution:**

1. Verify `JwtSettings:SecretKey` in all services appsettings.json
2. Check that `ValidIssuer` matches UserService issuer

### Issue: Role not included in token

**Cause:** User doesn't have role assigned, or GetUserRolesAsync not working

**Solution:**

1. Check database: `SELECT * FROM AspNetUserRoles WHERE UserId = 'user-id'`
2. Verify role exists in AspNetRoles table
3. Check that RegisterAsync calls `AddUserToRoleAsync`

### Issue: "Access Denied" for correct role

**Cause:** Role mismatch in Authorize attribute

**Solution:**

1. Check exact role name: `[Authorize(Roles = "Seller")]`
2. Verify JWT includes correct role claim
3. Check database role name matches

### Issue: Can't verify email

**Cause:** Email confirmation token issues

**Solution:**

1. Call send-confirmation-email endpoint first
2. Use the token returned or check email
3. Then call verify-email endpoint

---

## Postman Collection Import

You can import these pre-configured requests in Postman:

1. Create new Collection: "RBAC Testing"
2. Add folders: "Register & Login", "Seller Tests", "Buyer Tests"
3. Add the requests from above
4. Use Postman environment variables:
   - `seller_token` = Save response token
   - `buyer_token` = Save response token
   - `base_url` = http://localhost

---

## Next Steps

After confirming all tests pass:

1. **Implement Database:** Replace in-memory storage in controllers with real database
2. **Add Validations:** Implement business logic validations
3. **Add Logging:** Use Serilog for centralized logging
4. **Add Unit Tests:** Write unit tests for authorization logic
5. **Deploy:** Configure for staging/production environments
