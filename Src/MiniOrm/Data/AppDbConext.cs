using MiniOrm.Models;
using System;
using System.Collections.Generic;
using System.Text;

using MiniOrm.Data;


namespace MiniOrm.Data
{
    public class AppDbConext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public AppDbConext(string connStr) : base(connStr)
        {



        }
    }
}
