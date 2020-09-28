using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class BrandBAL : IBrandBAL
    {
        IBrandRepository _BrandRepository;

        public BrandBAL(IBrandRepository BrandRepository)
        {
            _BrandRepository = BrandRepository;
        }

        public Task<List<Brand>> GetBrand(Brand obj)
        {
            return _BrandRepository.GetBrand(obj);
        }

        public Task<List<Brand>> GetAllBrand(Brand obj)
        {
            return _BrandRepository.GetAllBrand(obj);
        }

        public Task<int> SaveBrand(Brand obj)
        {
            return _BrandRepository.SaveBrand(obj);
        }

    }
}
