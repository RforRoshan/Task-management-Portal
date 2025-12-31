# ARAS API

A .NET 8 ASP.NET Core Web API for task and user management.

## Project Structure

This solution is organized into multiple projects following clean architecture principles:

- **ARAS**: Main API project containing controllers, middleware, and startup configuration.
- **ARAS.Business**: Business logic layer with services and utilities.
- **ARAS.Domain.Models**: Domain models and DTOs.
- **ARAS.Infrastructure**: Data access layer with Entity Framework DbContext, repositories, and migrations.
- **ARAS.Models**: Request and response models.

## Features

- User authentication and registration
- Task management (create, update, delete tasks and subtasks)
- Resource management
- Project-based organization
- Encryption middleware for secure data handling
- Exception handling middleware
- Email utilities

## Prerequisites

- .NET 8.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or VS Code with C# extensions

## Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd API
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update the connection string in `ARAS/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=ARASDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

4. Run database migrations:
   ```bash
   dotnet ef database update --project ARAS.Infrastructure --startup-project ARAS
   ```

5. Build the project:
   ```bash
   dotnet build
   ```

## Running the Application

1. Start the API:
   ```bash
   dotnet run --project ARAS
   ```

2. The API will be available at `http://localhost:5145` (or the configured port).

## API Endpoints

Most endpoints require authentication using JWT tokens obtained from the login endpoint.

### User Management
- `POST /api/User/Login` - User login (returns JWT token)
- `POST /api/User/Register` - User registration
- `POST /api/User/ChangePassword` - Change password

### Task Management
- `POST /api/Task/GetAllResourceNames` - Get all resource names
- `POST /api/Task/AddDropDownValues` - Add dropdown values
- And many more task-related endpoints...

## Testing

You can use the `ARAS.http` file in VS Code with the REST Client extension to test the API endpoints.

Run unit tests using:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests
5. Submit a pull request

