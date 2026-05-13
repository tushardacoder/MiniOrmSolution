# MiniOrm - Lightweight ORM with PostgreSQL Support

MiniOrm is a lightweight ORM built with C# and PostgreSQL that demonstrates how modern ORMs work internally. It includes entity mapping via reflection, attribute-based configuration, generic DbSet operations, migration generation, and a command-based migration CLI.

# Features

- Reflection-Based Entity Mapping  
  Automatically scans entity classes and builds metadata at runtime.

- Attribute Filtering  
  Configure tables, columns, primary keys, and ignored properties using custom attributes.

- PostgreSQL Type Mapping  
  Converts C# types into PostgreSQL-native database types.

- Generic DbSet Operations  
  Supports CRUD operations using generic repositories and DbSets.

- Command-Based Migration CLI  
  Generate and apply SQL migrations directly from entity metadata.

- Lightweight Architecture  
  Minimal dependencies with clean, understandable source code.

---


# Project Structure

```text
MiniOrmSolution/
├── MiniOrm/
│   ├── Dependencies/
│   │
│   ├── Attributes/
│   │   ├── ColumnAttribute.cs
│   │   ├── PrimaryKeyAttribute.cs
│   │   └── TableAttribute.cs
│   │
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   ├── ColumnMetadata.cs
│   │   ├── DbContext.cs
│   │   ├── DbSet.cs
│   │   ├── EntityMetadata.cs
│   │   └── TypeMapper.cs
│   │
│   ├── Model/
│   │   ├── Order.cs
│   │   └── Product.cs
│   │
│   ├── config.json
│   └── Program.cs
│
└── MiniOrm.Migrations/
    ├── Dependencies/
    │
    ├── Commands/
    │   ├── MigrationFile.cs
    │   ├── MigrationRunner.cs
    │   ├── MigrationService.cs
    │   ├── SchemaReader.cs
    │   └── SqlGenerator.cs
    │
    ├── Migrations/
    │   └── 20260512163549_InitialCreate.sql
    │
    ├── config.json
    └── Program.cs
```


## Technologies Used

- **Framework:** .NET 10  
- **Database:** PostgreSQL  
- **Data Provider:** Npgsql  
- **Language:** C#  
- **Core Concepts**  
  - Reflection API  
 

## PostgreSQL Setup

### Install PostgreSQL

Download and install PostgreSQL from:

https://www.postgresql.org/download/

### After Installation

1. Open **pgAdmin** or **psql**
2. Create a new database:

```sql
CREATE DATABASE miniormdb;

## Environment Variable Setup

MiniOrm uses an environment variable named `MINIORM_CONN` for the PostgreSQL connection string.

### Windows PowerShell

```powershell
$env:MINIORM_CONN="Host=localhost;Port=5432;Database=miniormdb;Username=postgres;Password=yourpassword"
```

### Windows CMD

```cmd
set MINIORM_CONN=Host=localhost;Port=5432;Database=miniormdb;Username=postgres;Password=yourpassword
```

### Linux / macOS

```bash
export MINIORM_CONN="Host=localhost;Port=5432;Database=miniormdb;Username=postgres;Password=yourpassword"
```

## Running Migrations

The migration CLI compares entity metadata with the current PostgreSQL schema and generates SQL migration files.

### Generate Migration

```bash
dotnet run -- migrations add<Name> 
```

This generates a timestamped SQL migration file.

### Example

```text
Migrations/
└── 20260513153000_InitialCreate.sql
```

### Apply Migrations

```bash
dotnet run -- migrations apply
```

This executes all pending SQL migration files against PostgreSQL.

### Migrations List

```bash
dotnet run -- migrations list

```
Prints all files .

### RollBack

```bash
dotnet run -- migrations
rollback
```
Reverts the last applied migration
using its -- down script.




## Running the Demo

The demo project shows the full developer workflow:

- Define entities  
- Configure attributes  
- Register DbSets  
- Run CRUD operations  
- Query data  
- Print results

## Run the Demo

```bash
dotnet run --project MiniOrm
```

### Example Output

```text
Database connected successfully

Creating product...
Product inserted

Fetching products...
1 - Laptop - 1200

Updating product...
Product updated

Deleting product...
Product deleted
```

## Attribute Filtering

MiniOrm uses custom attributes to configure entities.

### Table Attribute

Defines the PostgreSQL table name.

```csharp
[Table("products")]
public class Product
{
}
```

### Column Attribute

Overrides the default column name.

```csharp
[Column("product_name")]
public string Name { get; set; }
```

### PrimaryKey Attribute

Marks a property as the primary key.

```csharp
[PrimaryKey]
public int Id { get; set; }
```

### Ignore Attribute

Excludes a property from database mapping.

```csharp
[Ignore]
public string TempCalculation { get; set; }
```

Ignored properties:

- Are not included in migrations  
- Are not inserted into PostgreSQL  
- Are not selected from queries  

---

## PostgreSQL Type Mapping

MiniOrm automatically maps C# types to PostgreSQL-native types.


### Value Types — Non-Nullable

| C# Type | PostgreSQL Type | Nullability |
|----------|------------------|-------------|
| `int` *(PrimaryKey)* | `SERIAL PRIMARY KEY` | NOT NULL |
| `int` | `INTEGER` | NOT NULL |
| `long` | `BIGINT` | NOT NULL |
| `float` | `REAL` | NOT NULL |
| `double` | `DOUBLE PRECISION` | NOT NULL |
| `decimal` | `NUMERIC` | NOT NULL |
| `bool` | `BOOLEAN` | NOT NULL |
| `DateTime` | `TIMESTAMP` | NOT NULL |
| `Guid` | `UUID` | NOT NULL |

---

### Nullable Value Types — `T?`

| C# Type | PostgreSQL Type | Nullability |
|----------|------------------|-------------|
| `int?` | `INTEGER` | NULL |
| `long?` | `BIGINT` | NULL |
| `float?` | `REAL` | NULL |
| `double?` | `DOUBLE PRECISION` | NULL |
| `decimal?` | `NUMERIC` | NULL |
| `bool?` | `BOOLEAN` | NULL |
| `DateTime?` | `TIMESTAMP` | NULL |
| `Guid?` | `UUID` | NULL |

---

### Reference Types

| C# Type | PostgreSQL Type | Nullability |
|----------|------------------|-------------|
| `string` | `TEXT` | NOT NULL |
| `string?` | `TEXT` | NULL |
---

## Example Entity

```csharp
[Table("products")]
public class Product
{
    [PrimaryKey]
    public int Id { get; set; }

    [Column("product_name")]
    public string Name { get; set; }

    public decimal Price { get; set; }

    [Ignore]
    public string TemporaryValue { get; set; }
}
```

---

## Reflection-Based Entity Mapping

MiniOrm scans entities using reflection:

```csharp
typeof(Product).GetProperties()
```

For each property it:

- Checks custom attributes  
- Determines PostgreSQL type  
- Builds metadata  
- Generates SQL dynamically  

### Generated Metadata Example

```text
Entity: products

Columns:
- Id → INTEGER PRIMARY KEY
- product_name → TEXT
- Price → NUMERIC
```

Ignored properties are skipped automatically.

---

## Example Migration SQL

Generated SQL:

```sql
CREATE TABLE products (
    id INTEGER PRIMARY KEY,
    product_name TEXT NOT NULL,
    price NUMERIC NOT NULL
);
```

---

## CRUD Example

```csharp
var product = new Product
{
     Name = "Keyboard",
     Price = 89.99m,
     Discount = 10,
     InStock = true
};

#insert
db.Products.Insert(product);
#get by Id
db.Products.FindById(id);
#get all products
var allProducts = db.Products.GetAll();
#update product
product.Price = 79.99m;
product.Discount = 5;
db.Products.Update(product);
#delete product
db.Products.Delete(product.Id);
```





## Future Improvements

- LINQ Support  
- Relationship Mapping  
- Lazy Loading  
- Query Builder  
- Transactions  
- Change Tracking  
- Async Operations  
- Fluent Configuration API  
