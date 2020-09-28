using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class OrderStatusHistory
    {
        public int OrderStatusHistoryId { get; set; } = 0;
        public int OrderDetailsID { get; set; } = 0;
        public int OrderStatusId { get; set; } = 0;
        public string CreatedDate { get; set; } = "";
        public int CreatedBy { get; set; } = 0;
        public int OrderId { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public int SetNo { get; set; } = 0;
        public int TransportID { get; set; } = 0;
        public string DispatchDate { get; set; }
        public string Bilty { get; set; }
    }
}
