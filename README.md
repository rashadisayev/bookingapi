# Booking API

A .NET Core Web API for finding available homes within specific date ranges. Users can search for homes that are available for their entire requested stay period.

## What This Does

This API helps users find homes that are available for booking during specific dates. It filters homes based on their availability slots and returns only those homes that are available for the complete date range requested.

## Project Structure

The solution follows a clean architecture approach with separate layers:

- **BookingApi**: The web API layer with controllers and HTTP handling
- **BookingApp**: Business logic, queries, and application services
- **BookingDomain**: Core entities and business rules
- **BookingInfra**: Data access and external service implementations
- **BookingTests**: Unit and integration tests

## Key Features

- Search homes by date range
- Pagination support for large result sets
- In-memory data storage (no database required)
- Async processing for better performance
- Comprehensive test coverage
- Input validation and error handling
- Swagger API documentation

## API Endpoint

### GET /api/AvailableHomes

Search for available homes within a date range.

**Parameters:**
- `startDate` (required): Start date (YYYY-MM-DD)
- `endDate` (required): End date (YYYY-MM-DD)
- `pageNumber` (optional): Page number, defaults to 1
- `pageSize` (optional): Items per page, defaults to 10

**Example:**
```
GET /api/AvailableHomes?startDate=2025-07-15&endDate=2025-07-17
```

**Response:**
```json
{
  "isSuccess": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "homeId": "123",
        "homeName": "Home 1",
        "availableSlots": [
          "2025-07-15",
          "2025-07-16",
          "2025-07-17"
        ]
      }
    ],
    "pageNumber": 1,
    "totalPages": 1,
    "totalCount": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  }
}
```

## Technologies

- .NET 8.0
- ASP.NET Core Web API
- MediatR (CQRS pattern)
- FluentValidation
- xUnit for testing
- FluentAssertions
- Swagger/OpenAPI

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git

## Getting Started

### 1. Clone and Setup
```bash
git clone <repository-url>
cd BookingApi
dotnet restore
```

### 2. Build and Run
```bash
dotnet build
cd src/BookingApi
dotnet run
```

The API will start at:
- API: https://localhost:7001
- Swagger: https://localhost:7001/swagger

### 3. Testing the API

**Using Swagger:**
1. Go to https://localhost:7001/swagger
2. Find the AvailableHomes endpoint
3. Click "Try it out" and enter your dates

**Using cURL:**
```bash
curl -X GET "https://localhost:7001/api/AvailableHomes?startDate=2025-07-15&endDate=2025-07-17"
```

**Using the HTTP file:**
The project includes a `BookingApi.http` file you can use with VS Code or similar tools.

## Running Tests

**All tests:**
```bash
dotnet test
```

**Just integration tests:**
```bash
dotnet test src/BookingTests/ --filter "AvailableHomesControllerIntegrationTests"
```

**Specific test:**
```bash
dotnet test --filter "GetAvailableHomes_WithValidDateRange_ReturnsCorrectHomes"
```

## How the Filtering Works

The API only returns homes that are available for every single day in your requested range. Here's how it works:

1. Takes your start and end dates
2. Creates a list of all dates in that range
3. Checks each home's availability
4. Only includes homes that have ALL the required dates available

## Performance Notes

- Uses HashSet for fast date lookups
- Async operations prevent blocking
- In-memory storage for speed
- Efficient LINQ filtering

## Error Handling

The API includes:
- Input validation for dates and parameters
- Global exception handling
- Proper HTTP status codes
- Consistent error response format

## Project Files

```
BookingApi/
├── src/
│   ├── BookingApi/           # Web API controllers
│   ├── BookingApp/           # Business logic and queries
│   ├── BookingDomain/        # Core entities
│   ├── BookingInfra/         # Data access
│   └── BookingTests/         # Tests
└── BookingApi.sln
```

## Development

This project uses:
- Clean Architecture principles
- CQRS pattern with MediatR
- Dependency injection
- Async/await throughout
- Comprehensive testing

