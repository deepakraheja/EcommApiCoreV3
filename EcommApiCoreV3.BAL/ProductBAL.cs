using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class ProductBAL : IProductBAL
    {
        IProductRepository _IProductRepository;

        public ProductBAL(IProductRepository IProductRepository)
        {
            _IProductRepository = IProductRepository;
        }

        public Task<List<Product>> GetProductBySubcatecode(Product obj)
        {
            return _IProductRepository.GetProductBySubcatecode(obj);
        }

        public Task<List<Product>> GetAllProductBySupplierId(Product obj)
        {
            return _IProductRepository.GetAllProductBySupplierId(obj);
        }
        public Task<List<Product>> GetProductById(Product obj)
        {
            return _IProductRepository.GetProductById(obj);
        }

        public Task<List<Product>> GetWithoutSetProductByRowID(Product obj)
        {
            return _IProductRepository.GetWithoutSetProductByRowID(obj);
        }
        public Task<List<Product>> GetWithSetProductByRowID(Product obj)
        {
            return _IProductRepository.GetWithSetProductByRowID(obj);
        }
        public Task<List<ProductSizeColor>> GetProductSizeColorByRowID(ProductSizeColor obj)
        {
            return _IProductRepository.GetProductSizeColorByRowID(obj);
        }
        public Task<int> SaveProduct(Product obj)
        {
            return _IProductRepository.SaveProduct(obj);
        }

        public Task<List<Product>> GetProductByPopular()
        {
            return _IProductRepository.GetProductByPopular();
        }
        public Task<int> SaveProductSizeColor(ProductSizeColor obj)
        {
            return _IProductRepository.SaveProductSizeColor(obj);
        }
        public Task<List<ProductSizeColor>> GetProductSizeColorById(ProductSizeColor obj)
        {
            return _IProductRepository.GetProductSizeColorById(obj);
        }

        public Task<List<ProductSizeColor>> GetProductSizeColorByRowID(string RowID)
        {
            return _IProductRepository.GetProductSizeColorByRowID(RowID);
        }

        public Task<int> DeleteProductSizeColor(ProductSizeColor obj)
        {
            return _IProductRepository.DeleteProductSizeColor(obj);
        }

        public Task<List<Product>> GetBannerProduct()
        {
            return _IProductRepository.GetBannerProduct();
        }
      

        public string[] GetProductColorByRowID(string RowID)
        {
            return _IProductRepository.GetProductColorByRowID(RowID);
        }

        public string[] GetProductSizeByRowID(string RowID)
        {
            return _IProductRepository.GetProductSizeByRowID(RowID);
        }
        public Task<List<ProductSizeSet>> SelectSETListbyRowID(string RowID)
        {
            return _IProductRepository.SelectSETListbyRowID(RowID);
        }
        public Task<List<ProductSizeSet>> SelectProductSizeColorWITHSETbyRowID(Cart obj)
        {
            return _IProductRepository.SelectProductSizeColorWITHSETbyRowID(obj);
        }
        public Task<List<Product>> GetProductCartQuantity(Product obj)
        {
            return _IProductRepository.GetProductCartQuantity(obj);
        }
        public Task<List<Product>> GetProductInCartById(Product obj)
        {
            return _IProductRepository.GetProductInCartById(obj);
        }
    }
}
