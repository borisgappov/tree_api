# Tree API

A robust ASP.NET Core 8 Web API for managing hierarchical tree structures with PostgreSQL database support. This API provides comprehensive functionality for tree management, journal logging, partner management, and exception handling.

## 🚀 Features

- **Tree Management**: Create, retrieve, and manage hierarchical tree structures
- **Node Operations**: Add, delete, rename, and organize nodes within trees
- **Journal System**: Comprehensive logging and event tracking with pagination
- **Partner Management**: Partner identification and tracking system
- **Exception Handling**: Centralized exception logging and handling
- **Swagger Documentation**: Interactive API documentation
- **Unit Testing**: Comprehensive test coverage for all components
- **PostgreSQL Integration**: Robust database support with Entity Framework Core
- **AutoMapper**: Efficient object mapping between entities and models

## 🏗️ Architecture

The project follows a clean architecture pattern with the following layers:

- **Controllers**: REST API endpoints with Swagger documentation
- **Services**: Business logic layer with dependency injection
- **Data Layer**: Entity Framework Core with PostgreSQL
- **Models**: DTOs and view models for API communication
- **Middleware**: Custom middleware for exception handling and routing
- **Extensions**: Custom service configurations and extensions

## 📋 Prerequisites

- .NET 8.0 SDK
- PostgreSQL 12.0 or higher
- Visual Studio 2022 or VS Code

## 🛠️ Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TreeApi
   ```

2. **Configure the database**
   - Install PostgreSQL if not already installed
   - Create a database named `tree_api`
   - Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=tree_api;Username=your_username;Password=your_password"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Build and run the application**
   ```bash
   dotnet build
   dotnet run
   ```

5. **Access the API**
   - API: `https://localhost:7001` (or the port shown in your terminal)
   - Swagger UI: `https://localhost:7001` (root path)

## 📚 API Endpoints

### Tree Management

#### Get Tree
```
POST /api.user.tree/get?treeName={treeName}
```
Returns the entire tree structure. If the tree doesn't exist, it will be created automatically.

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

## 🗄️ Database Schema

### Trees Table
- `id` (bigint, primary key): Unique tree identifier
- `name` (varchar(255)): Tree name
- `created_at` (timestamp): Creation timestamp
- `updated_at` (timestamp): Last update timestamp

### Nodes Table
- `id` (bigint, primary key): Unique node identifier
- `name` (varchar(255)): Node name
- `tree_id` (bigint, foreign key): Reference to tree
- `parent_id` (bigint, nullable, foreign key): Reference to parent node
- `created_at` (timestamp): Creation timestamp
- `updated_at` (timestamp): Last update timestamp

### Exception Journal Table
- `id` (bigint, primary key): Unique exception identifier
- `event_id` (bigint): Event identifier
- `text` (text): Exception details
- `created_at` (timestamp): Creation timestamp

### Partners Table
- `id` (bigint, primary key): Unique partner identifier
- `code` (varchar(255)): Partner identification code
- `created_at` (timestamp): Creation timestamp

## 🧪 Testing

The project includes comprehensive unit tests covering:

- **Controllers**: API endpoint testing
- **Services**: Business logic testing
- **Repositories**: Data access layer testing
- **Utilities**: Helper function testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test TreeApi.Tests/
```

## 🔧 Configuration

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

## 📦 Dependencies

### Core Dependencies
- **ASP.NET Core 8.0**: Web framework
- **Entity Framework Core 8.0**: ORM for database operations
- **Npgsql.EntityFrameworkCore.PostgreSQL**: PostgreSQL provider
- **AutoMapper**: Object mapping library

### Documentation
- **Swashbuckle.AspNetCore**: Swagger/OpenAPI documentation
- **Swashbuckle.AspNetCore.Annotations**: Enhanced Swagger documentation

## 🏃‍♂️ Running the Application

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
