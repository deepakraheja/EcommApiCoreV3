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
    public class TransportRepository:BaseRepository, ITransportRepository
    {
        public async Task<List<Transport>> GetTransport(Transport obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                List<Transport> lst = (await SqlMapper.QueryAsync<Transport>(con, "p_Transport_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Transport>> GetAllTransport()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Transport> lst = (await SqlMapper.QueryAsync<Transport>(con, "p_Transport_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<int> SaveTransport(Transport obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (obj.TransportID > 0)
                    parameters.Add("@TransportID", obj.TransportID);
                parameters.Add("@Name", obj.Name);
                parameters.Add("@Phone", obj.Phone);
                parameters.Add("@Address", obj.Address);
                parameters.Add("@City", obj.City);
                parameters.Add("@STATE", obj.State);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_Transport_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
