# API Examples & Usage Guide

## Prerequisites

### Get JWT Token

First, obtain a valid JWT token from User Service. The token should include:

- `sub` (NameIdentifier): User ID
- `role`: "Buyer"

Example token header (replace with your actual token):

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyLTEyMyIsInJvbGUiOiJCdXllciIsImlhdCI6IjIwMjYtMDMtMzEifQ...
```

---

## 📦 CART ENDPOINTS

### 1️⃣ Get User's Cart

Get all items in current user's cart.

**cURL:**

```bash
curl -X GET https://localhost:7000/api/cart \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json"
```

**Postman:**

- Method: `GET`
- URL: `https://localhost:7000/api/cart`
- Headers:
  - `Authorization: Bearer YOUR_JWT_TOKEN`
  - `Content-Type: application/json`

**Successful Response (200):**

```json
{
  "success": true,
  "message": "Cart retrieved successfully",
  "data": {
    "id": "5a8d9c2e-9f1b-4a3c-8e7d-1b5c9d7e2a4f",
    "userId": "user-123",
    "items": [
      {
        "id": "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
        "productId": "prod-001",
        "productName": "Laptop",
        "price": 999.99,
        "quantity": 1,
        "lineTotal": 999.99,
        "addedAt": "2026-03-31T09:30:00Z"
      },
      {
        "id": "4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a",
        "productId": "prod-002",
        "productName": "Mouse",
        "price": 49.99,
        "quantity": 2,
        "lineTotal": 99.98,
        "addedAt": "2026-03-31T10:15:00Z"
      }
    ],
    "total": 1099.97,
    "itemCount": 2,
    "createdAt": "2026-03-31T09:00:00Z",
    "updatedAt": "2026-03-31T10:15:00Z"
  },
  "timestamp": "2026-03-31T10:20:00Z"
}
```

**Empty Cart Response (200):**

```json
{
  "success": true,
  "message": "Cart retrieved successfully",
  "data": {
    "id": "5a8d9c2e-9f1b-4a3c-8e7d-1b5c9d7e2a4f",
    "userId": "user-123",
    "items": [],
    "total": 0,
    "itemCount": 0,
    "createdAt": "2026-03-31T09:00:00Z",
    "updatedAt": "2026-03-31T09:00:00Z"
  },
  "timestamp": "2026-03-31T10:20:00Z"
}
```

---

### 2️⃣ Add Product to Cart

Add a new product or increase quantity of existing product.

**cURL:**

```bash
curl -X POST https://localhost:7000/api/cart/add \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "prod-001",
    "productName": "Laptop",
    "price": 999.99,
    "quantity": 1
  }'
```

**Postman:**

- Method: `POST`
- URL: `https://localhost:7000/api/cart/add`
- Headers:
  - `Authorization: Bearer YOUR_JWT_TOKEN`
  - `Content-Type: application/json`
- Body (raw JSON):

```json
{
  "productId": "prod-001",
  "productName": "Laptop",
  "price": 999.99,
  "quantity": 1
}
```

**Request Fields:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| productId | string | Yes | ID from Product Service |
| productName | string | Yes | Display name |
| price | decimal | Yes | Current product price |
| quantity | int | Yes | Quantity to add |

**Success Response (200):**

```json
{
  "success": true,
  "message": "Product added to cart successfully",
  "data": {
    "id": "5a8d9c2e-9f1b-4a3c-8e7d-1b5c9d7e2a4f",
    "userId": "user-123",
    "items": [
      {
        "id": "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
        "productId": "prod-001",
        "productName": "Laptop",
        "price": 999.99,
        "quantity": 1,
        "lineTotal": 999.99,
        "addedAt": "2026-03-31T10:25:00Z"
      }
    ],
    "total": 999.99,
    "itemCount": 1,
    "createdAt": "2026-03-31T09:00:00Z",
    "updatedAt": "2026-03-31T10:25:00Z"
  },
  "timestamp": "2026-03-31T10:25:00Z"
}
```

**Error Response (400):**

```json
{
  "success": false,
  "message": "Invalid request data",
  "errorCode": "VALIDATION_ERROR",
  "timestamp": "2026-03-31T10:25:00Z"
}
```

---

### 3️⃣ Update Cart Item Quantity

Change the quantity of an item in cart.

**cURL:**

```bash
curl -X PUT https://localhost:7000/api/cart/update \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "cartItemId": "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
    "quantity": 5
  }'
```

**Postman:**

- Method: `PUT`
- URL: `https://localhost:7000/api/cart/update`
- Body (raw JSON):

```json
{
  "cartItemId": "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
  "quantity": 5
}
```

**Request Fields:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| cartItemId | guid | Yes | ID of the cart item |
| quantity | int | Yes | New quantity (0 = remove item) |

**Success Response (200):**

```json
{
  "success": true,
  "message": "Cart item updated successfully",
  "data": {
    "id": "5a8d9c2e-9f1b-4a3c-8e7d-1b5c9d7e2a4f",
    "userId": "user-123",
    "items": [
      {
        "id": "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
        "productId": "prod-001",
        "productName": "Laptop",
        "price": 999.99,
        "quantity": 5,
        "lineTotal": 4999.95,
        "addedAt": "2026-03-31T10:25:00Z"
      }
    ],
    "total": 4999.95,
    "itemCount": 1,
    "createdAt": "2026-03-31T09:00:00Z",
    "updatedAt": "2026-03-31T10:30:00Z"
  },
  "timestamp": "2026-03-31T10:30:00Z"
}
```

---

### 4️⃣ Remove from Cart

Remove a specific item from cart.

**cURL:**

```bash
curl -X DELETE https://localhost:7000/api/cart/remove/3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Postman:**

- Method: `DELETE`
- URL: `https://localhost:7000/api/cart/remove/3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f`
- Headers: Authorization header with token

**URL Parameter:**

- `cartItemId` (path parameter): The ID of the cart item to remove

**Success Response (200):**

```json
{
  "success": true,
  "message": "Item removed from cart successfully",
  "data": {
    "id": "5a8d9c2e-9f1b-4a3c-8e7d-1b5c9d7e2a4f",
    "userId": "user-123",
    "items": [],
    "total": 0,
    "itemCount": 0,
    "createdAt": "2026-03-31T09:00:00Z",
    "updatedAt": "2026-03-31T10:35:00Z"
  },
  "timestamp": "2026-03-31T10:35:00Z"
}
```

---

### 5️⃣ Clear Cart

Remove all items from cart.

**cURL:**

```bash
curl -X DELETE https://localhost:7000/api/cart/clear \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Postman:**

- Method: `DELETE`
- URL: `https://localhost:7000/api/cart/clear`

**Success Response (200):**

```json
{
  "success": true,
  "message": "Cart cleared successfully",
  "timestamp": "2026-03-31T10:40:00Z"
}
```

---

## 📋 ORDER ENDPOINTS

### 1️⃣ Create Order

Convert cart items into an order and initiate payment.

**cURL:**

```bash
curl -X POST https://localhost:7000/api/order/create \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "shippingAddress": "123 Main Street, Springfield, IL 62701",
    "notes": "Please deliver on weekends"
  }'
```

**Postman:**

- Method: `POST`
- URL: `https://localhost:7000/api/order/create`
- Body (raw JSON):

```json
{
  "shippingAddress": "123 Main Street, Springfield, IL 62701",
  "notes": "Please deliver on weekends"
}
```

**Request Fields:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| shippingAddress | string | Yes | Delivery address |
| notes | string | No | Optional order notes |

**Success Response (201):**

```json
{
  "success": true,
  "message": "Order created successfully",
  "data": {
    "orderId": "7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e",
    "orderNumber": "ORD-20260331-ABC123DE",
    "totalAmount": 5999.94,
    "paymentUrl": "https://payment-service.local/pay?id=pay-guid-123",
    "message": "Order created successfully. Please proceed to payment."
  },
  "timestamp": "2026-03-31T10:45:00Z"
}
```

**Error Response - Empty Cart (400):**

```json
{
  "success": false,
  "message": "Cart is empty",
  "errorCode": "INVALID_OPERATION",
  "timestamp": "2026-03-31T10:45:00Z"
}
```

---

### 2️⃣ Get Order Details

Retrieve details of a specific order.

**cURL:**

```bash
curl -X GET https://localhost:7000/api/order/7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Postman:**

- Method: `GET`
- URL: `https://localhost:7000/api/order/7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e`

**URL Parameter:**

- `orderId` (path parameter): The order ID

**Success Response (200):**

```json
{
  "success": true,
  "message": "Order retrieved successfully",
  "data": {
    "id": "7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e",
    "orderNumber": "ORD-20260331-ABC123DE",
    "userId": "user-123",
    "items": [
      {
        "id": "8c9d0e1f-2a3b-4c5d-6e7f-8a9b0c1d2e3f",
        "productId": "prod-001",
        "productName": "Laptop",
        "unitPrice": 999.99,
        "quantity": 5,
        "lineTotal": 4999.95
      },
      {
        "id": "9d0e1f2a-3b4c-5d6e-7f8a-9b0c1d2e3f4a",
        "productId": "prod-002",
        "productName": "Mouse",
        "unitPrice": 49.99,
        "quantity": 2,
        "lineTotal": 99.98
      }
    ],
    "totalAmount": 5999.94,
    "status": "Pending",
    "paymentStatus": "Pending",
    "paymentId": null,
    "shippingAddress": "123 Main Street, Springfield, IL 62701",
    "invoiceId": null,
    "createdAt": "2026-03-31T10:45:00Z",
    "updatedAt": "2026-03-31T10:45:00Z"
  },
  "timestamp": "2026-03-31T10:50:00Z"
}
```

---

### 3️⃣ Get All User Orders

Retrieve all orders for the current user.

**cURL:**

```bash
curl -X GET https://localhost:7000/api/order/user/all \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Postman:**

- Method: `GET`
- URL: `https://localhost:7000/api/order/user/all`

**Success Response (200):**

```json
{
  "success": true,
  "message": "Orders retrieved successfully",
  "data": [
    {
      "id": "7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e",
      "orderNumber": "ORD-20260331-ABC123DE",
      "userId": "user-123",
      "items": [...],
      "totalAmount": 5999.94,
      "status": "Paid",
      "paymentStatus": "Success",
      "paymentId": "pay-123",
      "shippingAddress": "123 Main Street, Springfield, IL 62701",
      "invoiceId": "inv-guid",
      "createdAt": "2026-03-31T10:45:00Z",
      "updatedAt": "2026-03-31T11:00:00Z"
    },
    {
      "id": "6a7b8c9d-0e1f-2a3b-4c5d-6e7f8a9b0c1d",
      "orderNumber": "ORD-20260330-XYZ789AB",
      "userId": "user-123",
      "items": [...],
      "totalAmount": 299.97,
      "status": "Pending",
      "paymentStatus": "Pending",
      "paymentId": null,
      "shippingAddress": "456 Oak Ave, Springfield, IL 62702",
      "invoiceId": null,
      "createdAt": "2026-03-30T14:30:00Z",
      "updatedAt": "2026-03-30T14:30:00Z"
    }
  ],
  "timestamp": "2026-03-31T10:55:00Z"
}
```

---

### 4️⃣ Confirm Payment

Verify payment and mark order as paid.

**cURL:**

```bash
curl -X POST "https://localhost:7000/api/order/7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e/confirm-payment?paymentId=pay-123" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json"
```

**Postman:**

- Method: `POST`
- URL: `https://localhost:7000/api/order/7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e/confirm-payment?paymentId=pay-123`

**Query Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| paymentId | string | Yes | Payment ID from Payment Service |

**Success Response (200):**

```json
{
  "success": true,
  "message": "Payment confirmed and invoice generated successfully",
  "data": {
    "id": "7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e",
    "orderNumber": "ORD-20260331-ABC123DE",
    "userId": "user-123",
    "items": [...],
    "totalAmount": 5999.94,
    "status": "Paid",
    "paymentStatus": "Success",
    "paymentId": "pay-123",
    "shippingAddress": "123 Main Street, Springfield, IL 62701",
    "invoiceId": "7f8a9b0c-1d2e-3f4a-5b6c-7d8e9f0a1b2c",
    "createdAt": "2026-03-31T10:45:00Z",
    "updatedAt": "2026-03-31T11:00:00Z"
  },
  "timestamp": "2026-03-31T11:00:00Z"
}
```

---

### 5️⃣ Get Order Invoice

Retrieve invoice for a paid order.

**cURL:**

```bash
curl -X GET https://localhost:7000/api/order/7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e/invoice \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Postman:**

- Method: `GET`
- URL: `https://localhost:7000/api/order/7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e/invoice`

**Success Response (200):**

```json
{
  "success": true,
  "message": "Invoice retrieved successfully",
  "data": {
    "id": "7f8a9b0c-1d2e-3f4a-5b6c-7d8e9f0a1b2c",
    "invoiceNumber": "INV-20260331-ABC123XY",
    "orderId": "7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e",
    "userId": "user-123",
    "subTotal": 5999.94,
    "taxAmount": 600.0,
    "discountAmount": 0.0,
    "totalAmount": 6599.94,
    "paymentId": "pay-123",
    "status": "Paid",
    "issuedAt": "2026-03-31T11:00:00Z",
    "paidAt": "2026-03-31T11:00:00Z",
    "dueDate": null,
    "notes": "Invoice for Order ORD-20260331-ABC123DE"
  },
  "timestamp": "2026-03-31T11:05:00Z"
}
```

---

### 6️⃣ Get All User Invoices

Retrieve all invoices for the current user.

**cURL:**

```bash
curl -X GET https://localhost:7000/api/order/invoices/all \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Postman:**

- Method: `GET`
- URL: `https://localhost:7000/api/order/invoices/all`

**Success Response (200):**

```json
{
  "success": true,
  "message": "Invoices retrieved successfully",
  "data": [
    {
      "id": "7f8a9b0c-1d2e-3f4a-5b6c-7d8e9f0a1b2c",
      "invoiceNumber": "INV-20260331-ABC123XY",
      "orderId": "7b8c9d0e-1f2a-3b4c-5d6e-7f8a9b0c1d2e",
      "userId": "user-123",
      "subTotal": 5999.94,
      "taxAmount": 600.0,
      "discountAmount": 0.0,
      "totalAmount": 6599.94,
      "paymentId": "pay-123",
      "status": "Paid",
      "issuedAt": "2026-03-31T11:00:00Z",
      "paidAt": "2026-03-31T11:00:00Z",
      "dueDate": null,
      "notes": "Invoice for Order ORD-20260331-ABC123DE"
    }
  ],
  "timestamp": "2026-03-31T11:05:00Z"
}
```

---

## 🔐 Authentication Examples

### Using Bearer Token with jQuery/JavaScript

```javascript
const headers = {
  Authorization: "Bearer " + jwtToken,
  "Content-Type": "application/json",
};

fetch("https://localhost:7000/api/cart", {
  method: "GET",
  headers: headers,
})
  .then((response) => response.json())
  .then((data) => console.log(data));
```

### Using Axios

```javascript
const config = {
  headers: {
    Authorization: "Bearer " + jwtToken,
    "Content-Type": "application/json",
  },
};

axios
  .get("https://localhost:7000/api/cart", config)
  .then((response) => console.log(response.data))
  .catch((error) => console.error(error));
```

---

## ✅ Testing Workflow

### 1. Add Items to Cart

```bash
# Add Laptop
curl -X POST https://localhost:7000/api/cart/add \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"productId":"prod-001","productName":"Laptop","price":999.99,"quantity":1}'

# Add Mouse
curl -X POST https://localhost:7000/api/cart/add \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"productId":"prod-002","productName":"Mouse","price":49.99,"quantity":2}'

# View Cart
curl -X GET https://localhost:7000/api/cart \
  -H "Authorization: Bearer TOKEN"
```

### 2. Create Order

```bash
curl -X POST https://localhost:7000/api/order/create \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "shippingAddress": "123 Main Street, Springfield, IL 62701",
    "notes": "Please deliver on weekends"
  }'
```

### 3. Confirm Payment (use paymentId from create order response)

```bash
curl -X POST "https://localhost:7000/api/order/ORDER_ID/confirm-payment?paymentId=PAYMENT_ID" \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json"
```

### 4. View Invoice

```bash
curl -X GET https://localhost:7000/api/order/ORDER_ID/invoice \
  -H "Authorization: Bearer TOKEN"
```

---

## 📊 Status Codes

| Code | Meaning      | Common Reason                 |
| ---- | ------------ | ----------------------------- |
| 200  | OK           | Request succeeded             |
| 201  | Created      | Resource successfully created |
| 400  | Bad Request  | Invalid input or operation    |
| 401  | Unauthorized | Missing or invalid JWT token  |
| 403  | Forbidden    | User lacks required role      |
| 404  | Not Found    | Resource not found            |
| 500  | Server Error | Internal error                |

---

## 🐛 Debugging Tips

1. **Enable Logging**: Check application logs for detailed errors
2. **Validate Token**: Ensure JWT token includes required claims
3. **Check Database**: Verify database is running and accessible
4. **HTTPS Certificate**: Accept/bypass self-signed certificate in dev
5. **Connection String**: Verify SQL Server connection details
6. **External Services**: Ensure Product Service is accessible (if testing)

---

For more information, see [README.md](README.md) and [SETUP.md](SETUP.md)
