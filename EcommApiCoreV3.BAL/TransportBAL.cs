using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class TransportBAL : ITransportBAL
    {
        ITransportRepository _Transport;

        public TransportBAL(ITransportRepository Transport)
        {
            _Transport = Transport;
        }

        public Task<List<Transport>> GetTransport(Transport obj)
        {
            return _Transport.GetTransport(obj);
        }
        public Task<List<Transport>> GetAllTransport()
        {
            return _Transport.GetAllTransport();
        }
        public Task<int> SaveTransport(Transport obj)
        {
            return _Transport.SaveTransport(obj);
        }
    }
}
