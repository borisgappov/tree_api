# Tree API - ASP.NET Core 8 Solution

A comprehensive solution for managing hierarchical tree structures built on ASP.NET Core 8 with PostgreSQL and comprehensive testing.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Installation and Setup](#installation-and-setup)
- [API Endpoints](#api-endpoints)
- [Database](#database)
- [Testing](#testing)
- [Configuration](#configuration)
- [Deployment](#deployment)
- [Contributing](#contributing)

## 🎯 Overview

Tree API is a modern web application for managing hierarchical data structures. The solution includes a main API project and a comprehensive set of unit tests that ensure code reliability and quality.

### Solution Structure

```
TreeApi/
├── TreeApi/                    # Main API project
│   ├── Controllers/           # REST API controllers
│   ├── Services/             # Business logic
│   ├── Data/                 # Data layer (EF Core)
│   ├── Models/               # DTOs and view models
│   ├── Middleware/           # Custom middleware
│   ├── Extensions/           # Configuration extensions
│   └── Swagger/              # Documentation settings
└── TreeApi.Tests/            # Test project
    ├── Controllers/          # Controller tests
    ├── Services/            # Service tests
    ├── Data/                # Data layer tests
    └── Utils/               # Utility tests
```

## 🏗️ Architecture

The project follows clean architecture principles with clear layer separation:

### Application Layers

- **Controllers Layer**: REST API endpoints with Swagger documentation
- **Services Layer**: Business logic with dependency injection
- **Data Layer**: Entity Framework Core with PostgreSQL
- **Models Layer**: DTOs and view models for API
- **Middleware Layer**: Custom middleware for exception handling and routing

### Design Patterns

- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transactional data management
- **Dependency Injection**: Dependency injection
- **AutoMapper**: Object mapping
- **Middleware Pattern**: Request and exception handling

## 🚀 Features

### Core Functionality

- **Tree Management**: Create, retrieve, and manage hierarchical structures
- **Node Operations**: Add, delete, rename, and organize nodes
- **Journal System**: Comprehensive event logging with pagination
- **Partner Management**: Partner identification and tracking system
- **Exception Handling**: Centralized logging and error handling

### Additional Features

- **Swagger Documentation**: Interactive API documentation
- **Comprehensive Testing**: Full test coverage for all components
- **PostgreSQL Integration**: Reliable database support
- **AutoMapper**: Efficient mapping between entities and models
- **Snowflake ID**: Unique identifier generation

## 🛠️ Technology Stack

### Core Technologies

- **.NET 8.0**: Modern development platform
- **ASP.NET Core 8**: Web framework
- **Entity Framework Core 8**: ORM for database operations
- **PostgreSQL**: Relational database
- **AutoMapper**: Object mapping library

### Documentation and Testing

- **Swashbuckle.AspNetCore**: Swagger/OpenAPI documentation
- **NUnit**: Unit testing framework
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database for tests

### Project Dependencies

#### TreeApi.csproj
```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

#### TreeApi.Tests.csproj
```xml
<PackageReference Include="NUnit" Version="4.2.2" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
<PackageReference Include="coverlet.collector" Version="6.0.2" />
```

## 📦 Installation and Setup

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL 12.0 or higher
- Visual Studio 2022 or VS Code
- Git

### Step-by-Step Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TreeApi
   ```

2. **Database setup**
   - Install PostgreSQL if not already installed
   - Create a database named `tree_api`
   - Update the connection string in `TreeApi/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=tree_api;Username=your_username;Password=your_password"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   cd TreeApi
   dotnet ef database update
   ```

4. **Build and run**
   ```bash
   # Build the entire solution
   dotnet build
   
   # Run the main application
   cd TreeApi
   dotnet run
   ```

5. **Access the application**
   - API: `https://localhost:7001` (or the port shown in your terminal)
   - Swagger UI: `https://localhost:7001` (root path)

## 📚 API Endpoints

### Tree Management

#### Get Tree
```
POST /api.user.tree/get?treeName={treeName}
```
Returns the complete tree structure. If the tree doesn't exist, it will be created automatically.

**Parameters:**
- `treeName` (string, required): Name of the tree to retrieve

**Response:** `MNode` - Root node with complete tree structure

### Node Operations

#### Create Node
```
POST /api.user.tree.node/create?treeName={treeName}&parentNodeId={parentNodeId}&nodeName={nodeName}
```
Creates a new node in the specified tree.

**Parameters:**
- `treeName` (string, required): Name of the tree
- `parentNodeId` (long, required): ID of the parent node
- `nodeName` (string, required): Name for the new node

#### Delete Node
```
POST /api.user.tree.node/delete?treeName={treeName}&nodeId={nodeId}
```
Deletes an existing node from the tree.

**Parameters:**
- `treeName` (string, required): Name of the tree
- `nodeId` (long, required): ID of the node to delete

#### Rename Node
```
POST /api.user.tree.node/rename?treeName={treeName}&nodeId={nodeId}&newNodeName={newNodeName}
```
Renames an existing node in the tree.

**Parameters:**
- `treeName` (string, required): Name of the tree
- `nodeId` (long, required): ID of the node to rename
- `newNodeName` (string, required): New name for the node

### Journal System

#### Get Journal Range
```
POST /api.user.journal/getRange?skip={skip}&take={take}
```
Retrieves journal entries with pagination support.

**Parameters:**
- `skip` (int, required): Number of items to skip
- `take` (int, required): Maximum number of items to return
- `filter` (VJournalFilter, optional): Filter criteria in request body

**Response:** `MRangeMJournalInfo` - Paginated journal entries

#### Get Single Journal Entry
```
POST /api.user.journal/getSingle?id={id}
```
Retrieves a specific journal entry by ID.

**Parameters:**
- `id` (long, required): Journal entry ID

**Response:** `MJournal` - Journal entry details

### Partner Management

#### Remember Partner
```
POST /api.user.partner/rememberMe?code={code}
```
Marks a partner as remembered by their unique code.

**Parameters:**
- `code` (string, required): Partner identification code

**Response:** Success message with partner ID

## 🗄️ Database

### Database Schema

#### Trees Table
- `id` (bigint, primary key): Unique tree identifier
- `name` (varchar(255)): Tree name
- `created_at` (timestamp): Creation timestamp
- `updated_at` (timestamp): Last update timestamp

#### Nodes Table
- `id` (bigint, primary key): Unique node identifier
- `name` (varchar(255)): Node name
- `tree_id` (bigint, foreign key): Reference to tree
- `parent_id` (bigint, nullable, foreign key): Reference to parent node
- `created_at` (timestamp): Creation timestamp
- `updated_at` (timestamp): Last update timestamp

#### Exception Journal Table
- `id` (bigint, primary key): Unique exception identifier
- `event_id` (bigint): Event identifier
- `text` (text): Exception details
- `created_at` (timestamp): Creation timestamp

#### Partners Table
- `id` (bigint, primary key): Unique partner identifier
- `code` (varchar(255)): Partner identification code
- `created_at` (timestamp): Creation timestamp

### Migrations

The project uses Entity Framework Core migrations for database schema management:

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName
```

## 🧪 Testing

### Test Project Structure

```
TreeApi.Tests/
├── Controllers/           # Controller tests
│   ├── JournalControllerTests.cs
│   ├── PartnerControllerTests.cs
│   ├── TreeControllerTests.cs
│   └── TreeNodeControllerTests.cs
├── Data/                  # Data layer tests
│   ├── Repositories/      # Repository tests
│   │   ├── NodeRepositoryTests.cs
│   │   └── TreeRepositoryTests.cs
│   └── UnitOfWork/        # UnitOfWork tests
│       └── UnitOfWorkTests.cs
├── Services/              # Service tests
│   ├── JournalServiceTests.cs
│   ├── PartnerServiceTests.cs
│   └── TreeServiceTests.cs
└── Utils/                 # Utility tests
    └── SnowflakeIdGeneratorTests.cs
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test TreeApi.Tests/

# Run tests with detailed output
dotnet test --verbosity normal
```

### Testing Features

- **In-Memory Database**: All tests use in-memory database for isolation and fast execution
- **Unit Testing**: Each component is tested independently
- **Integration Testing**: Testing interaction between components
- **Code Coverage**: Measuring code coverage with tests

## ⚙️ Configuration

### Environment Variables

The application supports different configuration environments:

- **Development**: `appsettings.Development.json`
- **Production**: `appsettings.json`

### Logging

Logging is configured with different levels:
- Default: Information
- Microsoft.AspNetCore: Warning

### Swagger Configuration

Custom Swagger filters are implemented for:
- Schema customization
- Operation documentation
- Request/response formatting

## 🚨 Exception Handling

The application implements a centralized exception handling system:

- **ExceptionHandlingMiddleware**: Catches and logs all unhandled exceptions
- **SecureException**: Custom exception type for security-related errors
- **ExceptionJournalService**: Logs exceptions to the database

## 🔄 Middleware

### Custom Middleware

- **ExceptionHandlingMiddleware**: Global exception handling
- **DotRouteMiddleware**: Custom routing logic

## 🚀 Deployment

### Development Mode
```bash
dotnet run --environment Development
```

### Production Mode
```bash
dotnet run --environment Production
```

### Docker Support
```bash
# Build Docker image
docker build -t tree-api .

# Run Docker container
docker run -p 8080:80 tree-api
```

### Docker Compose
```yaml
version: '3.8'
services:
  tree-api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    depends_on:
      - postgres
  
  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: tree_api
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
    ports:
      - "5432:5432"
```

## 📝 API Documentation

Interactive API documentation is available at the root URL when running in development mode. The Swagger UI provides:

- Complete endpoint documentation
- Request/response schemas
- Interactive testing interface
- Authentication information (if applicable)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Standards

- Use C# coding conventions
- Add comments to public APIs
- Write tests for new functionality
- Update documentation when necessary

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the API documentation at `/swagger`

## 🔄 Version History

- **v1.0.0**: Initial release with basic tree management functionality
- **v1.1.0**: Added journal system and partner management
- **v1.2.0**: Enhanced exception handling and middleware
- **v1.3.0**: Comprehensive testing suite and documentation improvements

## 📊 Project Statistics

- **Languages**: C# (100%)
- **Framework**: ASP.NET Core 8
- **Database**: PostgreSQL
- **Testing**: NUnit with in-memory EF Core
- **Documentation**: Swagger/OpenAPI

---

**Built with ❤️ on ASP.NET Core 8**
