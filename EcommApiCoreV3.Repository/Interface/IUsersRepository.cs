using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface IUsersRepository
    {
        Task<List<Users>> GetUserAccess(Users obj);
        Task<int> UserRegistration(Users obj);
        Task<List<Users>> ValidLogin(Users obj);
        Task<List<Users>> GetAllUsers();
        Task<List<Users>> GetAllCusotmers();
        Task<int> UpdateUser(Users obj);
        Task<int> UpdatePwd(Users obj);
        Task<List<Users>> GetUserInfo(Users obj);
        Task<List<Users>> ValidEmail(Users obj);
        Task<List<Users>> ResetPassword(Users obj);
        Task<List<Users>> CheckMobileAlreadyRegisteredOrNot(Users obj);
        Task<int> InsertOtp(OtpLog obj);
        Task<int> Verifymobileotp(OtpLog obj);
        Task<List<Users>> GetAgentCustomer(Users obj);
        Task<List<Users>> GetUserPages(Users obj);
        Task<List<Users>> GetAgentCustomerByAgentId(Users obj);
        Task<int> AgentCustomerStatusChange(Users obj);
        Task<int> SaveUserFunctions(Users obj);
        Task<int> UserRegistrationByAdmin(Users obj);
    }
}
