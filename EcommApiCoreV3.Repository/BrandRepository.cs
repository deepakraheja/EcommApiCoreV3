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
    public class BrandRepository : BaseRepository, IBrandRepository
    {
        

        public async Task<List<Brand>> GetBrand(Brand obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                List<Brand> lst = (await SqlMapper.QueryAsync<Brand>(con, "p_Brand_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Brand>> GetAllBrand(Brand obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Brand> lst = (await SqlMapper.QueryAsync<Brand>(con, "p_Brand_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveBrand(Brand obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BrandId", obj.BrandId);
                parameters.Add("@name", obj.Name);
                parameters.Add("@Description", obj.Description);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_Brand_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }
}
