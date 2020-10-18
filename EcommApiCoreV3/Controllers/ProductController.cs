using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository;

namespace EcommApiCoreV3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : BaseController<ProductController>
    {
        IProductBAL _IProductBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;

        public ProductController(IWebHostEnvironment hostingEnvironment, IProductBAL ProductBAL)
        {
            _IProductBAL = ProductBAL;
            webRootPath = hostingEnvironment.WebRootPath;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpPost]
        [Route("GetProductBySubcatecode")]
        [AllowAnonymous]
        public async Task<List<Product>> GetProductBySubcatecode([FromBody] Product obj)
        {
            try
            {
                List<Product> lst = this._IProductBAL.GetProductBySubcatecode(obj).Result;
                //for (int i = 0; i < lst.Count; i++)
                //{
                //    lst[i].ProductSizeColor = this._IProductBAL.GetProductSizeColorByRowID(lst[i].RowID).Result;
                //}
                return await Task.Run(() => new List<Product>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductBySubcatecode action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductBySubcatecode action: {ex.Message}");
                return null;
            }
        }



        [HttpPost]
        [Route("GetProductByPopular")]
        public async Task<List<Product>> GetProductByPopular()
        {
            try
            {
                return await this._IProductBAL.GetProductByPopular();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductByPopular action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductByPopular action: {ex.Message}");
                return null;
            }
        }


        [HttpPost]
        [Route("GetBannerProduct")]
        public async Task<List<Product>> GetBannerProduct()
        {
            try
            {
                return await this._IProductBAL.GetBannerProduct();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetBannerProduct action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetBannerProduct action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllProductBySupplierId")]
        public async Task<List<Product>> GetAllProductBySupplierId([FromBody] Product obj)
        {
            try
            {
                return await this._IProductBAL.GetAllProductBySupplierId(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetAllProductBySupplierId action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetAllProductBySupplierId action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetWithoutSetProductByRowID")]
        public async Task<List<Product>> GetWithoutSetProductByRowID([FromBody] Product obj)
        {
            try
            {
                List<Product> lst = this._IProductBAL.GetWithoutSetProductByRowID(obj).Result;
                //lst[0].BannerImg = _utilities.ProductImagePath(obj.ProductID, "bannerImage", webRootPath);
                //lst[0].ProductImg = _utilities.ProductImage(lst[0].ProductID, "productColorImage", webRootPath, lst[0].ProductSizeColorId);// commented on 12 2020 july by deepak
                lst[0].ProductSizeColor = this._IProductBAL.GetProductSizeColorByRowID(obj.RowID).Result;

                for (int i = 0; i < lst[0].ProductSizeColor.Count; i++) // added on 12 july 2020 by deepak
                {
                    lst[0].ProductSizeColor[i].ProductImg = _utilities.ProductImage(lst[0].ProductID, "productColorImage", webRootPath, lst[0].ProductSizeColor[i].ProductSizeColorId);
                }
                //lst[0].Prodsize = this._IProductBAL.GetProductSizeByRowID(obj.RowID);// commented on 10 july 2020 by deepak
                //lst[0].ProductImg = _utilities.ProductImage(lst[0].ProductID, "productImages", webRootPath);
                return await Task.Run(() => new List<Product>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductBybyRowID action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductBybyRowID action: {ex.Message}");
                return null;
            }
        }
        ///return await this._IProductBAL.GetProductBybyRowID(obj);
        [HttpPost]
        [AllowAnonymous]
        [Route("GetWithSetProductByRowID")]
        public async Task<List<Product>> GetWithSetProductByRowID([FromBody] Product obj)
        {
            try
            {
                List<Product> lst = this._IProductBAL.GetWithSetProductByRowID(obj).Result;
                lst[0].ProductSizeSet = this._IProductBAL.SelectSETListbyRowID(obj.RowID).Result;


                for (int i = 0; i < lst[0].ProductSizeSet.Count; i++) // added on 9 aug 2020 by deepak
                {
                    lst[0].ProductSizeSet[i].ProductImg = _utilities.ProductImage(lst[0].ProductID, "productSetImage", webRootPath, lst[0].ProductSizeSet[i].SetNo);
                }

                return await Task.Run(() => new List<Product>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductBybyRowID action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductBybyRowID action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("GetProductSizeColorByRowID")]
        public async Task<List<ProductSizeColor>> GetProductSizeColorByRowID([FromBody] ProductSizeColor obj)
        {
            try
            {
                List<ProductSizeColor> lst = this._IProductBAL.GetProductSizeColorByRowID(obj).Result;
                return await Task.Run(() => new List<ProductSizeColor>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductBybyRowID action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductBybyRowID action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetProductById")]
        public async Task<List<Product>> GetProductById([FromBody] Product obj)
        {
            try
            {
                //List<Product> lst = this._IProductBAL.GetProductById(obj).Result;
                ////lst[0].BannerImg = _utilities.ProductImagePath(obj.ProductID, "bannerImage", webRootPath);
                ////lst[0].SmallImg = _utilities.ProductImagePath(obj.ProductID, "frontImage", webRootPath);
                ////lst[0].ProductImg = _utilities.ProductImagePath(obj.ProductID, "productImages", webRootPath);
                //return await Task.Run(() => new List<Product>(lst));
                return await this._IProductBAL.GetProductById(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductById action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveProduct")]
        public async Task<int> SaveProduct([FromBody] Product obj)
        {
            try
            {
                int NewProductId = this._IProductBAL.SaveProduct(obj).Result;
                if (obj.ProductID > 0)
                {
                    //if (obj.BannerImg != null)
                    //    _utilities.SaveImage(obj.ProductID, obj.BannerImg, "bannerImage", webRootPath);
                    if (obj.SmallImg != null)
                        _utilities.SaveImage(obj.ProductID, obj.SmallImg, "frontImage", webRootPath);
                    //_utilities.SaveImage(obj.ProductID, obj.ProductImg, "productImages", webRootPath);
                    //return obj.ProductID;
                    return await Task.Run(() => obj.ProductID);
                }
                else
                {
                    //if (obj.BannerImg != null)
                    //    _utilities.SaveImage(NewProductId, obj.BannerImg, "bannerImage", webRootPath);
                    if (obj.SmallImg != null)
                        _utilities.SaveImage(NewProductId, obj.SmallImg, "frontImage", webRootPath);
                    //_utilities.SaveImage(NewProductId, obj.ProductImg, "productImages", webRootPath);
                    //return NewProductId;
                    return await Task.Run(() => NewProductId);
                }


            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController SaveProduct action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                
                Logger.LogError($"Something went wrong inside ProductController SaveProduct action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SaveProductSizeColor")]
        public async Task<int> SaveProductSizeColor([FromBody] ProductSizeColor obj)
        {
            try
            {
                int ProductSizeColorId = 0;
                if (obj.ArrayColor != null)
                {
                    foreach (var itemColor in obj.ArrayColor)
                    {
                        foreach (var itemSize in obj.ArraySize)
                        {
                            obj.SizeId = itemSize;
                            obj.LookupColorId = itemColor;
                            ProductSizeColorId = await this._IProductBAL.SaveProductSizeColor(obj);
                        }
                    }
                }
                else
                {
                    ProductSizeColorId = await this._IProductBAL.SaveProductSizeColor(obj);
                }
                return ProductSizeColorId;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController SaveProductSizeColor action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController SaveProductSizeColor action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("SaveProductSizeColorImages")]
        public async Task<int> SaveProductSizeColorImages([FromBody] ProductSizeColor obj)
        {
            try
            {
                if (obj.ProductSizeColorId > 0)
                {
                    if (obj.SetNo > 0)
                        _utilities.SaveImage(obj.ProductId, obj.ProductImg, ("productSetImage/" + obj.SetNo), webRootPath);
                    else
                        _utilities.SaveImage(obj.ProductId, obj.ProductImg, ("productColorImage/" + obj.ProductSizeColorId), webRootPath);
                }
                return await Task.Run(() => obj.ProductSizeColorId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController SaveProductSizeColor action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController SaveProductSizeColor action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("GetProductSizeColorById")]
        public async Task<List<ProductSizeColor>> GetProductSizeColorById([FromBody] ProductSizeColor obj)
        {
            try
            {
                List<ProductSizeColor> lst = this._IProductBAL.GetProductSizeColorById(obj).Result;
                foreach (var item in lst)
                {
                    if (item.SetNo > 0)
                        //item.ProductImg = _utilities.ProductImagePath(item.ProductId, ("productSetImage/" + item.SetNo), webRootPath);
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        //item.ProductImg = _utilities.ProductImagePath(item.ProductId, ("productColorImage/" + item.ProductSizeColorId), webRootPath);
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }

                return await Task.Run(() => new List<ProductSizeColor>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductSizeColorById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductSizeColorById action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("DeleteProductSizeColor")]
        public async Task<int> DeleteProductSizeColor([FromBody] ProductSizeColor obj)
        {
            try
            {
                var res = await this._IProductBAL.DeleteProductSizeColor(obj);
                if (res == -2)
                {
                    if (obj.SetNo > 0)
                        _utilities.DeleteProductImagePath(obj.ProductId, ("productSetImage/" + obj.SetNo), webRootPath);
                    else
                        _utilities.DeleteProductImagePath(obj.ProductId, ("productColorImage/" + obj.ProductSizeColorId), webRootPath);
                }
                return res;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController DeleteProductSizeColor action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController DeleteProductSizeColor action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("DeleteProductImage")]
        public async Task<int> DeleteProductImage([FromBody] Product obj)
        {
            try
            {
                _utilities.DeleteProductImage(obj.ImagePath, webRootPath);
                if (obj.ProductID > 0)
                {
                    ProductRepository obj1 = new ProductRepository();
                    Product product = new Product
                    {
                        ImagePath = "",
                        ProductID = obj.ProductID,
                        Type = "frontImage"
                    };
                    obj1.SaveProductImages(product);
                }
                return await Task.Run(() => 1);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController DeleteProductSizeColor action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController DeleteProductSizeColor action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("GetProductCartQuantity")]
        public async Task<List<Product>> GetProductCartQuantity([FromBody] Product obj)
        {
            try
            {
                return await this._IProductBAL.GetProductCartQuantity(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductCartQuantity action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductCartQuantity action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetProductInCartById")]
        public async Task<List<Product>> GetProductInCartById([FromBody] Product obj)
        {
            try
            {
                return await this._IProductBAL.GetProductInCartById(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside ProductController GetProductInCartById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside ProductController GetProductInCartById action: {ex.Message}");
                return null;
            }
        }
    }
}