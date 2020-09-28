using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class MainCategoryJson
    {
        public int MaincategoryId { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public bool megaMenu { get; set; } = true;
        public bool active { get; set; } = true;
        public List<CategoryJson> children { get; set; }
    }
}
