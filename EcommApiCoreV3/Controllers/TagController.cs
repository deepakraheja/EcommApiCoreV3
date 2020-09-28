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
    public class TagController : BaseController<TagController>
    {
        ILookupTagBAL _lookupTag;
        public TagController(ILookupTagBAL lookupTag)
        {
            _lookupTag = lookupTag;

        }

        [HttpPost]
        [Route("GetTag")]
        public async Task<List<LookupTag>> GetTag([FromBody] LookupTag obj)
        {
            try
            {
                return await this._lookupTag.GetTag(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside TagController GetTag action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside TagController GetTag action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllTag")]
        public async Task<List<LookupTag>> GetAllTag([FromBody] LookupTag obj)
        {
            try
            {
                return await this._lookupTag.GetAllTag(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside TagController GetAllTag action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside TagController GetAllTag action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveTag")]
        public async Task<int> SaveTag([FromBody] LookupTag obj)
        {
            try
            {
                return await this._lookupTag.SaveTag(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside TagController SaveTag action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside TagController SaveTag action: {ex.Message}");
                return -1;
            }
        }
    }
}
