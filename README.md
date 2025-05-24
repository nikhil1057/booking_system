# BookingSystem API

This is a layered .NET 7 Web API for managing inventory bookings by members. It supports CSV imports, booking/cancellation operations, and includes both REST and gRPC endpoints.

## Features

- Member and inventory management
- Booking creation and cancellation
- CSV file import for members and inventory
- Maximum booking limits per member
- Both REST API and gRPC support
- Comprehensive unit testing
- Entity Framework Core with SQL support
- Clean Architecture with CQRS pattern using MediatR
- AutoMapper for object mapping

### Prerequisites

- .NET 7 SDK
- SQL Server

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Update connection strings in `appsettings.json`
4. CREATE DATABASE [BookingSystem];
5. Run the application:


### Database Setup

The application will automatically create and migrate the database on startup in development mode.

For production, run migrations manually:

```bash
dotnet ef database update --project src/BookingSystem.Infrastructure --startup-project src/BookingSystem.API
```

## Script

If Migrations do not run, then Run the following SQL script in **SQL Server** to create the **BookingSystem** database and all required tables:

```sql
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'BookingSystem')
BEGIN
    CREATE DATABASE [BookingSystem];
END
GO

USE [BookingSystem];
GO

IF OBJECT_ID('dbo.Members') IS NULL
BEGIN
    CREATE TABLE [dbo].[Members](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FirstName] NVARCHAR(100) NOT NULL,
        [LastName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(255) NOT NULL,
        [DateJoined] DATETIME2 NOT NULL,
        [BookingCount] INT NOT NULL DEFAULT 0
    );
END
GO

IF OBJECT_ID('dbo.InventoryItems') IS NULL
BEGIN
    CREATE TABLE [dbo].[InventoryItems](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        [Description] NVARCHAR(1000) NOT NULL,
        [RemainingCount] INT NOT NULL,
        [ExpirationDate] DATETIME2 NOT NULL DEFAULT ('0001-01-01T00:00:00.0000000')
    );
END
GO

IF OBJECT_ID('dbo.Bookings') IS NULL
BEGIN
    CREATE TABLE [dbo].[Bookings](
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ReferenceNumber] NVARCHAR(50) NOT NULL,
        [BookingDate] DATETIME2 NOT NULL,
        [IsCancelled] BIT NOT NULL DEFAULT 0,
        [CancellationDate] DATETIME2 NULL,
        [MemberId] INT NOT NULL,
        [InventoryItemId] INT NOT NULL,
        FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Members]([Id]),
        FOREIGN KEY ([InventoryItemId]) REFERENCES [dbo].[InventoryItems]([Id])
    );
END
GO
```

# API Endpoints
**Bookings**
	POST /api/book
	Book a new item.
	Input: email, inventoryName

	POST /api/cancel
	Cancel a booking.
	Input: referenceNumber

**Inventory**
	GET /api/inventory
	Returns all inventory items.

	POST /api/inventory/import
	Upload CSV to import inventory data.

**Members**
	GET /api/members
	Returns all members.

	GET /api/members/{email}/bookings
	Get all bookings for a member using email.

	POST /api/members/import
	Upload CSV to import member data.

# Note: Member names may be duplicated in the CSV. A unique email is generated automatically for each member.
# Use the GET /api/members endpoint to retrieve the email to use in bookings.

# Setup Instructions
Clone or download the code

Open in Visual Studio

Run the SQL script above in SQL Server to create DB and tables

Add your connection string to appsettings.json

Build and run the project

You're ready to go!

# Tech Stack
ASP.NET Core 6+

Entity Framework Core

CQRS with MediatR

AutoMapper

gRPC

SQL Server

xUnit (Testing)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request
