using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class SubCategoryBAL : ISubCategoryBAL
    {
        ISubCategoryRepository _SubCategoryRepository;

        public SubCategoryBAL(ISubCategoryRepository SubCategoryRepository)
        {
            _SubCategoryRepository = SubCategoryRepository;
        }
      
        public Task<List<SubCategory>> GetSubcategoryByCatid(SubCategory obj)
        {
            return _SubCategoryRepository.GetSubcategoryByCatid(obj);
        }

        public Task<List<SubCategory>> GetSideSubcategory(SubCategory obj)
        {
            return _SubCategoryRepository.GetSideSubcategory(obj);
        }
    }
}
