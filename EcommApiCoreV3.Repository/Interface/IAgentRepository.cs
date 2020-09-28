using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IAgentRepository
    {
        Task<int> AgentRegistration(Agents obj);
        Task<int> UpdateAgent(Agents obj);
        Task<List<Users>> GetAgentInfo(Users obj);
        Task<int> SaveAgentCustomer(Agents obj);
        Task<List<Users>> ValidAgentLogin(Users obj);
    }
}
