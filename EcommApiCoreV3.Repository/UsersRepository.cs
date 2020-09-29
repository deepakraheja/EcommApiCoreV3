using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;
using static System.Data.CommandType;

namespace EcommApiCoreV3.Repository
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        public async Task<int> UserRegistration(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@PASSWORD", obj.password);
                parameters.Add("@email", obj.email);
                parameters.Add("@Name", obj.Name);
                parameters.Add("@MobileNo", obj.MobileNo);

                parameters.Add("@BusinessPhone", obj.BusinessPhone);
                parameters.Add("@BusinessType", obj.BusinessType);
                parameters.Add("@Industry", obj.Industry);
                parameters.Add("@BusinessLicenseType", obj.BusinessLicenseType);
                
                parameters.Add("@GSTNo", obj.GSTNo); 
                parameters.Add("@PANNo", obj.PANNo);
                parameters.Add("@BusinessName", obj.BusinessName);
                parameters.Add("@Address1", obj.Address1);
                parameters.Add("@Address2", obj.Address2);

                parameters.Add("@PinCode", obj.PinCode);
                parameters.Add("@City", obj.City);
                parameters.Add("@State", obj.State);
                parameters.Add("@AadharCard", obj.AadharCard);

                var res = await SqlMapper.ExecuteScalarAsync(con, "p_Users_ins", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Users>> ValidLogin(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoginId", obj.LoginId);
                parameters.Add("@PASSWORD", obj.password);
                parameters.Add("@UserType", obj.UserType);
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_ValidLogin", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_Users_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> UpdateUser(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                parameters.Add("@email", obj.email);
                parameters.Add("@Name", obj.Name);
                parameters.Add("@MobileNo", obj.MobileNo);

                //parameters.Add("@IsActive", obj.IsActive);
                //parameters.Add("@IsApproval", obj.IsApproval);
                parameters.Add("@ApprovedBy", obj.ApprovedBy);
                parameters.Add("@ApprovedDate", obj.ApprovedDate);
                parameters.Add("@AdditionalDiscount", obj.AdditionalDiscount);

                parameters.Add("@BusinessType", obj.BusinessType);
                parameters.Add("@Industry", obj.Industry);
                parameters.Add("@BusinessLicenseType", obj.BusinessLicenseType);
                parameters.Add("@GSTNo", obj.GSTNo);
                parameters.Add("@PANNo", obj.PANNo);
                parameters.Add("@BusinessName", obj.BusinessName);
                parameters.Add("@Address1", obj.Address1);
                parameters.Add("@Address2", obj.Address2);
                parameters.Add("@PinCode", obj.PinCode);
                parameters.Add("@City", obj.City);
                parameters.Add("@State", obj.State);
                parameters.Add("@IsAgent", obj.IsAgent);
                parameters.Add("@StatusId", obj.StatusId);
                parameters.Add("@IsVIPMember", obj.IsVIPMember);
                var res = await SqlMapper.ExecuteScalarAsync(con, "p_Users_upd", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> UpdatePwd(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                parameters.Add("@password", obj.password);
                parameters.Add("@NewPassword", obj.NewPassword);
                var res = await SqlMapper.ExecuteScalarAsync(con, "p_UpdatePwd", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Users>> GetUserInfo(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_Users_sel_userId", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<Users>> ValidEmail(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoginId", obj.email);
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_ValidEmail", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<int> ResetPassword(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                parameters.Add("@password", obj.password);
                var res = await SqlMapper.ExecuteAsync(con, "p_ResetPassword", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
		
		public async Task<List<Users>> CheckMobileAlreadyRegisteredOrNot(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MobileNo", obj.MobileNo);
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_Users_sel_MobileNo", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public async Task<int> InsertOtp(OtpLog obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MobileNo", obj.MobileNo);
                parameters.Add("@OTP", obj.OTP);
                parameters.Add("@SessionId", obj.SessionId);
                var res = await SqlMapper.ExecuteScalarAsync(con, "p_OtpLog_ins", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        } 
         
        public async Task<int> Verifymobileotp(OtpLog obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MobileNo", obj.MobileNo);
                parameters.Add("@OTP", obj.OTP);
                
                var res = await SqlMapper.ExecuteScalarAsync(con, "p_CheckOtp_mobile", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Users>> GetAgentCustomer(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AgentId", obj.AgentId);
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_AgentCustomer_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Users>> GetAgentCustomerByAgentId(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AgentId", obj.AgentId);
                List<Users> lst = (await SqlMapper.QueryAsync<Users>(con, "p_GetAgentCustomer_sel", param: parameters, commandType: StoredProcedure)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<int> AgentCustomerStatusChange(Users obj)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", obj.UserID);
                parameters.Add("@AdditionalDiscount", obj.AdditionalDiscount);
                parameters.Add("@StatusId", obj.StatusId);
                var res = await SqlMapper.ExecuteAsync(con, "p_AgentCustomerStatusChange", param: parameters, commandType: StoredProcedure);
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
