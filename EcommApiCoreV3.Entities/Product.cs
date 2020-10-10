using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Product 
    {
        public int ProductID { get; set; } = 0;
        public string ProductName { get; set; }
        public string ShortDetails { get; set; } = "";
        public string Description { get; set; } = "";
        public int SupplierID { get; set; } = 0;
        public int BrandId { get; set; } = 0;

        public bool ProductAvailable { get; set; } = false;
        public int CreatedBy { get; set; } = 0;
        public int Modifiedby { get; set; } = 0;
        public bool Featured { get; set; } = false;
        public bool Latest { get; set; } = false;
        public bool OnSale { get; set; } = false;
        public bool TopSelling { get; set; } = false;
        public bool HotOffer { get; set; } = false;
        public bool Active { get; set; } = false;
        public string Subcatecode { get; set; } = "";
        //public string Large { get; set; }
        public string SubcategoryName { get; set; }
        public int SubCategoryID { get; set; } = 0;
        public int CategoryID { get; set; } = 0;
        public int MainCategoryID { get; set; } = 0;
        public string BrandName { get; set; }
        //public string[] BannerImg { get; set; }
        public string[] SmallImg { get; set; }
        //public string[] ProductImg { get; set; }

        public string RowID { get; set; } = "";
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string BannerImage { get; set; }
        public string FrontImage { get; set; }
        public string SupplierName { get; set; }
        public int Price { get; set; } = 0;
        public int SalePrice { get; set; } = 0;
        public string ArticalNo { get; set; }
        public int TagId { get; set; } = 0;
        public int FabricId { get; set; } = 0;
        public int FabricTypeId { get; set; } = 0;
        public List<ProductSizeColor> ProductSizeColor { get; set; }
        public int SetType { get; set; } = 0; 
        public int minimum { get; set; } = 0;
        public int ProductSizeId { get; set; } = 0;
        public string VideoURL { get; set; }
		public List<ProductSizeSet> ProductSizeSet { get; set; }

        public int Piece { get; set; }

        public  int AveragePrice { get; set; }

        public int UserId { get; set; }

        public string Color { get; set; }
        public string Size { get; set; }
        // Product Qty
        public int Qty { get; set; } = 0;
        public int CartId { get; set; } = 0;
        public int CartProductQty { get; set; } = 0;
        public int SetNo { get; set; } = 0;
        // cart Quantity 
        public int Quantity { get; set; } = 0;
        public int LookupHSNId { get; set; }
    }
}

