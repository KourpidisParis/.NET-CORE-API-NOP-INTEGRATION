# ErpConnector

**ErpConnector** is a lightweight console application designed to **synchronize** products and categories  
from an **external API** into the **NopCommerce** database.

The project is designed as a **template**, making it easy to adjust for different APIs with minimal changes.

---

## Features

- Connects to any external API (easily adjustable).
- **FluentValidation** for data integrity and validation.
- Maps API data to NopCommerce database models.
- Inserts or updates **Products** and **Categories**.
- Handles database initialization.
- Clean separation between services, repositories, processors, and mappings.
- Modular and scalable architecture for future enhancements.
- **Automatic validation** of products and categories before database operations.
- Detailed logging and error tracking.

---

## Project Structure

```plaintext
ErpConnector
│
├── Controllers
│    ├── ProductController.cs
│    ├── CategoryController.cs
│    ├── TestController.cs
│    └── ValidationTestController.cs
│
├── DTOs
│    ├── ProductFromApiDto.cs
|    ├── ProductsResponseDto.cs  
│    └── CategoryFromApiDto.cs
│
├── Validators
│    ├── ProductFromApiDtoValidator.cs
│    └── CategoryFromApiDtoValidator.cs
│
├── Mappers
│    ├── ProductMapper.cs
│    └── CategoryMapper.cs
│
├── Models
│    ├── Product.cs
│    ├── Category.cs
│    ├── LocalizedProperty.cs
│    └── ApiSettings.cs
│
├── Data/Repository
│    ├── ApiRepository.cs
│    ├── NopProductRepository.cs
│    ├── NopLocalizedPropertyRepository.cs
│    └── NopCategoryRepository.cs
│
├── Services
│    ├── ApiService.cs
│    ├── NopProductService.cs
|    ├── NopLocalizedPropertyService.cs
│    ├── NopCategoryService.cs
│    └── ValidationService.cs
│
├── Data
|    ├── DbInitializer.cs
│    └── DataContextDapper.cs
│
├── Program.cs
└── appsettings.json
```

## Technologies Used

- **.NET 9** — for building the console application.
- **FluentValidation** — for data validation and integrity.
- **Dapper** — lightweight ORM for fast database access.
- **NopCommerce** — target platform database (products and categories).
- **Dependency Injection (DI)** — for clean architecture and loose coupling.
- **HttpClient** — for API communication.
- **MSSQL** — database used by NopCommerce.

---

## FluentValidation Implementation

### Overview
The project includes **FluentValidation** to ensure data integrity before syncing to the database. The implementation follows SOLID principles and keeps validation logic simple and clean.

### Validation Rules

**Category Validation:**
- Name: Required, 1-255 characters
- Slug: Required, 1-255 characters, lowercase letters/numbers/hyphens only

**Product Validation:**
- Id: Must be greater than 0
- Title: Required, 1-255 characters
- Price: Must be >= 0
- Description: Optional, max 1000 characters
- Category: Required, 1-100 characters

### How Validation Works

Validation is automatically applied in the **services** where the actual `foreach` loops process the data:

```csharp
// In NopProductService.SyncProducts method
foreach (var productDto in products)
{
    try
    {
        // Validate product first
        var validationResult = _validationService.ValidateProduct(productDto);
        
        if (!_validationService.IsValid<ProductFromApiDto>(validationResult, out var validationErrors))
        {
            validationErrorCount++;
            _logger.LogWarning("Invalid product {ProductTitle} (ID: {ProductId}): {Errors}", 
                productDto.Title, productDto.Id, string.Join(", ", validationErrors));
            continue; // Skip invalid product
        }
        
        // Process valid product...
        var productModel = _productMapper.MapToProduct(productDto);
        // ... rest of processing
    }
    catch (Exception ex)
    {
        // Handle other errors
    }
}
```

### Benefits

✅ **Data Integrity** - Invalid data is rejected before reaching the database  
✅ **Clean Separation** - Validation logic is separated from business logic  
✅ **Service-Level Validation** - Validation happens in services where the foreach loops are  
✅ **Extensible** - Easy to add new validation rules  
✅ **Testable** - Validation can be tested independently  
✅ **Logging** - Invalid data is logged with detailed error messages  
✅ **SOLID Principles** - Each validator has a single responsibility  
✅ **Performance** - Invalid items are skipped early in the process

---

## Usage

### Available Commands

```bash
# Sync products with validation
dotnet run products

# Sync categories with validation
dotnet run categories

# Test validation with sample data
dotnet run validate

# Run existing tests
dotnet run test
```

### Console Output

The application provides clear feedback during operation:

```
🔍 Validating 150 products...
📨 Processed 10/150 products...
📨 Processed 20/150 products...
...
📊 Summary: 142 processed, 2 errors, 3 skipped, 3 validation errors
✅ Product synchronization completed
```

### Configuration

Update `appsettings.json` with your API and database settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=your-nop-db;Trusted_Connection=true;"
  },
  "ApiSettings": {
    "BaseUrl": "https://your-api.com",
    "ProductsEndpoint": "/api/products",
    "CategoriesEndpoint": "/api/categories"
  }
}
```

### Adding New Validation Rules

To add new validation rules, simply modify the appropriate validator:

```csharp
// In ProductFromApiDtoValidator.cs
RuleFor(x => x.Title)
    .NotEmpty().WithMessage("Product title is required")
    .Length(1, 255).WithMessage("Product title must be between 1 and 255 characters")
    .Must(BeUniqueTitle).WithMessage("Product title must be unique"); // New rule
```

The validation will automatically apply to all products during sync operations.
