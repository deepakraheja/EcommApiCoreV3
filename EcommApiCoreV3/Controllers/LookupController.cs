using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class LookupController : BaseController<LookupController>
    {
        ILookupBAL _lookupBAL;
        public LookupController(ILookupBAL lookupBAL)
        {
            _lookupBAL = lookupBAL;
        }

        [HttpPost]
        [Route("GetActiveColor")]
        public async Task<List<LookupColor>> GetActiveColor()
        {
            return await _lookupBAL.GetActiveColor();
        }
        [HttpPost]
        [Route("GetActiveSize")]
        public async Task<List<LookupSize>> GetActiveSize()
        {
            return await _lookupBAL.GetActiveSize();
        }
        [HttpPost]
        [Route("GetOrderStatus")]
        public async Task<List<LookupOrderStatus>> GetOrderStatus()
        {
            return await _lookupBAL.GetOrderStatus();
        }
    }
}
