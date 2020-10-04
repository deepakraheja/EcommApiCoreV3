using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;
using static System.Data.CommandType;

namespace EcommApiCoreV3.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public async Task<List<Product>> GetProductBySubcatecode(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (!string.IsNullOrEmpty(obj.Subcatecode) && (obj.Subcatecode != "all"))
                    parameters.Add("@Subcatecode", obj.Subcatecode);
                if (obj.BrandId > 0)
                    parameters.Add("@BrandId", obj.BrandId);

                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_product_selbySubcatecode", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public async Task<List<Product>> GetProductByPopular()
        {
            try
            {
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_product_selbyPopular", commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Product>> GetAllProductBySupplierId(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (obj.SupplierID > 0)
                    parameters.Add("@SupplierID", obj.SupplierID);
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_GetAllproductBySupplierId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Product>> GetProductById(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductID", obj.ProductID);
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_product_selbyId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Product>> GetWithoutSetProductByRowID(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.RowID);
                parameters.Add("@ProductSizeId", obj.ProductSizeId);
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_WithoutSetProduct_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Product>> GetWithSetProductByRowID(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.RowID);
                parameters.Add("@ProductSizeId", obj.ProductSizeId);
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_WithSetProduct_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<ProductSizeColor>> GetProductSizeColorByRowID(ProductSizeColor obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.ProductSizeColorId);
                List<ProductSizeColor> lst = (await SqlMapper.QueryAsync<ProductSizeColor>(con, "p_ProductSizeColor_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<ProductSizeColor>> GetProductSizeColorByRowID(string RowID)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", RowID);
                List<ProductSizeColor> lst = (await SqlMapper.QueryAsync<ProductSizeColor>(con, "p_ProductSizeColor_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<ProductSizeSet>> SelectSETListbyRowID(string RowID)
        {
            try
            {
                List<ProductSizeSet> lstProductSizeSet = new List<ProductSizeSet>();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", RowID);
                lstProductSizeSet = (await SqlMapper.QueryAsync<ProductSizeSet>(con, "p_selectSET_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                return lstProductSizeSet;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<ProductSizeSet>> SelectProductSizeColorWITHSETbyRowID(Cart obj)
        { 
            try
            {
                List<ProductSizeSet> lstProductSizeSet = new List<ProductSizeSet>();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.RowID);
                lstProductSizeSet = (await SqlMapper.QueryAsync<ProductSizeSet>(con, "p_ProductSizeColorWITHSET_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                return lstProductSizeSet;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public string[] GetProductColorByRowID(string RowID)
        {
            try
            {
                string[] color = new string[0];
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", RowID);
                List<Product> lst = (SqlMapper.Query<Product>(con, "p_ProductColor_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                color = new string[lst.Count];

                for (int i = 0; i < lst.Count; i++)
                {
                    color[i] = lst[i].Color;
                }
                return color;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string[] GetProductSizeByRowID(string RowID)
        {
            try
            {
                string[] size = new string[0];
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", RowID);
                List<Product> lst = (SqlMapper.Query<Product>(con, "p_Productsize_selbyRowID", param: parameters, commandType: StoredProcedure)).ToList();
                size = new string[lst.Count];

                for (int i = 0; i < lst.Count; i++)
                {
                    size[i] = lst[i].Size;
                }
                return size;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        public async Task<int> SaveProduct(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductID", obj.ProductID); //int
                parameters.Add("@ProductName", obj.ProductName); //nvarchar
                parameters.Add("@ShortDetails", obj.ShortDetails); //ntext
                parameters.Add("@Description", obj.Description); //ntext
                parameters.Add("@SupplierID", obj.SupplierID); //int
                parameters.Add("@SubCategoryID", obj.SubCategoryID); //int
                parameters.Add("@BrandId", obj.BrandId); //int

                parameters.Add("@ProductAvailable", obj.ProductAvailable); //bit
                parameters.Add("@CreatedBy", obj.CreatedBy); //int
                //parameters.Add("@CreatedDate", obj.CreatedDate); //datetime
                parameters.Add("@Modifiedby", obj.Modifiedby); //int
                //parameters.Add("@ModifiedDate", obj.ModifiedDate); //datetime
                parameters.Add("@Featured", obj.Featured); //bit
                parameters.Add("@Latest", obj.Latest); //bit
                parameters.Add("@OnSale", obj.OnSale); //bit
                parameters.Add("@TopSelling", obj.TopSelling); //bit
                parameters.Add("@HotOffer", obj.HotOffer); //bit
                parameters.Add("@Active", obj.Active); //bit
                parameters.Add("@Title", obj.Title);
                parameters.Add("@SubTitle", obj.SubTitle);
                parameters.Add("@ArticalNo", obj.ArticalNo);
                parameters.Add("@TagId", obj.TagId);
                parameters.Add("@FabricId", obj.FabricId);
                parameters.Add("@FabricTypeId", obj.FabricTypeId);
                parameters.Add("@SetType", obj.SetType);
                parameters.Add("@minimum", obj.minimum);
                parameters.Add("@VideoURL", obj.VideoURL);
                var res = await SqlMapper.ExecuteScalarAsync(con, "p_Product_ins", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<int> SaveProductSizeColor(ProductSizeColor obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductSizeId", obj.ProductSizeId);
                parameters.Add("@ProductSizeColorId", obj.ProductSizeColorId);
                parameters.Add("@ProductID", obj.ProductId); //int
                parameters.Add("@Qty", obj.Qty); //int
                parameters.Add("@Price", obj.Price); //decimal
                parameters.Add("@SalePrice", obj.SalePrice); //decimal
                parameters.Add("@AvailableSize", obj.AvailableSize); //bit
                parameters.Add("@AvailableColors", obj.AvailableColors); //bit
                //parameters.Add("@Size", obj.SizeId); //nvarchar commented on 5 aug 2020

                parameters.Add("@Size", obj.SizeId); //nvarchar
                parameters.Add("@SetNo", obj.SetNo);
                parameters.Add("@LookupColorId", obj.LookupColorId); //nvarchar
                parameters.Add("@Discount", obj.Discount); //decimal
                parameters.Add("@DiscountAvailable", obj.DiscountAvailable); //bit
                parameters.Add("@CreatedBy", obj.CreatedBy); //int
                parameters.Add("@Modifiedby", obj.Modifiedby); //int
                var res = await SqlMapper.ExecuteAsync(con, "p_ProductSizeColor_ins", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<ProductSizeColor>> GetProductSizeColorById(ProductSizeColor obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductID", obj.ProductId);
                List<ProductSizeColor> lst = (await SqlMapper.QueryAsync<ProductSizeColor>(con, "p_ProductSizeColor_selByProdcutId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<int> DeleteProductSizeColor(ProductSizeColor obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductId", obj.ProductId);
                parameters.Add("@ProductSizeColorId", obj.ProductSizeColorId);
                parameters.Add("@ProductSizeId", obj.ProductSizeId);
                parameters.Add("@SetNo", obj.SetNo);
                var res = await SqlMapper.ExecuteScalarAsync(con, "p_ProductSizeColor_del", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void SaveProductImages(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductId", obj.ProductID);
                parameters.Add("@ImagePath", obj.ImagePath);
                parameters.Add("@Type", obj.Type);
                var res = SqlMapper.ExecuteAsync(con, "p_ProductImage_upd", param: parameters, commandType: StoredProcedure);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Product>> GetBannerProduct()
        {
            try
            {
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_GetBannerProduct", commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Product>> GetProductCartQuantity(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.RowID);
                parameters.Add("@UserID", obj.UserId);
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_GetProductCartQuantity", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Product>> GetProductInCartById(Product obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserId);
                parameters.Add("@ProductId", obj.ProductID);
                List<Product> lst = (await SqlMapper.QueryAsync<Product>(con, "p_GetProductInCartById", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
    public class SetList
    {
        public int SetNo { get; set; }
        public int Qty { get; set; } = 1;
        public Boolean color { get; set; } = true;
        public string size { get; set; }
    }
}
