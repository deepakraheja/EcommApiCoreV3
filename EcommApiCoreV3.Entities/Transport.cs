using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Transport
    {
        public int TransportID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public int CreatedBy { get; set; } = 0;
        public string CreatedDate { get; set; } = "";
        public bool Active { get; set; } = false;
    }
}
