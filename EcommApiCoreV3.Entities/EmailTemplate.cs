using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class EmailTemplate
    {
        public int EmailTemplateID { get; set; } = 0;
        public string EmailType { get; set; }
        public string Template { get; set; }
    }
}
