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
    public class CartRepository : BaseRepository, ICartRepository
    {
        public async Task<int> AddToCart(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserID);
                parameters.Add("@ProductSizeId", obj.ProductSizeId);
                parameters.Add("@Quantity", obj.Quantity);
                var res = await SqlMapper.ExecuteAsync(con, "p_AddToCart", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Cart>> DelCartById(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CartId", obj.CartId);
                parameters.Add("@UserId", obj.UserID);
                parameters.Add("@SetNo", obj.SetNo);
                parameters.Add("@SetType", obj.SetType);
                parameters.Add("@ProductID", obj.ProductId);
                List<Cart> lst = (await SqlMapper.QueryAsync<Cart>(con, "p_DelCartById", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Cart>> GetCartById(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserID);
                List<Cart> lst = (await SqlMapper.QueryAsync<Cart>(con, "p_GetCartById", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Cart>> GetCartByUserId(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserID);
                List<Cart> lst = (await SqlMapper.QueryAsync<Cart>(con, "p_Cart_selbyUserId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Cart>> GetCartProcessedById(Cart obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", obj.UserID);
                List<Cart> lst = (await SqlMapper.QueryAsync<Cart>(con, "p_GetCartProcessedById", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
