# API Test Plan & Security Audit

## Overview
This document outlines the test plan for the Resources Platform API. It covers authentication, role-based access control, and CRUD operations for all resources.

## Security Findings
**CRITICAL**: `MajorsController` (`api/Majors`) appears to be missing the `[Authorize]` attribute on the class level. While some methods might be intended to be public, `Create`, `Update`, and `Delete` should likely be restricted to Admins. Currently, they might be accessible anonymously or by any authenticated user depending on global filters (if any).

## Test Scenarios

### 1. Authentication (`/Auth`)
- **Login (Admin)**: Verify valid admin credentials return a JWT token with `Role: Admin`.
- **Login (User)**: Verify valid user credentials return a JWT token with `Role: User`.
- **Invalid Login**: Verify 401 Unauthorized for wrong password or non-existent user.

### 2. File Management (`/api/Files`)
- **List Files**: Public access. Verify pagination and filtering.
- **Upload File**: Admin only. Verify file size limits, type validation, and metadata creation.
- **Download File**: Authenticated users. Verify file content delivery.
- **Delete File**: Admin only. Verify physical file deletion and DB record removal.

### 3. User Management (`/api/Users`)
- **Register**: Public access. Verify unique username/email checks.
- **Get Me**: Authenticated. Verify user sees their own data.
- **Admin Operations**: Verify only Admins can List, GetById, Update, and Delete other users.

### 4. Admin Resources (Reference Data)
All the following controllers should be restricted to **Admin** role:
- `AcademicLevelsController` (`api/academic-levels`)
- `CoursesController` (`api/Courses`)
- `DocumentTypesController` (`api/document-types`)
- `ProfessorsController` (`api/Professors`)
- `SemestersController` (`api/Semesters`)
- `UniversitiesController` (`api/Universities`)

**Tests for each:**
- **GET**: Verify list retrieval.
- **POST**: Verify creation (check for duplicates where applicable).
- **PUT**: Verify update.
- **DELETE**: Verify deletion (check for foreign key constraints).

### 5. Majors (`/api/Majors`)
- **Security Check**: Verify if non-admins can Create/Update/Delete. **(Likely Vulnerable)**
- **Functionality**: Verify linking Majors to Universities.

## Automated Testing Artifacts
- **`requests.http`**: VS Code REST Client file for quick manual testing.
- **`IntegrationTests.cs`**: xUnit test class template for automated integration testing.
- **`postman_collection.json`**: Importable Postman collection.

## Recommendations
1.  **Fix Security**: Add `[Authorize(Roles = "Admin")]` to `MajorsController`.
2.  **Validation**: Ensure all DTOs have `[Required]` attributes and proper string length limits.
3.  **Error Handling**: Standardize error responses (currently mixed between string messages and objects).
