📘 Technical Documentation - English Vocabulary & Grammar Notes Website

🔐 Authentication Feature - Technical Specification

🎯 Goal
Design a secure and scalable authentication system for the English learning web application. This system must support user registration, login via email/username and password, permission-based access control (RBAC), and token-based authentication using OAuth2 with refresh/expire/revoke capabilities.

🧱 Database Schema Design (MySQL)

🔹 Users
- Id: INT UNSIGNED, PK, AUTO_INCREMENT
- Username: VARCHAR(100), UNIQUE, NOT NULL
- Email: VARCHAR(255), UNIQUE, NOT NULL
- PasswordHash: VARCHAR(255), NOT NULL
- FullName: VARCHAR(255), NULLABLE
- CreatedAt: DATETIME, DEFAULT CURRENT_TIMESTAMP
- UpdatedAt: DATETIME, ON UPDATE CURRENT_TIMESTAMP

🔹 Permissions
- Id: INT UNSIGNED, PK, AUTO_INCREMENT
- Name: VARCHAR(100), UNIQUE, NOT NULL (e.g., vocabulary:create)
- Description: TEXT, NULLABLE

🔹 Groups
- Id: INT UNSIGNED, PK, AUTO_INCREMENT
- Name: VARCHAR(100), UNIQUE, NOT NULL
- Description: TEXT, NULLABLE

🔹 GroupPermissions (many-to-many)
- GroupId: INT UNSIGNED, FK -> Groups(Id), PK
- PermissionId: INT UNSIGNED, FK -> Permissions(Id), PK

🔹 UserGroups (many-to-many)
- UserId: INT UNSIGNED, FK -> Users(Id), PK
- GroupId: INT UNSIGNED, FK -> Groups(Id), PK

🔹 UserPermissions (custom overrides)
- UserId: INT UNSIGNED, FK -> Users(Id), PK
- PermissionId: INT UNSIGNED, FK -> Permissions(Id), PK

🔹 RefreshTokens
- Id: INT UNSIGNED, PK, AUTO_INCREMENT
- UserId: INT UNSIGNED, FK -> Users(Id), NOT NULL
- Token: TEXT, NOT NULL
- ExpiryDate: DATETIME, NOT NULL
- IsRevoked: BOOLEAN, DEFAULT FALSE
- CreatedAt: DATETIME, DEFAULT CURRENT_TIMESTAMP
- CreatedByIp: VARCHAR(50), NULLABLE

🔐 Authentication Flow (OAuth2 - Bearer Token)

🔹 Registration
- User submits username, email, password (and optionally, full name)
- System hashes the password (e.g., BCrypt) and saves user info

🔹 Login
- Accepts username or email with password
- Validates credentials
- Generates JWT Access Token and Refresh Token

🔹 Token Management
- JWT Access Token contains claims (userId, permissions, group info)
- Short-lived Access Token (e.g., 15 min)
- Long-lived Refresh Token stored in RefreshTokens table
- On refresh request:
  - Validate refresh token
  - Check for revocation/expiry
  - Issue new tokens

🔹 Permission Resolution Logic
1. On login, resolve all permissions for the user:
   - Collect permissions from user's assigned groups
   - Include user-specific claims in UserPermissions
2. Embed the resolved claims into JWT
3. On each request, validate token and claims for route access

🔹 Force Logout (Token Revocation)
- Admin/user action sets IsRevoked = true on active refresh tokens
- Middleware checks this flag during token refresh or access attempts

📌 Notes
- Designed for scalability and clarity
- Easily extendable for additional domains (e.g., Vocabulary, Grammar)
- Compatible with .NET 9 + EF Core (Code-First) + Pomelo MySQL Provider