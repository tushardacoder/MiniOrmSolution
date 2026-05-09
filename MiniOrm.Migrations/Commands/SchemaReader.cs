using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Migrations.Commands
{
    public class SchemaReader
    {
        private readonly string _connectionString;

        public SchemaReader(string connectionString)
        {
            _connectionString = connectionString;
        }



        public Dictionary<string, HashSet<string>> ReadSchema()
        {
            var schema = new Dictionary<string, HashSet<string>>();

            using var connection = new NpgsqlConnection(_connectionString);

            connection.Open();

            string sql = @"
                SELECT table_name, column_name
                FROM information_schema.columns
                WHERE table_schema='public';
            ";

            using var command = new NpgsqlCommand(sql, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string table = reader.GetString(0);
                string column = reader.GetString(1);

                if (!schema.ContainsKey(table))
                {
                    schema[table] = new HashSet<string>();
                }

                schema[table].Add(column);
            }

            return schema;
        }
    }
}
