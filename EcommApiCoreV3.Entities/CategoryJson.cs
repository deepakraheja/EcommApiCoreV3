using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class CategoryJson
    {
        public int CategoryId { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        
        public bool active { get; set; } = true;
        public List<SubCategoryJson> children { get; set; }
    }
}
