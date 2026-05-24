# Task Management System

Full-stack task management application built for a .NET technical exercise using Clean Architecture principles.

## Tech Stack

### Backend

- .NET 10
- ASP.NET Core Web API
- JWT Authentication
- ADO.NET
- SQL Server
- Swagger
- xUnit + Moq

### Frontend

- React
- Vite
- CSS

---

# Architecture

```txt
src
├── TaskManager.Api
├── TaskManager.Application
├── TaskManager.Core
├── TaskManager.Infrastructure
├── TaskManager.Tests
└── TaskManager.WebUI
```

---

# Features

- User registration and login
- JWT authentication
- Protected task CRUD
- Swagger integration
- Responsive React frontend
- Unit tests
- SQL Server persistence
- Clean Architecture

---

# Database Setup

Run the SQL scripts located in:

```txt
TaskManager.Infrastructure/Database
```

Execution order:

```txt
1. init.sql
2. seed.sql
```

---

# Environment Variables

## Backend

Create:

```txt
src/.env
```

Example:

```env
CONNECTION_STRING=Server=localhost;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True

JWT_SECRET=your-secret-key

JWT_ISSUER=TaskManager.Api
JWT_AUDIENCE=TaskManager.WebUI

CORS_ALLOWED_ORIGIN=http://localhost:5173
```

---

## Frontend

Create:

```txt
src/TaskManager.WebUI/.env
```

Example:

```env
VITE_API_BASE_URL=https://localhost:7058/api
```

---

# Running the Backend

```powershell
dotnet run --project .\src\TaskManager.Api
```

Swagger:

```txt
https://localhost:7058/swagger/index.html
```

---

# Running the Frontend

```powershell
cd .\src\TaskManager.WebUI

npm install
npm run dev
```

Frontend:

```txt
http://localhost:1234
```

---

# Demo Credentials

```txt
see.sql
```

---

# Running Tests

```powershell
dotnet test
```

---

# AI-Assisted Development

Generative AI tools were used during development for:

- architecture planning
- scaffolding
- repository generation
- unit test generation
- frontend structure

Tools used:

- ChatGPT
- GitHub Copilot

All generated code was manually reviewed, adjusted, validated, and tested.
