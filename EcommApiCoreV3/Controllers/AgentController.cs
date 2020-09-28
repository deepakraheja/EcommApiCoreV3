using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.JWT;

namespace EcommApiCoreV3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AgentController : BaseController<AgentController>
    {
        IAgentBAL _Agent;
        private readonly ApplicationSettings _appSettings;

        public AgentController(IAgentBAL Agent, IOptions<ApplicationSettings> appSettings)
        {
            _Agent = Agent;
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        [Route("AgentRegistration")]
        public async Task<int> AgentRegistration([FromBody] Agents obj)
        {
            try
            {
                return await this._Agent.AgentRegistration(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside AgentController AgentRegistration action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside AgentController AgentRegistration action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("UpdateAgent")]
        public async Task<int> UpdateAgent([FromBody] Agents obj)
        {
            try
            {
                return await this._Agent.UpdateAgent(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside AgentController UpdateAgent action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside AgentController UpdateAgent action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("GetAgentInfo")]
        public async Task<List<Users>> GetAgentInfo([FromBody] Users obj)
        {
            try
            {
                return await this._Agent.GetAgentInfo(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside AgentController GetAgentInfo action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside AgentController GetAgentInfo action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("SaveAgentCustomer")]
        public async Task<int> SaveAgentCustomer([FromBody] Agents obj)
        {
            try
            {
                return await this._Agent.SaveAgentCustomer(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside AgentController SaveAgentCustomer action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside AgentController SaveAgentCustomer action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("ValidAgentLogin")]
        [AllowAnonymous]
        public async Task<List<Users>> ValidAgentLogin([FromBody] Users obj)
        {
            try
            {
                List<Users> lstLogin = new List<Users>();
                lstLogin = await this._Agent.ValidAgentLogin(obj);
                if (lstLogin.Count > 0)
                {
                    AuthorizeService auth = new AuthorizeService();
                    string _token = auth.Authenticate(Convert.ToString(lstLogin[0].UserID), _appSettings);
                    lstLogin[0].Token = _token;
                }
                return lstLogin;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside AgentController ValidAgentLogin action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside AgentController ValidAgentLogin action: {ex.Message}");
                return null;
            }
        }
    }
}
