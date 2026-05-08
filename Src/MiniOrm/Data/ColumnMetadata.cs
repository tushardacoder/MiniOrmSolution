using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Data
{
    public class ColumnMetadata
    {
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public string PostgreSqlType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}

