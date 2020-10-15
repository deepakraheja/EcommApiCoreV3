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
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public async Task<List<Order>> SaveOrder(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                //parameters.Add("@FName", obj.FName);
                //parameters.Add("@LName", obj.LName);
                //parameters.Add("@CompanyName", obj.CompanyName);
                //parameters.Add("@Address", obj.Address);
                //parameters.Add("@City", obj.City);
                //parameters.Add("@State", obj.State);
                //parameters.Add("@ZipCode", obj.ZipCode);
                //parameters.Add("@EmailId", obj.EmailId);
                //parameters.Add("@Phone", obj.Phone);
                //parameters.Add("@Country", obj.Country);
                parameters.Add("@OrderNumber", obj.OrderNumber);
                parameters.Add("@OrderDate", obj.OrderDate);
                parameters.Add("@PaymentTypeId", obj.PaymentTypeId);
                //parameters.Add("@Notes", obj.Notes);
                parameters.Add("@StatusId", obj.StatusId);
                parameters.Add("@SubTotal", obj.SubTotal);
                parameters.Add("@Tax", obj.Tax);
                parameters.Add("@ShippingCharge", obj.ShippingCharge);
                parameters.Add("@TotalAmount", obj.TotalAmount);
                parameters.Add("@BillingAddressId", obj.BillingAddressId);
                //var res = await SqlMapper.ExecuteScalarAsync(con, "p_SaveOrder", param: parameters, commandType: StoredProcedure);
                //return Convert.ToInt32(res);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_SaveOrder", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetOrderByOrderId(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", obj.OrderId);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetOrderByOrderId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetOrderDetailsByOrderId(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", obj.OrderId);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetOrderDetailsByOrderId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetOrderByUserId(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetOrderByUserId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetOrderDetailsByUserId(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetOrderDetailsByUserId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetAllOrder(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (obj.StatusId > 0)
                    parameters.Add("@StatusId", obj.StatusId);
                parameters.Add("@StartDate", Convert.ToDateTime(obj.StartDate));
                parameters.Add("@EndDate", Convert.ToDateTime(obj.EndDate));
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetAllOrder", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetAllOrderDetails(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", obj.OrderId);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetAllOrderDetails", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<int> UpdateOrderDetailStatus(OrderStatusHistory obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderStatusHistoryId", obj.OrderStatusHistoryId);
                parameters.Add("@OrderDetailsID", obj.OrderDetailsID);
                parameters.Add("@OrderStatusId", obj.OrderStatusId);
                parameters.Add("@CreatedDate", obj.CreatedDate);
                parameters.Add("@CreatedBy", obj.CreatedBy);
                parameters.Add("@OrderId", obj.OrderId);
                parameters.Add("@SetNo", obj.SetNo);
                parameters.Add("@ProductId", obj.ProductId);
                if (obj.OrderStatusId == 3)
                {
                    parameters.Add("@TransportID", obj.TransportID);
                    parameters.Add("@DispatchDate", obj.DispatchDate);
                    parameters.Add("@Bilty", obj.Bilty);
                }
                var res = await SqlMapper.ExecuteAsync(con, "p_OrderStatusHistory_ins", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Order>> GetDashboardSummary()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetDashboardSummary", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Order>> GetSuccessOrderDetailsByOrderId(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", obj.OrderId);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetSuccessOrderDetailsByOrderId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetPrintOrderByGUID(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.GUID);
                //List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetPrintOrderByGUID", param: parameters, commandType: StoredProcedure)).ToList();

                var lst = await SqlMapper.QueryMultipleAsync(con, "p_GetPrintOrderByGUID", param: parameters, commandType: StoredProcedure);
                List<Order> lstOrder = new List<Order>();

                List<Order> lstOrderDetails = new List<Order>();
                List<Users> lstUsers = new List<Users>();

                lstOrder = lst.Read<Order>().ToList();
                lstOrderDetails = lst.Read<Order>().ToList();
                lstUsers = lst.Read<Users>().ToList();
                foreach (var item in lstOrderDetails)
                {
                    lstOrder[0].OrderDetails.Add(item);
                }
                foreach (var item in lstUsers)
                {
                    lstOrder[0].ListUsers.Add(item);
                }

                return lstOrder;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetNewOrderByGUID(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GUID", obj.GUID);
                var lst = await SqlMapper.QueryMultipleAsync(con, "p_GetNewOrderByGUID", param: parameters, commandType: StoredProcedure);
                List<Order> lstOrder = new List<Order>();

                List<Order> lstOrderDetails = new List<Order>();

                lstOrder = lst.Read<Order>().ToList();
                lstOrderDetails = lst.Read<Order>().ToList();
                foreach (var item in lstOrderDetails)
                {
                    lstOrder[0].OrderDetails.Add(item);
                }
                return lstOrder;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Order>> GetPrintOrderDetailsByOrderId(Order obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderId", obj.OrderId);
                List<Order> lst = (await SqlMapper.QueryAsync<Order>(con, "p_GetPrintOrderDetailsByOrderId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
