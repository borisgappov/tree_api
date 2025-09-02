# TreeApi.Tests

Test project for TreeApi, organized in accordance with the structure of the main project.

## Project Structure

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
├── Utils/                 # Utility tests
│   └── SnowflakeIdGeneratorTests.cs
└── TreeApi.Tests.csproj
```

## Correspondence to Main Project Structure

The test project structure mirrors the main TreeApi project structure:

- **Controllers/** - tests for `TreeApi/Controllers/`
- **Data/Repositories/** - tests for `TreeApi/Data/Repositories/`
- **Data/UnitOfWork/** - tests for `TreeApi/Data/UnitOfWork/`
- **Services/** - tests for `TreeApi/Services/`
- **Utils/** - tests for `TreeApi/Utils/`

## Namespaces

All test classes use corresponding namespaces:
- `TreeApi.Tests.Controllers`
- `TreeApi.Tests.Data.Repositories`
- `TreeApi.Tests.Data.UnitOfWork`
- `TreeApi.Tests.Services`
- `TreeApi.Tests.Utils`

## Running Tests

```bash
dotnet test
```

All tests use in-memory database for isolation and fast execution.
