using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class AgentBAL : IAgentBAL
    {
        IAgentRepository _IAgentRepository;

        public AgentBAL(IAgentRepository IAgentRepository)
        {
            _IAgentRepository = IAgentRepository;
        }
        public Task<int> AgentRegistration(Agents obj)
        {
            return _IAgentRepository.AgentRegistration(obj);
        }
        public Task<int> UpdateAgent(Agents obj)
        {
            return _IAgentRepository.UpdateAgent(obj);
        }
        public Task<List<Users>> GetAgentInfo(Users obj)
        {
            return _IAgentRepository.GetAgentInfo(obj);
        }
        public Task<int> SaveAgentCustomer(Agents obj)
        {
            return _IAgentRepository.SaveAgentCustomer(obj);
        }
        public Task<List<Users>> ValidAgentLogin(Users obj)
        {
            return _IAgentRepository.ValidAgentLogin(obj);
        }
    }
}
