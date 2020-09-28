using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class MainCategory
    {
        public int MaincategoryId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool MegaMenu { get; set; } = true;
        public bool Active { get; set; } = true;
      
        public string MainCategoryName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; } = 0;
    }
}
