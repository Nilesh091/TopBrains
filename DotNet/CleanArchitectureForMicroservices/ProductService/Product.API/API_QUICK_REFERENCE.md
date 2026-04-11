# ProductService API - Quick Reference

## 🔑 Authentication

### Step 1: Get JWT Token from UserService
```bash
curl -X POST https://userservice-api/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "password123"
  }'
```

**Response:**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login successful"
}
```

### Step 2: Use Token in ProductService Requests
```bash
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 📌 API Endpoints Reference

### 1️⃣ Get All Products (Public)
```http
GET /api/products HTTP/1.1
Host: localhost:5001
Accept: application/json
```

**cURL:**
```bash
curl -X GET "https://localhost:7xxx/api/products" \
  -H "Accept: application/json"
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "Laptop",
      "description": "High-performance laptop",
      "price": 999.99,
      "stock": 15,
      "category": "Electronics",
      "isActive": true,
      "createdAt": "2025-03-28T10:30:00Z",
      "updatedAt": "2025-03-28T10:30:00Z"
    }
  ],
  "message": "Products retrieved successfully"
}
```

---

### 2️⃣ Get Single Product (Public)
```http
GET /api/products/{id} HTTP/1.1
```

**Example:**
```bash
curl "https://localhost:7xxx/api/products/550e8400-e29b-41d4-a716-446655440000"
```

---

### 3️⃣ Create Product (Admin Only ✅ Protected)
```http
POST /api/products HTTP/1.1
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Wireless Mouse",
  "description": "Ergonomic wireless mouse",
  "price": 29.99,
  "stock": 100,
  "category": "Accessories"
}
```

**cURL:**
```bash
curl -X POST "https://localhost:7xxx/api/products" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Wireless Mouse",
    "description": "Ergonomic wireless mouse",
    "price": 29.99,
    "stock": 100,
    "category": "Accessories"
  }'
```

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": "660e8400-e29b-41d4-a716-446655440001",
    "name": "Wireless Mouse",
    "description": "Ergonomic wireless mouse",
    "price": 29.99,
    "stock": 100,
    "category": "Accessories",
    "isActive": true,
    "createdAt": "2025-03-28T14:45:00Z",
    "updatedAt": "2025-03-28T14:45:00Z"
  },
  "message": "Product created successfully"
}
```

---

### 4️⃣ Update Product (Admin Only ✅ Protected)
```http
PUT /api/products/{id} HTTP/1.1
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": "660e8400-e29b-41d4-a716-446655440001",
  "name": "Wireless Mouse Pro",
  "description": "Professional wireless mouse",
  "price": 49.99,
  "stock": 80,
  "category": "Accessories"
}
```

**cURL:**
```bash
curl -X PUT "https://localhost:7xxx/api/products/660e8400-e29b-41d4-a716-446655440001" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "660e8400-e29b-41d4-a716-446655440001",
    "name": "Wireless Mouse Pro",
    "description": "Professional wireless mouse",
    "price": 49.99,
    "stock": 80,
    "category": "Accessories"
  }'
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "id": "660e8400-e29b-41d4-a716-446655440001",
    "name": "Wireless Mouse Pro",
    "description": "Professional wireless mouse",
    "price": 49.99,
    "stock": 80,
    "category": "Accessories",
    "isActive": true,
    "createdAt": "2025-03-28T14:45:00Z",
    "updatedAt": "2025-03-28T15:20:00Z"
  },
  "message": "Product updated successfully"
}
```

---

### 5️⃣ Delete Product (Admin Only ✅ Protected)
```http
DELETE /api/products/{id} HTTP/1.1
Authorization: Bearer {token}
```

**cURL:**
```bash
curl -X DELETE "https://localhost:7xxx/api/products/660e8400-e29b-41d4-a716-446655440001" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": true,
  "message": "Product deleted successfully"
}
```

---

## 🔴 Error Responses

### Bad Request (400)
```json
{
  "success": false,
  "data": null,
  "message": "Error creating product: Name cannot be empty"
}
```

### Unauthorized (401)
```json
{
  "success": false,
  "data": null,
  "message": "Unauthorized"
}
```

### Forbidden (403)
```json
{
  "success": false,
  "data": null,
  "message": "Forbidden"
}
```

### Not Found (404)
```json
{
  "success": false,
  "data": null,
  "message": "Product not found"
}
```

---

## 🧪 Testing with Postman

1. **Create Environment** with variable `baseUrl`
2. **Add Pre-request Script**:
```javascript
// Get token from UserService login response and save it
pm.environment.set("token", pm.response.json().token);
```

3. **Add Authorization Header to Each Request**:
```
Authorization: Bearer {{token}}
```

---

## 🔗 Production Deployment Checklist

- [ ] Update `appsettings.json` with production connection string
- [ ] Change JWT secret key to a strong, random value
- [ ] Enable HTTPS with valid SSL certificate
- [ ] Set `Environment` to `Production` (disables Swagger in production)
- [ ] Configure CORS for frontend domain
- [ ] Set up database backups
- [ ] Enable logging and monitoring
- [ ] Configure API rate limiting
- [ ] Set up automated deployment pipeline

---

## 📊 Database Operations

### Apply Migrations
```bash
dotnet ef database update -p Product.Infrastructure -s Product.API
```

### Create New Migration
```bash
dotnet ef migrations add MigrationName -p Product.Infrastructure
```

### View Current Database State
```bash
dotnet ef dbcontext info -p Product.Infrastructure -s Product.API
```

---

## 💡 Tips & Best Practices

1. **Always use HTTPS** in production
2. **Validate input** on the client side before sending
3. **Store JWT tokens securely** (HttpOnly cookies, secure storage)
4. **Implement request rate limiting** to prevent abuse
5. **Log all API calls** for debugging and monitoring
6. **Use pagination** for large product lists
7. **Cache frequently accessed products** with Redis
8. **Monitor API performance** and database queries

---

**Last Updated**: March 28, 2025
