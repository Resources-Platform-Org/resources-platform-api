# Resources Platform API

A robust RESTful API built with **.NET 9** for managing academic resources. This platform serves as a central hub connecting students, professors, and universities, facilitating the sharing and management of educational materials.

## 🚀 Technologies Used

- **Framework:** [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Architecture:** Clean Architecture (Onion Architecture)
- **Database:** SQL Server
- **ORM:** Entity Framework Core 9.0
- **Authentication:** JWT (JSON Web Tokens)
- **Mapping:** AutoMapper
- **Documentation:** Swagger / OpenAPI
- **Password Hashing:** BCrypt.Net

## 🏛 Project Architecture

The solution follows a Clean Architecture approach separating concerns into three distinct projects:

1.  **Core (Domain Layer)**
    -   Contains domain **Entities** (User, Resource, Course, etc.)
    -   Defines **Interfaces** (Repository contracts, Service contracts)
    -   Includes **Enums** and **Settings** objects
    -   *Dependencies: None*

2.  **Infrastructure (Data & Service Layer)**
    -   Implements **Data Access** using Entity Framework Core (`ApplicationDbContext`)
    -   Implements **Repositories** (`GenericRepository`, `UserRepository`, etc.)
    -   Implements **Services** (AuthService, FileService, TokenService)
    -   Manage **Migrations**
    -   *Dependencies: Core*

3.  **Api (Presentation Layer)**
    -   **Controllers** handling HTTP requests
    -   **DTOs** (Data Transfer Objects) for request/response payloads
    -   **Mapping Profiles** for AutoMapper
    -   **Middleware** configuration (Auth, Swagger, Error Handling)
    -   *Dependencies: Core, Infrastructure*

## ✨ Key Features

-   **Authentication & Authorization**: Secure User Sign-up/Sign-in with JWT Bearer tokens.
-   **User Management**: Manage student and professor profiles.
-   **Resource Management**: Upload, categorize, and retrieve educational resources.
-   **Academic Structure**:
    -   **Universities & Majors**: Organize hierarchies of study.
    -   **Courses**: Manage courses linked to majors.
    -   **Professors**: Associate professors with courses and universities.
-   **File Handling**: Dedicated service for managing file uploads (local storage configured).

## 🛠 Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB)
-   Visual Studio 2022 / VS Code

### Installation

1.  **Clone the repository**
    ```bash
    git clone <repository-url>
    cd resources-platform-api/api
    ```

2.  **Restore dependencies**
    ```bash
    dotnet restore
    ```

### Configuration

1.  Open `Api/appsettings.json`.
2.  Update the **ConnectionStrings** section if your SQL Server instance differs from local default.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=ResourceManagmentSystem;Trusted_Connection=True;TrustServerCertificate=true;"
    }
    ```
3.  Review **JWT** settings (Ensure `Key`, `Issuer`, and `Audience` match your environment needs).

### Database Setup

Apply the Entity Framework Core migrations to create the database schema.

Open a terminal in the root solution folder (or `Infrastructure` folder context) and run:

```bash
# Using .NET Core CLI
dotnet ef database update --project Infrastructure --startup-project Api
```

### Running the Application

Run the API project:

```bash
cd Api
dotnet run
```

The API will start (usually on `http://localhost:5xxx`).

## 📚 API Documentation

The project includes Swagger UI for interactive API documentation and testing.

1.  Run the application.
2.  Navigate to `/swagger/index.html` in your browser (e.g., `http://localhost:5038/swagger/index.html`).
3.  Use the **Authorize** button to input your JWT token (Format: `Bearer <your_token>`) to access protected endpoints.

## 📂 Folder Structure

```
Api.sln
├── Api/                 # Presentation Layer
│   ├── Controllers/     # API Endpoints
│   ├── Dtos/            # Data Transfer Objects
│   ├── Mapping/         # Automapper Profiles
│   └── Program.cs       # App Entry Point & Config
│
├── Core/                # Domain Layer
│   ├── Entities/        # Database Models
│   ├── Interfaces/      # Abstractions
│   ├── Enums/           # Global Enums
│   └── Settings/        # Config Objects
│
└── Infrastructure/      # Implementation Layer
    ├── Data/            # DB Context & Migrations
    ├── Repository/      # Repository Implementations
    └── Services/        # Logic Implementations
```
