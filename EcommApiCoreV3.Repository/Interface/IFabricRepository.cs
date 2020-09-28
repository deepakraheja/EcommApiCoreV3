using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IFabricRepository
    {
        Task<List<Fabric>> GetFabric(Fabric obj);
        Task<List<Fabric>> GetAllFabric(Fabric obj);
        Task<int> SaveFabric(Fabric obj);
        Task<List<LookupFabricType>> GetFabricType(LookupFabricType obj);
        Task<List<LookupFabricType>> GetAllFabricType(LookupFabricType obj);
        Task<int> SaveFabricType(LookupFabricType obj);
    }
}
