using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;

namespace EcommApiCoreV3.BAL
{
    public class UsersBAL : IUsersBAL
    {
        IUsersRepository _users;
        public UsersBAL(IUsersRepository users)
        {
            _users = users;
        }

        public Task<int> UserRegistration(Users obj)
        {
            return _users.UserRegistration(obj);
        }
        public Task<List<Users>> ValidLogin(Users obj)
        {
            return _users.ValidLogin(obj);
        }
        public Task<List<Users>> AdminValidLogin(Users obj)
        {
            return _users.AdminValidLogin(obj);
        }
        public Task<List<Users>> GetAllUsers()
        {
            return _users.GetAllUsers();
        }
        public Task<List<Users>> GetAllCusotmers()
        {
            return _users.GetAllCusotmers();
        }
        public Task<int> UpdateUser(Users obj)
        {
            return _users.UpdateUser(obj);
        }
        public Task<int> UpdatePwd(Users obj)
        {
            return _users.UpdatePwd(obj);
        }
        public Task<List<Users>> GetUserInfo(Users obj)
        {
            return _users.GetUserInfo(obj);
        }
        public Task<List<Users>> ValidEmail(Users obj)
        {
            return _users.ValidEmail(obj);
        }
        public Task<List<Users>> ResetPassword(Users obj)
        {
            return _users.ResetPassword(obj);
        }
        public Task<List<Users>> CheckMobileAlreadyRegisteredOrNot(Users obj)
        {
            return _users.CheckMobileAlreadyRegisteredOrNot(obj);
        }
        public Task<int> InsertOtp(OtpLog obj)
        {
            return _users.InsertOtp(obj);
        }
        public Task<int> Verifymobileotp(OtpLog obj)
        {
            return _users.Verifymobileotp(obj);
        }
        public Task<List<Users>> GetAgentCustomer(Users obj)
        {
            return _users.GetAgentCustomer(obj);
        }
        public Task<List<Users>> GetUserPages(Users obj)
        {
            return _users.GetUserPages(obj);
        }
        public Task<List<Users>> GetAgentCustomerByAgentId(Users obj)
        {
            return _users.GetAgentCustomerByAgentId(obj);
        }
        public Task<int> AgentCustomerStatusChange(Users obj)
        {
            return _users.AgentCustomerStatusChange(obj);
        }

        public Task<List<Users>> GetUserAccess(Users obj)
        {
            return _users.GetUserAccess(obj);
        }
        public Task<int> SaveUserFunctions(Users obj)
        {
            return _users.SaveUserFunctions(obj);
        }
        public Task<int> UserRegistrationByAdmin(Users obj)
        {
            return _users.UserRegistrationByAdmin(obj);
        }
    }
}
