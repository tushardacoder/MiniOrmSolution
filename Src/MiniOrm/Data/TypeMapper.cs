using MiniOrm.Attributes;
using MiniOrm.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MiniOrm.Models
{
    public class TypeMapper
    {
        public static EntityMetadata GetTableName(Type type)
        {
            var tableAttr = type.GetCustomAttribute<TableAttribute>();

            if (tableAttr == null)
            {
                throw new Exception($"Type {type.Name} does not have [Table] attribute.");
            }

            var entitymetadata = new EntityMetadata
            {

                TableName = tableAttr.Name

            };


            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                var primaryKeyAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();

                if (columnAttr == null && primaryKeyAttr == null) continue;

                bool isPrimaryKey = primaryKeyAttr != null;

                Type propertyType = property.PropertyType;

                bool isNullable = false;

                // Detect nullable value types (int?, Guid?, etc.)
                if (propertyType.IsValueType)
                {
                    Type underLayingType = Nullable.GetUnderlyingType(propertyType);

                    if (underLayingType != null)
                    {
                        isNullable = true;
                        propertyType = underLayingType;
                    }

                }

                // Detect nullable reference type string?

                if (!propertyType.IsValueType)
                {
                    isNullable = NullabilityInfo(property);

                }


                string postGraceType = GetPostgraceSql(propertyType, isPrimaryKey);



                entitymetadata.Columns.Add(new ColumnMetadata
                {

                    PropertyName = property.Name,
                    ColumnName = columnAttr?.Name ?? property.Name,
                    PostgreSqlType = postGraceType,
                    IsNullable = isNullable,
                    IsPrimaryKey = isPrimaryKey




                });



            }

            return entitymetadata;

        }


        public static string GetPostgraceSql(Type type, bool isPrimaryKey)
        {

            if (type == typeof(int) && isPrimaryKey) return "SERIAL";


            if (type == typeof(int)) return "INTEGER";

            if (type == typeof(long)) return "BIGINT";

            if (type == typeof(float)) return "REAL";

            if (type == typeof(double)) return "DOUBLE PRECISION";

            if (type == typeof(decimal)) return "NUMERIC";

            if (type == typeof(bool)) return "BOOLEAN";

            if (type == typeof(DateTime)) return "TIMESTAMP";

            if (type == typeof(Guid)) return "UUID";


            if (type == typeof(string)) return "TEXT";







            throw new Exception($"Unsupported data type {type.Name} ");




        }


        // Detect nullable reference types (string?)
        private static bool NullabilityInfo(PropertyInfo property)
        {
            var nullabitycontext = new NullabilityInfoContext();

            var nullability = nullabitycontext.Create(property);

            return nullability.WriteState == NullabilityState.Nullable;
        }



    }
}


