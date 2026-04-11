# UserService API - Debug & Testing Guide

## 🚀 QUICK START

### 1. Setup Prerequisites

```bash
cd UserService

# Restore packages
dotnet restore

# Create/update database
dotnet ef database update --project UserService.Infrastructure --startup-project UserService.API

# (Optional) Configure User Secrets for development
dotnet user-secrets init --project UserService.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=UserServiceDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;" --project UserService.API
```

---

## 🎯 RUN THE API

### Option 1: Visual Studio / VS Code

```bash
# Open terminal in UserService.API folder
cd UserService.API

# Run with hot reload
dotnet watch run
```

**API will be available at:** `https://localhost:5190`

### Option 2: Command Line

```bash
cd UserService

dotnet run --project UserService.API
```

---

## 🧪 RUN UNIT TESTS

### Run All Tests

```bash
cd UserService

# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

### Run Specific Test Class

```bash
# Test UserService class only
dotnet test --filter "ClassName=UserServiceTests"

# Test Value Objects
dotnet test --filter "ClassName=EmailValueObjectTests"

# Test Validators
dotnet test --filter "ClassName=RegisterDTOValidatorTests"
```

### Run Single Test Method

```bash
dotnet test --filter "FullyQualifiedName=UserService.Tests.UserServiceTests.RegisterAsync_WithValidData_ShouldSucceed"
```

### Watch Mode (Continuous Testing)

```bash
dotnet watch test
```

---

## 📡 TEST API ENDPOINTS

### Option 1: Using .http File (Built-in VS Code)

VS Code has a built-in REST Client. Use the `.http` file format:

Open or create `UserService.API/requests.http`:

```http
@baseUrl = http://localhost:5190/api/user
@token = YOUR_JWT_TOKEN_HERE

### 1. REGISTER NEW USER
POST {{baseUrl}}/register
Content-Type: application/json

{
  "userName": "testuser123",
  "email": "testuser@example.com",
  "password": "SecurePass123!",
  "fullName": "Test User",
  "phoneNumber": "+1234567890"
}

### 2. SEND CONFIRMATION EMAIL
POST {{baseUrl}}/send-confirmation-email
Content-Type: application/json

{
  "email": "testuser@example.com"
}

### 3. VERIFY EMAIL (after getting token from email)
POST {{baseUrl}}/verify-email
Content-Type: application/json

{
  "userId": "USER_ID_HERE",
  "token": "TOKEN_FROM_EMAIL"
}

### 4. LOGIN
POST {{baseUrl}}/login
Content-Type: application/json

{
  "emailOrUserName": "testuser@example.com",
  "password": "SecurePass123!",
  "clientId": "web"
}

### 5. REFRESH TOKEN (with refresh token from login)
POST {{baseUrl}}/refresh-token
Content-Type: application/json

{
  "refreshToken": "REFRESH_TOKEN_FROM_LOGIN",
  "clientId": "web"
}

### 6. GET PROFILE (requires authorization)
GET {{baseUrl}}/profile/USER_ID_HERE
Authorization: Bearer {{token}}

### 7. UPDATE PROFILE (requires authorization)
PUT {{baseUrl}}/profile
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "userId": "USER_ID_HERE",
  "fullName": "Updated Name",
  "phoneNumber": "+9876543210",
  "profilePhotoUrl": "https://example.com/photo.jpg"
}

### 8. ADD ADDRESS (requires authorization)
POST {{baseUrl}}/addresses
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "userId": "USER_ID_HERE",
  "addressLine1": "123 Main Street",
  "addressLine2": "Apt 4B",
  "city": "New York",
  "state": "NY",
  "postalCode": "10001",
  "country": "USA",
  "isDefaultShipping": true,
  "isDefaultBilling": true
}

### 9. GET ALL ADDRESSES
GET {{baseUrl}}/USER_ID_HERE/addresses

### 10. GET SPECIFIC ADDRESS
GET {{baseUrl}}/USER_ID_HERE/address/ADDRESS_ID_HERE

### 11. DELETE ADDRESS (requires authorization)
POST {{baseUrl}}/delete-address
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "userId": "USER_ID_HERE",
  "addressId": "ADDRESS_ID_HERE"
}

### 12. CHANGE PASSWORD (requires authorization)
POST {{baseUrl}}/change-password
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "currentPassword": "SecurePass123!",
  "newPassword": "NewSecurePass456!"
}

### 13. FORGOT PASSWORD
POST {{baseUrl}}/forgot-password
Content-Type: application/json

{
  "email": "testuser@example.com"
}

### 14. RESET PASSWORD (with token from forgot-password email)
POST {{baseUrl}}/reset-password
Content-Type: application/json

{
  "userId": "USER_ID_HERE",
  "token": "TOKEN_FROM_EMAIL",
  "newPassword": "NewSecurePass789!"
}

### 15. CHECK IF USER EXISTS
GET {{baseUrl}}/USER_ID_HERE/exists

### 16. REVOKE TOKEN
POST {{baseUrl}}/revoke-token
Content-Type: application/json

{
  "refreshToken": "REFRESH_TOKEN_HERE",
  "clientId": "web"
}
```

**How to use in VS Code:**

1. Install "REST Client" extension (humao.rest-client)
2. Click "Send Request" above each request
3. Response appears in split panel

---

### Option 2: Using Postman

1. **Create a new Postman Collection**
2. **Set up environment variables:**
   - `baseUrl`: `http://localhost:5190/api/user`
   - `token`: (update after login)
   - `userId`: (update with user ID)
   - `refreshToken`: (update after login)

3. **Create requests following the .http examples above**

4. **Import the .http file:**
   - Postman → File → Import
   - Select → requests.http file
   - Auto-converts HTTP requests to Postman collection

---

### Option 3: Using cURL

```bash
# REGISTER
curl -X POST http://localhost:5190/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "testuser",
    "email": "test@example.com",
    "password": "SecurePass123!",
    "fullName": "Test User",
    "phoneNumber": "+1234567890"
  }'

# LOGIN
curl -X POST http://localhost:5190/api/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUserName": "test@example.com",
    "password": "SecurePass123!",
    "clientId": "web"
  }'

# GET PROFILE (with JWT token)
curl -X GET http://localhost:5190/api/user/profile/USER_ID \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# UPDATE PROFILE
curl -X PUT http://localhost:5190/api/user/profile \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID",
    "fullName": "Updated Name",
    "phoneNumber": "+9876543210",
    "profilePhotoUrl": "https://example.com/photo.jpg"
  }'
```

---

## 🐛 DEBUG THE API

### Option 1: VS Code Debugging

Create `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Debug",
      "type": "coreclr",
      "request": "launch",
      "program": "${workspaceFolder}/UserService.API/bin/Debug/net10.0/UserService.API.dll",
      "args": [],
      "cwd": "${workspaceFolder}/UserService.API",
      "stopAtEntry": false,
      "console": "internalConsole",
      "serverReadyAction": {
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
        "uriFormat": "%s",
        "action": "openExternally"
      }
    }
  ]
}
```

**Steps:**

1. Set breakpoints (click line number in code)
2. Press F5 or Debug → Start Debugging
3. API runs with debugger attached
4. Step through code with F10, F11
5. Watch variables in Debug panel

### Option 2: Add Console Logging

```csharp
// In UserController.cs methods
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
{
    Console.WriteLine($"[DEBUG] Register called with email: {dto.Email}");

    try
    {
        var result = await _userService.RegisterAsync(dto);
        Console.WriteLine($"[DEBUG] Registration result: {result}");

        if (!result)
            return BadRequest(...);

        return Ok(...);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Registration failed: {ex.Message}");
        Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
        throw;
    }
}
```

### Option 3: View Serilog Logs

**Real-time log viewing:**

```bash
cd UserService.API

# Watch log file (macOS/Linux)
tail -f logs/userservice-*.txt

# Watch log file (Windows PowerShell)
Get-Content logs\userservice-*.txt -Wait -Tail 15
```

**Logs show:**

- Request/Response times
- Exception details
- Correlation IDs
- Service operations

---

## 🧪 TEST WITH SPECIFIC SCENARIOS

### Scenario 1: Test Rate Limiting

```bash
# Quick script to test rate limiting
for i in {1..6}; do
  echo "Request $i:"
  curl -X POST http://localhost:5190/api/user/login \
    -H "Content-Type: application/json" \
    -d '{
      "emailOrUserName": "test@example.com",
      "password": "wrong",
      "clientId": "web"
    }' -w "\nStatus: %{http_code}\n\n"
  sleep 1
done

# You should see:
# Requests 1-5: 401 Unauthorized (rate limit allows 5)
# Request 6: 429 Too Many Requests (rate limited)
```

### Scenario 2: Test Authorization

```bash
# Without token (should fail)
curl -X PUT http://localhost:5190/api/user/profile \
  -H "Content-Type: application/json" \
  -d '{"userId": "...", "fullName": "Test"}'

# Response: 401 Unauthorized

# With token (should succeed)
curl -X PUT http://localhost:5190/api/user/profile \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"userId": "USER_ID", "fullName": "Updated"}'

# Response: 200 OK
```

### Scenario 3: Test Validation

```bash
# Send invalid email
curl -X POST http://localhost:5190/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "test",
    "email": "invalid-email",
    "password": "SecurePass123!",
    "fullName": "Test"
  }'

# Response: 400 Bad Request with validation errors

# Send weak password
curl -X POST http://localhost:5190/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "test",
    "email": "test@example.com",
    "password": "weak",
    "fullName": "Test"
  }'

# Response: 400 Bad Request with password validation error
```

### Scenario 4: Test Exception Handling

```bash
# Try to update profile with invalid user ID
curl -X PUT http://localhost:5190/api/user/profile \
  -H "Authorization: Bearer VALID_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "invalid-guid",
    "fullName": "Test",
    "phoneNumber": "+1234567890"
  }'

# Response includes:
# - Correlation ID (X-Correlation-ID header)
# - User-friendly error message
# - Logged in logs/userservice-*.txt
```

---

## 📊 PERFORMANCE TESTING

### Load Test with Apache Bench

```bash
# 100 requests, 10 concurrent
ab -n 100 -c 10 http://localhost:5190/api/user/USER_ID/exists

# Show results:
# - Requests per second
# - Response times
# - Failed requests
```

### Load Test with k6 (JavaScript)

```javascript
// loadtest.js
import http from "k6/http";
import { check, sleep } from "k6";

export let options = {
  vus: 10,
  duration: "30s",
};

export default function () {
  let loginResponse = http.post(
    "http://localhost:5190/api/user/login",
    JSON.stringify({
      emailOrUserName: "test@example.com",
      password: "SecurePass123!",
      clientId: "web",
    }),
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  check(loginResponse, {
    "login status is 200": (r) => r.status === 200,
    "login returns token": (r) => r.json("data.Token") !== null,
  });

  sleep(1);
}
```

```bash
# Run load test
k6 run loadtest.js
```

---

## 🔍 DEBUGGING CHECKLIST

- [ ] API is running (`dotnet run`)
- [ ] Database is accessible (SQL Server running)
- [ ] Logs are being generated (`logs/userservice-*.txt`)
- [ ] Breakpoints set in VS Code (F5 to debug)
- [ ] Check correlation ID in response headers
- [ ] Verify JWT token format (copy token to jwt.io to decode)
- [ ] Validate request/response JSON format
- [ ] Check rate limiter headers: `X-RateLimit-*`
- [ ] Verify authorization claims in JWT
- [ ] Test with various input validations

---

## 📋 COMMON ISSUES & FIXES

### Issue: "Connection refused"

```
Error: Unable to connect to localhost:1433
Fix: Start SQL Server or Docker container
```

### Issue: "Invalid token"

```
Error: Invalid audience/issuer in JWT
Fix: Verify JwtSettings in appsettings.json/User Secrets
```

### Issue: "Rate limit exceeded"

```
Error: 429 Too Many Requests
Fix: Wait for rate limit window to reset (15 min for login)
```

### Issue: "Authorization failed on protected endpoint"

```
Error: 401 Unauthorized / 403 Forbidden
Fix: Include valid JWT in Authorization header: "Bearer {token}"
```

### Issue: "Validation errors on register"

```
Error: 400 Bad Request with validation messages
Fix: Check password (8+ chars, upper, lower, digit), email format, phone format
```

---

## 🚀 PRODUCTION DEBUG TIPS

1. **Enable Application Insights (Azure)**

   ```csharp
   builder.Services.AddApplicationInsightsTelemetry();
   ```

2. **Log to Application Insights**
   - Serilog already configured for console + file
   - Add Serilog.Sinks.ApplicationInsights for cloud logging

3. **Monitor with Azure Monitor**
   - View real-time logs
   - Set up alerts
   - Track performance metrics

4. **Health Check Endpoint**
   ```bash
   curl http://localhost:5190/health
   ```

---

**Happy Testing! 🎉**
