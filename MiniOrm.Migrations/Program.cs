

using MiniOrm.Migrations.Commands;

string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=softwaredev;Database=Miniorm";

var schemereader = new SchemaReader(connectionString);

var sqlGenerator = new SqlGenerator();

var migrationservice = new MigrationService(schemereader, sqlGenerator);

var runner = new MigrationRunner(connectionString);


if (args.Length < 2)
{
    Console.WriteLine("Invalid command");
    return;
}



if (args[0] == "migrations")
{
    switch (args[1])
    {
        case "add":

            if (args.Length < 3)
            {
                Console.WriteLine("Migration name missing");
                return;
            }

            migrationservice.AddMigration(args[2]);
            break;

        case "apply":
            runner.ApplyMigrations();
            break;

        case "list":
            runner.ListMigrations();
            break;

        case "rollback":
            runner.RollbackLastMigration();
            break;
    }
}
       