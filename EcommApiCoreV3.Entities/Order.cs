using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Order : BillingAddress
    {
        public int OrderId { get; set; } = 0;
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public int PaymentTypeId { get; set; } = 0;
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; } = 0;
        public decimal ShippingCharge { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public string Notes { get; set; }
        public int StatusId { get; set; }
        public int UserID { get; set; } = 0;
        public int OrderDetailsID { get; set; } = 0;
        public int ProductSizeColorId { get; set; } = 0;
        public int ProductSizeId { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double SalePrice { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public List<Order> OrderDetails { get; set; } = new List<Order>();
        public string[] ProductImg { get; set; }
        public string ProductName { get; set; }
        public int SetNo { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TotalCustomers { get; set; } = 0;
        public int GSTRate { get; set; }
        public double GSTAmount { get; set; }
        public bool IsSelected { get; set; } = false;
        public string SelectedOrderDetailsIds { get; set; }
        public string StatusName { get; set; }
        public string GUID { get; set; } = "";
        public List<Users> ListUsers { get; set; } = new List<Users>();
    }
}
