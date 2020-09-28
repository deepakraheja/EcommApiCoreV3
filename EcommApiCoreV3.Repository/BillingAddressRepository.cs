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
    public class BillingAddressRepository : BaseRepository, IBillingAddressRepository
    {
        public async Task<List<BillingAddress>> SaveBillingAddress(BillingAddress obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BillingAddressId", obj.BillingAddressId);
                parameters.Add("@UserID", obj.UserID);
                parameters.Add("@FName", obj.FName);
                parameters.Add("@LName", obj.LName);
                parameters.Add("@CompanyName", obj.CompanyName);
                parameters.Add("@Address", obj.Address);
                parameters.Add("@City", obj.City);
                parameters.Add("@State", obj.State);
                parameters.Add("@ZipCode", obj.ZipCode);
                parameters.Add("@EmailId", obj.EmailId);
                parameters.Add("@Phone", obj.Phone);
                parameters.Add("@Country", obj.Country);
                parameters.Add("@CreatedDate", obj.CreatedDate);
                List<BillingAddress> lst = (await SqlMapper.QueryAsync<BillingAddress>(con, "p_BillingAddress_Save", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<BillingAddress>> GetBillingAddress(BillingAddress obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                List<BillingAddress> lst = (await SqlMapper.QueryAsync<BillingAddress>(con, "p_BillingAddress_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<BillingAddress>> DeleteBillingAddress(BillingAddress obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BillingAddressId", obj.BillingAddressId);
                parameters.Add("@UserID", obj.UserID);
                List<BillingAddress> lst = (await SqlMapper.QueryAsync<BillingAddress>(con, "p_BillingAddress_del", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
