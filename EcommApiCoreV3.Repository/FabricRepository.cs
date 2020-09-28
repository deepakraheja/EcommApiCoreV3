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
    public class FabricRepository : BaseRepository, IFabricRepository
    {
        public async Task<List<Fabric>> GetFabric(Fabric obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@IsActive", obj.IsActive);
                List<Fabric> lst = (await SqlMapper.QueryAsync<Fabric>(con, "p_Fabric_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Fabric>> GetAllFabric(Fabric obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Fabric> lst = (await SqlMapper.QueryAsync<Fabric>(con, "p_Fabric_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveFabric(Fabric obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FabricId", obj.FabricId);
                parameters.Add("@name", obj.Name);
                parameters.Add("@IsActive", obj.IsActive);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_Fabric_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<LookupFabricType>> GetFabricType(LookupFabricType obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                parameters.Add("@FabricId", obj.FabricId);
                List<LookupFabricType> lst = (await SqlMapper.QueryAsync<LookupFabricType>(con, "p_FabricType_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<LookupFabricType>> GetAllFabricType(LookupFabricType obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                //parameters.Add("@FabricId", obj.FabricId);
                if (obj.Active)
                    parameters.Add("@Active", obj.Active);
                List<LookupFabricType> lst = (await SqlMapper.QueryAsync<LookupFabricType>(con, "p_FabricType_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveFabricType(LookupFabricType obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FabricTypeId", obj.FabricTypeId);
                parameters.Add("@FabricId", obj.FabricId);
                parameters.Add("@FabricType", obj.FabricType);
                parameters.Add("@Description", obj.Description);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_FabricType_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
