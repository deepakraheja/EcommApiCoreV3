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
    public class FabricController : BaseController<FabricController>
    {
        IFabricBAL _Fabric;
        public FabricController(IFabricBAL Fabric)
        {
            _Fabric = Fabric;

        }

        [HttpPost]
        [Route("GetFabric")]
        public async Task<List<Fabric>> GetFabric([FromBody] Fabric obj)
        {
            try
            {
                return await this._Fabric.GetFabric(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside FabricController GetFabric action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside FabricController GetFabric action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllFabric")]
        public async Task<List<Fabric>> GetAllFabric([FromBody] Fabric obj)
        {
            try
            {
                return await this._Fabric.GetAllFabric(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside FabricController GetAllFabric action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside FabricController GetAllFabric action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveFabric")]
        public async Task<int> SaveFabric([FromBody] Fabric obj)
        {
            try
            {
                return await this._Fabric.SaveFabric(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside FabricController SaveFabric action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside FabricController SaveFabric action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("GetFabricType")]
        public async Task<List<LookupFabricType>> GetFabricType([FromBody] LookupFabricType obj)
        {
            try
            {
                return await this._Fabric.GetFabricType(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside FabricController GetFabricType action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside FabricController GetFabricType action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllFabricType")]
        public async Task<List<LookupFabricType>> GetAllFabricType([FromBody] LookupFabricType obj)
        {
            try
            {
                return await this._Fabric.GetAllFabricType(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside FabricController GetAllFabricType action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside FabricController GetAllFabricType action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveFabricType")]
        public async Task<int> SaveFabricType([FromBody] LookupFabricType obj)
        {
            try
            {
                return await this._Fabric.SaveFabricType(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside FabricController SaveFabricType action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside FabricController SaveFabricType action: {ex.Message}");
                return -1;
            }
        }
    }
}
