using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class LookupBAL : ILookupBAL
    {
        ILookupRepository _lookupRepository;
        public LookupBAL(ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }

        public Task<List<LookupColor>> GetActiveColor()
        {
            return _lookupRepository.GetActiveColor();
        }
        public Task<List<LookupSize>> GetActiveSize()
        {
            return _lookupRepository.GetActiveSize();
        }
        public Task<List<LookupOrderStatus>> GetOrderStatus()
        {
            return _lookupRepository.GetOrderStatus();
        }
    }
}
