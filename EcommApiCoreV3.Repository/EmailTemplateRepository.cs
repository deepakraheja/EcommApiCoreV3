using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;
using static System.Data.CommandType;

namespace EcommApiCoreV3.Repository
{
    public class EmailTemplateRepository : BaseRepository, IEmailTemplateRepository
    {
        public async Task<int> SaveEmailTemplate(EmailTemplate obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmailType", obj.EmailType);
                parameters.Add("@Template", obj.Template);
                var res = await SqlMapper.ExecuteAsync(con, "p_EmailTemplate_Ins", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<EmailTemplate>> GetEmailTemplate(EmailTemplate obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmailType", obj.EmailType);
                List<EmailTemplate> lst = (await SqlMapper.QueryAsync<EmailTemplate>(con, "p_EmailTemplateGet", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
