using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Enumerations
{
    public enum ProductTypeEnum
    {
        [Description("Books")]
        Books = 1,
        [Description("Electronics")]
        Electronics = 2,
        [Description("Food")]
        Food = 3,
        [Description("Furniture")]
        Furniture = 4,
        [Description("Toys")]
        Toys = 5,
    }
}
