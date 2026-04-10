# Vehicle Inventory Microservice (DDD + Clean Architecture)

This project is the **Inventory** part of a car rental platform.  
Its job is to manage vehicles at pickup/return locations, track whether they’re available, and enforce the rules around a vehicle’s lifecycle — using **Domain-Driven Design (DDD)** and **Clean Architecture**.

---

## What this service is responsible for (Inventory Context)

**In scope**
- Manage vehicles at specific locations
- Track vehicle status: **Available, Reserved, Rented, Serviced**
- Enforce vehicle lifecycle rules in the **Domain** layer
- Expose inventory data through a **REST API**

**Out of scope (handled by other services)**
- Reservations
- Customers
- Payments
- Loyalty / rewards

---

## Project Structure (Clean Architecture)


### 1) Domain Layer (`VehicleInventory.Domain`)

Contains:
- Entity: `Vehicle`
- Enum: `VehicleStatus`
- Exception: `DomainException`

**Rules this layer follows**
- No EF Core
- No ASP.NET
- No controllers

---

### 2) Application Layer (`VehicleInventory.Application`)

Contains:
- DTOs: `CreateVehicleRequest`, `UpdateVehicleStatusRequest`, `VehicleDto`
- Repository contract: `IVehicleRepository`
- Use case service: `VehicleService`
- Validators (FluentValidation)

**Important rule**
- The Application layer does **not** set `Status` directly.  
  It calls domain behavior like `MarkRented()`, `MarkReserved()`, etc.

**Rules this layer follows**
- No EF Core
- No ASP.NET

---

### 3) Infrastructure Layer (`VehicleInventory.Infrastructure`)
EF Core is implemented here, along with migrations.

Contains:
- EF Core DbContext: `InventoryDbContext`
- Repository implementation: `VehicleRepository`
- EF Core migrations

**Rules this layer follows**
- No business rules enforced here
- Depends on Application (and Domain for entity mapping)

---

### 4) WebAPI Layer (`VehicleInventory.WebAPI`) 
It wires dependency injection, exposes endpoints, and translates errors into HTTP responses.

Contains:
- `VehiclesController`
- Swagger setup
- Dependency Injection configuration
- Exception middleware: maps `DomainException` → **HTTP 409 Conflict**

- Controllers delegate to Application services
- No EF Core logic in controllers
- No domain rules in controllers

---

## Domain Model (DDD)

### Vehicle Aggregate
A `Vehicle` represents a single rentable car in inventory.

**Properties**
- `Id`
- `VehicleCode`
- `LocationId`
- `VehicleType`
- `Status`

**Domain methods**
- `MarkAvailable()`
- `MarkRented()`
- `MarkReserved()`
- `MarkServiced()`
- `ReleaseReservation()` 

---

## Business Rules (Enforced inside `Vehicle`)
These rules are enforced inside the Domain layer (not in controllers, not in EF).

- A vehicle **cannot be rented** if it is already rented
- A vehicle **cannot be rented** if it is reserved
- A vehicle **cannot be rented** if it is under service
- A reserved vehicle **cannot be marked available** unless `ReleaseReservation()` is called first
- Invalid transitions throw a `DomainException`

---

## API Contract (Contract-First)

### Base Route
`/api/vehicles`

### Endpoints

#### 1) GET `/api/vehicles`
Returns all vehicles.

**Response: 200 OK**
```json
[
  {
    "id": "guid",
    "vehicleCode": "VH-1001",
    "locationId": "guid",
    "vehicleType": "Sedan",
    "status": 0
  }
]
