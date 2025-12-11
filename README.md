# ISEMESAPI

A unified .NET 8 Web API that consolidates the functionality of InventoryApi and Ticketing System API into a single comprehensive application.

## Project Structure

```
ISEMESAPI/
├── ISEMESAPI.sln
├── Dockerfile
├── azure-pipelines.yml
├── log4net.config
├── README.md
└── src/
    ├── ISEMES.API/              # Web API project
    │   ├── Controllers/         # API Controllers
    │   ├── Services/            # API-level services (TokenProvider)
    │   ├── Program.cs           # Application entry point
    │   └── appsettings.json     # Configuration
    │
    ├── ISEMES.Models/           # Shared data models
    │
    ├── ISEMES.Repositories/     # Data access layer
    │   ├── AppDbContext.cs      # Main database context
    │   ├── TFSDbContext.cs      # TFS database context
    │   └── *Repository.cs       # Repository implementations
    │
    └── ISEMES.Services/         # Business logic layer
        └── *Service.cs          # Service implementations
```

## Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

## Getting Started

### 1. Clone the repository
```bash
git clone <repository-url>
cd ISEMESAPI
```

### 2. Update connection strings
Edit `src/ISEMES.API/appsettings.Development.json` with your database connection strings:

```json
"ConnectionStrings": {
    "InventoryConnection": "Server=YOUR_SERVER;Database=Inventory_Dev;...",
    "InventoryTFS_Prod2Connection": "Server=YOUR_SERVER;Database=TFS_Prod2;..."
}
```

### 3. Build and run
```bash
dotnet restore
dotnet build
dotnet run --project src/ISEMES.API
```

### 4. Access Swagger UI
Navigate to `https://localhost:5001/swagger` to view the API documentation.

## API Endpoints

All endpoints are prefixed with `/api/v1/ise/inventory/`

### Authentication
- `POST /login` - Authenticate user (supports both internal Azure AD and external users)

### Receiving
- Various endpoints for receipt management, device details, hardware, etc.

### Shipment
- Endpoints for shipment creation, tracking, and management

### Inventory
- Check-in/check-out, holds, moves, and reports

### Tickets
- Ticket creation, search, and management

### Split/Merge
- Lot search, merge operations, and split management

## Technology Stack

- **Framework**: .NET 8.0
- **Authentication**: JWT Bearer + Azure AD
- **Database**: SQL Server with Entity Framework Core
- **Documentation**: Swagger/OpenAPI
- **Logging**: log4net
- **Shipping**: Shippo SDK
- **Storage**: Azure Blob Storage

## Configuration

Key configuration sections in `appsettings.json`:

- `AzureAd` - Azure Active Directory settings
- `Jwt` - JWT token configuration
- `ShippoSDK` - Shipping SDK settings
- `BlobStorage` - Azure Blob Storage configuration
- `ConnectionStrings` - Database connections

## Docker Support

Build and run with Docker:

```bash
docker build -t isemesapi .
docker run -p 8080:8080 isemesapi
```

## CI/CD

Azure DevOps pipeline is configured in `azure-pipelines.yml` for automated builds and deployments.

## Migration Notes

This API is a consolidation of:
- **InventoryApi** - Original inventory management API
- **Ticketing System API** - Ticketing and workflow API

Both APIs shared the same codebase structure and were merged with unified namespaces under `ISEMES.*`.

## License

Proprietary - All rights reserved.
