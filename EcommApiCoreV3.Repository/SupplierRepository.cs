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
    public class SupplierRepository : BaseRepository, ISupplierRepository
    {
        public async Task<List<Supplier>> GetSupplier(Supplier obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                List<Supplier> lst = (await SqlMapper.QueryAsync<Supplier>(con, "p_Supplier_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Supplier>> GetAllSupplier()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Supplier> lst = (await SqlMapper.QueryAsync<Supplier>(con, "p_Supplier_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<int> SaveSupplier(Supplier obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (obj.SupplierID > 0)
                    parameters.Add("@SupplierID", obj.SupplierID);
                parameters.Add("@CompanyName", obj.CompanyName);
                parameters.Add("@Name", obj.Name);
                parameters.Add("@Designation", obj.Designation);
                parameters.Add("@Address1", obj.Address1);
                parameters.Add("@City", obj.City);
                parameters.Add("@STATE", obj.State);
                parameters.Add("@Country", obj.Country);
                parameters.Add("@PostalCode", obj.PostalCode);
                parameters.Add("@Phone", obj.Phone);
                parameters.Add("@Email", obj.Email);
                parameters.Add("@PostalCode", obj.PostalCode);
                parameters.Add("@PaymentMethod", obj.PaymentMethod);
                parameters.Add("@Notes", obj.Notes);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_Supplier_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
