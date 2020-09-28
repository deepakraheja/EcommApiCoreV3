using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.BAL.Interface
{
    public interface ISupplierBAL
    {
        Task<List<Supplier>> GetSupplier(Supplier obj);
        Task<List<Supplier>> GetAllSupplier();
        Task<int> SaveSupplier(Supplier obj);
    }
}
