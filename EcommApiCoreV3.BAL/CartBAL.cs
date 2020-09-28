using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class CartBAL : ICartBAL
    {
        ICartRepository _ICartRepository;

        public CartBAL(ICartRepository ICartRepository)
        {
            _ICartRepository = ICartRepository;
        }

        public Task<int> AddToCart(Cart obj)
        {
            return _ICartRepository.AddToCart(obj);
        }
        public Task<List<Cart>> DelCartById(Cart obj)
        {
            return _ICartRepository.DelCartById(obj);
        }
        public Task<List<Cart>> GetCartById(Cart obj)
        {
            return _ICartRepository.GetCartById(obj);
        }
        public Task<List<Cart>> GetCartByUserId(Cart obj)
        {
            return _ICartRepository.GetCartByUserId(obj);
        }
        public Task<List<Cart>> GetCartProcessedById(Cart obj)
        {
            return _ICartRepository.GetCartProcessedById(obj);
        }
    }
}
