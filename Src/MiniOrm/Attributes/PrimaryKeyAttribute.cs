using System;
using System.Collections.Generic;
using System.Text;

namespace MiniOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
