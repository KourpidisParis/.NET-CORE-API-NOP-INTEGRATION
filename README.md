# ErpConnector

**ErpConnector** is a lightweight console application designed to **synchronize** products and categories  
from an **external API** into the **NopCommerce** database.

The project is designed as a **template**, making it easy to adjust for different APIs with minimal changes.

---

## Features

- Connects to any external API (easily adjustable).
- Maps API data to NopCommerce database models.
- Inserts or updates **Products** and **Categories**.
- Handles database initialization.
- Clean separation between services, repositories, processors, and mappings.
- Uses **AutoMapper** for efficient object mapping.
- Modular and scalable architecture for future enhancements.

---

## Project Structure

```plaintext
ErpConnector
│
├── Controllers
│    ├── ProductController.cs
│    └── CategoryController.cs
│
├── DTOs
│    ├── ProductFromApiDto.cs
│    └── CategoryFromApiDto.cs
│
├── Mapping
│    └── MappingProfile.cs
│
├── Models
│    ├── Product.cs
│    └── Category.cs
│
├── Processors
│    ├── ProductProcessor.cs
│    └── CategoryProcessor.cs
│
├── Data/Repository
│    ├── ApiRepository.cs
│    ├── NopProductRepository.cs
│    └── NopCategoryRepository.cs
│
├── Services
│    ├── ApiService.cs
│    ├── NopProductService.cs
│    └── NopCategoryService.cs
│
├── Data
│    └── DataContextDapper.cs
│
├── Program.cs
└── appsettings.json
```

## Technologies Used

- **.NET 9** — for building the console application.
- **Dapper** — lightweight ORM for fast database access.
- **AutoMapper** — for efficient and clean object-to-object mapping.
- **NopCommerce** — target platform database (products and categories).
- **Dependency Injection (DI)** — for clean architecture and loose coupling.
- **HttpClient** — for API communication.
- **MSSQL** — database used by NopCommerce.
