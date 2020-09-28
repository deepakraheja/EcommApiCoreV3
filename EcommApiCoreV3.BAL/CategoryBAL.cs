using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class CategoryBAL : ICategoryBAL
    {
        ICategoryRepository _CategoryRepository;

        public CategoryBAL(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }
        public Task<List<Category>> GetMainCategory(Category obj)
        {
            return _CategoryRepository.GetMainCategory(obj);
        }
        public Task<List<Category>> GetAllMainCategory(Category obj)
        {
            return _CategoryRepository.GetAllMainCategory(obj);
        }
        public Task<int> SaveMainCategory(Category obj)
        {
            return _CategoryRepository.SaveMainCategory(obj);
        }
        public Task<List<Category>> GetCategory(Category obj)
        {
            return _CategoryRepository.GetCategory(obj);
        }
        public Task<List<Category>> GetAllCategory(Category obj)
        {
            return _CategoryRepository.GetAllCategory(obj);
        }
        public Task<int> SaveCategory(Category obj)
        {
            return _CategoryRepository.SaveCategory(obj);
        }

        public Task<List<Category>> GetSubCategory(Category obj)
        {
            return _CategoryRepository.GetSubCategory(obj);
        }
        public Task<List<Category>> GetAllSubCategory(Category obj)
        {
            return _CategoryRepository.GetAllSubCategory(obj);
        }
        public Task<int> SaveSubCategory(Category obj)
        {
            return _CategoryRepository.SaveSubCategory(obj);
        }
        public Task<List<MainCategoryJson>> SelecteMainCategoryforJson()
        {
            return _CategoryRepository.SelecteMainCategoryforJson();
        }

        public Task<List<CategoryJson>> SelecteCategoryforJson(int MaincategoryId)
        {
            return _CategoryRepository.SelecteCategoryforJson(MaincategoryId);
        }

        public Task<List<SubCategoryJson>> SelecteSubCategoryforJson(int CategoryId)
        {
            return _CategoryRepository.SelecteSubCategoryforJson(CategoryId);
        }
    }
}
