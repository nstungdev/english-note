# GitHub Copilot Coding Guidelines for C# Projects

## Overview

This document defines strict and professional coding guidelines for GitHub Copilot to follow when generating C# code. These instructions ensure consistency, maintainability, and adherence to best practices for enterprise-level software development.

## 1. Domain Structure and Organization

- Each business domain should be isolated in its own dedicated folder.
  - Examples:
    - `UserDomain/`
    - `AuthDomain/`
    - `VocabularyDomain/`
- Inside each domain folder, include the following standard subfolders:
  - `Models/` – Contains data models specific to the domain.
  - `DTOs/` – Contains Data Transfer Objects used for communication.
  - `Services/` – Business logic and service classes for the domain.
  - `Controllers/` – ASP.NET Controllers for API endpoints.
  - Additional subfolders can be created as necessary (e.g., `Mappers/`, `Validators/`, etc.).

## 2. Common Utilities and Shared Resources

- Shared classes, constants, values, services, or utilities used across multiple domains should be placed in a top-level `Common/` folder.
  - Examples:
    - `Common/Constants/`
    - `Common/Helpers/`
    - `Common/Services/`
    - `Common/Models/ApiResponse.cs`

## 3. Code Formatting and Limits

- Each function should be **concise and focused**, with a **maximum length of 100 lines**.
- Each line of code must not exceed **100 characters** in length.
  - Use line breaks and proper indentation when necessary.
  - This improves readability and helps with version control diffs.

## 4. Comments and Documentation

- Only add comments when absolutely necessary to clarify non-obvious logic.
- Avoid redundant or excessive comments that restate what the code already expresses clearly.
- Prefer **self-explanatory names** for variables, methods, and classes.
- Use XML documentation comments (`///`) for public methods and classes where appropriate.

## 5. Standardized API Response Format

- All services should return a standardized response type using the `ApiResponse` model.
  - Successful Response:
    ```csharp
    return new ApiResponse<T>(
        message: "Your message here",
        data: yourDataObject
    );
    ```
  - Error Response:
    ```csharp
    return new ApiResponse<T>(
        message: "Error message here",
        errorCode: "ERROR_CODE"
    );
    ```
- Always specify parameter names explicitly for optional or default parameters in the constructor (e.g., `message:`, `data:`, `errorCode:`).

## 6. SOLID Principles

GitHub Copilot must generate code that adheres to the **SOLID principles**:

- **S - Single Responsibility Principle**: Each class and method should have one clear purpose.
- **O - Open/Closed Principle**: Classes should be open for extension but closed for modification.
- **L - Liskov Substitution Principle**: Derived types must be substitutable for their base types.
- **I - Interface Segregation Principle**: Prefer small, role-specific interfaces over large, general ones.
- **D - Dependency Inversion Principle**: Depend on abstractions, not on concrete implementations. Use dependency injection where applicable.

## 7. Example Folder Structure

```
/ProjectRoot
├── AuthDomain
│   ├── Controllers
│   ├── DTOs
│   ├── Models
│   └── Services
├── UserDomain
│   ├── Controllers
│   ├── DTOs
│   ├── Models
│   └── Services
├── Common
│   ├── Constants
│   ├── Models
│   ├── Services
│   └── Helpers
```

## 8. Code Style Conventions

- Use PascalCase for class names and method names.
- Use camelCase for local variables and method parameters.
- Use appropriate access modifiers (`private`, `protected`, `public`) explicitly.
- Use `async`/`await` where applicable and follow asynchronous programming best practices.

---

By following these instructions, GitHub Copilot will be able to generate consistent, clean, and maintainable C# code that aligns with enterprise development standards.