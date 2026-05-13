using MiniOrm.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data
{
    [Table("orders")]
    public class Order
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Column("customer_name")]
        public string CustomerName { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("is_paid")]
        public bool IsPaid { get; set; }
    }
}
