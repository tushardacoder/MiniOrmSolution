using MiniOrm.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Models
{
    public class EntityMetadata
    {
        public string TableName { get; set; }
        public List<ColumnMetadata> Columns { get; set; } = new();
    }
}
