using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.BAL.Interface
{
    public interface ITransportBAL
    {
        Task<List<Transport>> GetTransport(Transport obj);
        Task<List<Transport>> GetAllTransport();
        Task<int> SaveTransport(Transport obj);
    }
}
