using System.Collections.Generic;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface ITransportRepository
    {
        Task<List<Transport>> GetTransport(Transport obj);
        Task<List<Transport>> GetAllTransport();
        Task<int> SaveTransport(Transport obj);
    }
}
