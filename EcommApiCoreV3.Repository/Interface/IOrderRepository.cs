using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<int> SaveOrder(Order obj);
        Task<List<Order>> GetOrderByOrderId(Order obj);
        Task<List<Order>> GetOrderDetailsByOrderId(Order obj);
        Task<List<Order>> GetOrderByUserId(Order obj);
        Task<List<Order>> GetOrderDetailsByUserId(Order obj);
        Task<List<Order>> GetAllOrder(Order obj);
        Task<List<Order>> GetAllOrderDetails(Order obj);
        Task<int> UpdateOrderDetailStatus(OrderStatusHistory obj);
        Task<List<Order>> GetDashboardSummary();
        Task<List<Order>> GetSuccessOrderDetailsByOrderId(Order obj);
    }
}
