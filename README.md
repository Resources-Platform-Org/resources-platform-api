# � Resources Platform API

A comprehensive ASP.NET Core Web API for managing educational resources, built with Clean Architecture principles. This system provides robust user management, university/college structure, file management, and secure authentication for an educational resource sharing platform.

---

## 📋 Table of Contents

- [Project Overview](#-project-overview)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Database Design](#-database-design)
- [Key Features](#-key-features)
- [API Endpoints](#-api-endpoints)
- [Setup & Installation](#-setup--installation)
- [Configuration](#-configuration)
- [Contributing](#-contributing)

---

## 🎯 Project Overview

The **Resources Platform API** is a backend system designed for educational institutions to manage and share academic resources. It enables:

- **User Management**: Registration, authentication, profile management with role-based access control (Admin/User)
- **Educational Structure**: Multi-level hierarchy supporting Universities → Majors → Courses → Resources
- **Resource Management**: Upload, download, and manage educational materials (PDFs, documents, presentations)
- **File Handling**: Secure file upload/download with automatic cleanup and validation
- **Authentication & Authorization**: JWT-based authentication with BCrypt password hashing

---

## 🛠️ Tech Stack

### Core Technologies
- **.NET 9.0** - Latest ASP.NET Core framework
- **Entity Framework Core 9.0** - ORM with Code-First approach
- **SQL Server** - Relational database

### Architecture & Patterns
- **Clean Architecture** - Separation of concerns (Core, Infrastructure, API layers)
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **DTOs Pattern** - Data transfer objects with AutoMapper

### Security & Authentication
- **JWT (JSON Web Tokens)** - Stateless authentication
- **BCrypt.Net** - Password hashing (v4.0.3)
- **ASP.NET Core Identity** - Role-based authorization

### Libraries & Tools
- **AutoMapper 12.0.1** - Object-to-object mapping
- **Swashbuckle (Swagger) 6.5.0** - API documentation
- **Microsoft.AspNetCore.Authentication.JwtBearer 9.0.0** - JWT authentication middleware

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── api/
│   ├── Api/                      # Presentation Layer
│   │   ├── Controllers/          # API Controllers (Auth, Users, Resources)
│   │   ├── Dtos/                # Data Transfer Objects
│   │   ├── Mapping/             # AutoMapper profiles
│   │   ├── Wrappers/            # Response wrappers
│   │   └── Program.cs           # Application entry point
│   │
│   ├── Core/                     # Domain Layer (Business Logic)
│   │   ├── Entities/            # Domain entities (User, Resource, Course, etc.)
│   │   ├── Enums/               # Enumerations (Roles, Extensions, Levels)
│   │   ├── Interfaces/          # Abstraction contracts
│   │   └── Setting/             # Configuration models
│   │
│   └── Infrastructure/           # Data Access Layer
│       ├── Data/                # DbContext & configurations
│       ├── Migrations/          # EF Core migrations
│       ├── Repository/          # Repository implementations
│       └── Services/            # Business services (Auth, File, Token)
```

### Layer Responsibilities

#### 🎨 **API Layer** (Presentation)
- **Controllers**: Handle HTTP requests/responses
- **DTOs**: Define request/response contracts
- **Middleware**: JWT authentication, error handling
- **Dependency Injection**: Service registration

#### 💼 **Core Layer** (Domain)
- **Entities**: Business models (User, Resource, Course, University, etc.)
- **Interfaces**: Contracts for repositories and services
- **Enums**: Domain-specific enumerations
- **Business Rules**: Domain logic and validations

#### 🗄️ **Infrastructure Layer** (Data Access)
- **DbContext**: Entity Framework database context
- **Repositories**: Data access implementations
- **Services**: Authentication, file management, token generation
- **Configurations**: Entity mappings and relationships

---

## 🗃️ Database Design

![Database Design](DB%20Design.png)

### Entity Relationships

#### **Core Entities**

1. **User**
   - Primary authentication entity
   - Properties: Id, Name, Email, PasswordHash, ProfilePicture, Role
   - Relationships: One-to-Many with Resources (uploaded resources)

2. **University**
   - Top-level educational institution
   - Properties: Id, Name
   - Relationships: One-to-Many with Majors

3. **Major**
   - Academic department/program
   - Properties: Id, Name, UniversityId (FK)
   - Relationships: 
     - Many-to-One with University
     - Many-to-Many with Courses (via CourseMajor)

4. **Course**
   - Individual academic course
   - Properties: Id, Name, Code
   - Relationships: 
     - Many-to-Many with Majors (via CourseMajor)
     - One-to-Many with Resources
     - One-to-Many with Professors

5. **Resource**
   - Educational file/document
   - Properties: Id, Name, Path, StoredFileName, Extension, DownloadsCount, IsApproved, DocumentTypeId (FK), CourseId (FK), UploaderId (FK)
   - Relationships:
     - Many-to-One with Course
     - Many-to-One with DocumentType
     - Many-to-One with User (uploader)

6. **DocumentType**
   - Type of educational resource (Lecture, Assignment, etc.)
   - Properties: Id, Name
   - Relationships: One-to-Many with Resources

7. **Professor**
   - Teaching staff information
   - Properties: Id, Name, Email, CourseId (FK)
   - Relationships: Many-to-One with Course

8. **CourseMajor** (Join Table)
   - Many-to-Many relationship between Courses and Majors
   - Properties: CourseId (FK), MajorId (FK)

### Key Database Features
- **Foreign Key Constraints**: Enforce referential integrity
- **Code-First Migrations**: Version-controlled schema changes
- **Cascade Behaviors**: Configured for data consistency
- **Indexing**: Optimized for common queries

---

## ✨ Key Features

### 🔐 **Secure Authentication & Authorization**
- **User Registration**: Email-based registration with automatic password hashing (BCrypt)
- **JWT Authentication**: Stateless token-based authentication
- **Role-Based Access**: Admin and User roles with attribute-based authorization
- **Password Management**: Secure password change functionality

### 👤 **User Management**
- **Profile Management**: View and update user profiles
- **Image Upload**: Profile picture upload with validation (JPG, JPEG, PNG)
  - Max file size: 2MB
  - Automatic file cleanup on replacement
- **User Listing**: Paginated user listing (Admin only)
- **Role Management**: Change user roles (Admin only)

### 🎓 **Educational Structure**
- **Multi-Level Hierarchy**: University → Major → Course structure
- **Course Management**: Create and manage courses with unique codes
- **Professor Assignments**: Link professors to courses

### 📂 **Resource Management**
- **File Upload**: Secure file upload with validation
  - Supported formats: PDF, DOCX, JPG, JPEG, PNG
  - Max file size: 5MB (configurable)
  - Automatic file naming and storage
- **File Download**: Tracked downloads with business logic
- **Resource Approval**: Admin approval workflow for resources
- **Pagination & Filtering**: Efficient resource browsing by course

### 🛡️ **File Handling Service**
- Centralized file management service
- Automatic file cleanup on deletion/replacement
- Configurable upload directories
- File validation (size, extensions)
- Full URL generation for uploaded files

---

## 🌐 API Endpoints

### 🔑 **Authentication** (`/api/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/register` | Register new user | ❌ |
| POST | `/login` | Authenticate user and get JWT | ❌ |

**Example Request (Register)**
```json
POST /api/auth/register
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Example Response**
```json
{
  "success": true,
  "message": "User registered successfully",
  "data": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "role": "User"
  }
}
```

---

### 👥 **Users** (`/api/users`)

| Method | Endpoint | Description | Auth Required | Role |
|--------|----------|-------------|---------------|------|
| GET | `/me` | Get current user profile | ✅ | Any |
| POST | `/change-password` | Change user password | ✅ | Any |
| POST | `/image` | Upload profile picture | ✅ | Any |
| DELETE | `/image` | Delete profile picture | ✅ | Any |
| GET | `/paged` | Get paginated users | ✅ | Admin |
| PUT | `/{id}/role` | Change user role | ✅ | Admin |

**Example Request (Upload Image)**
```http
POST /api/users/image
Content-Type: multipart/form-data
Authorization: Bearer {token}

file: [binary data]
```

---

### 📚 **Resources** (`/api/resources`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/paged` | Get paginated resources (filter by courseId) | ❌ |
| POST | `/create` | Upload new resource | ✅ |
| GET | `/download/{id}` | Download resource file | ❌ |
| DELETE | `/{id}` | Delete resource | ✅ |

**Example Request (Create Resource)**
```json
POST /api/resources/create
Authorization: Bearer {token}
{
  "name": "Introduction to C# Programming",
  "documentTypeId": 1,
  "courseId": 5,
  "uploaderId": 2,
  "file": [multipart file data]
}
```

**Example Response (Paginated Resources)**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Lecture 1 - Introduction",
      "extension": "PDF",
      "downloadsCount": 45,
      "downloadUrl": "https://localhost:5000/api/resources/download/1",
      "course": "Data Structures",
      "documentType": "Lecture",
      "uploader": "Dr. Smith"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalRecords": 48
}
```

---

### 🏛️ **Admin Controllers** (`/api/admin`)

Additional controllers for managing:
- Universities
- Majors
- Courses
- Document Types
- Professors

*All admin endpoints require `Admin` role authorization.*

---

## 🚀 Setup & Installation

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) (Local or Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- Git

### Step 1: Clone the Repository
```bash
git clone https://github.com/yourusername/resources-platform-api.git
cd resources-platform-api/api
```

### Step 2: Configure Database Connection

Edit `appsettings.json` in [api/Api/appsettings.json](api/Api/appsettings.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ResourceManagmentSystem;Trusted_Connection=True;TrustServerCertificate=true;"
  }
}
```

**Connection String Options:**
- **Local SQL Server**: `Server=.;Database=ResourceManagmentSystem;Trusted_Connection=True;TrustServerCertificate=true;`
- **SQL Server Express**: `Server=.\\SQLEXPRESS;Database=ResourceManagmentSystem;Trusted_Connection=True;TrustServerCertificate=true;`
- **SQL Server with credentials**: `Server=YOUR_SERVER;Database=ResourceManagmentSystem;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true;`

### Step 3: Configure JWT Settings

Update JWT configuration in [api/Api/appsettings.json](api/Api/appsettings.json):

```json
{
  "JWT": {
    "Key": "YOUR_SUPER_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG",
    "Issuer": "ResourcePlatformIssuer",
    "Audience": "ResourcePlatformAudience",
    "ExpiresInHours": 4
  }
}
```

⚠️ **Security Note**: Use a strong, randomly generated key in production. Generate one using:
```bash
openssl rand -base64 32
```

### Step 4: Configure File Upload Settings

```json
{
  "FileSetting": {
    "UserImageFolder": "uploads/users",
    "MaxFileSizeMB": 5,
    "AllowedExtentions": [".jpg", ".jpeg", ".png", ".pdf", ".docx"],
    "UploadFolder": "Uploads"
  }
}
```

### Step 5: Restore Dependencies

```bash
cd api
dotnet restore
```

### Step 6: Apply Database Migrations

```bash
# Navigate to the API project
cd Api

# Apply migrations to create database
dotnet ef database update

# If the above fails, use this command
dotnet ef database update --project ../Infrastructure/Infrastructure.csproj --startup-project .
```

This will create the database and all tables based on the schema shown in the [Database Design](#-database-design) diagram.

### Step 7: Run the Application

```bash
# Run the API
dotnet run

# Or use watch mode for development
dotnet watch run
```

The API will start at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

### Step 8: Access Swagger Documentation

Navigate to:
```
https://localhost:5001/swagger
```

Swagger UI provides interactive API documentation where you can test all endpoints.

---

## ⚙️ Configuration

### Environment-Specific Settings

- **Development**: [appsettings.Development.json](api/Api/appsettings.Development.json)
- **Production**: [appsettings.json](api/Api/appsettings.json)

### JWT Token Usage

After logging in, include the JWT token in subsequent requests:

```http
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

### File Upload Directories

Ensure the application has write permissions to:
- `api/Api/Resources/Uploads/` - General file uploads
- `api/Api/Resources/uploads/users/` - User profile pictures

These directories are created automatically on first upload.

---

## 📝 Contributing

### Branching Strategy
- **main** → Production-ready stable code
- **dev** → Main development branch (no direct pushes)
- **feature/** → Feature branches

### Commit Convention
```
feat(scope): add new feature
fix(scope): fix bug
refactor(scope): code improvement
docs(scope): documentation update
chore(scope): maintenance tasks
```

### Pull Request Process
1. Create feature branch from `dev`
2. Make changes and commit with conventional messages
3. Open Pull Request to `dev`
4. Await code review and approval
5. Merge after approval

---

## 📄 License

This project is licensed under the MIT License.

---

## 👨‍💻 developed by team

For questions or support, please open an issue or contact the development team.

---

**Built with ❤️ using ASP.NET Core Clean Architecture**
