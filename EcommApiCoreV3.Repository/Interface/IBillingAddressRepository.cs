using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IBillingAddressRepository
    {
        Task<List<BillingAddress>> SaveBillingAddress(BillingAddress obj);
        Task<List<BillingAddress>> GetBillingAddress(BillingAddress obj);
        Task<List<BillingAddress>> DeleteBillingAddress(BillingAddress obj);
    }
}
