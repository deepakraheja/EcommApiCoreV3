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
    public class WishListController : BaseController<WishListController>
    {
        IWishListBAL _IWishListBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;
        public WishListController(IHostingEnvironment hostingEnvironment, IWishListBAL WishListBAL)
        {
            _IWishListBAL = WishListBAL;
            webRootPath = hostingEnvironment.WebRootPath;
        }

        [HttpPost]
        [Route("AddToWishList")]
        public async Task<int> AddToWishList([FromBody] Cart obj)
        {
            try
            {
                obj.UserID = UserService.LoggedInUser;
                var res = await this._IWishListBAL.AddToWishList(obj);
                return res;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside WishListController AddToWishList action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside WishListController AddToWishList action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("DelWishListById")]
        public async Task<List<Cart>> DelWishListById([FromBody] Cart obj)
        {
            try
            {
                obj.UserID = UserService.LoggedInUser;
                return await this._IWishListBAL.DelWishListById(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside WishListController DelWishListById action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside WishListController DelWishListById action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetWishListById")]
        public async Task<List<Cart>> GetWishListById()
        {
            try
            {
                Cart obj = new Cart();
                obj.UserID = UserService.LoggedInUser;
                List<Cart> lst = this._IWishListBAL.GetWishListByUserId(obj).Result;
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

    }
}
