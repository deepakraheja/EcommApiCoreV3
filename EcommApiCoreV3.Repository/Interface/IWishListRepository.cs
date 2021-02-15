using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IWishListRepository
    {
        Task<int> AddToWishList(Cart obj);
        Task<List<Cart>> DelWishListById(Cart obj);
        Task<List<Cart>> GetWishListByUserId(Cart obj);
    }
}
