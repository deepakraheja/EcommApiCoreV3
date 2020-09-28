using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Fabric
    {
        public int FabricId { get; set; } = 0;
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
        public int CreatedBy { get; set; } = 0;
        public string ModifiedDate { get; set; }
        public int ModifiedBy { get; set; } = 0;
    }
}
