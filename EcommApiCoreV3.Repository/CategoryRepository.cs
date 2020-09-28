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
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {

        public async Task<List<MainCategoryJson>> SelecteMainCategoryforJson()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", 1);
                List<MainCategoryJson> lst = (await SqlMapper.QueryAsync<MainCategoryJson>(con, "p_MainCategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<CategoryJson>> SelecteCategoryforJson(int MaincategoryId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", 1);
                parameters.Add("@MaincategoryId", MaincategoryId);
                List<CategoryJson> lst = (await SqlMapper.QueryAsync<CategoryJson>(con, "p_CategoryJson_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public async Task<List<SubCategoryJson>> SelecteSubCategoryforJson(int CategoryId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", 1);
                parameters.Add("@CategoryId", CategoryId);
                List<SubCategoryJson> lst = (await SqlMapper.QueryAsync<SubCategoryJson>(con, "p_SubCategoryJson_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Category>> GetMainCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                List<Category> lst = (await SqlMapper.QueryAsync<Category>(con, "p_MainCategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Category>> GetAllMainCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Category> lst = (await SqlMapper.QueryAsync<Category>(con, "p_MainCategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveMainCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MainCategoryID", obj.MainCategoryID);
                parameters.Add("@MainCategoryName", obj.MainCategoryName);
                parameters.Add("@Description", obj.Description);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_MainCategory_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Category>> GetCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                parameters.Add("@MainCategoryID", obj.MainCategoryID);
                List<Category> lst = (await SqlMapper.QueryAsync<Category>(con, "p_category_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Category>> GetAllCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MainCategoryID", obj.MainCategoryID);
                List<Category> lst = (await SqlMapper.QueryAsync<Category>(con, "p_category_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryID", obj.CategoryID);
                parameters.Add("@MainCategoryID", obj.MainCategoryID);
                parameters.Add("@CategoryName", obj.CategoryName);
                parameters.Add("@Description", obj.Description);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_Category_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Category>> GetSubCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Active", obj.Active);
                parameters.Add("@CategoryID", obj.CategoryID);
                List<Category> lst = (await SqlMapper.QueryAsync<Category>(con, "p_Subcategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Category>> GetAllSubCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryID", obj.CategoryID);
                List<Category> lst = (await SqlMapper.QueryAsync<Category>(con, "p_Subcategory_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> SaveSubCategory(Category obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@SubCategoryID", obj.SubCategoryID);
                parameters.Add("@MainCategoryID", obj.MainCategoryID);
                parameters.Add("@CategoryID", obj.CategoryID);
                parameters.Add("@name", obj.Name);
                parameters.Add("@Description", obj.Description);
                parameters.Add("@Active", obj.Active);
                parameters.Add("@UserId", obj.CreatedBy);
                var res = await SqlMapper.ExecuteAsync(con, "p_SubCategory_ins", param: parameters, commandType: StoredProcedure);
                return res;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
