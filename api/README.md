# 📚 Resources Platform API

This is the backend API documentation. For the main project documentation, see the [root README](../README.md).

## 🚀 API Overview

The Resources Platform API is a RESTful service built with ASP.NET Core 9.0, designed to manage educational resources, users, and university structures.

### Base URL
```
https://localhost:7041
```

### Authentication
The API uses **JWT (JSON Web Token)** for authentication.
1. obtain a token via `/api/auth/login`
2. Include the token in the `Authorization` header:
   ```http
   Authorization: Bearer <your_token>
   ```

## 📚 API Endpoints

This API fully supports **Swagger UI**.
👉 **[View Interactive API Documentation](https://localhost:7041/swagger/index.html)**

### 🔐 Authentication (`/api/auth`)

| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| `POST` | `/register` | Register a new user | ❌ |
| `POST` | `/login` | Login and get JWT token | ❌ |

### 👤 Users (`/api/users`)

| Method | Endpoint | Description | Auth | Role |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/me` | Get current user profile | ✅ | Any |
| `POST` | `/change-password` | Change password | ✅ | Any |
| `POST` | `/image` | Upload profile picture | ✅ | Any |
| `DELETE` | `/image` | Delete profile picture | ✅ | Any |
| `GET` | `/paged` | Get all users (paginated) | ✅ | Admin |
| `PUT` | `/{id}/role` | Change user role | ✅ | Admin |

### 📚 Resources (`/api/resources`)

| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| `GET` | `/paged` | Get resources (filter by course) | ❌ |
| `POST` | `/create` | Upload a new resource | ✅ |
| `GET` | `/download/{id}` | Download resource file | ❌ |
| `DELETE` | `/{id}` | Delete a resource | ✅ |

### 🏛 Universities & Structure (`/api/admin`)

*Requires Admin Role*

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/universities` | List all universities |
| `POST` | `/universities` | Create university |
| `PUT` | `/universities/{id}` | Update university |
| `DELETE` | `/universities/{id}` | Delete university |
| `GET` | `/majors` | List all majors |
| `POST` | `/majors` | Create major |
| `GET` | `/courses` | List all courses |
| `POST` | `/courses` | Create course |

---

## 🛠 Development Setup

### 1. Prerequisites
- .NET 9.0 SDK
- SQL Server

### 2. Configuration (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ResourceManagmentSystem;..."
  },
  "JWT": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "...",
    "Audience": "..."
  }
}
```

### 3. Run Migrations
```bash
dotnet ef database update --project ../Infrastructure --startup-project Api
```

### 4. Run API
```bash
cd Api
dotnet run
```
