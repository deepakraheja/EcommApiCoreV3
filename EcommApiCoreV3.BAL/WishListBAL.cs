using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class WishListBAL : IWishListBAL
    {
        IWishListRepository _IWishListRepository;

        public WishListBAL(IWishListRepository IWishListRepository)
        {
            _IWishListRepository = IWishListRepository;
        }

        public Task<int> AddToWishList(Cart obj)
        {
            return _IWishListRepository.AddToWishList(obj);
        }
        public Task<List<Cart>> DelWishListById(Cart obj)
        {
            return _IWishListRepository.DelWishListById(obj);
        }
        public Task<List<Cart>> GetWishListByUserId(Cart obj)
        {
            return _IWishListRepository.GetWishListByUserId(obj);
        }
    }
}
