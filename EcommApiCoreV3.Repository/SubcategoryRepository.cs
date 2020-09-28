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
    public class SubCategoryRepository : BaseRepository, ISubCategoryRepository
    {


        public async Task<List<SubCategory>> GetSubcategoryByCatid(SubCategory obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                parameters.Add("@CategoryID", obj.CategoryID);

                List<SubCategory> lst = (await SqlMapper.QueryAsync<SubCategory>(con, "p_Subcategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<SubCategory>> GetSideSubcategory(SubCategory obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (obj.SideSubCategory != null)
                    parameters.Add("@SideSubCategory", obj.SideSubCategory);

                List<SubCategory> lst = (await SqlMapper.QueryAsync<SubCategory>(con, "p_SideSubcategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
