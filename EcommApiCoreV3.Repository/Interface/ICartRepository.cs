using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface ICartRepository
    {
        Task<int> AddToCart(Cart obj);
        Task<List<Cart>> DelCartById(Cart obj);
        Task<List<Cart>> GetCartById(Cart obj);
        Task<List<Cart>> GetCartByUserId(Cart obj);
        Task<List<Cart>> GetCartProcessedById(Cart obj);
    }
}
