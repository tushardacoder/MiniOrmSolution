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


Technologies Used
Framework: .NET 10
Database: PostgreSQL
Data Provider: Npgsql
Language: C#
Reflection API
ADO.NET

PostgreSQL Setup
Install PostgreSQL

Download and install PostgreSQL from:

https://www.postgresql.org/download/

After installation:

Open pgAdmin or psql
Create a database
CREATE DATABASE miniormdb;


Environment Variable Setup

MiniOrm uses an environment variable named MINIORM_CONN for the PostgreSQL connection string.

Windows PowerShell
$env:MINIORM_CONN="Host=localhost;Port=5432;Database=miniormdb;Username=postgres;Password=yourpassword"
Windows CMD
set MINIORM_CONN=Host=localhost;Port=5432;Database=miniormdb;Username=postgres;Password=yourpassword
Linux / macOS
export MINIORM_CONN="Host=localhost;Port=5432;Database=miniormdb;Username=postgres;Password=yourpassword"





Future Improvements
LINQ Support
Relationship Mapping
Lazy Loading
Query Builder
Transactions
Change Tracking
Async Operations
Fluent Configuration API

