# ProductService - Localhost Not Opening Troubleshooting Guide

## ✅ Issues Fixed

Your Product.API had **two configuration issues** that prevented localhost from opening:

### Issue 1: Browser Launch Disabled
**Problem**: `"launchBrowser": false` in `launchSettings.json`  
**Solution**: Changed to `"launchBrowser": true` and added `"launchUrl": "swagger"`  
**Result**: Now automatically opens browser when you run the app

### Issue 2: HTTPS Redirection in Development
**Problem**: `app.UseHttpsRedirection()` was always active  
**Solution**: Wrapped it in `else` block so it only runs in Production  
**Result**: No HTTPS certificate errors in development

---

## 🚀 How to Run Now

### Option 1: Using Visual Studio
1. **Right-click** Product.API project → **Set as Startup Project**
2. **Press F5** or click **▶️ Run**
3. **Browser automatically opens** to `http://localhost:5082/swagger`

### Option 2: Using Command Line
```bash
cd Product.API
dotnet run
```

**Output should show:**
```
Now listening on: http://localhost:5082
Now listening on: https://localhost:7082
Application started. Press Ctrl+C to quit.
```

Then **manually open** in browser:
- HTTP: `http://localhost:5082`
- Swagger: `http://localhost:5082/swagger`

---

## 📋 What Changed

### 1. launchSettings.json
```json
{
  "profiles": {
    "http": {
      "launchBrowser": true,           // ✅ Changed from false
      "launchUrl": "swagger",           // ✅ Added
      "applicationUrl": "http://localhost:5082",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {                          // ✅ Added HTTPS profile
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7082;http://localhost:5082",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 2. Program.cs
```csharp
// Before: HTTPS always enabled
app.UseHttpsRedirection();

// After: HTTPS only in Production
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // ...
}
else
{
    app.UseHttpsRedirection();
}
```

---

## ✅ Ports Configuration

| Service | HTTP Port | HTTPS Port |
|---------|-----------|------------|
| Product.API | 5082 | 7082 |
| User.API | 5000 | 7000 |
| Others | 5083+ | 7083+ |

If ports are already in use, you can change them in `launchSettings.json`:
```json
"applicationUrl": "http://localhost:YOUR_PORT"
```

---

## 🧪 Verification Steps

After running `dotnet run`:

1. **Check Console Output**
   ```
   ✅ Should see "Now listening on: http://localhost:5082"
   ✅ Should see "Application started"
   ```

2. **Check Browser**
   ```
   ✅ Browser should auto-open
   ✅ Should show Swagger UI at http://localhost:5082/swagger
   ```

3. **Test Swagger**
   - Click "Try it out" on any endpoint
   - Swagger should respond (try GET /api/products)

---

## ❌ If Still Not Opening

### Problem: Port already in use
**Solution:**
```bash
# Find process using port 5082
netstat -ano | findstr :5082

# Kill the process (get PID from above)
taskkill /PID [PID] /F

# Or use different port in launchSettings.json
```

### Problem: HTTPS certificate error
**Solution:**
- Trust the development certificate:
```bash
dotnet dev-certs https --trust
```

### Problem: Database connection fails
**Fix in appsettings.json:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER;Database=ProductDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
}
```

Then apply migrations:
```bash
dotnet ef database update -p Product.Infrastructure -s Product.API
```

### Problem: Dependencies not installed
**Solution:**
```bash
dotnet restore Product.API/Product.API.csproj
dotnet run
```

---

## 📊 Launch Profiles Explained

Your updated `launchSettings.json` now has two profiles:

### HTTP Profile (Default)
- **Port**: 5082
- **Protocol**: HTTP (no SSL/TLS)
- **Best for**: Development without certificate hassles
- **Use**: When you don't need HTTPS

### HTTPS Profile
- **Port**: 7082 (for HTTPS)
- **Also has**: 5082 (for HTTP)
- **Protocol**: HTTPS with self-signed certificate
- **Best for**: Testing security features
- **Use**: When testing JWT authentication

---

## 🎯 Quick Start (5 Steps)

1. **Open terminal** in Product.API folder
2. **Run**: `dotnet run`
3. **Wait** for "Now listening on" message
4. **Browser** auto-opens (if not, go to `http://localhost:5082`)
5. **Use Swagger** to test endpoints

---

## 💡 Pro Tips

### Tip 1: Hot Reload
Press `Ctrl+M` in the running terminal to toggle hot reload (auto-restart on code changes)

### Tip 2: Change Default Profile
Edit `launchSettings.json` `"commandName"` to switch default:
- `"Project"` = Use launchSettings
- `"IISExpress"` = Use IIS if installed

### Tip 3: Environment Variables
Add to launchSettings for override:
```json
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "CUSTOM_VAR": "value"
}
```

### Tip 4: Multiple Instances
Run multiple profiles:
```bash
# Terminal 1: HTTP
dotnet run --launch-profile http

# Terminal 2: HTTPS
dotnet run --launch-profile https
```

---

## 🔍 Common Ports

If 5082/7082 are occupied, use these alternatives:
- 5001/7001 (UserService uses 5000/7000)
- 5003/7003
- 5004/7004
- 5005/7005

**Update in launchSettings.json:**
```json
"applicationUrl": "http://localhost:5003"
```

---

## ✨ Summary of Fixes

| Issue | Before | After | Status |
|-------|--------|-------|--------|
| Browser Launch | `false` | `true` | ✅ Fixed |
| Launch URL | None | `swagger` | ✅ Fixed |
| HTTPS Redirect | Always on | Dev only | ✅ Fixed |
| HTTPS Profile | Missing | Added | ✅ Fixed |

---

**Your Product.API should now open automatically when you run it!** 🎉

If you still experience issues, check the console output for error messages - they'll tell you exactly what's wrong.

