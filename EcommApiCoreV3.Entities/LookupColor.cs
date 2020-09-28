using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class LookupColor
    {
        public int LookupColorId { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
    }
}
