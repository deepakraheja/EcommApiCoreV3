using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class CartController : BaseController<CartController>
    {
        ICartBAL _ICartBAL;
        IProductBAL _IProductBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;
        public CartController(IHostingEnvironment hostingEnvironment, ICartBAL CartBAL, IProductBAL ProductBAL)
        {
            _ICartBAL = CartBAL;
            _IProductBAL = ProductBAL;
            webRootPath = hostingEnvironment.WebRootPath;
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<int> AddToCart([FromBody] List<Cart> obj)
        {
            try
            {
                var res = -1;
                List<ProductSizeSet> lst = new List<ProductSizeSet>();
                if (obj[0].SetNo > 0)
                {
                    lst = _IProductBAL.SelectProductSizeColorWITHSETbyRowID(obj[0]).Result;
                }
                foreach (var item in obj)
                {
                    if (item.SetNo > 0)
                    {
                        List<ProductSizeSet> lstselect = lst.Where(x => x.SetNo == item.SetNo).ToList();

                        if (lstselect.Count > 0)
                        {
                            foreach (var itemnew in lstselect)
                            {
                                Cart _obj = new Cart();
                                _obj.ProductSizeId = itemnew.ProductSizeId;
                                _obj.Quantity = item.Quantity;
                                _obj.UserID = item.UserID;
                                res = await this._ICartBAL.AddToCart(_obj);
                            }
                        }
                    }
                    else
                    {
                        res = await this._ICartBAL.AddToCart(item);
                    }

                }
                return res;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CartController AddToCart action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CartController AddToCart action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("UpdateToCart")]
        public async Task<int> UpdateToCart([FromBody] List<Cart> obj)
        {
            try
            {
                var res = -1;
                List<Cart> lst = new List<Cart>();
                if (obj[0].SetNo > 0)
                {
                    lst = _ICartBAL.GetCartByUserId(obj[0]).Result;
                }
                foreach (var item in obj)
                {
                    if (item.SetNo > 0)
                    {
                        List<Cart> lstselect = lst.Where(x => x.SetNo == item.SetNo).ToList();

                        if (lstselect.Count > 0)
                        {
                            foreach (var itemnew in lstselect)
                            {
                                Cart _obj = new Cart();
                                _obj.ProductSizeId = itemnew.ProductSizeId;
                                _obj.Quantity = item.Quantity;
                                _obj.UserID = item.UserID;
                                res = await this._ICartBAL.AddToCart(_obj);
                            }
                        }
                    }
                    else
                    {
                        res = await this._ICartBAL.AddToCart(item);
                    }

                }
                return res;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CartController UpdateToCart action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CartController UpdateToCart action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("DelCartById")]
        public async Task<List<Cart>> DelCartById([FromBody] Cart obj)
        {
            try
            {
                return await this._ICartBAL.DelCartById(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CartController DelCartById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CartController DelCartById action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetCartById")]
        public async Task<List<Cart>> GetCartById([FromBody] Cart obj)
        {
            try
            {
                //return await this._ICartBAL.GetCartById(obj);
                List<Cart> lst = this._ICartBAL.GetCartById(obj).Result;
                foreach (var item in lst)
                {
                    if (item.SetNo > 0)
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }

                return await Task.Run(() => new List<Cart>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CartController GetCartById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CartController GetCartById action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetCartProcessedById")]
        public async Task<List<Cart>> GetCartProcessedById([FromBody] Cart obj)
        {
            try
            {
                //return await this._ICartBAL.GetCartById(obj);
                List<Cart> lst = this._ICartBAL.GetCartProcessedById(obj).Result;
                foreach (var item in lst)
                {
                    if (item.SetNo > 0)
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }

                return await Task.Run(() => new List<Cart>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CartController GetCartProcessedById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CartController GetCartProcessedById action: {ex.Message}");
                return null;
            }
        }
    }
}
