using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class TransportController : BaseController<TransportController>
    {
        ITransportBAL _Transport;
        public TransportController(ITransportBAL Transport)
        {
            _Transport = Transport;
        }
        [HttpPost]
        [Route("GetTransport")]
        public async Task<List<Transport>> GetTransport([FromBody] Transport obj)
        {
            try
            {
                return await this._Transport.GetTransport(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside TransportController GetTransport action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside TransportController GetTransport action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("GetAllTransport")]
        public async Task<List<Transport>> GetAllTransport()
        {
            try
            {
                return await this._Transport.GetAllTransport();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside TransportController GetAllTransport action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside TransportController GetAllTransport action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("SaveTransport")]
        public async Task<int> SaveTransport([FromBody] Transport obj)
        {
            try
            {
                return await this._Transport.SaveTransport(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside TransportController SaveTransport action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside TransportController SaveTransport action: {ex.Message}");
                return -1;
            }
        }
    }
}