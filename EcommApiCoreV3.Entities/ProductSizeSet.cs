using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class ProductSizeSet
    {
        public int SetNo { get; set; } = 0;
        public int Qty { get; set; } = 1;
        public int SelectedQty { get; set; } = 1;
        public Boolean IsSelected { get; set; } = false;
        public string Details { get; set; }

        public int ProductSizeId { get; set; } = 0;

        public string[] ProductImg { get; set; }

        public int Piece { get; set; } 

        public double AveragePrice
        {
            get { return Convert.ToDouble(((SalePrice / Piece).ToString("0.00"))); }

        }

        public double Price { get; set; } = 0;
        public double SalePrice { get; set; } = 0;

        public string Discount
        {
            get
            {
                double percentage;
                percentage = (double)((Price - SalePrice) * 100 / Price);

                return Math.Round(percentage, 2).ToString();
            }

        }
    }
}
