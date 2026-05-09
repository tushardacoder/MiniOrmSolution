using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Migrations.Commands
{
    public class MigrationFile
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string UpSql { get; set; }
        public string DownSql { get; set; }
    }
}
