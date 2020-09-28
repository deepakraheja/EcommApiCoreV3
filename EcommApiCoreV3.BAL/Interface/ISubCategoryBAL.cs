using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.BAL.Interface
{
    public interface ISubCategoryBAL
    {
       
        Task<List<SubCategory>> GetSubcategoryByCatid(SubCategory obj);
        Task<List<SubCategory>> GetSideSubcategory(SubCategory obj);
    }
}
