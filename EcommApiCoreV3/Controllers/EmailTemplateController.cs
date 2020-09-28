using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class EmailTemplateController : BaseController<EmailTemplateController>
    {
        IEmailTemplateBAL _IEmailTemplateBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;
        public EmailTemplateController(IHostingEnvironment hostingEnvironment, IEmailTemplateBAL EmailTemplateBAL)
        {
            _IEmailTemplateBAL = EmailTemplateBAL;
            webRootPath = hostingEnvironment.WebRootPath;
        }

        [HttpPost]
        [Route("SaveEmailTemplate")]
        public async Task<int> SaveEmailTemplate([FromBody] EmailTemplate obj)
        {
            try
            {
                return await this._IEmailTemplateBAL.SaveEmailTemplate(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside EmailTemplateController SaveEmailTemplate action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside EmailTemplateController SaveEmailTemplate action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("GetEmailTemplate")]
        public async Task<List<EmailTemplate>> GetEmailTemplate([FromBody] EmailTemplate obj)
        {
            try
            {
                return await this._IEmailTemplateBAL.GetEmailTemplate(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside EmailTemplateController GetEmailTemplate action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside EmailTemplateController GetEmailTemplate action: {ex.Message}");
                return null;
            }
        }
    }
}
