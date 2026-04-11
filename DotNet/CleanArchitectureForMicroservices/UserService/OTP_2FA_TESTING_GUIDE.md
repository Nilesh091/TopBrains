# OTP Two-Factor Authentication (2FA) Implementation Guide

## 📋 Overview

The UserService now implements **OTP-based 2FA for login** with the following flow:

```
1. User Login (email/password) ✓
   ↓
2. Validate email/password ✓
   ↓
3. IF 2FA enabled → Generate 6-digit OTP ✓
   ↓
4. Send OTP via Email (Notification Service) ✓
   ↓
5. Return RequiresTwoFactor = true (NO token yet) ✓
   ↓
6. User receives OTP in email
   ↓
7. User submits OTP to /verify-otp endpoint
   ↓
8. Backend validates OTP (6 attempts, 10 min expiry)
   ↓
9. If valid → Generate JWT & Refresh Token
   ↓
10. User gets bearer token for authorized endpoints ✓
```

---

## 🔧 Setup & Database Migration

### Step 1: Apply Database Migration

The new OTP fields have been added to the User entity:

- `OtpCode` - Stores the 6-digit OTP
- `OtpExpiryTime` - Expires after 10 minutes
- `OtpAttempts` - Max 3 verification attempts
- `IsTwoFactorEnabled` - Enables/disables 2FA per user (default: true)

```bash
cd UserService

# Apply the migration
dotnet ef database update --project UserService.Infrastructure --startup-project UserService.API
```

**Or using User Secrets** (recommended for production):

```bash
dotnet user-secrets init --project UserService.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=UserServiceDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"

# Then apply migration
dotnet ef database update --project UserService.Infrastructure --startup-project UserService.API
```

### Step 2: Start the API

```bash
cd UserService.API
dotnet watch run
```

API available at: `https://localhost:5190`

---

## 🧪 TEST THE OTP 2FA FLOW

### Option 1: Using VS Code REST Client

Create `UserService.API/requests-otp.http`:

```http
@baseUrl = https://localhost:5190/api/user
@userId = YOUR_USER_ID_HERE
@otpCode = 000000

### 1. REGISTER NEW USER (Step 1)
POST {{baseUrl}}/register
Content-Type: application/json

{
  "userName": "otp_test_user",
  "email": "otp_test@example.com",
  "password": "SecurePass123!",
  "fullName": "OTP Test User",
  "phoneNumber": "+1234567890"
}

### 2. VERIFY EMAIL (Step 2)
POST {{baseUrl}}/verify-email
Content-Type: application/json

{
  "userId": "{{userId}}",
  "token": "TOKEN_FROM_EMAIL"
}

### 3. LOGIN - Triggers OTP (Step 3)
POST {{baseUrl}}/login
Content-Type: application/json

{
  "emailOrUserName": "otp_test@example.com",
  "password": "SecurePass123!",
  "clientId": "web"
}

@Response = will have RequiresTwoFactor: true and NO Token

### 4. VERIFY OTP - Get Bearer Token (Step 4)
POST {{baseUrl}}/verify-otp
Content-Type: application/json

{
  "userId": "{{userId}}",
  "otpCode": "{{otpCode}}",
  "clientId": "web"
}

@Response = will have Token and RefreshToken

### 5. USE TOKEN ON PROTECTED ENDPOINT
GET {{baseUrl}}/profile/{{userId}}
Authorization: Bearer {{token}}
```

---

### Option 2: Using cURL Scripts

**Step 1: Register User**

```bash
curl -X POST https://localhost:5190/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "otp_test",
    "email": "otp_test@example.com",
    "password": "SecurePass123!",
    "fullName": "OTP Test",
    "phoneNumber": "+1234567890"
  }' -k
```

**Step 2: Verify Email** (get token from email sent via Notification Service)

```bash
curl -X POST https://localhost:5190/api/user/verify-email \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID_FROM_REGISTER",
    "token": "TOKEN_FROM_EMAIL"
  }' -k
```

**Step 3: Login** (triggers OTP)

```bash
curl -X POST https://localhost:5190/api/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUserName": "otp_test@example.com",
    "password": "SecurePass123!",
    "clientId": "web"
  }' -k

# Response will be:
# {
#   "succeeded": false,
#   "token": null,
#   "refreshToken": null,
#   "requiresTwoFactor": true,
#   "errorMessage": null
# }
```

**Step 4: Check Email for OTP**

Look in logs or notification service:

```bash
# View application logs
tail -f UserService.API/logs/userservice-*.txt | grep -i otp
```

**Step 5: Verify OTP** (get the 6-digit code from email)

```bash
curl -X POST https://localhost:5190/api/user/verify-otp \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID_FROM_REGISTER",
    "otpCode": "123456",
    "clientId": "web"
  }' -k

# Response will be:
# {
#   "succeeded": true,
#   "token": "eyJhbGc...",
#   "refreshToken": "refresh_token_here",
#   "requiresTwoFactor": false,
#   "errorMessage": null
# }
```

**Step 6: Use Token on Protected Endpoint**

```bash
curl -X GET https://localhost:5190/api/user/profile/USER_ID \
  -H "Authorization: Bearer eyJhbGc..." \
  -k
```

---

## 🧪 TEST ERROR SCENARIOS

### Scenario 1: Wrong OTP Code (3 attempts max)

```bash
# Attempt 1
curl -X POST https://localhost:5190/api/user/verify-otp \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID",
    "otpCode": "000000",
    "clientId": "web"
  }' -k

# Response: Invalid OTP. 2 attempt(s) remaining.

# Attempt 2
# ... same request ...
# Response: Invalid OTP. 1 attempt(s) remaining.

# Attempt 3
# ... same request ...
# Response: Maximum OTP attempts exceeded. Please login again.
```

### Scenario 2: Expired OTP (10 minute window)

```bash
# Wait > 10 minutes after login
curl -X POST https://localhost:5190/api/user/verify-otp \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID",
    "otpCode": "CORRECT_CODE",
    "clientId": "web"
  }' -k

# Response: OTP has expired. Please login again.
```

### Scenario 3: Missing OTP Code

```bash
curl -X POST https://localhost:5190/api/user/verify-otp \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID",
    "otpCode": "",
    "clientId": "web"
  }' -k

# Response: 400 Bad Request with validation errors
# - OTP code is required
# - OTP code must be exactly 6 digits
# - OTP code must contain only numeric digits
```

### Scenario 4: Invalid OTP Format

```bash
curl -X POST https://localhost:5190/api/user/verify-otp \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "USER_ID",
    "otpCode": "ABC123",
    "clientId": "web"
  }' -k

# Response: 400 Bad Request
# - OTP code must contain only numeric digits
```

---

## 🔐 DISABLE/ENABLE 2FA PER USER

To disable 2FA for a user, update the `IsTwoFactorEnabled` field:

```sql
-- Disable 2FA for a user
UPDATE Users
SET IsTwoFactorEnabled = 0
WHERE Id = 'USER_ID_HERE';

-- Enable 2FA for a user
UPDATE Users
SET IsTwoFactorEnabled = 1
WHERE Id = 'USER_ID_HERE';
```

Or **update programmatically** (requires adding endpoint):

```csharp
[Authorize]
[HttpPost("toggle-2fa")]
public async Task<IActionResult> Toggle2FA([FromBody] Toggle2FADTO dto)
{
    var user = await _userRepository.FindByIdAsync(dto.UserId);
    if (user == null)
        return NotFound();

    user.IsTwoFactorEnabled = dto.Enabled;
    await _userRepository.UpdateUserAsync(user);

    return Ok(ApiResponse<string>.SuccessResponse(
        $"2FA {(dto.Enabled ? "enabled" : "disabled")} successfully"
    ));
}
```

---

## 📧 OTP EMAIL FORMAT

When OTP is sent, the email contains:

```
Subject: Your 2FA One-Time Password (OTP)

Body:
Your OTP for Two-Factor Authentication is: 123456

This code will expire in 10 minutes.

Do not share this code with anyone.
```

The OTP is sent via the **Notification Service** → ensure it's configured in `appsettings.json`:

```json
{
  "Services": {
    "NotificationService": "http://localhost:5191"
  }
}
```

---

## 📊 LOGIN FLOW COMPARISON

### Before OTP 2FA:

```
POST /login
↓
Validate password
↓
Generate JWT + Refresh Token
↓
Return Token (user can access endpoints)
```

### With OTP 2FA:

```
POST /login
↓
Validate password
↓
Generate OTP + Send Email
↓
Return RequiresTwoFactor: true (NO token)
↓
POST /verify-otp
↓
Validate OTP
↓
Generate JWT + Refresh Token
↓
Return Token (user can access endpoints)
```

---

## 🔍 DEBUGGING & LOGS

### View OTP in Logs

```bash
# Watch for OTP generation
tail -f UserService.API/logs/userservice-*.txt | grep -i "otp\|2fa"
```

### Check Database for OTP

```sql
-- View OTP details for a user
SELECT Id, UserName, Email, IsTwoFactorEnabled, OtpCode, OtpExpiryTime, OtpAttempts
FROM Users
WHERE Email = 'otp_test@example.com';
```

### Test OTP Generation Directly

```bash
# Check the random OTP order (in code):
OtpCode = new Random().Next(100000, 999999).ToString();
# This generates values from 100000-999999 (6 digits)
```

---

## ⚙️ IMPLEMENTATION DETAILS

### Files Modified/Created:

**New Files:**

- `UserService.Application/DTOs/VerifyOtpDTO.cs` - OTP verification request
- `UserService.Application/Validators/VerifyOtpDTOValidator.cs` - OTP validation rules
- Migration: `AddOtpTwoFactorAuthentication`

**Modified Files:**

- `UserService.Domain/Entities/User.cs` - Added OTP fields
- `UserService.Infrastructure/Identity/ApplicationUser.cs` - Added OTP fields
- `UserService.Application/Services/IUserService.cs` - Added 2 new methods
- `UserService.Application/Services/UserService.cs` - Implemented OTP logic
- `UserService.Application/Services/IEmailService.cs` - Added SendOtpAsync method
- `UserService.Application/Services/EmailService.cs` - Implemented SendOtpAsync
- `UserService.API/Controllers/UserController.cs` - Added /verify-otp endpoint
- `UserService.Tests/UnitTest1.cs` - Updated test mocks

### Key Classes:

**VerifyOtpDTO:**

```csharp
public class VerifyOtpDTO
{
    public Guid UserId { get; set; }
    public string OtpCode { get; set; } = null!;  // 6 digits
    public string ClientId { get; set; } = null!;
}
```

**OTP Lifecycle:**

1. Generated: `Random.Next(100000, 999999)`
2. Stored: `User.OtpCode`, `User.OtpExpiryTime` (+10 min)
3. Attempts: Max 3, tracked in `User.OtpAttempts`
4. Validation: Must match exactly, case-sensitive
5. Cleanup: Cleared after successful verification or expiry

---

## 🚀 NEXT STEPS

1. ✅ Start API: `dotnet watch run`
2. ✅ Apply migration: `dotnet ef database update ...`
3. ✅ Test register → verify email → login → OTP → access endpoints
4. ⚠️ **TODO**: Integrate with Notification Service sending real emails
5. ⚠️ **TODO**: Add admin endpoint to toggle 2FA per user
6. ⚠️ **TODO**: Add backup OTP codes generation
7. ⚠️ **TODO**: Rate limit OTP generation (1 per 2 minutes)

---

## 📞 Troubleshooting

| Issue                       | Solution                                                             |
| --------------------------- | -------------------------------------------------------------------- |
| "OTP has expired"           | OTP expires after 10 minutes. Login again to get new OTP.            |
| "Maximum attempts exceeded" | Too many wrong OTPs. Login again and request new OTP.                |
| "No OTP found"              | Session expired. Login again to generate new OTP.                    |
| "Invalid OTP format"        | OTP must be exactly 6 digits, all numbers.                           |
| OTP not sent in email       | Check Notification Service is running at correct URL in appsettings. |
| Database migration failed   | Ensure SQL Server is running and connection string is correct.       |

---

**Happy Testing! 🎉**
