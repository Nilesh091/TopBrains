# User Service Configuration Guide

## Secure Configuration Setup

### Development Environment

#### 1. Set Up User Secrets (instead of appsettings)

```bash
cd UserService.API/

# Initialize user secrets
dotnet user-secrets init

# Set database connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=UserServiceDb;User Id=sa;Password=YOUR_SA_PASSWORD;TrustServerCertificate=True;"

# Set JWT settings
dotnet user-secrets set "JwtSettings:SecretKey" "fPXxcJw8TW5sA+S4rl4tIPcKk+oXAqoRBo+1s2yjUS4="
dotnet user-secrets set "JwtSettings:Issuer" "UserService.API"
dotnet user-secrets set "JwtSettings:Audience" "UserService"
dotnet user-secrets set "JwtSettings:AccessTokenExpirationMinutes" "15"

# Set application URL
dotnet user-secrets set "AppSettings:AppUrl" "http://localhost:3000"

# Set notification service URL
dotnet user-secrets set "Services:NotificationService" "http://localhost:5191"
```

#### 2. View Configured Secrets

```bash
dotnet user-secrets list
```

#### 3. Clear Secrets (if needed)

```bash
dotnet user-secrets clear
```

---

### Production Environment (Azure Key Vault)

#### 1. Create Azure Key Vault

```bash
# Set variables
$resourceGroup = "your-resource-group"
$keyVaultName = "your-key-vault-name"
$location = "eastus"

# Create resource group (if not exists)
az group create --name $resourceGroup --location $location

# Create Key Vault
az keyvault create --resource-group $resourceGroup --name $keyVaultName --location $location
```

#### 2. Add Secrets to Key Vault

```bash
az keyvault secret set --vault-name $keyVaultName --name "ConnectionStrings--DefaultConnection" --value "Server=your-server;Database=userservicedb;..."
az keyvault secret set --vault-name $keyVaultName --name "JwtSettings--SecretKey" --value "your-secret-key"
az keyvault secret set --vault-name $keyVaultName --name "JwtSettings--Issuer" --value "UserService.API"
az keyvault secret set --vault-name $keyVaultName --name "JwtSettings--Audience" --value "UserService"
az keyvault secret set --vault-name $keyVaultName --name "Services--NotificationService" --value "https://notification-service.azurewebsites.net"
```

#### 3. Configure App Service to Access Key Vault

```bash
# Assign Managed Identity
az webapp identity assign --resource-group $resourceGroup --name "your-app-name"

# Get Principal ID
$principalId = az webapp identity show --resource-group $resourceGroup --name "your-app-name" --query principalId --output tsv

# Grant access
az keyvault set-policy --name $keyVaultName --object-id $principalId --secret-permissions get list
```

#### 4. Update Program.cs (Production Build)

See updated Program.cs with Serilog configuration pointing to Key Vault.

---

## Security Features Implemented

### ✅ Authorization Checks

- Profile update endpoint: Only users can update their own profile
- Address management endpoints: Only users can manage their own addresses
- `[Authorize]` attributes on protected endpoints

### ✅ Email Integration

- Confirmation emails sent via Notification Service
- Password reset emails sent securely
- Welcome emails on registration
- No tokens exposed in API responses

### ✅ Global Exception Handling

- Centralized exception middleware
- Correlation IDs for tracing
- Structured error responses

### ✅ Rate Limiting

- **Login**: 5 attempts per 15 minutes
- **Register**: 3 attempts per hour
- **General API**: 100 requests per minute

### ✅ Validation

- FluentValidation with comprehensive rules
- Password strength requirements
- Email format validation
- Phone number validation (E.164 format)
- URL validation for profile photos

### ✅ JWT Configuration

- Audience validation enabled (`ValidAudience = "UserService"`)
- Proper issuer validation
- Configurable expiration times

### ✅ Structured Logging with Serilog

- Console and file output
- Rolling log files (daily)
- Structured logging format
- CorrelationId tracking

### ✅ DDD Value Objects

- **Email**: Email validation and equality
- **Password**: Password strength validation (never exposed)
- **PhoneNumber**: E.164 format validation

### ✅ Comprehensive Testing

- Unit tests for UserService
- Validator tests with FluentValidation
- Value object tests
- Mocking with Moq
- FluentAssertions for readable assertions

---

## Running the Application

### Development

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run migrations
dotnet ef database update --project UserService.Infrastructure --startup-project UserService.API

# Run application
dotnet run --project UserService.API
```

### Run Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test class
dotnet test --filter "ClassName=UserServiceTests"
```

### View Logs

For file-based logs (Development):

```bash
tail -f UserService.API/logs/userservice-*.txt
```

---

## API Endpoints

### Authentication

- `POST /api/user/register` - Register (Rate limited: 3/hour)
- `POST /api/user/login` - Login (Rate limited: 5/15min)
- `POST /api/user/refresh-token` - Refresh JWT token
- `POST /api/user/revoke-token` - Revoke refresh token

### Email

- `POST /api/user/send-confirmation-email` - Send confirmation email
- `POST /api/user/verify-email` - Verify email token
- `POST /api/user/forgot-password` - Send password reset email
- `POST /api/user/reset-password` - Reset password with token

### Profile (Authorized)

- `GET /api/user/profile/{userId}` - Get user profile
- `PUT /api/user/profile` - Update profile (AuthorizedOnly)
- `POST /api/user/change-password` - Change password (AuthorizedOnly)

### Addresses (Authorized)

- `POST /api/user/addresses` - Add/Update address (AuthorizedOnly)
- `GET /api/user/{userId}/addresses` - Get all addresses
- `GET /api/user/{userId}/address/{addressId}` - Get specific address
- `POST /api/user/delete-address` - Delete address (AuthorizedOnly)

### Utilities

- `GET /api/user/{userId}/exists` - Check if user exists
- `GET /health` - Health check endpoint
- `GET /swagger` - Swagger UI documentation

---

## Environment Variables

### appsettings.json (Local/Public)

```json
{
  "Logging": { ... },
  "AppSettings": {
    "AppUrl": "http://localhost:3000"
  }
}
```

### User Secrets / Key Vault (Private)

- `ConnectionStrings:DefaultConnection`
- `JwtSettings:SecretKey`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`
- `Services:NotificationService`

---

## Important Security Notes

⚠️ **CRITICAL**: Never commit secrets to version control
⚠️ Keep JWT secret key long (32+ characters) and random
⚠️ Always use HTTPS in production
⚠️ Validate all inputs with FluentValidation
⚠️ Use correlation IDs for tracing across services
⚠️ Monitor rate limits and adjust based on usage patterns
⚠️ Regularly update NuGet packages for security patches

---

## Troubleshooting

### Tokens exposed in response

✅ Fixed - All authentication tokens now sent via email

### Authorization errors on profile updates

✅ Fixed - Added proper Authorize checks and user ID validation

### Missing validation errors

✅ Fixed - Added FluentValidation to all DTOs with comprehensive rules

### Rate limiting not working

✅ Verify middleware order in Program.cs

```csharp
app.UseRateLimiter();
```

### Email not sending

✅ Verify Notification Service is running
✅ Check configuration: `Services:NotificationService`
✅ Review logs for correlation ID

---

For more information, see the complete audit report in the repository.
