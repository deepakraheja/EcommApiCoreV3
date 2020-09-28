using System;
using System.Collections.Generic;

namespace EcommApiCoreV3.Entities
{
    public class Category
    {

        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public int Modifiedby { get; set; }

        public string Name { get; set; } = "";
        public int SubCategoryID { get; set; } = 0;
        public int MainCategoryID { get; set; } = 0;
        public string MainCategoryName { get; set; } = "";
        public string Title { get; set; }
        public string Type { get; set; }
        public List<SubCategory> Children { get; set; }
    }
}
