# Task Management System

Task management system built as a technical test using Clean Architecture.

## Technology Stack

Backend:

- .NET 10
- ASP.NET Core Web API
- ADO.NET
- SQL Server
- JWT Authentication
- BCrypt password hashing

Frontend:

- React
- Vite

Testing:

- xUnit
- Moq

## Architecture

```txt
src/
├── TaskManager.Core/
├── TaskManager.Application/
├── TaskManager.Infrastructure/
├── TaskManager.Api/
├── TaskManager.WebUI/
└── TaskManager.Tests/
```

## Constraints

This project intentionally follows these rules:

✅ ADO.NET only  
✅ Clean Architecture  
✅ JWT authentication  
✅ Repository pattern

Not used:

❌ Entity Framework  
❌ Dapper  
❌ MediatR

## Project Status

Current progress:

- [x] Initial solution structure
- [ ] Domain entities
- [ ] Repository interfaces
- [ ] ADO.NET implementation
- [ ] Authentication
- [ ] Task CRUD API
- [ ] React frontend
- [ ] Unit tests
- [ ] Integration tests

## Setup

Coming soon

## Demo credentials

Coming soon
