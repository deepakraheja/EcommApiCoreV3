using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IEmailTemplateRepository
    {
        Task<int> SaveEmailTemplate(EmailTemplate obj);
        Task<List<EmailTemplate>> GetEmailTemplate(EmailTemplate obj);
    }
}
