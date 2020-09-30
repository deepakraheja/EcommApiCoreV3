using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductBySubcatecode(Product obj);
        Task<List<Product>> GetAllProductBySupplierId(Product obj);
        Task<List<Product>> GetProductById(Product obj);
        Task<int> SaveProduct(Product obj);
        Task<List<Product>> GetProductByPopular();
		 Task<int> SaveProductSizeColor(ProductSizeColor obj);
        Task<List<ProductSizeColor>> GetProductSizeColorById(ProductSizeColor obj);
        Task<int> DeleteProductSizeColor(ProductSizeColor obj);

        Task<List<Product>> GetBannerProduct();
        Task<List<Product>> GetWithoutSetProductByRowID(Product obj);
        Task<List<Product>> GetWithSetProductByRowID(Product obj);
        Task<List<ProductSizeColor>> GetProductSizeColorByRowID(ProductSizeColor obj);

        Task<List<ProductSizeColor>> GetProductSizeColorByRowID(string RowID);

        string[] GetProductColorByRowID(string RowID);

        string[] GetProductSizeByRowID(string RowID);

        Task<List<ProductSizeSet>> SelectSETListbyRowID(string RowID);
        Task<List<ProductSizeSet>> SelectProductSizeColorWITHSETbyRowID(Cart obj);
        Task<List<Product>> GetProductCartQuantity(Product obj);
        Task<List<Product>> GetProductInCartById(Product obj);
    }
}
