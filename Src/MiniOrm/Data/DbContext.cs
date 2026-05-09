using MiniOrm.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Models
{
    public class DbContext : IDisposable
    {

        private readonly string _connectionstring;

        public DbContext(string connection)
        {

            _connectionstring = connection;

        }


        public void Dispose()
        {

        }
    }











}
