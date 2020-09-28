using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Supplier
    {
        public int SupplierID { get; set; } = 0;
        public string CompanyName { get; set; }
        public string Name { get; set; } = "";
        public string Designation { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; } = "";
        public int CreatedBy { get; set; } = 0;
        public string CreatedDate { get; set; } = "";
        public int Modifiedby { get; set; } = 0;
        public string ModifiedDate { get; set; } = "";
        public bool Active { get; set; } = false;
    }
}
