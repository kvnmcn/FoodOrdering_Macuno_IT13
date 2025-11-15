# Food Ordering System

Built with **Blazor Hybrid Web App (.NET 9)** and **ASP.NET Core Web API**. This system allows users to manage menu items and create orders.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Blazor](https://img.shields.io/badge/Blazor-Hybrid-green.svg)

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
- [Database Schema](#database-schema)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Technologies Used](#technologies-used)
- [License](#license)
- [Author](#author)

## Features

### Menu Management
- Create, read, update, and delete menu items
- Set item availability status
- Categorize menu items
- Price management
- Form validation with error messages
- Loading states and error handling

### Order Management
- Create orders with multiple items
- Dynamic quantity adjustment with +/- buttons
- Real-time subtotal and total calculation
- Duplicate item detection
- Modern card-based UI design
- Comprehensive error handling

### Order History
- View all orders with details
- Display order items with menu information
- Order date and total amount tracking

## Architecture

```
FoodOrdering/
├── FoodOrder/                  # Blazor Hybrid Web App
│   ├── Components/
│   │   ├── Pages/
│   │   │   ├── CreateOrder.razor
│   │   │   ├── MenuItems.razor
│   │   │   └── Orders.razor
│   │   ├── Layout/
│   │   │   ├── MainLayout.razor
│   │   │   └── NavMenu.razor
│   │   └── Routes.razor
│   ├── wwwroot/
│   │   └── css/
│   │       ├── create-order.css
│   │       ├── menu-items.css
│   │       └── app.css
│   └── Program.cs
│
├── WebAPI/                     # ASP.NET Core Web API Backend
│   ├── Controllers/
│   │   ├── MenuItemsController.cs
│   │   └── OrdersController.cs
│   ├── Models/
│   │   ├── MenuItem.cs
│   │   ├── Order.cs
│   │   └── OrderItem.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   └── Program.cs
│
└── DATABASE.sql                # Database creation script
```

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) 
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer Edition)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/food-ordering-system.git
   cd food-ordering-system
   ```

2. **Configure the database connection**
   
   Update `appsettings.json` in the `WebAPI` project:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=FoodOrder_MACUNO;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

### Database Setup

**Option A: Import Database from SQL Script (Recommended)**

1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Open the `DATABASE.sql` file located in the root folder of the project
4. Execute the script to create the database and tables
5. The database `FoodOrder_MACUNO` will be created with all necessary tables and structure

**Option B: Use Entity Framework Migrations**

Open Package Manager Console in Visual Studio:
```powershell
# Set WebAPI as the default project
Add-Migration InitialCreate
Update-Database
```

Or using .NET CLI:
```bash
cd WebAPI
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Running the Application

**Option A: Using Visual Studio**
1. Set both projects as startup projects:
   - Right-click solution → Properties → Multiple startup projects
   - Select `FoodOrder` (Blazor Hybrid) and `WebAPI` and set both to "Start"
2. Press F5 to run

**Option B: Using Command Line**
```bash
# Terminal 1 - Run API
cd WebAPI
dotnet run

# Terminal 2 - Run Blazor Hybrid App
cd FoodOrder
dotnet run
```

**Access the application**
- Blazor Hybrid App: Check console output for the exact URL (typically `https://localhost:7XXX`)
- API: `https://localhost:5XXX` (check console output for exact port)

## Database Schema

### MenuItem Table
```sql
CREATE TABLE menu_items (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    category NVARCHAR(50) NOT NULL,
    price DECIMAL(18,2) NOT NULL,
    is_available BIT NOT NULL DEFAULT 1
);
```

### Order Table
```sql
CREATE TABLE orders (
    id INT IDENTITY(1,1) PRIMARY KEY,
    total_amount DECIMAL(18,2) NOT NULL,
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
```

### OrderItem Table
```sql
CREATE TABLE order_items (
    id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT NOT NULL,
    menu_item_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (menu_item_id) REFERENCES menu_items(id)
);
```

## API Endpoints

### Menu Items
```
GET    /api/menuitems          # Get all menu items
GET    /api/menuitems/{id}     # Get menu item by ID
POST   /api/menuitems          # Create new menu item
PUT    /api/menuitems/{id}     # Update menu item
DELETE /api/menuitems/{id}     # Delete menu item
```

### Orders
```
GET    /api/orders             # Get all orders
GET    /api/orders/{id}        # Get order by ID
POST   /api/orders             # Create new order
```

## Configuration

### CORS Settings (WebAPI/Program.cs)
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        builder => builder
            .WithOrigins("https://localhost:7XXX") // Your Blazor Hybrid app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### HTTP Client (FoodOrder/Program.cs)
```csharp
// Blazor Hybrid uses HttpClientFactory
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:5XXX/");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>());
```

## Technologies Used

### Frontend
- **Blazor Hybrid Web App (.NET 9)** - Combines server-side and client-side rendering
- **Blazor Server** - Server-side rendering with SignalR
- **Blazor WebAssembly** - Client-side rendering (interactive components)
- **C#** - Programming language
- **Razor Components** - Component-based UI
- **HTML/CSS** - Markup and styling
- **JavaScript Interop** - For browser APIs

### Backend
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database

### Tools
- **Visual Studio 2022** - IDE
- **SQL Server Management Studio** - Database management
- **Postman** - API testing (optional)

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author
- GitHub: [@kvnmcn](https://github.com/kvnmcn)

---
