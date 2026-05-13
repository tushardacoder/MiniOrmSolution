using MiniOrm.Data;
using MiniOrm.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Migrations.Commands
{
    public class SqlGenerator
    {
        public string GenerateCreateTable(EntityMetadata entity)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"CREATE TABLE IF NOT EXISTS {entity.TableName} (");

            for (int i = 0; i < entity.Columns.Count; i++)
            {
                var col = entity.Columns[i];

                sb.Append($"    {col.ColumnName} {col.PostgreSqlType}");

                if (col.IsPrimaryKey)
                {
                    sb.Append(" PRIMARY KEY");
                }

                sb.Append(col.IsNullable ? " NULL" : " NOT NULL");

                if (i < entity.Columns.Count - 1)
                {
                    sb.Append(",");
                }

                sb.AppendLine();
            }

            sb.AppendLine(");");

            return sb.ToString();
        }

        public string GenerateDropTable(string tableName)
        {
            return $"DROP TABLE IF EXISTS {tableName};";
        }

        public string GenerateAddColumn(string tableName, ColumnMetadata col)
        {
            return $@"
              ALTER TABLE {tableName}
             ADD COLUMN {col.ColumnName} {col.PostgreSqlType}
             {(col.IsNullable ? "NULL" : "NOT NULL")};
              ";
        }

        public string GenerateDropColumn(string tableName, string columnName)
        {
            return $@"
           ALTER TABLE {tableName}
           DROP COLUMN IF EXISTS {columnName};
            ";
        }
    }
}



