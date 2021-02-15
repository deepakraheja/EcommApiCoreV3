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
    public class WishListRepository : BaseRepository, IWishListRepository
    {
        public async Task<int> AddToWishList(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserID);
                parameters.Add("@ProductSizeId", obj.ProductSizeId);
                var res = await SqlMapper.ExecuteAsync(con, "p_WishList_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Cart>> DelWishListById(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@WishListId", obj.WishListId);
                parameters.Add("@UserId", obj.UserID);
                List<Cart> lst = (await SqlMapper.QueryAsync<Cart>(con, "p_DelWishListById", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Cart>> GetWishListByUserId(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserID);
                List<Cart> lst = (await SqlMapper.QueryAsync<Cart>(con, "p_WishList_selbyUserId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
