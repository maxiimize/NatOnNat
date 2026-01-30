# NätOnNät E-Commerce Platform

##  Overview
NätOnNät is a modern e-commerce platform built with .NET 9 and Clean Architecture principles. The system consists of a Web API, an MVC web application, and an administrative console application.

##  Architecture

### Clean Architecture
The project follows Clean Architecture with the following layers:

#### **Domain Layer** (`Domain/`)
- Contains business entities and interfaces
- No dependencies on other layers
- Implements Builder Pattern for the Product entity

#### **Infrastructure Layer** (`Infrastructure/`)
- Data Access Layer with Entity Framework Core
- Repository Pattern implementation
- Identity management with ASP.NET Core Identity
- SQL Server database connection

#### **Application Layer** (`Application/`)
- Services and DTOs
- Business logic and orchestration
- HTTPClient integration for API calls

#### **Presentation Layer** 
- **API** (`Api/`): RESTful Web API
- **MVC** (`NätOnNät.web/`): Responsive web application
- **Console** (`Console/`): Administration tool

##  Technical Stack

### Backend
- **.NET 9** - Latest .NET framework
- **ASP.NET Core** - Web framework
- **Entity Framework Core 9** - ORM for database management
- **SQL Server** - Database
- **ASP.NET Core Identity** - Authentication and authorization

### Frontend
- **Bootstrap 5.3** - CSS framework
- **Bootstrap Icons** - Icon library
- **Vanilla JavaScript** - Interactivity
- **Mobile First Design** - Responsive design

##  Design Patterns & Principles

### Design Patterns
1. **Builder Pattern** - For creating Product objects in a structured way
2. **Repository Pattern** - Abstraction of data access
3. **Dependency Injection** - Loosely coupled components
4. **Service Pattern** - Business logic separated from controllers

### SOLID Principles
- **Single Responsibility** - Each class has a clear responsibility
- **Open/Closed** - Open for extension, closed for modification
- **Liskov Substitution** - Interfaces can replace implementations
- **Interface Segregation** - Small, specific interfaces
- **Dependency Inversion** - Dependencies through abstractions

### Other Principles
- **DRY (Don't Repeat Yourself)** - Reusable code
- **Clean Code** - Readable and maintainable code
- **Async/Await** - Asynchronous database calls
- **Mobile First** - Responsive design for mobile devices

##  Features

### Web API
- `GET /api/products` - Retrieve all products
- `GET /api/products/favorites` - Retrieve favorite products
- `GET /api/products/newest` - Retrieve newest products
- `GET /api/products/{id}` - Retrieve specific product

### MVC Web Application
- **Landing Page** - Modern SPA-like homepage
- **Product Display** - Three sections: Favorites, New Arrivals, All Products
- **Weather Widget** - Real-time weather in navigation via external API
- **Responsive Design** - Works on all devices
- **Animations** - Smooth transitions and hover effects
- **Identity Integration** - Login and registration

### Console Application
- Display all registered users
- Display complete product catalog in table format
- Inventory statistics and summary
- Color-coded output for better readability

##  Installation & Setup

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB works)
- Visual Studio 2022 or VS Code

### Step-by-step

1. **Clone repository**

2. **Create database (runs automatically at startup)**
Migration runs automatically when the application starts for the first time.

3.  **IMPORTANT**: Always select either "MultipleStartupProjects" or "New Profile" as Startup Project for the application to run correctly.

4. **Start the applications**

### Startup Order

1. API

2. Console

3. Web App

##  Pre-configured Admin User

- **Email**: richard.chalk@admin.se
- **Password**: Abc123#

##  Database

- NatOnNatDb (SQL Server)

### Seeded Products
The system contains 11 pre-configured products:
- MacBook Pro 16"
- iPhone 15 Pro Max
- Sony WH-1000XM5
- Samsung Odyssey G9
- Keychron K8 Pro
- NVIDIA RTX 4090
- Samsung 990 PRO 2TB
- ASUS ROG Strix
- Ubiquiti Dream Machine
- iPad Pro 12.9"
- LG C3 OLED 65"

### Connection String
```
"Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```


### Input Validation
- All user input is validated on both client and server side
- Parameterized SQL queries via Entity Framework
- XSS protection through Razor syntax

### Authentication
- ASP.NET Core Identity for secure user management
- Password requirements: at least 6 characters, numbers, uppercase, lowercase and special characters
- Secure password hashing


### Optimizations
- Asynchronous database calls
- Lazy loading for images
- Responsive image sizing
- Efficient caching of static resources


### Future Features

- There are several buttons and quick links on the Landing Page that currently have no functionality. These are reserved for future development.


##  License

This project was created for educational purposes.

##  Developer

Developed as part of an educational assignment in .NET development.

---

**NätOnNät** - *Your reliable tech store online*
