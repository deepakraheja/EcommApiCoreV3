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
    public class LookupTagRepository : BaseRepository, ILookupTagRepository
    {

        public async Task<List<LookupTag>> GetTag(LookupTag obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                List<LookupTag> lst = (await SqlMapper.QueryAsync<LookupTag>(con, "p_Tag_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<LookupTag>> GetAllTag(LookupTag obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<LookupTag> lst = (await SqlMapper.QueryAsync<LookupTag>(con, "p_Tag_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveTag(LookupTag obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TagId", obj.TagId);
                parameters.Add("@TagName", obj.TagName);
                parameters.Add("@Description", obj.Description);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_Tag_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
