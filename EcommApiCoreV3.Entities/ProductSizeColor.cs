using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class ProductSizeColor
    {
        public int ProductSizeColorId { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        //public string RowID { get; set; }
        public int ProductSizeId { get; set; } = 0;
        public int Qty { get; set; } = 0;
        public int Price { get; set; } = 0;
        public int SalePrice { get; set; } = 0;
        public bool AvailableSize { get; set; } = false;
        public bool AvailableColors { get; set; } = false;
        public int SizeId { get; set; }
        public string Size { get; set; }
        public int SetNo { get; set; } = 0;
        public string Color { get; set; }
        public string[] ProdColor { get; set; }

        public string[] Prodsize { get; set; }

        public decimal Discount { get; set; } = 0;
        public bool DiscountAvailable { get; set; } = false;
        public int CreatedBy { get; set; } = 0;
        public int Modifiedby { get; set; } = 0;
        public string[] ProductImg { get; set; }
        public int LookupColorId { get; set; }
        public bool IsEdit { get; set; } = false;
        public int[] ArrayColor { get; set; }
        public string[] ArraySize { get; set; }

        public int SelectedQty { get; set; } = 1;

        public Boolean IsSelected { get; set; } = false;

    }
}
