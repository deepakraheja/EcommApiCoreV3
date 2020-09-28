using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class OrderBAL : IOrderBAL
    {
        IOrderRepository _OrderRepository;

        public OrderBAL(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public Task<int> SaveOrder(Order obj)
        {
            return _OrderRepository.SaveOrder(obj);
        }
        public Task<List<Order>> GetOrderByOrderId(Order obj)
        {
            return _OrderRepository.GetOrderByOrderId(obj);
        }
        public Task<List<Order>> GetOrderDetailsByOrderId(Order obj)
        {
            return _OrderRepository.GetOrderDetailsByOrderId(obj);
        }
        public Task<List<Order>> GetOrderByUserId(Order obj)
        {
            return _OrderRepository.GetOrderByUserId(obj);
        }
        public Task<List<Order>> GetOrderDetailsByUserId(Order obj)
        {
            return _OrderRepository.GetOrderDetailsByUserId(obj);
        }
        public Task<List<Order>> GetAllOrder(Order obj)
        {
            return _OrderRepository.GetAllOrder(obj);
        }
        public Task<List<Order>> GetAllOrderDetails(Order obj)
        {
            return _OrderRepository.GetAllOrderDetails(obj);
        }
        public Task<int> UpdateOrderDetailStatus(OrderStatusHistory obj)
        {
            return _OrderRepository.UpdateOrderDetailStatus(obj);
        }
        public Task<List<Order>> GetDashboardSummary()
        {
            return _OrderRepository.GetDashboardSummary();
        }
        public Task<List<Order>> GetSuccessOrderDetailsByOrderId(Order obj)
        {
            return _OrderRepository.GetSuccessOrderDetailsByOrderId(obj);
        }
    }
}
