using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EcommApiCoreV3.BAL;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.JWT;
using static EcommApiCoreV3.Controllers.Common.SendEmails;
using EcommApiCoreV3.Services;
using Microsoft.AspNetCore.Hosting;
//using mailinblue;

namespace EcommApiCoreV3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : BaseController<UsersController>
    {
        IUsersBAL _usersBAL;
        IEmailTemplateBAL _IEmailTemplateBAL;
        private readonly ApplicationSettings _appSettings;
        IOrderBAL _IOrderBAL;
        Utilities _utilities = new Utilities();
        public UsersController(IUsersBAL usersBAL, IOptions<ApplicationSettings> appSettings, IEmailTemplateBAL emailTemplateBAL, IOrderBAL OrderBAL, IWebHostEnvironment hostingEnvironment)
        {
            _usersBAL = usersBAL;
            _appSettings = appSettings.Value;
            _IEmailTemplateBAL = emailTemplateBAL;
            _IOrderBAL = OrderBAL;
            webRootPath = hostingEnvironment.WebRootPath;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpPost]
        [Route("GetUserAccess")]

        public async Task<List<Users>> GetUserAccess([FromBody] Users _obj)
        {
            try
            {
                //string page = "mngemp1";
                List<Users> _objuser = new List<Users>();

                _obj.UserID = UserService.LoggedInUser;

                if (_obj.UserID != 1069)
                {
                    //_obj.PageName = page;
                    _objuser = await _usersBAL.GetUserAccess(_obj);
                }
                else
                {
                    _obj.CanAdd = true;
                    _obj.CanUpdate = true;
                    _obj.CanDelete = true;
                    _obj.ViewOnly = true;
                    _objuser.Add(_obj);
                }

                if (_objuser.Count > 0)
                {
                    return _objuser;
                }
                else
                    throw new Exception("You dont have access to review this page.");

            }

            catch (Exception ex)
            {
                Logger.LogError($"Something went wrong inside UserController GetUserAccess action: {ex.Message}");
                return null;
            }
        }



        /*  [HttpPost] /* commented on 3 oct 2010 by deepak
          [Route("SendEmail1")]
          [AllowAnonymous]
          public void SendEmail1()
          {
              try
              {

                  API sendinBlue = new mailinblue.API("nzysR4DgbSBG7dvK"); //add your api key here 

                  Dictionary<string, Object> data = new Dictionary<string, Object>();
                  Dictionary<string, string> to = new Dictionary<string, string>();
                  to.Add("deepakrah1234@mailinator.com", "to whom!");

                  //to.Add("deepak12345@mailinator.com", "to whom!");
                  List<string> from_name = new List<string>();
                  from_name.Add("esales@vikramcreations.com");

                  //from_name.Add("from email!");
                  //List<string> attachment = new List<string>();
                  //attachment.Add("https://domain.com/path-to-file/filename1.pdf");
                  //attachment.Add("https://domain.com/path-to-file/filename2.jpg");

                  data.Add("to", to);
                  data.Add("from", from_name);
                  data.Add("subject", "hello");
                  data.Add("html", "This is the test email.");
                  //data.Add("attachment", attachment);

                  Object sendEmail = sendinBlue.send_email(data);
                  string InnerHtml = sendEmail.ToString();

              }
              catch (Exception exx)
              {
                  ErrorLogger.Log($"Something went wrong inside UsersController SendEmail action: {exx.Message}");
                  ErrorLogger.Log(exx.StackTrace);
                  string str = exx.Message.ToString();

              }

          }



          [HttpPost]
          [Route("SendEmail")]
          [AllowAnonymous]
          public void SendEmail()
          {
              string strFrom = "esales@vikramcreations.com";

              SmtpClient smtpClient = new SmtpClient();
              MailMessage message = new MailMessage();

              string userId = "Loginwecart2014@gmail.com";// Convert.ToString(ConfigurationManager.AppSettings["MailUserId"]); //MAIL ID FOR AUTHENTICATION
              string password = "nzysR4DgbSBG7dvK";// Convert.ToString(ConfigurationManager.AppSettings["MailPassword"]); ;//PASSWORD FOR AUTHENTICATION
              bool EnableSsl = true;

              bool flag = true;
              string strSub = "Hello";
              string strBody = "Hello, This is Email sending test using gmail.";

              //  string addMessage = Convert.ToString(ConfigurationManager.AppSettings["Subject"]);
              String host = "smtp-relay.sendinblue.com";// ConfigurationManager.AppSettings["mailServer"];
              MailAddress FromAddress = new MailAddress(strFrom);
              try
              {
                  smtpClient.EnableSsl = EnableSsl;//Convert.ToBoolean(EnableSsl);
                  smtpClient.Port = 587;
                  smtpClient.Host = host;
                  message.From = FromAddress;
                  message.To.Add("deepakrah1234@mailinator.com");
                  //message.CC.Add(strCc);
                  //message.Bcc.Add(strBcc);
                  message.Subject = strSub;
                  message.Body = strBody;
                  message.IsBodyHtml = true;
                  if (flag)
                  {
                      NetworkCredential oCredential = new NetworkCredential(userId, password);
                      smtpClient.UseDefaultCredentials = false;
                      smtpClient.Credentials = oCredential;
                  }
                  else
                  {
                      smtpClient.UseDefaultCredentials = true;

                  }
                  smtpClient.Send(message);

              }
              catch (Exception ex)
              {
                  string str = ex.Message.ToString();

                  ErrorLogger.Log(ex.Message);
                  ErrorLogger.Log(ex.StackTrace);

              }



              //try
              //{
              //    string smtpAddress = "smtp.gmail.com";
              //    int portNumber = 587;
              //    bool enableSSL = true;
              //    string emailFromAddress = "esales@vikramcreations.com"; //Sender Email Address  
              //    string password = "Sales@123"; //Sender Password  
              //    string emailToAddress = "deepakrahejain@gmail.com"; //Receiver Email Address  
              //    string subject = "Hello";
              //    string body = "Hello, This is Email sending test using gmail.";

              //    using (MailMessage mail = new MailMessage())
              //    {
              //        mail.From = new MailAddress(emailFromAddress);
              //        mail.To.Add(emailToAddress);
              //        mail.Subject = subject;
              //        mail.Body = body;
              //        mail.IsBodyHtml = true;
              //        //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
              //        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
              //        {
              //            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
              //            smtp.EnableSsl = enableSSL;
              //            smtp.Send(mail);

              //        }

              //    }
              //}
              //catch (Exception ex)
              //{
              //    Logger.LogError($"Something went wrong inside UsersController GetAllUsers action: {ex.Message}");

              //}
          }*/


        [HttpPost]
        [Route("SendEmailWithGmail")]
        [AllowAnonymous]
        public void SendEmailWithGmail()
        {
            string strFrom = "esales@vikramcreations.com";

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            string userId = "esales@vikramcreations.com";// Convert.ToString(ConfigurationManager.AppSettings["MailUserId"]); //MAIL ID FOR AUTHENTICATION
            string password = "Sales@123";// Convert.ToString(ConfigurationManager.AppSettings["MailPassword"]); ;//PASSWORD FOR AUTHENTICATION
            bool EnableSsl = true;

            bool flag = true;
            string strSub = "Hello";
            string strBody = "Hello, This is Email sending test using gmail.";

            //  string addMessage = Convert.ToString(ConfigurationManager.AppSettings["Subject"]);
            String host = "smtp.gmail.com";// ConfigurationManager.AppSettings["mailServer"];
            MailAddress FromAddress = new MailAddress(strFrom);
            try
            {
                smtpClient.Port = 587;
                smtpClient.EnableSsl = EnableSsl;//Convert.ToBoolean(EnableSsl);
                smtpClient.Host = host;
                message.From = FromAddress;
                message.To.Add("deepakrahejain@mailinator.com");
                //message.CC.Add(strCc);
                //message.Bcc.Add(strBcc);
                message.Subject = strSub;
                message.Body = strBody;
                message.IsBodyHtml = true;
                if (flag)
                {
                    NetworkCredential oCredential = new NetworkCredential(userId, password);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = oCredential;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = true;

                }
                smtpClient.Send(message);

            }
            catch (Exception exx)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController SendEmailWithGmail action: {exx.Message}");
                ErrorLogger.Log(exx.StackTrace);
                string str = exx.Message.ToString();

            }
        }


        [HttpPost]
        [Route("UserRegistration")]
        [AllowAnonymous]
        public async Task<int> UserRegistration([FromBody] Users obj)
        {
            try
            {
                int res = await this._usersBAL.UserRegistration(obj);
                if (res > 1)
                {
                    if (obj.UserDocument != null)
                        _utilities.SaveUserDocumentImages(res, obj.UserDocument, webRootPath);
                   
                    SendEmails sendEmails = new SendEmails(_usersBAL, _IEmailTemplateBAL, _IOrderBAL);
                    Users objUsers = new Users();
                    objUsers.UserID = res;
                    sendEmails.setMailContent(objUsers, EStatus.Registration.ToString());
                }
                return res;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController UserRegistration action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController UserRegistration action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("ValidLogin")]
        [AllowAnonymous]
        public async Task<List<Users>> ValidLogin([FromBody] Users obj)
        {
            try
            {
                List<Users> lstLogin = new List<Users>();
                lstLogin = await this._usersBAL.ValidLogin(obj);

                if (lstLogin.Count > 0)
                {
                    AuthorizeService auth = new AuthorizeService();
                    string _token = auth.Authenticate(Convert.ToString(lstLogin[0].UserID), _appSettings);
                    lstLogin[0].Token = _token;
                    // return lstLogin;
                }

                return lstLogin;

            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController ValidLogin action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController ValidLogin action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllUsers")]
        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                //return await this._usersBAL.GetAllUsers();
                List<Users> lst = await this._usersBAL.GetAllUsers();
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].UserDocument = _utilities.UserDocument(lst[i].UserID, webRootPath);
                }

                return await Task.Run(() => new List<Users>(lst));
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController GetAllUsers action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController GetAllUsers action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetUserInfo")]
        public async Task<List<Users>> GetUserInfo()
        {
            try
            {
                Users obj = new Users();
                obj.UserID = UserService.LoggedInUser;
                return await this._usersBAL.GetUserInfo(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController GetAllUsers action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController GetAllUsers action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<int> UpdateUser([FromBody] Users obj)
        {
            try
            {
                var res = await this._usersBAL.UpdateUser(obj);
                if (obj.StatusId == 2)
                {
                    SendEmails sendEmails = new SendEmails(_usersBAL, _IEmailTemplateBAL, _IOrderBAL);
                    Users objUsers = new Users();
                    objUsers.UserID = obj.UserID;
                    sendEmails.setMailContent(objUsers, EStatus.RegistrationApproval.ToString());
                }
                return Convert.ToInt32(res);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController UpdateUser action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController UpdateUser action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("UpdatePwd")]
        public async Task<int> UpdatePwd([FromBody] Users obj)
        {
            try
            {
                obj.UserID = UserService.LoggedInUser;
                return await this._usersBAL.UpdatePwd(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController UpdatePwd action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController UpdatePwd action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("ValidEmail")]
        [AllowAnonymous]
        public async Task<int> ValidEmail([FromBody] Users obj)
        {
            try
            {
                List<Users> lstUser = await this._usersBAL.ValidEmail(obj);
                if (lstUser.Count > 0)
                {
                    SendEmails sendEmails = new SendEmails(_usersBAL, _IEmailTemplateBAL, _IOrderBAL);
                    Users objUsers = new Users();
                    objUsers.UserID = lstUser[0].UserID;
                    sendEmails.setMailContent(objUsers, EStatus.PasswordReset.ToString());
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController ValidEmail action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController ValidEmail action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<int> ResetPassword([FromBody] Users obj)
        {
            try
            {
                List<Users> lstUser = await this._usersBAL.ResetPassword(obj);
                if (lstUser.Count > 0)
                {
                    SendEmails sendEmails = new SendEmails(_usersBAL, _IEmailTemplateBAL, _IOrderBAL);
                    Users objUsers = new Users();
                    objUsers.UserID = lstUser[0].UserID;
                    sendEmails.setMailContent(objUsers, EStatus.PasswordResetConfirmation.ToString());
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController ResetPassword action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController ResetPassword action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("VerifyMobileOtp")]
        [AllowAnonymous]
        public async Task<int> VerifyMobileOtp([FromBody] OtpLog obj)
        {
            try
            {
                int res = await this._usersBAL.Verifymobileotp(obj);
                if (res > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController Verifymobileotp action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController Verifymobileotp action: {ex.Message}");
                return -1;
            }
        }


        [HttpPost]
        [Route("CheckMobileAllReadyRegisteredOrNot")]
        [AllowAnonymous]
        public async Task<int> CheckMobileAllReadyRegisteredOrNot([FromBody] Users obj)
        {
            try
            {
                List<Users> lstuser = new List<Users>();
                lstuser = this._usersBAL.CheckMobileAlreadyRegisteredOrNot(obj).Result;
                string urlParameters = "";

                if (lstuser.Count == 0)
                {

                    Random _random = new Random();
                    int otp = _random.Next(100000, 999999);
                    string api_key = "c47c40de-e3cf-11ea-9fa5-0200cd936042";

                    string URL = "https://2factor.in/API/V1/" + api_key + "/SMS/+91" + obj.MobileNo.ToString() + "/" + otp.ToString();

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(URL);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // List data response.
                    HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        dynamic data = JObject.Parse(jsonData);
                        string OTPsessionid = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)data).Last).Value).Value.ToString();

                        OtpLog _objOtpLog = new OtpLog();
                        _objOtpLog.MobileNo = obj.MobileNo.ToString();
                        _objOtpLog.OTP = otp.ToString();
                        _objOtpLog.SessionId = OTPsessionid;

                        int res = await Task.Run(() => this._usersBAL.InsertOtp(_objOtpLog));

                        return 0;
                    }

                    else
                    {
                        return -1;
                        //return 0;
                    }


                }

                return await Task.Run(() => lstuser.Count);

            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController checkMobileAllReadyRegisteredOrNot action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController checkMobileAllReadyRegisteredOrNot action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("GetAgentCustomer")]
        public async Task<List<Users>> GetAgentCustomer([FromBody] Users obj)
        {
            try
            {
                return await this._usersBAL.GetAgentCustomer(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController GetAgentCustomer action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController GetAgentCustomer action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("GetAgentCustomerByAgentId")]
        public async Task<List<Users>> GetAgentCustomerByAgentId([FromBody] Users obj)
        {
            try
            {
                return await this._usersBAL.GetAgentCustomerByAgentId(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController GetAgentCustomerByAgentId action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController GetAgentCustomerByAgentId action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("AgentCustomerStatusChange")]
        public async Task<int> AgentCustomerStatusChange([FromBody] Users obj)
        {
            try
            {
                return await this._usersBAL.AgentCustomerStatusChange(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UsersController AgentCustomerStatusChange action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);

                Logger.LogError($"Something went wrong inside UsersController AgentCustomerStatusChange action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("DeleteUserDocument")]
        public async Task<int> DeleteUserDocument([FromBody] Product obj)
        {
            try
            {
                _utilities.DeleteProductImage(obj.ImagePath, webRootPath);
                return await Task.Run(() => 1);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UserController DeleteUserDocument action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside UserController DeleteUserDocument action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("SaveUserDocumentImages")]
        public async Task<int> SaveUserDocumentImages([FromBody] Users obj)
        {
            try
            {
                _utilities.SaveUserDocumentImages(obj.UserID, obj.UserDocument, webRootPath);
                return await Task.Run(() => obj.UserID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside UserController SaveUserDocumentImages action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside UserController SaveUserDocumentImages action: {ex.Message}");
                return -1;
            }
        }
    }
}
