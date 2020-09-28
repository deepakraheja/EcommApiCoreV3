using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class EmailTemplateBAL : IEmailTemplateBAL
    {
        IEmailTemplateRepository _EmailTemplateRepository;

        public EmailTemplateBAL(IEmailTemplateRepository EmailTemplateRepository)
        {
            _EmailTemplateRepository = EmailTemplateRepository;
        }

        public Task<int> SaveEmailTemplate(EmailTemplate obj)
        {
            return _EmailTemplateRepository.SaveEmailTemplate(obj);
        }

        public Task<List<EmailTemplate>> GetEmailTemplate(EmailTemplate obj)
        {
            return _EmailTemplateRepository.GetEmailTemplate(obj);
        }
    }
}
