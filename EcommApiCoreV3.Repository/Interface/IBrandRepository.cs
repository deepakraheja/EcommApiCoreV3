using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IBrandRepository
    {
        
        Task<List<Brand>> GetBrand(Brand obj);

        Task<List<Brand>> GetAllBrand(Brand obj);
        Task<int> SaveBrand(Brand obj);
    }
}
