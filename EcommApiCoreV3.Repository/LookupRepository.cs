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
    public class LookupRepository : BaseRepository, ILookupRepository
    {
        public async Task<List<LookupColor>> GetActiveColor()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<LookupColor> lst = (await SqlMapper.QueryAsync<LookupColor>(con, "p_LookupColor_Sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<LookupSize>> GetActiveSize()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<LookupSize> lst = (await SqlMapper.QueryAsync<LookupSize>(con, "p_LookupSize_Sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<LookupOrderStatus>> GetOrderStatus()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<LookupOrderStatus> lst = (await SqlMapper.QueryAsync<LookupOrderStatus>(con, "p_LookupOrderStatus_Sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<LookupHSN>> GetHSN()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<LookupHSN> lst = (await SqlMapper.QueryAsync<LookupHSN>(con, "p_LookupHSN_Sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
