using MiniOrm.Attributes;
using System;
using System.Collections.Generic;


using System.Text;

namespace MiniOrm.Data
{

    [Table("products")]
    public class Product
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("discount")]
        public decimal? Discount { get; set; }

        [Column("in_stock")]
        public bool InStock { get; set; }
    }
}


