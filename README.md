# ErpConnector - NOP Integration Template

> **🎯 This is a reusable template for ERP-to-eCommerce integrations**  
> Designed with clean architecture principles and best practices to serve as a foundation for similar integration projects. The focus is on maintainable, testable, and scalable code that can be easily adapted for different ERP systems and eCommerce platforms.

## 🏛️ Template Architecture Overview

This template demonstrates:
- **Clean Architecture** with clear separation of concerns
- **Repository Pattern** for data access abstraction
- **Dependency Injection** for loose coupling
- **Comprehensive Validation** with FluentValidation
- **Unit Testing** with 100% validator coverage
- **Configurable API Integration** for external systems
- **Extensible Design** for multiple platform support

## 📦 Template Features

### **🔄 Integration Capabilities**
- External API data synchronization
- Database operations with transaction support
- Data transformation and mapping
- Validation and error handling
- Logging and monitoring

### **🧪 Testing Framework**
- XUnit for unit testing
- FluentValidation testing helpers
- Mock-friendly architecture
- Comprehensive test coverage

### **⚙️ Configuration Management**
- Environment-based settings
- API endpoint configuration
- Database connection management
- Logging configuration

---

This project provides an integration between an ERP system and NOP Commerce platform using .NET 9.0.

## 🏗️ Project Structure

```
ErpConnector/
├── ErpConnector/                           
│   ├── Controllers/                        
│   │   ├── CategoryController.cs           
│   │   ├── ProductController.cs            
│   │   ├── TestController.cs               
│   │   └── ValidationTestController.cs     
│   ├── Services/                           
│   │   ├── IServices/                      
│   │   │   ├── IApiService.cs              
│   │   │   ├── INopCategoryService.cs      
│   │   │   ├── INopProductService.cs       
│   │   │   ├── INopLocalizedPropertyService.cs
│   │   │   └── IValidationService.cs       
│   │   ├── ApiService.cs                   
│   │   ├── NopCategoryService.cs           
│   │   ├── NopProductService.cs            
│   │   ├── NopLocalizedPropertyService.cs  
│   │   └── ValidationService.cs            
│   ├── Data/                               
│   │   ├── Repository/                     
│   │   │   ├── IRepository/                
│   │   │   │   ├── IApiRepository.cs       
│   │   │   │   ├── INopCategoryRepository.cs 
│   │   │   │   ├── INopProductRepository.cs  
│   │   │   │   └── INopLocalizedPropertyRepository.cs 
│   │   │   ├── ApiRepository.cs            
│   │   │   ├── NopCategoryRepository.cs    
│   │   │   ├── NopProductRepository.cs     
│   │   │   └── NopLocalizedPropertyRepository.cs
│   │   ├── DbInitializer/                  
│   │   │   ├── IDbInitializer.cs          
│   │   │   └── DbInitializer.cs            
│   │   └── DataContextDapper.cs            
│   ├── DTOs/                               
│   │   ├── CategoryFromApiDto.cs           
│   │   ├── ProductFromApiDto.cs            
│   │   └── ProductsResponseDto.cs          
│   ├── Models/                             
│   │   ├── Category.cs                     
│   │   ├── Product.cs                      
│   │   ├── LocalizedProperty.cs            
│   │   └── ApiSettings.cs                  
│   ├── Validators/                         
│   │   ├── CategoryFromApiDtoValidator.cs 
│   │   └── ProductFromApiDtoValidator.cs   
│   ├── Mappers/                            
│   │   ├── IMapper/                        
│   │   │   ├── ICategoryMapper.cs         
│   │   │   └── IProductMapper.cs          
│   │   ├── CategoryMapper.cs             
│   │   └── ProductMapper.cs                
│   ├── ErpConnector.csproj              
│   ├── Program.cs                         
│   └── appsettings.json                    
├── ErpConnector.Tests/                     
│   ├── Validators/                         
│   │   ├── CategoryFromApiDtoValidatorTests.cs  
│   │   └── ProductFromApiDtoValidatorTests.cs   
│   └── ErpConnector.Tests.csproj         
├── ErpConnector.sln                        
├── README.md                              
└── GIT-COMMANDS.md                         
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

## 🔧 Template Customization

### **Adapting for Different ERP Systems**
1. **Update API Configuration**: Modify `ApiSettings.cs` and `appsettings.json`
2. **Modify DTOs**: Update data transfer objects to match your API response structure
3. **Customize Validators**: Adjust validation rules in validator classes
4. **Update Mappers**: Modify mapping logic for data transformation
5. **Extend Services**: Add new service classes for additional business logic

### **Adapting for Different eCommerce Platforms**
1. **Update Database Models**: Modify domain models to match target platform
2. **Customize Repositories**: Update data access logic for different databases
3. **Modify Controllers**: Adjust controller logic for platform-specific operations
4. **Update Database Schema**: Modify `DbInitializer.cs` for platform tables

### **Adding New Entity Types**
1. **Create Domain Model**: Add new model in `Models/` folder
2. **Create DTO**: Add corresponding DTO in `DTOs/` folder
3. **Create Validator**: Add validation rules in `Validators/` folder
4. **Create Mapper**: Add mapping logic in `Mappers/` folder
5. **Create Repository**: Add data access in `Data/Repository/` folder
6. **Create Service**: Add business logic in `Services/` folder
7. **Create Controller**: Add API controller in `Controllers/` folder
8. **Add Tests**: Create comprehensive unit tests

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
  - Name validation (required, length 1-255)
  - Slug validation (required, length 1-255, format: lowercase, numbers, hyphens)
  - Edge cases (null, empty, maximum length)
- **ProductFromApiDtoValidator**: 21 tests
  - ID validation (must be > 0)
  - Title validation (required, length 1-255)
  - Price validation (must be >= 0)
  - Description validation (optional, max 1000 chars)
  - Category validation (required, length 1-100)
  - Multiple validation errors scenarios
- **100% validation rule coverage**

### **Template Testing Strategy**
- **Unit Tests**: Focus on individual components
- **Integration Tests**: Test component interactions
- **Validation Tests**: Comprehensive input validation
- **Mock-Friendly Design**: Easy to mock dependencies
- **Test Data Builders**: Reusable test data creation

## 📋 Template Architecture Benefits

### **Clean Architecture Principles**
- **Dependency Inversion**: High-level modules don't depend on low-level modules
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed Principle**: Open for extension, closed for modification
- **Interface Segregation**: Focused, specific interfaces
- **Separation of Concerns**: Clear boundaries between layers

### **Scalability Features**
- **Modular Design**: Easy to add new features
- **Configuration-Driven**: Behavior controlled by settings
- **Extensible Validation**: Easy to add new validation rules
- **Pluggable Components**: Swap implementations easily
- **Testable Architecture**: High code coverage achievable

### **Maintainability Features**
- **Clear Structure**: Easy to navigate and understand
- **Consistent Patterns**: Repeated architectural patterns
- **Comprehensive Logging**: Detailed application insights
- **Error Handling**: Graceful error management
- **Documentation**: Well-documented code and architecture

## 🔧 Configuration

Update `ErpConnector/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "ApiSettings": {
    "BaseUrl": "https://your-erp-system.com/api/",
    "TimeoutSeconds": 30,
    "ProductsEndpoint": "products",
    "CategoriesEndpoint": "categories"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "ErpConnector": "Debug"
    }
  }
}
```

## 🏆 Code Quality

### Best Practices Implemented
- **Clean Architecture**: Separation of concerns with distinct layers
- **SOLID Principles**: Single responsibility, open/closed, dependency inversion
- **Dependency Injection**: Built-in .NET DI container
- **Repository Pattern**: Abstracted data access
- **Interface Segregation**: Focused interfaces for each component
- **Unit Testing**: Comprehensive test coverage with XUnit
- **Validation**: Input validation with FluentValidation
- **Logging**: Structured logging throughout the application
- **Error Handling**: Proper exception handling and user feedback

### Testing Framework
- **XUnit**: Modern .NET testing framework
- **FluentAssertions**: Readable test assertions
- **FluentValidation.TestHelper**: Specialized validation testing
- **Theory Tests**: Parameterized tests for multiple scenarios
- **AAA Pattern**: Arrange-Act-Assert test structure

## 📊 Project Statistics

- **Main Project**: 25+ classes across 7 architectural layers
- **Controllers**: 4 controller classes handling different operations
- **Services**: 5 service classes with corresponding interfaces
- **Repositories**: 4 repository classes with interface abstractions
- **Validators**: 2 comprehensive validator classes
- **Mappers**: 2 mapper classes for data transformation
- **Models/DTOs**: 7 model classes for data representation
- **Test Project**: 36 comprehensive unit tests
- **Validation Rules**: 10+ validation rules with full coverage
- **API Integration**: RESTful API client with error handling

## 🔄 Development Workflow

1. **Feature Development**: Add new features in appropriate layers
2. **Interface First**: Define interfaces before implementations
3. **Testing**: Write unit tests for new functionality
4. **Validation**: Add validation rules as needed
5. **Mapping**: Create mappers for data transformation
6. **Documentation**: Update README and code comments

## 🛠️ Technologies Used

- **.NET 9.0**: Latest .NET framework
- **C#**: Primary programming language
- **FluentValidation**: Input validation framework
- **Dapper**: Lightweight ORM for database operations
- **XUnit**: Modern testing framework
- **FluentAssertions**: Fluent test assertions
- **Microsoft.Extensions.***: Logging, DI, Configuration, HTTP
- **SQL Server**: Database platform
- **RESTful APIs**: External service integration



## 📝 Contributing

1. Follow the established layered architecture
2. Implement interfaces before concrete classes
3. Write unit tests for new features
4. Use FluentValidation for input validation
5. Follow clean architecture principles
6. Update documentation as needed
7. Maintain consistent naming conventions

## 📄 License

This template is provided as-is for educational and development purposes. Adapt and modify according to your specific integration requirements.
