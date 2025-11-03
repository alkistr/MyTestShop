# MyTestShop - Database Setup

This document describes how to set up and manage the MS SQL Server database for the MyTestShop application.

## Prerequisites

- Docker and Docker Compose installed
- .NET 9 SDK
- Entity Framework Core tools (`dotnet tool install --global dotnet-ef`)

## Database Setup

### 1. Start SQL Server Container

```bash
# Start the SQL Server container
sudo docker-compose up -d

# Or use the management script
./manage-db.sh start
```

### 2. Apply Database Migrations

```bash
# From the solution root directory
cd MyTestShop
dotnet ef database update --project ../MyTestShop.Infrastructure --startup-project .

# Or use the management script
./manage-db.sh migrate
```

## Container Management

The `manage-db.sh` script provides easy database management:

```bash
./manage-db.sh start      # Start the SQL Server container
./manage-db.sh stop       # Stop the SQL Server container  
./manage-db.sh restart    # Restart the SQL Server container
./manage-db.sh logs       # Show SQL Server logs (follow mode)
./manage-db.sh status     # Show container status
./manage-db.sh migrate    # Run database migrations
./manage-db.sh connect    # Connect to SQL Server using sqlcmd
```

## Database Configuration

### Connection String
The application uses the following connection string (configured in `appsettings.Development.json`):

```
Server=localhost;Database=MyTestShopDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;
```

### Docker Compose Configuration
- **Container Name**: `mytestshop-sqlserver`
- **SQL Server Version**: 2022-latest (Express Edition)
- **Port**: 1433 (mapped to host)
- **SA Password**: `Your_password123`
- **Database**: `MyTestShopDb`

## Database Schema

The application includes the following entities:

- **Customers**: Customer information (Id, FirstName, LastName, Address, PostalCode)
- **Products**: Product catalog (Id, Name, Price)
- **Orders**: Customer orders (Id, OrderDate, Cancelled, TotalPrice, CustomerId)
- **Items**: Order line items (Id, ProductId, Quantity, OrderId)

## Entity Framework Migrations

### Creating a New Migration

```bash
cd MyTestShop
dotnet ef migrations add <MigrationName> --project ../MyTestShop.Infrastructure --startup-project .
```

### Applying Migrations

```bash
cd MyTestShop
dotnet ef database update --project ../MyTestShop.Infrastructure --startup-project .
```

### Removing the Last Migration

```bash
cd MyTestShop
dotnet ef migrations remove --project ../MyTestShop.Infrastructure --startup-project .
```

## Troubleshooting

### Container Issues
- Check container status: `sudo docker ps`
- View logs: `sudo docker logs mytestshop-sqlserver`
- Restart container: `sudo docker-compose restart`

### Migration Issues
- Ensure SQL Server container is running and healthy
- Check connection string in `appsettings.Development.json`
- Verify EF Core tools are installed: `dotnet ef --version`

### Connection Issues
- Verify container is listening on port 1433: `sudo docker port mytestshop-sqlserver`
- Test connectivity: `telnet localhost 1433`
- Check firewall settings if running on a different host

## Development Workflow

1. Start the SQL Server container: `./manage-db.sh start`
2. Make changes to entity classes
3. Create migration: `dotnet ef migrations add <Name> --project ../MyTestShop.Infrastructure --startup-project .`
4. Apply migration: `./manage-db.sh migrate`
5. Test your application: `dotnet run`

## Production Considerations

- Change the SA password to a secure value
- Use SQL Server Standard/Enterprise edition for production
- Configure backup strategies
- Set up proper monitoring and logging
- Use Azure SQL Database or managed SQL Server instances for cloud deployments