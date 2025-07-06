# ErpConnector - NOP Integration Project

This project provides an integration between an ERP system and NOP Commerce platform using .NET 9.0.

## 🏗️ Project Structure

```
ErpConnector/
├── ErpConnector/                    # Main application
│   ├── Controllers/                 # API controllers
│   ├── Services/                    # Business logic
│   ├── Data/                        # Database context and repositories
│   ├── DTOs/                        # Data transfer objects
│   ├── Models/                      # Domain models
│   ├── Validators/                  # FluentValidation validators
│   ├── Mappers/                     # Object mapping
│   ├── ErpConnector.csproj         # Main project file
│   ├── Program.cs                   # Application entry point
│   └── appsettings.json            # Configuration
├── ErpConnector.Tests/              # Unit tests
│   ├── Validators/                  # Validator tests
│   │   ├── CategoryFromApiDtoValidatorTests.cs
│   │   └── ProductFromApiDtoValidatorTests.cs
│   └── ErpConnector.Tests.csproj   # Test project file
├── ErpConnector.sln                 # Solution file
└── README.md                        # This file
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code

### Setup
1. Clone the repository
2. Update connection string in `ErpConnector/appsettings.json`
3. Restore packages: `dotnet restore ErpConnector.sln`
4. Build solution: `dotnet build ErpConnector.sln`

### Running the Application
```bash
cd ErpConnector
dotnet run products    # Sync products from API
dotnet run categories  # Sync categories from API
dotnet run test        # Run test operations
dotnet run validate    # Test validation
```

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test ErpConnector.sln

# Run tests with detailed output
dotnet test ErpConnector.sln --verbosity normal

# Run only validator tests
dotnet test ErpConnector.Tests --filter "Category=Validators"
```

### Test Coverage
- **36 unit tests** covering all validators
- **CategoryFromApiDtoValidator**: 15 tests
- **ProductFromApiDtoValidator**: 21 tests
- **100% validation rule coverage**

## 📋 Features

### Core Functionality
- **Product Synchronization**: Fetch and sync products from external API
- **Category Management**: Sync product categories
- **Data Validation**: Comprehensive validation using FluentValidation
- **Database Integration**: SQL Server with Dapper ORM
- **Logging**: Structured logging with Microsoft.Extensions.Logging

### Architecture
- **Clean Architecture**: Separation of concerns with distinct layers
- **Dependency Injection**: Built-in .NET DI container
- **Validation**: FluentValidation for data integrity
- **Mapping**: Custom mappers for data transformation
- **Error Handling**: Comprehensive error handling and logging

## 🔧 Configuration

Update `ErpConnector/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "ApiSettings": {
    "BaseUrl": "https://dummyjson.com/",
    "TimeoutSeconds": 30,
    "ProductsEndpoint": "products",
    "CategoriesEndpoint": "products/categories"
  }
}
```

## 🏆 Code Quality

### Best Practices Implemented
- **Unit Testing**: Comprehensive test coverage with XUnit
- **Validation**: Input validation with FluentValidation
- **Logging**: Structured logging throughout the application
- **Error Handling**: Proper exception handling and user feedback
- **Clean Code**: SOLID principles and clean architecture

### Testing Framework
- **XUnit**: Modern .NET testing framework
- **FluentAssertions**: Readable test assertions
- **FluentValidation.TestHelper**: Specialized validation testing
- **Theory Tests**: Parameterized tests for multiple scenarios

## 📊 Project Statistics

- **Main Project**: 50+ classes across 7 layers
- **Test Project**: 36 comprehensive unit tests
- **Validation Rules**: 10+ validation rules with full coverage
- **API Integration**: RESTful API client with error handling

## 🔄 Development Workflow

1. **Feature Development**: Add new features in appropriate layers
2. **Testing**: Write tests for new functionality
3. **Validation**: Add validation rules as needed
4. **Documentation**: Update README and code comments

## 🛠️ Technologies Used

- **.NET 9.0**: Latest .NET framework
- **C#**: Primary programming language
- **FluentValidation**: Input validation
- **Dapper**: Lightweight ORM
- **XUnit**: Testing framework
- **Microsoft.Extensions.***: Logging, DI, Configuration
- **SQL Server**: Database

## 📝 Contributing

1. Follow the established project structure
2. Write unit tests for new features
3. Use FluentValidation for input validation
4. Follow clean architecture principles
5. Update documentation as needed

## 🐛 Troubleshooting

### Common Issues
- **Connection Issues**: Check SQL Server connection string
- **Build Errors**: Ensure all NuGet packages are restored
- **Test Failures**: Verify test data and validation rules

### Support
For issues or questions, check the code comments and validation rules in the respective classes.
