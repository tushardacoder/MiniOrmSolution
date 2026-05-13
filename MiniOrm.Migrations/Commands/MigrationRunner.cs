using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Migrations.Commands
{
    public class MigrationRunner
    {
        private readonly string _connectionString;

        public MigrationRunner(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void EnsureMigrationTable()
        {
            using var connection =
                new NpgsqlConnection(_connectionString);

            connection.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS __migrations (
                    id SERIAL PRIMARY KEY,
                    name TEXT NOT NULL UNIQUE,
                    applied_at TIMESTAMP NOT NULL
                );
            ";

            using var cmd =
                new NpgsqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
        }

        public List<string> GetAppliedMigrations()
        {
            var list = new List<string>();

            using var connection =
                new NpgsqlConnection(_connectionString);

            connection.Open();

            string sql = "SELECT name FROM __migrations";

            using var cmd =
                new NpgsqlCommand(sql, connection);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }

            return list;
        }

        public void ApplyMigrations()
        {
            EnsureMigrationTable();

            var applied = GetAppliedMigrations();

            var files = Directory.GetFiles("Migrations", "*.sql")
                .OrderBy(x => x);

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            foreach (var file in files)
            {
                var name = Path.GetFileName(file);

                if (applied.Contains(name))
                    continue;

                var text = File.ReadAllText(file);

                var upIndex = text.IndexOf("-- up");
                var downIndex = text.IndexOf("-- down");

                if (upIndex == -1 || downIndex == -1)
                {
                    throw new Exception($"Invalid migration format in {name}");
                }

                var upStart = upIndex + "-- up".Length;

                string upSql = text.Substring(
                    upStart,
                    downIndex - upStart
                ).Trim();

                using var cmd = new NpgsqlCommand(upSql, connection);
                cmd.ExecuteNonQuery();

                const string insert = @"
        INSERT INTO __migrations(name, applied_at)
        VALUES(@name, NOW());
    ";

                using var insertCmd = new NpgsqlCommand(insert, connection);
                insertCmd.Parameters.AddWithValue("name", name);
                insertCmd.ExecuteNonQuery();

                Console.WriteLine($"Applied: {name}");
            }

        }

        public void ListMigrations()
        {
            EnsureMigrationTable();

            var applied = GetAppliedMigrations();

            var files = Directory.GetFiles("Migrations", "*.sql")
                .OrderBy(x => x);

            foreach (var file in files)
            {
                string name = Path.GetFileName(file);

                if (applied.Contains(name))
                {
                    Console.WriteLine($"{name} [applied]");
                }
                else
                {
                    Console.WriteLine($"{name} [pending]");
                }
            }
        }

        public void RollbackLastMigration()
        {
            EnsureMigrationTable();

            using var connection =
                new NpgsqlConnection(_connectionString);

            connection.Open();

            string sql = @"
                SELECT name
                FROM __migrations
                ORDER BY applied_at DESC
                LIMIT 1;
            ";

            using var cmd =
                new NpgsqlCommand(sql, connection);

            var result = cmd.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("No migrations to rollback.");
                return;
            }

            string migrationName = result.ToString();

            string path =
                Path.Combine("Migrations", migrationName);

            string text = File.ReadAllText(path);

            int downIndex = text.IndexOf("-- down");

            string downSql =
                text.Substring(downIndex + 8);

            using var rollbackCmd =
                new NpgsqlCommand(downSql, connection);

            rollbackCmd.ExecuteNonQuery();

            string deleteSql =
                "DELETE FROM __migrations WHERE name=@name";

            using var deleteCmd =
                new NpgsqlCommand(deleteSql, connection);

            deleteCmd.Parameters.AddWithValue(
                "name",
                migrationName);

            deleteCmd.ExecuteNonQuery();

            Console.WriteLine($"Rolled back: {migrationName}");
        }
    }
}


