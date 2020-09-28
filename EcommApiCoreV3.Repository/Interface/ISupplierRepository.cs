using System.Collections.Generic;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetSupplier(Supplier obj);
        Task<List<Supplier>> GetAllSupplier();
        Task<int> SaveSupplier(Supplier obj);
    }
}
