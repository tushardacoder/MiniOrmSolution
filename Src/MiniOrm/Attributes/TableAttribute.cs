using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; }

        public TableAttribute(string name)
        {


            Name = name;
        }
    }
}
