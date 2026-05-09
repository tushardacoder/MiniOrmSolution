using MiniOrm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MiniOrm.Migrations.Commands
{
    public class MigrationService
    {
        private readonly SchemaReader _schemaReader;
        private readonly SqlGenerator _sqlGenerator;

        public MigrationService(
            SchemaReader schemaReader,
            SqlGenerator sqlGenerator)
        {
            _schemaReader = schemaReader;
            _sqlGenerator = sqlGenerator;
        }

        public void AddMigration(string name)
        {
            var schema = _schemaReader.ReadSchema();

            var assembly = Assembly.Load("MiniOrm");

            var entityTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes()
                .Any(a => a.GetType().Name == "TableAttribute"))
                .ToList();

            var upBuilder = new StringBuilder();
            var downBuilder = new StringBuilder();

            foreach (var type in entityTypes)
            {
                var entity = TypeMapper.MapEntity(type);


                // CREATE TABLE
                if (!schema.ContainsKey(entity.TableName))
                {
                    upBuilder.AppendLine(
                        _sqlGenerator.GenerateCreateTable(entity));

                    downBuilder.AppendLine(
                        _sqlGenerator.GenerateDropTable(entity.TableName));

                    continue;
                }

                // ADD COLUMN
                var dbColumns = schema[entity.TableName];

                foreach (var column in entity.Columns)
                {
                    if (!dbColumns.Contains(column.ColumnName))
                    {
                        upBuilder.AppendLine(
                            _sqlGenerator.GenerateAddColumn(
                                entity.TableName,
                                column));

                        downBuilder.AppendLine(
                            _sqlGenerator.GenerateDropColumn(
                                entity.TableName,
                                column.ColumnName));
                    }
                }
            }

            string timestamp =
                DateTime.Now.ToString("yyyyMMddHHmmss");

            string fileName =
                $"{timestamp}_{name}.sql";

            Directory.CreateDirectory("Migrations");

            string path =
                Path.Combine("Migrations", fileName);

            var finalSql = new StringBuilder();

            finalSql.AppendLine("-- up");
            finalSql.AppendLine(upBuilder.ToString());

            finalSql.AppendLine("-- down");
            finalSql.AppendLine(downBuilder.ToString());

            File.WriteAllText(path, finalSql.ToString());

            Console.WriteLine(
                $"Migration created: {fileName}");
        }
    }
}