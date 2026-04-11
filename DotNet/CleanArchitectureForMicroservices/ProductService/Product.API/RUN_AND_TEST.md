# ProductService - Run & Test Guide

## 🚀 The Fixes Applied

### ✅ Fix #1: Enable Browser Launch
```json
// BEFORE
"launchBrowser": false,

// AFTER  
"launchBrowser": true,
"launchUrl": "swagger"
```

### ✅ Fix #2: Fix HTTPS in Development
```csharp
// BEFORE
app.UseHttpsRedirection();  // ❌ Always active

// AFTER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
else
{
    app.UseHttpsRedirection();  // ✅ Only in Production
}
```

---

## 🎯 How to Run Product.API

### **Method 1: Visual Studio (Easiest)**

```
1. Open Solution
2. Right-click "Product.API" project
3. Click "Set as Startup Project"
4. Press F5 (or Click ▶️ Run)
5. ✅ Browser auto-opens to Swagger!
```

### **Method 2: Command Line**

```bash
cd Product.API
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5082
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to quit.
```

**Then Open:** `http://localhost:5082/swagger`

### **Method 3: VS Code**

```bash
code Product.API
dotnet run
```

---

## 🌐 Access Points

Once running, use these URLs:

| URL | Purpose |
|-----|---------|
| `http://localhost:5082` | API Root |
| `http://localhost:5082/swagger` | Swagger UI (Interactive Docs) |
| `http://localhost:5082/api/products` | Get All Products (API call) |

---

## 🧪 Test an Endpoint

### Using Swagger UI (Easiest)

1. Open `http://localhost:5082/swagger`
2. Click "GET /api/products"
3. Click "Try it out"
4. Click "Execute"
5. ✅ See response!

### Using cURL

```bash
curl -X GET "http://localhost:5082/api/products" \
  -H "accept: application/json"
```

### Using PowerShell

```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5082/api/products" -Method Get
$response | ConvertTo-Json
```

---

## ❓ Troubleshooting

### **Browser didn't open?**
✅ Manually navigate to `http://localhost:5082`

### **Port already in use?**
Change in `Properties/launchSettings.json`:
```json
"applicationUrl": "http://localhost:5083"  // Change 5082 to 5083
```

### **Database connection error?**
Update `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER;Database=ProductDb;..."
}
```

Then run migrations:
```bash
dotnet ef database update -p Product.Infrastructure -s Product.API
```

### **HTTPS certificate error?**
```bash
dotnet dev-certs https --trust
```

---

## 📋 Checklist

- [ ] Run `dotnet run` in Product.API folder
- [ ] See "Now listening on" in console
- [ ] Browser opens to Swagger
- [ ] Can see Swagger UI with endpoints
- [ ] Try GET /api/products endpoint
- [ ] See JSON response with success
- [ ] ✅ All working!

---

## 📊 Swagger UI Guide

### What You'll See
```
Product Service API v1
├── GET /api/products          (Get all products)
├── GET /api/products/{id}     (Get single product)
├── POST /api/products         (Create product - needs Auth)
├── PUT /api/products/{id}     (Update product - needs Auth)
└── DELETE /api/products/{id}  (Delete product - needs Auth)
```

### Testing Protected Endpoints

1. Get JWT Token from UserService:
   ```bash
   curl -X POST "http://localhost:5000/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{"email":"admin@example.com","password":"password123"}'
   ```

2. Copy the `token` from response

3. In Swagger, click "Authorize" button (🔒)

4. Paste: `Bearer YOUR_TOKEN_HERE`

5. Now try protected endpoints (POST, PUT, DELETE)

---

## 🔄 Startup Flow

```
1. You run: dotnet run
                 │
2. Program.cs executes
                 │
3. Services registered (DB, JWT, etc.)
                 │
4. launchSettings.json loaded
                 │
5. App starts listening on port 5082
                 │
6. launchBrowser: true
                 │
7. Browser opens to launchUrl: "swagger"
                 │
8. Swagger UI loads at http://localhost:5082/swagger
                 │
9. ✅ Ready to test!
```

---

## 💻 Console Messages Explained

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5082
      ✅ App is running and listening

info: Microsoft.Hosting.Lifetime[14]  
      Now listening on: https://localhost:7082
      ✅ HTTPS also available

info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to quit.
      ✅ App fully started and ready!
```

---

## 🛑 Stopping the App

Press in the terminal:
```
Ctrl+C
```

You'll see:
```
info: Microsoft.Hosting.Lifetime[0]
      Application is shutting down...
```

---

## 📝 Next Steps After Running

1. **Test GET /api/products**
   - Should return empty list (no products yet)

2. **Get JWT Token**
   - Login to UserService to get token
   - Use token for POST/PUT/DELETE operations

3. **Create a Product**
   - POST /api/products with Admin token
   - Add a product with name, price, etc.

4. **Test CRUD Operations**
   - GET all → GET one → POST → PUT → DELETE

5. **Review Documentation**
   - See Product.API/README.md
   - See Product.API/API_QUICK_REFERENCE.md

---

**You're all set! Run the app and start testing! 🚀**

