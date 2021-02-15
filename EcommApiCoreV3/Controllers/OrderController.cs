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
using static EcommApiCoreV3.Controllers.Common.SendEmails;

namespace EcommApiCoreV3.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : BaseController<OrderController>
    {
        IOrderBAL _IOrderBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;
        IEmailTemplateBAL _IEmailTemplateBAL;
        IUsersBAL _usersBAL;
        public OrderController(IWebHostEnvironment hostingEnvironment,
            IOrderBAL OrderBAL, IEmailTemplateBAL emailTemplateBAL, IUsersBAL usersBAL)
        {
            _IOrderBAL = OrderBAL;
            webRootPath = hostingEnvironment.WebRootPath;
            _IEmailTemplateBAL = emailTemplateBAL;
            _usersBAL = usersBAL;
        }

        [HttpPost]
        [Route("SaveOrder")]
        public async Task<string> SaveOrder([FromBody] Order obj)
        {
            try
            {
                obj.UserID = UserService.LoggedInUser;
                List<Order> lst = await this._IOrderBAL.SaveOrder(obj);
                SendEmails sendEmails = new SendEmails(_usersBAL, _IEmailTemplateBAL, _IOrderBAL);
                SendEmails.webRootPath = webRootPath;
                Users objUser = new Users();
                objUser.OrderID = lst[0].OrderId.ToString();
                objUser.UserID = obj.UserID;
                objUser.GUID = lst[0].GUID;
                sendEmails.setMailContent(objUser, EStatus.NewOrderCompletion.ToString());
                return lst[0].GUID;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController SaveOrder action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController SaveOrder action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetOrderByOrderId")]
        public async Task<List<Order>> GetOrderByOrderId([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
                foreach (var item in lst[0].OrderDetails)
                {
                    if (item.SetNo > 0)
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }
                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetOrderByOrderId action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetOrderByOrderId action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetSuccessOrderByOrderId")]
        public async Task<List<Order>> GetSuccessOrderByOrderId([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                lst[0].OrderDetails = this._IOrderBAL.GetSuccessOrderDetailsByOrderId(obj).Result;
                foreach (var item in lst[0].OrderDetails)
                {
                    if (item.SetNo > 0)
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }
                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetSuccessOrderByOrderId action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetSuccessOrderByOrderId action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetSuccessPrintOrderByGUID")]
        public async Task<List<Order>> GetSuccessPrintOrderByGUID([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetPrintOrderByGUID(obj).Result;
                //obj.OrderId = lst[0].OrderId;
                //lst[0].OrderDetails = this._IOrderBAL.GetPrintOrderDetailsByOrderId(obj).Result;
                foreach (var item in lst[0].OrderDetails)
                {
                    if (item.SetNo > 0)
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }
                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetSuccessPrintOrderByGUID action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetSuccessPrintOrderByGUID action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetNewOrderByGUID")]
        public async Task<List<Order>> GetNewOrderByGUID([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetNewOrderByGUID(obj).Result;
                //obj.OrderId = lst[0].OrderId;
                //lst[0].OrderDetails = this._IOrderBAL.GetPrintOrderDetailsByOrderId(obj).Result;


                /*   foreach (var item in lst[0].OrderDetails)  //commnented on 10 jan 2021 by deepak
                   {
                       if (item.SetNo > 0)
                           item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                       else
                           item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                   }*/  //commnented on 10 jan 2021 by deepak

                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetNewOrderByGUID action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetNewOrderByGUID action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetOrderByUserId")]
        public async Task<List<Order>> GetOrderByUserId()
        {
            try
            {
                Order obj = new Order();
                obj.UserID = UserService.LoggedInUser;
                List<Order> lst = this._IOrderBAL.GetOrderByUserId(obj).Result;
                obj.OrderId = lst[0].OrderId;
                lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByUserId(obj).Result;

                /*   foreach (var item in lst[0].OrderDetails)  //commnented on 10 jan 2021 by deepak
                foreach (var item in lst[0].OrderDetails)
              {
                  if (item.SetNo > 0)
                      item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                  else
                      item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
               }*/  //commnented on 10 jan 2021 by deepak

                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetOrderByUserId action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetOrderByUserId action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("GetAllOrder")]
        public async Task<List<Order>> GetAllOrder([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetAllOrder(obj).Result;
                for (int i = 0; i < lst.Count; i++)
                {
                    obj.OrderId = lst[i].OrderId;
                    lst[i].OrderDetails = this._IOrderBAL.GetAllOrderDetails(obj).Result;
                    foreach (var item in lst[i].OrderDetails)
                    {
                        if (item.SetNo > 0)
                            item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                        else
                            item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                    }
                }
                //return lst;
                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetAllOrder action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetAllOrder action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("UpdateOrderDetailStatus")]
        public async Task<int> UpdateOrderDetailStatus([FromBody] OrderStatusHistory[] obj)
        {
            try
            {
                //return await this._IOrderBAL.UpdateOrderDetailStatus(obj);
                int res = 0;
                foreach (var item in obj)
                {
                    res = await this._IOrderBAL.UpdateOrderDetailStatus(item);
                }

                SendEmails sendEmails = new SendEmails(_usersBAL, _IEmailTemplateBAL, _IOrderBAL);
                SendEmails.webRootPath = webRootPath;
             
                Users objUser = new Users();
                objUser.OrderID = obj[0].OrderId.ToString();
                //objUser.UserID = obj[0].CreatedBy;
                objUser.UserID = UserService.LoggedInUser;
                objUser.OrderID = Convert.ToString( obj[0].OrderId);
                objUser.OrderDetailsID = obj[0].OrderDetailsID.ToString();

                //if (obj[0].OrderStatusId == 1)
                //    sendEmails.setMailContent(objUser, EStatus.NewOrderCompletion.ToString());

                if (obj[0].OrderStatusId == 2)
                    sendEmails.setMailContent(objUser, EStatus.NewOrderProcess.ToString());
                if (obj[0].OrderStatusId == 3)
                    sendEmails.setMailContent(objUser, EStatus.DispatchedConfirmation.ToString());
                if (obj[0].OrderStatusId == 4)
                    sendEmails.setMailContent(objUser, EStatus.DeliveredConfirmation.ToString());

                return res;

            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController UpdateOrderDetailStatus action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController UpdateOrderDetailStatus action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("GetDashboardSummary")]
        public async Task<List<Order>> GetDashboardSummary()
        {
            try
            {
                return await this._IOrderBAL.GetDashboardSummary();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside OrderController GetDashboardSummary action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside OrderController GetDashboardSummary action: {ex.Message}");
                return null;
            }
        }
    }
}
