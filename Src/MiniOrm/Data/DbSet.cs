using MiniOrm.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MiniOrm.Models
{
    public class DbSet<T> where T: class,new()
    {
        private readonly string _connectionString;
        private readonly EntityMetadata _metadata;

        public DbSet(string connectionString)
        {
               _connectionString = connectionString;
               _metadata= TypeMapper.MapEntity(typeof(T));
        }


        public int Insert(T entity)
        {

             var columns=_metadata.Columns.Where(c=>!c.IsPrimaryKey).ToList();

            string columnNames = string.Join(",",
                 columns.Select(c => c.ColumnName));


            string parameterNames = string.Join(",",
                    columns.Select(c => "@"+c.ColumnName));


            string sql = $@"
                 INSERT INTO {_metadata.TableName}
                ({columnNames})
                VALUES ({parameterNames})
                 RETURNING id";

            using var connection =
       new NpgsqlConnection(_connectionString);


            connection.Open();

            using var command =   new NpgsqlCommand(sql, connection);

            foreach(var column in columns)
            {
                PropertyInfo property = typeof(T).GetProperty(column.PropertyName)!;
                object? value = property.GetValue(entity);

                command.Parameters.AddWithValue(

                          "@" + column.ColumnName,
                           value ?? DBNull.Value

                    );

            }
            




            return Convert.ToInt32(command.ExecuteScalar());

        }



        public T? FindById(int id)
        {
            var pk = _metadata.Columns.First(c => c.IsPrimaryKey);

            string sql = $@"
             SELECT *
             FROM {_metadata.TableName}
             WHERE {pk.ColumnName} = @id";

            using var connection =
             new NpgsqlConnection(_connectionString);

            connection.Open();


            using var command =
      new NpgsqlCommand(sql, connection);

            // Prevents SQL injection
            command.Parameters.AddWithValue("@id", id);


            using var reader = command.ExecuteReader();


            if (!reader.Read())
                return null;


            T entity = new T();

            foreach (var column in _metadata.Columns)
            {
                PropertyInfo property = typeof(T).GetProperty(column.PropertyName)!;
                object value = reader[column.ColumnName];

                if (value == DBNull.Value)
                {
                    property.SetValue(entity, null);
                }


                else
                {
                    Type targetType = Nullable.GetUnderlyingType(property.PropertyType)?? property.PropertyType;

                    object safevalue = Convert.ChangeType(value, targetType);

                    property.SetValue(entity, safevalue);
                }



            }




            return entity;





        }




        public IEnumerable<T> GetAll()
        {
            List<T> list = new List<T>();

            string sql = $@"SELECT * FROM {_metadata.TableName}";

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var command = new NpgsqlCommand(sql, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T entity = new T();

       
                foreach (var column in _metadata.Columns)
                {
               
                    PropertyInfo property =
                        typeof(T).GetProperty(column.PropertyName)!;

                
                    object value = reader[column.ColumnName];

                   
                    if (value == DBNull.Value)
                    {
                        property.SetValue(entity, null);
                    }
                    else
                    {
                       
                        Type targetType =
                            Nullable.GetUnderlyingType(property.PropertyType)
                            ?? property.PropertyType;

                        object safeValue =
                            Convert.ChangeType(value, targetType);

                        
                        property.SetValue(entity, safeValue);
                    }
                }

                list.Add(entity);
            }

            return list;
        }


        public void Update(T entity)
        {

            var pk = _metadata.Columns.First(c => c.IsPrimaryKey);

            var columns = _metadata.Columns
                          .Where(c => !c.IsPrimaryKey)
                          .ToList();

            string setClause = string.Join(", ",
                             columns.Select(c => $"{c.ColumnName} = @{c.ColumnName}"));

            string sql = $@"
           UPDATE {_metadata.TableName}
           SET {setClause}
           WHERE {pk.ColumnName} = @id";


            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var command = new NpgsqlCommand(sql, connection);

            foreach (var column in columns)
            {

                PropertyInfo property = typeof(T).GetProperty(column.PropertyName)!;

                object? value = property.GetValue(entity);

                command.Parameters.AddWithValue(
                      "@" + column.ColumnName,
                       value ?? DBNull.Value);




            }


            PropertyInfo pkProperty = typeof(T).GetProperty(pk.PropertyName)!;
            object pkValue = pkProperty.GetValue(entity)!;

            command.Parameters.AddWithValue("@id", pkValue);

            command.ExecuteNonQuery();



        }


        public void Delete(int id)
        {
            var pk = _metadata.Columns.First(c => c.IsPrimaryKey);

            string sql = $@"
            DELETE FROM {_metadata.TableName}
             WHERE {pk.ColumnName} = @id";


            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();


            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }





    }
    
}
