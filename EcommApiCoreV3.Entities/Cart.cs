using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Cart
    {
        public int CartId { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public int ProductSizeColorId { get; set; } = 0;
        public int ProductSizeId { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public string ProductName { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal SalePrice { get; set; } = 0; 
        public decimal Discount { get; set; } = 0;
        public decimal PerDiscount { get; set; } = 0;
        public int Qty { get; set; } = 0;
        public string FrontImage { get; set; } = "";
        public int ProductId { get; set; } = 0;
        public string[] ProductImg { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }
        public int SetNo { get; set; } = 0;
        public string RowID { get; set; }
        public int GSTRate { get; set; }
        public decimal AdditionalDiscount { get; set; }
        public decimal AdditionalDiscountAmount
        {
            get { return Convert.ToDecimal((SalePrice * Quantity) * AdditionalDiscount / 100); }
        }
        public double GSTAmount
        {
            get { return Convert.ToDouble((((SalePrice * Quantity)- AdditionalDiscountAmount) * GSTRate / 100).ToString("0.00")); }
        }
        public int TotalPieces { get; set; }
        public int SetType { get; set; } = 0;
        
    }
}
