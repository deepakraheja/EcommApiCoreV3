using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class BillingAddressBAL : IBillingAddressBAL
    {
        IBillingAddressRepository _BillingAddressRepository;

        public BillingAddressBAL(IBillingAddressRepository BillingAddressRepository)
        {
            _BillingAddressRepository = BillingAddressRepository;
        }

        public Task<List<BillingAddress>> SaveBillingAddress(BillingAddress obj)
        {
            return _BillingAddressRepository.SaveBillingAddress(obj);
        }
        public Task<List<BillingAddress>> GetBillingAddress(BillingAddress obj)
        {
            return _BillingAddressRepository.GetBillingAddress(obj);
        }
        public Task<List<BillingAddress>> DeleteBillingAddress(BillingAddress obj)
        {
            return _BillingAddressRepository.DeleteBillingAddress(obj);
        }
    }
}
