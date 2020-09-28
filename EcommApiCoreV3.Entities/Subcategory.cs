using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class SubCategory
    {
        public int SubCategoryID { get; set; }

        public int CategoryID { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public int Modifiedby { get; set; }
        public string SideSubCategory { get; set; }
        public string SubCatCode { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        

    }
}

