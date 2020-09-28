using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class LookupFabricType
    {
        public int FabricTypeId { get; set; } = 0;
        public int FabricId { get; set; } = 0;
        public string FabricType { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; } = false;
        public int CreatedBy { get; set; } = 0;
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; } = 0;
        public string ModifiedDate { get; set; }
    }
}
