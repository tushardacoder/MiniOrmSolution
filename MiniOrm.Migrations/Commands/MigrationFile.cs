using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Migrations.Commands
{
    public class MigrationFile
    {
        public required string Name { get; set; }
        public required string FilePath { get; set; }
        public required string UpSql { get; set; }
        public required string DownSql { get; set; }
    }
}
