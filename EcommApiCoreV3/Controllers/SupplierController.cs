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
    public class SupplierController : BaseController<SupplierController>
    {
        ISupplierBAL _supplier;
        public SupplierController(ISupplierBAL supplier)
        {
            _supplier = supplier;
        }
        [HttpPost]
        [Route("GetSupplier")]
        public async Task<List<Supplier>> GetSupplier([FromBody] Supplier obj)
        {
            try
            {
                return await this._supplier.GetSupplier(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SupplierController GetSupplier action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside SupplierController GetSupplier action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("GetAllSupplier")]
        public async Task<List<Supplier>> GetAllSupplier()
        {
            try
            {
                return await this._supplier.GetAllSupplier();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SupplierController GetAllSupplier action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside SupplierController GetAllSupplier action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("SaveSupplier")]
        public async Task<int> SaveSupplier([FromBody] Supplier obj)
        {
            try
            {
                return await this._supplier.SaveSupplier(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SupplierController SaveSupplier action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside SupplierController SaveSupplier action: {ex.Message}");
                return -1;
            }
        }
    }
}
