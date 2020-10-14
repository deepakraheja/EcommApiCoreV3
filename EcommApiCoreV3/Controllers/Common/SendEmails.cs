using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using EcommApiCoreV3.BAL;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Entities;
using EcommApiCoreV3.Repository.Interface;


using System.Diagnostics;
using EcommApiCoreV3.Repository;

//using sib_api_v3_sdk.Api;
//using sib_api_v3_sdk.Client;
//using sib_api_v3_sdk.Model;

//using mailinblue;

namespace EcommApiCoreV3.Controllers.Common
{
    public class SendEmails : BaseRepository
    {
        public enum EStatus
        {

            All = 0,
            Registration = 1,
            PasswordReset = 2,
            NewOrderCompletion = 3,
            RegistrationApproval = 4,
            PasswordResetConfirmation = 5,
            DispatchedConfirmation = 6,
            DeliveredConfirmation = 7
        }
        //public static readonly logger = "";//LogManager.GetLogger(typeof(SendEmails));
        private static IConfiguration configuration;

        public void EmailSetting(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        public int PlayerId { get; set; }
        IUsersBAL _usersBAL;
        IEmailTemplateBAL _IEmailTemplateBAL;
        IOrderBAL _IOrderBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;
        public SendEmails(IUsersBAL usersBAL, IEmailTemplateBAL emailTemplateBAL, IOrderBAL OrderBAL)
        {
            _usersBAL = usersBAL;
            _IEmailTemplateBAL = emailTemplateBAL;
            _IOrderBAL = OrderBAL;
        }

        // static string UsesmtpSSL = Startup.UsesmtpSSL;
        //static string enableMail = Startup.enableMail;
        static string mailServer = Startup.mailServer;
        static string userId = Startup.userId;
        static string password = Startup.password;
        //static string authenticate = Startup.authenticate;
        //static string AdminEmailID = Startup.AdminEmailID;
        static string fromEmailID = Startup.fromEmailID;
        //static string DomainName = Startup.DomainName;
        //static string AllowSendMails = Startup.AllowSendMails;
        //static string UserName = Startup.UserName;
        static string WebSiteURL = Startup.WebSiteURL;

        public void setMailContent(Users objUser, string Type, string subject = null, string emailBody = null)
        {
            //Users objUser = new Users();
            //objUser.UserID = UserID;
            var sendOnType = (EStatus)Enum.Parse(typeof(EStatus), Type);

            List<Users> objuserInfo = GetUserInfo(objUser, sendOnType);

            switch (sendOnType)
            {
                case EStatus.Registration:
                    {
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            password = objuserInfo[0].password,
                            XMLFilePath = "1",
                            email = objuserInfo[0].email,
                            Subject = "Application Received"
                        };

                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.PasswordReset:
                    {

                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            Link = "<a href ='" + WebSiteURL + "/pages/ResetPassword/" + Convert.ToString(objuserInfo[0].UserID) + "' target = '_blank' style='text-decoration:none;'>Link </a>",
                            Subject = "Password Reset",
                            XMLFilePath = "2",
                        };
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.NewOrderCompletion:
                    {
                        //Order obj = new Order();
                        //obj.OrderId = Convert.ToInt32(objUser.OrderID);
                        //List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        //lst[0].OrderDetails = this._IOrderBAL.GetSuccessOrderDetailsByOrderId(obj).Result;
                        //foreach (var item in lst[0].OrderDetails)
                        //{
                        //    if (item.SetNo > 0)
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                        //    else
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        //}
                        Order obj = new Order();
                        obj.GUID = objUser.GUID;
                        List<Order> lst = this._IOrderBAL.GetPrintOrderByGUID(obj).Result;
                        //obj.OrderId = lst[0].OrderId;
                        //lst[0].OrderDetails = this._IOrderBAL.GetPrintOrderDetailsByOrderId(obj).Result;
                        foreach (var item in lst[0].OrderDetails)
                        {
                            if (item.SetNo > 0)
                                item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                            else
                                item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        }

                        objUser.UserID = lst[0].UserID;
                        objuserInfo = GetUserInfo(objUser, sendOnType);
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            MobileNo = objuserInfo[0].MobileNo,
                            Subject = "New Order Completion",
                            XMLFilePath = "3",
                            OrderDetails = GenerateNewOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode
                        };
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.RegistrationApproval:
                    {
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            LoginURL = WebSiteURL,
                            Subject = "Registration Approval",
                            XMLFilePath = "4",
                        };
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.PasswordResetConfirmation:
                    {
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            Subject = "Password reset confirmation",
                            XMLFilePath = "5",
                        };
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.DispatchedConfirmation:
                    {
                        Order obj = new Order();
                        obj.OrderId = Convert.ToInt32(objUser.OrderID);
                        List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
                        foreach (var item in lst[0].OrderDetails)
                        {
                            if (item.SetNo > 0)
                                item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                            else
                                item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        }
                        objUser.UserID = lst[0].UserID;
                        objuserInfo = GetUserInfo(objUser, sendOnType);
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            MobileNo = objuserInfo[0].MobileNo,
                            Subject = "Dispatched Confirmation",
                            XMLFilePath = "6",
                            OrderDetails = GenerateOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode
                        };
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.DeliveredConfirmation:
                    {
                        Order obj = new Order();
                        obj.OrderId = Convert.ToInt32(objUser.OrderID);
                        List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
                        foreach (var item in lst[0].OrderDetails)
                        {
                            if (item.SetNo > 0)
                                item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                            else
                                item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        }
                        objUser.UserID = lst[0].UserID;
                        objuserInfo = GetUserInfo(objUser, sendOnType);
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            MobileNo = objuserInfo[0].MobileNo,
                            Subject = "Delivered Confirmation",
                            XMLFilePath = "7",
                            OrderDetails = GenerateOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode
                        };
                        SendEmail(emailParameters);
                    }
                    break;
            }
        }
        public string GenerateNewOrderDetails(List<Order> lst)
        {
            string StyleStr = "<style>" +
                                "table { border: 1px solid black; border - collapse: collapse; width: 80 %;}" +
                                "th {  background - color: black;  color: white; }" +
                                "td { border: 1px solid black; height: 35px; vertical - align: bottom; }" +
                                "</style>";
            string orderdetailsHeaderStr = "<table>" +
                                          "<tr>" +
                                            "<th>Product Image</th>" +
                                            "<th>Product Name</th>" +
                                            "<th>price</th>" +
                                            "<th>Qty</th>" +
                                            "<th>GST Rate(%)</th>" +
                                            "<th>GST Amount</th>" +
                                            "<th>Total</th>" +
                                          "</tr>";

            string orderdetailsStr = "";
            double TotalGSTAmount = 0, Total = 0;

            for (int i = 0; i < lst[0].OrderDetails.Count; i++)
            {
                orderdetailsStr += "<tr>" +
                                            "<td>" +
                                            GetProductImage(lst, i)
                                            + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].ProductName + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].Price + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].Quantity + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].GSTRate + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].GSTAmount + "</td>" +
                                            "<td>" + (lst[0].OrderDetails[i].Price * lst[0].OrderDetails[i].Quantity) + lst[0].OrderDetails[i].GSTAmount + "</td>" +
                                          "</tr>";
                TotalGSTAmount += lst[0].OrderDetails[i].GSTAmount;
                Total += (lst[0].OrderDetails[i].Price * lst[0].OrderDetails[i].Quantity) + lst[0].OrderDetails[i].GSTAmount;
            }
            string SubTotal = "<tr>" +
                                    "<td colspan='5'>" +
                                        "<b>Subtotal</b>" +
                                    "</td>" +
                                    "<td>" +
                                        "<b>" + TotalGSTAmount + "</b>" +
                                    "</td>" +
                                    "<td>" +
                                        "<span style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'>" +
                                        Total + "</span>" +
                                    "</td>" +
                                "</tr>" +

                                "<tr>" +
                                    "<td colspan='5'>" +
                                        "<b> Shipping</b>" +
                                    "</td>" +
                                    "<td colspan='2'>" +
                                        "<span style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'>" +
                                             "<b>Free Shipping</b></span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr class='total'>" +
                                    "<td colspan='6'>" +
                                        "<b>Total</b>" +
                                    "</td>" +
                                    "<td>" +
                                        "<span style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'>" +
                                            Total + "</span>" +
                                    "</td>" +
                                "</tr>";

            return StyleStr + orderdetailsHeaderStr + orderdetailsStr + SubTotal + "</table>";
        }
        public string GenerateOrderDetails(List<Order> lst)
        {
            string StyleStr = "<style>" +
                                "table { border: 1px solid black; border - collapse: collapse; width: 80 %;}" +
                                "th {  background - color: black;  color: white; }" +
                                "td { border: 1px solid black; height: 35px; vertical - align: bottom; }" +
                                "</style>";
            string orderdetailsHeaderStr = "<table>" +
                                          "<tr>" +
                                            "<th>Product Image</th>" +
                                            "<th>Product Name</th>" +
                                            "<th>Qty</th>" +
                                            "<th>price</th>" +
                                          "</tr>";

            string orderdetailsStr = "";
            for (int i = 0; i < lst[0].OrderDetails.Count; i++)
            {
                orderdetailsStr += "<tr>" +
                                            "<td>" + ""
                                            //GetProductImage(lst, i)
                                            + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].ProductName + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].Quantity + "</td>" +
                                            "<td>" + lst[0].OrderDetails[i].Price + "</td>" +
                                          "</tr>";
            }
            return StyleStr + orderdetailsHeaderStr + orderdetailsStr + "</table>";
        }

        public string GetProductImage(List<Order> lst, int index)
        {
            if (lst[0].OrderDetails[index].SetNo > 0)
            {
                return "<img style='width: 100px;' src= '" + ProductImagePath + lst[0].OrderDetails[index].ProductId + "/productSetImage/" + lst[0].OrderDetails[index].SetNo + "/" + (lst[0].OrderDetails[index].ProductImg.Length == 0 ? "" : lst[0].OrderDetails[index].ProductImg[0]) + "'>";
                //return "<img style='width: 100px;' src='http://34.67.65.213/EcommApiV3/ProductImage/13/productSetImage/2/13-07222020054952-1.jpg'/>'";
            }
            if (lst[0].OrderDetails[index].SetNo == 0)
            {
                return "<img style='width: 100px;' src= '" + ProductImagePath + lst[0].OrderDetails[index].ProductId + "/productColorImage/" + lst[0].OrderDetails[index].ProductSizeColorId + "/" + (lst[0].OrderDetails[index].ProductImg.Length == 0 ? "" : lst[0].OrderDetails[index].ProductImg[0]) + "'>";
                //return "<img style='width: 100px;' src='http://34.67.65.213/EcommApiV3/ProductImage/13/productSetImage/2/13-07222020054952-1.jpg'/>'";
            }
            return "";
        }
        public string GetMailBody(Users objEP)
        {
            try
            {
                EmailTemplate objEmailTemplate = new EmailTemplate
                {
                    EmailType = objEP.XMLFilePath
                };
                List<EmailTemplate> objET = _IEmailTemplateBAL.GetEmailTemplate(objEmailTemplate).Result;
                string template = objET[0].Template;
                template = template.Replace("[Name]", objEP.Name ?? "");
                template = template.Replace("[Email]", objEP.email ?? "");
                template = template.Replace("[Password]", objEP.password ?? "");
                template = template.Replace("[Mobile]", objEP.MobileNo ?? "");

                template = template.Replace("[OrderID]", objEP.OrderID ?? "");
                template = template.Replace("[OrderDate]", objEP.OrderDate ?? "");
                template = template.Replace("[OrderDetails]", objEP.OrderDetails ?? "");
                template = template.Replace("[DeliveryAddress]", objEP.DeliveryAddress ?? "");
                template = template.Replace("[Link]", objEP.Link ?? "");
                template = template.Replace("[LoginURL]", objEP.LoginURL ?? "");
                return template;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public List<Users> GetUserInfo(Users objUser, EStatus obj)
        {
            List<Users> objLstUser = new List<Users>();
            //if (obj != EStatus.Registration)
            //{
            objLstUser = _usersBAL.GetUserInfo(objUser).Result;
            //}
            //else
            //{
            //    objLstUser = _usersBAL.GetUserInfo(objUser).Result;
            //}
            return objLstUser;
        }

        //public string GetTime(String time)
        //{
        //    string str = Convert.ToDateTime(DateTime.Now).ToShortDateString() + " " + time.Substring(0, 5) + " " + time.Substring(time.Length - 2);
        //    DateTime objDate = Convert.ToDateTime(str);
        //    return objDate.ToString("HH:mm");
        //}

        public void SendEmail(Users emailParameters, List<Attachment> strAttachment = null, AlternateView EventData = null)
        {
            //string xmlData = emailParameters.GetXML();
            // string strBody = !String.IsNullOrEmpty(emailParameters.EmailBody) ? emailParameters.EmailBody : MailerUtility.GetMailBody(HttpContext.Current.Server.MapPath("~") + "\\xslt\\" + emailParameters.XMLFilePath, xmlData);
            //Utilities utilities = new Utilities();
            string strBody = GetMailBody(emailParameters);
            //****************Calling the Send Mail Function *******************************
            MailContent objMailContent = new MailContent() { From = "esales@vikramcreations.com", toEmailaddress = emailParameters.email, displayName = "Vikram Creations Private Limited", subject = emailParameters.Subject, emailBody = strBody, strAttachment = strAttachment, EventData = EventData };
            SendEmailInBackgroundThread(objMailContent);
        }


        public void SendEmailInBackgroundThread(MailContent objMailContent)
        {
            //Thread bgThread = new Thread(new ParameterizedThreadStart(SendAttachment));
            //bgThread.IsBackground = true;
            //bgThread.Start(objMailContent);

            SendEmailWithGmail(objMailContent);

            //SendAttachment(objMailContent);

            //SendMailBySendBlue(objMailContent);
        }



        public void SendEmailWithGmail(Object objMail)
        {
            MailContent objMC = (MailContent)objMail;

            string strFrom = "esales@vikramcreations.com";

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            string userId = "esales@vikramcreations.com";// Convert.ToString(ConfigurationManager.AppSettings["MailUserId"]); //MAIL ID FOR AUTHENTICATION
            string password = "Sales@123";// Convert.ToString(ConfigurationManager.AppSettings["MailPassword"]); ;//PASSWORD FOR AUTHENTICATION
            bool EnableSsl = true;

            bool flag = true;
            //string strSub = "Hello";
            //gdstring strBody = "Hello, This is Email sending test using gmail.";

            //  string addMessage = Convert.ToString(ConfigurationManager.AppSettings["Subject"]);
            String host = "smtp.gmail.com";// ConfigurationManager.AppSettings["mailServer"];
            MailAddress FromAddress = new MailAddress(strFrom);
            try
            {
                smtpClient.Port = 587;
                smtpClient.EnableSsl = EnableSsl;//Convert.ToBoolean(EnableSsl);
                smtpClient.Host = host;
                message.From = FromAddress;


                message.To.Add(objMC.toEmailaddress);
                message.Subject = objMC.subject;
                message.Body = objMC.emailBody;
                message.IsBodyHtml = true;


                //message.To.Add("deepakrahejain@mailinator.com");
                //message.CC.Add(strCc);
                //message.Bcc.Add(strBcc);
                //message.Subject = strSub;
                //message.Body = strBody;
                //message.IsBodyHtml = true;

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



        //public static void SendMailBySendBlue(Object objMail)
        //{
        //    MailContent objMC = (MailContent)objMail;

        //    SmtpClient smtpClient = new SmtpClient();
        //    MailMessage mailMessage = new MailMessage();



        //    bool flag = true;
        //    //bool UseSMTPSSL = false;
        //    //if (!string.IsNullOrEmpty(authenticate))
        //    //{
        //    //    flag = Convert.ToBoolean(authenticate);
        //    //}
        //    //if (!string.IsNullOrEmpty(UsesmtpSSL))
        //    //{
        //    //    UseSMTPSSL = Convert.ToBoolean(UsesmtpSSL);
        //    //}

        //    string host = mailServer;

        //    string address = fromEmailID;

        //    if (!String.IsNullOrEmpty(objMC.From))
        //    {
        //        address = objMC.From;
        //    }

        //    MailAddress from = new MailAddress(address, objMC.displayName);

        //    if (objMC.CopyTo.Count > 0)
        //    {
        //        foreach (string copyTo in objMC.CopyTo)
        //        {
        //            if (!string.IsNullOrEmpty(copyTo))
        //            {
        //                mailMessage.CC.Add(new MailAddress(copyTo));
        //            }
        //        }
        //    }


        //    try
        //    {

        //        //smtpClient.EnableSsl = false;
        //        smtpClient.Host = host;
        //        mailMessage.From = from;
        //        smtpClient.Port = 587;
        //        mailMessage.To.Add(objMC.toEmailaddress);
        //        mailMessage.Subject = objMC.subject;
        //        mailMessage.IsBodyHtml = true;

        //        if (objMC.EventData != null)
        //        {
        //            mailMessage.AlternateViews.Add(objMC.EventData);

        //            System.Net.Mime.ContentType typeHTML = new System.Net.Mime.ContentType("text/html");
        //            AlternateView viewHTML = AlternateView.CreateAlternateViewFromString(objMC.emailBody, typeHTML);
        //            viewHTML.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
        //            mailMessage.AlternateViews.Add(viewHTML);
        //        }
        //        else
        //        {
        //            mailMessage.Body = objMC.emailBody;
        //        }
        //        if (objMC.strAttachment != null)
        //        {
        //            foreach (Attachment a in objMC.strAttachment)
        //            {

        //                mailMessage.Attachments.Add(a);

        //            }
        //        }
        //        if (flag)
        //        {
        //            NetworkCredential credentials = new NetworkCredential(userId, password);
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.Credentials = credentials;
        //        }
        //        else
        //        {
        //            smtpClient.UseDefaultCredentials = true;
        //        }

        //        // string sendMail = AllowSendMails;


        //        //if (string.IsNullOrEmpty(sendMail) || (sendMail.ToUpper() != "NO"))
        //        //{
        //        if (!string.IsNullOrEmpty(objMC.emailBody))
        //        {
        //            smtpClient.Send(mailMessage);
        //            smtpClient.Dispose();
        //        }

        //        //}


        //        // update Mail log status of IsDelivered
        //        //MailBoxManager.UpdateDeliveryStatus(MailerLogID, true, "Successfully Send.");

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.Log(ex.Message);
        //        ErrorLogger.Log(ex.StackTrace);
        //        //update Mail log status of IsDelivered = False and Logtext = ex.mesage
        //        //logger.ErrorFormat(ex.Message);
        //        throw ex;
        //    }


        //}

        //public static void SendMailBySendBlue(MailContent objMailContent)
        //{
        //    // Configure API key authorization: api-key
        //    //Configuration.Default.ApiKey.Add("api-key", "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");
        //    // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
        //    // Configuration.Default.ApiKeyPrefix.Add("api-key", "Bearer");
        //    // Configure API key authorization: partner-key
        //    //Configuration.Default.ApiKey.Add("partner-key", "YOUR_API_KEY");
        //    // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
        //    // Configuration.Default.ApiKeyPrefix.Add("partner-key", "Bearer");

        //    //var apiInstance = new AccountApi();
        //    System.Net.Mime.ContentType typeHTML = new System.Net.Mime.ContentType("text/html");
        //    AlternateView viewHTML = AlternateView.CreateAlternateViewFromString(objMailContent.emailBody, typeHTML);
        //    viewHTML.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
        //    //mailMessage.AlternateViews.Add(viewHTML);

        //    try
        //    {
        //        API sendinBlue = new mailinblue.API("sHnhr4w0fTbga7c3"); //add your api key here 

        //        Dictionary<string, Object> data = new Dictionary<string, Object>();
        //        //Dictionary<string, string> to = new Dictionary<string, string>();
        //        //to.Add(objMailContent.toEmailaddress, "");

        //        //to.Add("deepak12345@mailinator.com", "to whom!");
        //        List<string> from_name = new List<string>();
        //        from_name.Add("esales@vikramcreations.com");

        //        //from_name.Add("from email!");
        //        //List<string> attachment = new List<string>();
        //        //attachment.Add("https://domain.com/path-to-file/filename1.pdf");
        //        //attachment.Add("https://domain.com/path-to-file/filename2.jpg");

        //        data.Add("to", objMailContent.toEmailaddress);
        //        data.Add("from", objMailContent.From);
        //        data.Add("subject", objMailContent.subject);
        //        data.Add("html", objMailContent.emailBody);
        //        //data.Add("attachment", attachment);

        //        Object sendEmail = sendinBlue.send_email(data);
        //        string InnerHtml = sendEmail.ToString();

        //        // Get your account information, plan and credits details
        //        //GetAccount result = apiInstance.GetAccount();

        //        //Debug.WriteLine(result);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Print("Exception when calling AccountApi.GetAccount: " + e.Message);
        //    }

        //}
        //public static void SendAttachment(Object objMail)
        //{
        //    MailContent objMailContent = (MailContent)objMail;
        //    System.Net.Mime.ContentType typeHTML = new System.Net.Mime.ContentType("text/html");
        //    AlternateView viewHTML = AlternateView.CreateAlternateViewFromString(objMailContent.emailBody, typeHTML);
        //    viewHTML.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
        //    //mailMessage.AlternateViews.Add(viewHTML);

        //    try
        //    {
        //        API sendinBlue = new mailinblue.API("sHnhr4w0fTbga7c3"); //add your api key here 

        //        Dictionary<string, Object> data = new Dictionary<string, Object>();
        //        Dictionary<string, string> to = new Dictionary<string, string>();
        //        to.Add(objMailContent.toEmailaddress, "to whom!");

        //        //to.Add("deepak12345@mailinator.com", "to whom!");
        //        List<string> from_name = new List<string>();
        //        from_name.Add("esales@vikramcreations.com");

        //        //from_name.Add("from email!");
        //        //List<string> attachment = new List<string>();
        //        //attachment.Add("https://domain.com/path-to-file/filename1.pdf");
        //        //attachment.Add("https://domain.com/path-to-file/filename2.jpg");

        //        data.Add("to", to);
        //        data.Add("from", from_name);
        //        data.Add("subject", objMailContent.subject);
        //        data.Add("html", objMailContent.emailBody);
        //        //data.Add("attachment", attachment);

        //        Object sendEmail = sendinBlue.send_email(data);
        //        string InnerHtml = sendEmail.ToString();

        //        // Get your account information, plan and credits details
        //        //GetAccount result = apiInstance.GetAccount();

        //        //Debug.WriteLine(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }


        //    //commented on 12sep2020

        //    /* MailContent objMC = (MailContent)objMail;
        //    if (objMC.toEmailaddress.StartsWith("admin"))
        //    {

        //    }

        //    SmtpClient smtpClient = new SmtpClient();
        //    MailMessage mailMessage = new MailMessage();



        //    bool flag = false;
        //    bool UseSMTPSSL = false;
        //    if (!string.IsNullOrEmpty(authenticate))
        //    {
        //        flag = Convert.ToBoolean(authenticate);
        //    }
        //    if (!string.IsNullOrEmpty(UsesmtpSSL))
        //    {
        //        UseSMTPSSL = Convert.ToBoolean(UsesmtpSSL);
        //    }

        //    string host = mailServer;

        //    string address = fromEmailID;


        //    ////if (UserName != null)
        //    //if (objMC.BranchId > 0)
        //    //{

        //    //    Branch obj = new Branch();
        //    //    string email = obj.GetEmailId(objMC.BranchId);
        //    //    if (email != "")
        //    //    {
        //    //        address = email;
        //    //    }
        //    //}

        //    if (!String.IsNullOrEmpty(objMC.From))
        //    {
        //        address = objMC.From;
        //    }

        //    MailAddress from = new MailAddress(address, objMC.displayName);

        //    if (objMC.CopyTo.Count > 0)
        //    {
        //        foreach (string copyTo in objMC.CopyTo)
        //        {
        //            if (!string.IsNullOrEmpty(copyTo))
        //            {
        //                mailMessage.CC.Add(new MailAddress(copyTo));
        //            }
        //        }
        //    }


        //    try
        //    {

        //        smtpClient.EnableSsl = false;
        //        smtpClient.Host = host;
        //        mailMessage.From = from;
        //        smtpClient.Port = 25;
        //        mailMessage.To.Add(objMC.toEmailaddress);
        //        mailMessage.Subject = objMC.subject;
        //        mailMessage.IsBodyHtml = true;

        //        if (objMC.EventData != null)
        //        {
        //            mailMessage.AlternateViews.Add(objMC.EventData);

        //            System.Net.Mime.ContentType typeHTML = new System.Net.Mime.ContentType("text/html");
        //            AlternateView viewHTML = AlternateView.CreateAlternateViewFromString(objMC.emailBody, typeHTML);
        //            viewHTML.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
        //            mailMessage.AlternateViews.Add(viewHTML);
        //        }
        //        else
        //        {
        //            mailMessage.Body = objMC.emailBody;
        //        }
        //        if (objMC.strAttachment != null)
        //        {
        //            foreach (Attachment a in objMC.strAttachment)
        //            {

        //                mailMessage.Attachments.Add(a);

        //            }
        //        }
        //        if (flag)
        //        {
        //            NetworkCredential credentials = new NetworkCredential(userId, password);
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.Credentials = credentials;
        //        }
        //        else
        //        {
        //            smtpClient.UseDefaultCredentials = true;
        //        }

        //        string sendMail = AllowSendMails;


        //        if (string.IsNullOrEmpty(sendMail) || (sendMail.ToUpper() != "NO"))
        //        {
        //            if (!string.IsNullOrEmpty(objMC.emailBody))
        //            {
        //                smtpClient.Send(mailMessage);
        //                smtpClient.Dispose();
        //            }

        //        }


        //        // update Mail log status of IsDelivered
        //        //MailBoxManager.UpdateDeliveryStatus(MailerLogID, true, "Successfully Send.");

        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLogger.Log(ex.Message);
        //        //ErrorLogger.Log(ex.StackTrace);
        //        //update Mail log status of IsDelivered = False and Logtext = ex.mesage
        //        //logger.ErrorFormat(ex.Message);
        //        throw ex;
        //    }*/
        //}



        //public static void SendAttachment(Object objMail, string UserName)
        //{
        //    MailContent objMC = (MailContent)objMail;
        //    try
        //    {

        //        SmtpClient smtpClient = new SmtpClient();
        //        MailMessage message = new MailMessage();
        //        bool EnableSsl = true;
        //        bool flag = true;
        //        string strSub = objMC.subject;
        //        string strBody = objMC.emailBody;
        //        MailAddress FromAddress = new MailAddress(fromEmailID.ToString(), objMC.displayName);

        //        smtpClient.EnableSsl = EnableSsl;//Convert.ToBoolean(EnableSsl);
        //        smtpClient.Host = mailServer.ToString();
        //        message.From = FromAddress;
        //        message.To.Add(objMC.toEmailaddress);
        //        //message.CC.Add(strCc);
        //        //message.Bcc.Add(strBcc);
        //        message.Subject = strSub;
        //        message.Body = strBody;
        //        message.IsBodyHtml = true;
        //        if (flag)
        //        {
        //            NetworkCredential oCredential = new NetworkCredential(userId.ToString(), password.ToString());
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.Credentials = oCredential;
        //        }
        //        else
        //        {
        //            smtpClient.UseDefaultCredentials = true;

        //        }
        //        smtpClient.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }


    public class MailContent
    {
        public string toEmailaddress { get; set; }
        public string displayName { get; set; }
        public string subject { get; set; }
        public string emailBody { get; set; }
        public List<Attachment> strAttachment { get; set; }
        public AlternateView EventData { get; set; }
        public string From { get; set; }
        List<String> _copyList = new List<string>();
        public List<String> CopyTo { get { return _copyList; } }
        public int BranchId { get; set; }
    }

    //public class SendEmailUserInfo
    //{

    //    public string FullName { get; set; }
    //    public string LogginedUserFullName { get; set; }
    //    public string Email { get; set; }
    //    public string LogginedUserEmail { get; set; }
    //    public string UserType { get; set; }
    //    public int UserID { get; set; }
    //    public string ApplicationNumber { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Address { get; set; }
    //    public string InterviewDate { get; set; }
    //    public string InterviewTime { get; set; }
    //    public string Password { get; set; }
    //    public string VenueName { get; set; }
    //    public string VenueAddress { get; set; }
    //    public string VenueId { get; set; }
    //    public string RoomNo { get; set; }
    //    public string ParkingInstructions { get; set; }
    //    public string InterviewType { get; set; }
    //    public string StartTime { get; set; }
    //    public string EndTime { get; set; }
    //    public string Duration { get; set; }
    //    public int Sessions { get; set; }
    //    public string Branch { get; set; }
    //    public string BranchEmail { get; set; }
    //    public int BranchId { get; set; }
    //    public string Interviewlink { get; set; }
    //    public string OnlineInterview { get; set; }
    //    public string Comment { get; set; }
    //}
}