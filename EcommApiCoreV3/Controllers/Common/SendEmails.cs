﻿using Microsoft.AspNetCore.Hosting;
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
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

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
            DeliveredConfirmation = 7,
            NewOrderProcess = 8,
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
        //static string mailServer = Startup.mailServer;
        //static string userId = Startup.userId;
        //static string password = Startup.password;
        //static string authenticate = Startup.authenticate;
        //static string AdminEmailID = Startup.AdminEmailID;
        //static string fromEmailID = Startup.fromEmailID;
        //static string DomainName = Startup.DomainName;
        //static string AllowSendMails = Startup.AllowSendMails;
        //static string UserName = Startup.UserName;
        static string WebSiteURL = Startup.WebSiteURL;
        static string ServiceURL = Startup.ServiceURL;
        public string GetHtmlTemplateAsString(string urlAddress)
        {

            string MailerBody = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                MailerBody = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            return MailerBody;
        }

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
                            Subject = "Application Received",
                            MobileNo = objuserInfo[0].MobileNo,
                        };
                        SendNewUserSMS(emailParameters.email, emailParameters.MobileNo, emailParameters.password);
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.PasswordReset:
                    {

                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            Link = "<a href ='" + WebSiteURL + "/pages/ResetPassword/" + objuserInfo[0].GUID + "' target = '_blank' style='text-decoration:none;'>Link </a>",
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
                        //List<Order> lst = this._IOrderBAL.GetEmailOrderByOrderID(obj).Result;

                        //List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        //lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
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
                            Subject = "Your order is confirmed",
                            XMLFilePath = "3",
                            OrderDetails = GenerateNewOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode,
                            TemplateType = "NewOrderCompletion.html"
                        };

                        SendOrderSMS(lst[0].OrderNumber, objuserInfo[0].Name, objuserInfo[0].MobileNo, 1, "", "");
                        SendEmail(emailParameters);
                    }
                    break;

                case EStatus.NewOrderProcess:
                    {

                        Order obj = new Order();
                        obj.OrderId = Convert.ToInt32(objUser.OrderID);
                        obj.OrderDetailsID = Convert.ToInt32(objUser.OrderDetailsID);

                        List<Order> lst = this._IOrderBAL.GetEmailOrderByOrderID(obj).Result;

                        //List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        //lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
                        //foreach (var item in lst[0].OrderDetails)
                        //{
                        //    if (item.SetNo > 0)
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                        //    else
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        //}
                        string Name = lst[0].FName;
                        string ProductName = lst[0].OrderDetails[0].ProductName;
                        string OrderNumber = lst[0].OrderNumber + " and product name -" + ProductName;


                        objUser.UserID = lst[0].UserID;
                        objuserInfo = GetUserInfo(objUser, sendOnType);

                        Users emailParameters = new Users()
                        {
                            Name = Name,//objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            MobileNo = objuserInfo[0].MobileNo,
                            Subject = "Your order started processing",
                            XMLFilePath = "3",
                            OrderDetails = GenerateNewOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode,
                            TemplateType = "NewOrderProcess.html"
                        };


                        SendOrderSMS(OrderNumber, Name, objuserInfo[0].MobileNo, 2, "", "");
                        SendEmail(emailParameters);
                    }
                    break;

                case EStatus.RegistrationApproval:
                    {
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            MobileNo = objuserInfo[0].MobileNo,
                            email = objuserInfo[0].email,
                            LoginURL = WebSiteURL,
                            Subject = "Registration Approval",
                            XMLFilePath = "4",
                        };
                        SendRegistrationSMS(emailParameters.Name, emailParameters.MobileNo, emailParameters.email);
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
                        obj.OrderDetailsID = Convert.ToInt32(objUser.OrderDetailsID);

                        List<Order> lst = this._IOrderBAL.GetEmailOrderByOrderID(obj).Result;

                        //List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        //lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
                        //foreach (var item in lst[0].OrderDetails)
                        //{
                        //    if (item.SetNo > 0)
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                        //    else
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        //}


                        string ProductName = lst[0].OrderDetails[0].ProductName;
                        string OrderNumber = lst[0].OrderNumber + " and product name -" + ProductName;

                        string TrackingName = lst[0].OrderDetails[0].TransportName;
                        string TrackingURL = "";// lst[0].OrderDetails[0].TrackingURL;
                        string TrackingID = lst[0].OrderDetails[0].Bilty;


                        objUser.UserID = lst[0].UserID;
                        string Name = lst[0].FName;
                        objuserInfo = GetUserInfo(objUser, sendOnType);

                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            MobileNo = objuserInfo[0].MobileNo,
                            Subject = "Your order is dispatched",
                            XMLFilePath = "6",
                            OrderDetails = GenerateNewOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode,
                            TemplateType = "DispatchedConfirmation.html"
                        };
                        SendOrderSMS(OrderNumber, Name, objuserInfo[0].MobileNo, 3, TrackingURL, TrackingID);
                        SendEmail(emailParameters);
                    }
                    break;
                case EStatus.DeliveredConfirmation:
                    {
                        Order obj = new Order();
                        obj.OrderId = Convert.ToInt32(objUser.OrderID);
                        obj.OrderDetailsID = Convert.ToInt32(objUser.OrderDetailsID);

                        List<Order> lst = this._IOrderBAL.GetEmailOrderByOrderID(obj).Result;
                      
                        //List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
                        //lst[0].OrderDetails = this._IOrderBAL.GetOrderDetailsByOrderId(obj).Result;
                        //foreach (var item in lst[0].OrderDetails)
                        //{
                        //    if (item.SetNo > 0)
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                        //    else
                        //        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                        //}


                        string ProductName = lst[0].OrderDetails[0].ProductName;
                        string OrderNumber = lst[0].OrderNumber + " and product name -" + ProductName;

                        objUser.UserID = lst[0].UserID;
                        string Name = lst[0].FName;

                        objuserInfo = GetUserInfo(objUser, sendOnType);
                        Users emailParameters = new Users()
                        {
                            Name = objuserInfo[0].Name,
                            email = objuserInfo[0].email,
                            MobileNo = objuserInfo[0].MobileNo,
                            Subject = "Your order is delivered",
                            XMLFilePath = "7",
                            OrderDetails = GenerateNewOrderDetails(lst),
                            OrderID = lst[0].OrderNumber,
                            OrderDate = lst[0].OrderDate,
                            DeliveryAddress = lst[0].Address + ", " + lst[0].City + "<br/>" + lst[0].State + "<br/>" + lst[0].Country + ", " + lst[0].ZipCode,
                            TemplateType = "DeliveredConfirmation.html"
                        };
                        SendOrderSMS(OrderNumber, Name, objuserInfo[0].MobileNo, 4, "", "");
                        SendEmail(emailParameters);
                    }
                    break;
            }
        }
        public string GenerateNewOrderDetails(List<Order> lst)
        {
            string StyleStr = "<style>" +
                                "table { border: 1px solid; border-collapse: collapse; width: 80%;}" +
                                "th { border: 1px solid; background-color: black; color: white; }" +
                                "td { border: 1px solid; height: 35px; vertical-align: bottom; }" +
                                "</style>";
            string orderdetailsHeaderStr = "<table style='width: 100%;'>" +
                                          "<tr style='background-color: black;color: white;'>" +
                                            "<th style='text-align: center;'>Product Image</th>" +
                                            "<th style='text-align: center;'>Product Name</th>" +
                                            "<th style='text-align: center;'>Price</th>" +
                                            "<th style='text-align: center;'>Qty</th>";
            if (lst[0].OrderDetails[0].AdditionalDiscount > 0)
            {
                orderdetailsHeaderStr += "<th style='text-align: center;'>Add. Discount (%)</th>" +
                                            "<th style='text-align: center;'>Add. Discount Amount</th>";
            }
            orderdetailsHeaderStr += "<th style='text-align: right;'>Total Amount</th>" +
                                            "<th style='text-align: center;'>GST Rate(%)</th>" +
                                            "<th style='text-align: center;'>GST Amount</th>" +
                                            "<th style='text-align: center;'>Total</th>" +
                                          "</tr>";

            string orderdetailsStr = "";
            double TotalGSTAmount = 0, Total = 0, TotalQty = 0, TotalAdditionalDiscountAmount = 0, TotalAmountWithDis = 0;

            for (int i = 0; i < lst[0].OrderDetails.Count; i++)
            {
                orderdetailsStr += "<tr>" +
                                            "<td style='text-align: center;'>" +
                                            GetProductImage(lst, i)
                                            + "</td>" +
                                            "<td style='text-align: center;'>" + lst[0].OrderDetails[i].ProductName + "</td>" +
                                            "<td style='text-align: right;'>" + lst[0].OrderDetails[i].SalePrice.ToString("0.00") + "</td>" +
                                            "<td style='text-align: center;'>" + lst[0].OrderDetails[i].Quantity + "</td>";
                if (lst[0].OrderDetails[i].AdditionalDiscount > 0)
                {
                    orderdetailsStr += "<td style='text-align: right;'>" + lst[0].OrderDetails[i].AdditionalDiscount + "</td>" +
                                       "<td style='text-align: right;'>" + lst[0].OrderDetails[i].AdditionalDiscountAmount.ToString("0.00") + "</td>";
                }

                orderdetailsStr += "<td style='text-align: right;'>" + (Convert.ToDecimal(lst[0].OrderDetails[i].SalePrice * lst[0].OrderDetails[i].Quantity) - lst[0].OrderDetails[i].AdditionalDiscountAmount).ToString("0.00") + "</td>" +
                                            "<td style='text-align: center;'>" + lst[0].OrderDetails[i].GSTRate.ToString() + "</td>" +
                                            "<td style='text-align: right;'>" + lst[0].OrderDetails[i].GSTAmount.ToString("0.00") + "</td>" +
                                            "<td style='text-align: right;'>" + Convert.ToDouble((lst[0].OrderDetails[i].SalePrice * lst[0].OrderDetails[i].Quantity) - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount) + lst[0].OrderDetails[i].GSTAmount).ToString("0.00") + "</td>" +
                                          "</tr>";
                TotalQty += lst[0].OrderDetails[i].Quantity;
                TotalAdditionalDiscountAmount += Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount);
                TotalAmountWithDis += lst[0].OrderDetails[i].SalePrice * lst[0].OrderDetails[i].Quantity - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount);
                TotalGSTAmount += lst[0].OrderDetails[i].GSTAmount;
                Total += (lst[0].OrderDetails[i].SalePrice * lst[0].OrderDetails[i].Quantity) - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount) + lst[0].OrderDetails[i].GSTAmount;
            }
            string SubTotal = "<tr><td colspan='8'></td></tr>";
            SubTotal += "<tr><td colspan='8'></td></tr>";
            SubTotal += "<tr>" +
                                    "<td colspan='3'>" +
                                        "<b>Subtotal</b>" +
                                    "</td>" +
                                    "<td style='text-align: center;'>" +
                                        "<b>" + TotalQty + "</b>" +
                                    "</td>";
            if (lst[0].OrderDetails[0].AdditionalDiscount > 0)
            {
                SubTotal += "<td></td>" +
                                    "<td style='text-align: right;'>" +
                                        "<b>" + TotalAdditionalDiscountAmount.ToString("0.00") + "</b>" +
                                    "</td>";
            }
            SubTotal += "<td style='text-align: right;'>" +
                                    "<b>" + TotalAmountWithDis.ToString("0.00") + "</b>" +
                                    "</td>" +
                                    "<td></td>" +
                                    "<td style='text-align: right;'>" +
                                        "<b>" + TotalGSTAmount.ToString("0.00") + "</b>" +
                                    "</td>" +
                                    "<td style='text-align: right;'><b>" +
                                        Total.ToString("0.00") + "</b>" +
                                    "</td>" +
                                "</tr>" +

                                "<tr>" +
                                    "<td colspan='6'>" +
                                        "<b> Packing</b>" +
                                    "</td>";
            if (lst[0].OrderDetails[0].AdditionalDiscount > 0)
            {
                SubTotal += "<td></td>" +
                                    "<td></td>";
            }
            SubTotal += "<td colspan='2' style='text-align: right;'>" +
                                             "<b>Free Packing</b>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr class='total'>" +
                                    "<td colspan='6'>" +
                                        "<b>Total</b>" +
                                    "</td>";
            if (lst[0].OrderDetails[0].AdditionalDiscount > 0)
            {
                SubTotal += "<td></td>" +
                                    "<td></td>";
            }
            SubTotal += "<td></td>" +
                        "<td style='text-align: right;'>" +
                                        "<span style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'><b>" +
                                            Total.ToString("0.00") + "</b></span>" +
                                    "</td>" +
                                "</tr>";

            return orderdetailsHeaderStr + orderdetailsStr + SubTotal + "</table>" + StyleStr;
        }
        //public string GenerateOrderDetails(List<Order> lst)
        //{
        //    string StyleStr = "<style>" +
        //                        "table { border: 1px solid black; border - collapse: collapse; width: 80 %;}" +
        //                        "th {  background - color: black;  color: white; }" +
        //                        "td { border: 1px solid black; height: 35px; vertical - align: bottom; }" +
        //                        "</style>";
        //    string orderdetailsHeaderStr = "<table>" +
        //                                  "<tr>" +
        //                                    "<th>Product Image</th>" +
        //                                    "<th>Product Name</th>" +
        //                                    "<th>Qty</th>" +
        //                                    "<th>price</th>" +
        //                                  "</tr>";

        //    string orderdetailsStr = "";
        //    for (int i = 0; i < lst[0].OrderDetails.Count; i++)
        //    {
        //        orderdetailsStr += "<tr>" +
        //                                    "<td>" + ""
        //                                    //GetProductImage(lst, i)
        //                                    + "</td>" +
        //                                    "<td>" + lst[0].OrderDetails[i].ProductName + "</td>" +
        //                                    "<td>" + lst[0].OrderDetails[i].Quantity + "</td>" +
        //                                    "<td>" + lst[0].OrderDetails[i].Price + "</td>" +
        //                                  "</tr>";
        //    }
        //    return StyleStr + orderdetailsHeaderStr + orderdetailsStr + "</table>";
        //}

        public string GetProductImage(List<Order> lst, int index)
        {
            if (lst[0].OrderDetails[index].SetNo > 0)
            {
                return "<img style='width: 100px;' src= '" + ProductImagePath + lst[0].OrderDetails[index].ProductId + "/productSetImage/" + lst[0].OrderDetails[index].SetNo + "/" + (lst[0].OrderDetails[index].ProductImg.Length == 0 ? "" : lst[0].OrderDetails[index].ProductImg[0]) + "'>";
                //return "<img style='width: 100px;' src='http://34.67.65.213/EcommApiV3/ProductImage/13/productSetImage/2/13-07222020054952-1.jpg'/>'";
            }
            if (lst[0].OrderDetails[index].SetNo == 0)
            {
                //ErrorLogger.Log("<img style = 'width: 100px;' src = '" + ProductImagePath + lst[0].OrderDetails[index].ProductId + "/frontImage/" + lst[0].OrderDetails[index].FrontImage);
                //return "<img style='width: 100px;' src= '" + ProductImagePath + lst[0].OrderDetails[index].ProductId + "/productColorImage/" + lst[0].OrderDetails[index].ProductSizeColorId + "/" + (lst[0].OrderDetails[index].ProductImg.Length == 0 ? "" : lst[0].OrderDetails[index].ProductImg[0]) + "'>";
                return "<img style='width: 100px;' src= '" + ProductImagePath + lst[0].OrderDetails[index].ProductId + "/frontImage/" + lst[0].OrderDetails[index].FrontImage + "'>";
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
                string template = "";
                if (objEP.TemplateType == "")
                    template = objET[0].Template;
                else
                    template = GetHtmlTemplateAsString(ServiceURL + "\\htmlTemplate\\" + objEP.TemplateType);
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
            catch (Exception exx)
            {
                ErrorLogger.Log($"Something went wrong inside SendEmail.cs GetMailBody action: {exx.Message}");
                ErrorLogger.Log(exx.StackTrace);
                return "";
            }
        }

        public void SendRegistrationSMS(string FName, string Phone, string email)
        {
            try
            {

                String url = "https://2factor.in/API/R1/";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                ASCIIEncoding encoding = new ASCIIEncoding();

                string template_name = "";
                template_name = HttpUtility.UrlEncode("RegisttrationApproval");

                string var1 = HttpUtility.UrlEncode(FName);
                string var2 = HttpUtility.UrlEncode(email);

                string postData = "module=TRANS_SMS";
                postData += "&apikey=c47c40de-e3cf-11ea-9fa5-0200cd936042";
                postData += "&to=" + Phone;
                postData += "&from=IVIKRM";
                postData += "&templatename=" + template_name;
                postData += "&var1=" + var1;
                postData += "&var2=" + var2;

                byte[] data = encoding.GetBytes(postData);

                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;

                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                //Get Response
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                System.Diagnostics.Debug.Print(responseString);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SendEmail.cs SendOrderSMS action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);


            }
        }

        public void SendNewUserSMS(string LoginId, string Phone, string password)
        {
            try
            {

                String url = "https://2factor.in/API/R1/";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                ASCIIEncoding encoding = new ASCIIEncoding();

                string template_name = "";
                template_name = HttpUtility.UrlEncode("Newuser");

                string var1 = HttpUtility.UrlEncode(LoginId);
                string var2 = HttpUtility.UrlEncode(password);

                string postData = "module=TRANS_SMS";
                postData += "&apikey=c47c40de-e3cf-11ea-9fa5-0200cd936042";
                postData += "&to=" + Phone;
                postData += "&from=IVIKRM";
                postData += "&templatename=" + template_name;
                postData += "&var1=" + var1;
                postData += "&var2=" + var2;


                byte[] data = encoding.GetBytes(postData);

                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;

                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                //Get Response


                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                System.Diagnostics.Debug.Print(responseString);



            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SendEmail.cs SendOrderSMS action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);


            }
        }
        public void SendOrderSMS(string OrderNumber, string FName, string Phone, int type)
        {
            try
            {
                String url = "https://2factor.in/API/R1/";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                ASCIIEncoding encoding = new ASCIIEncoding();
                string template_name = "";
                if (type == 1)
                    template_name = HttpUtility.UrlEncode("OrderConfirmation");
                if (type == 2)
                    template_name = HttpUtility.UrlEncode("OrderProcessed");
                if (type == 3)
                    template_name = HttpUtility.UrlEncode("OrderDispatched");
                if (type == 4)
                    template_name = HttpUtility.UrlEncode("OrderDelivered");
                string var1 = HttpUtility.UrlEncode(FName);
                string var2 = HttpUtility.UrlEncode(OrderNumber);
                string postData = "module=TRANS_SMS";
                postData += "&apikey=4f4e2253-3adb-11eb-83d4-0200cd936042";
                postData += "&to=" + Phone;
                postData += "&from=ALBABA";
                postData += "&templatename=" + template_name;
                postData += "&var1=" + var1;
                postData += "&var2=" + var2;
                byte[] data = encoding.GetBytes(postData);
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                System.Diagnostics.Debug.Print(responseString);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SendEmail.cs SendOrderSMS action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
            }
        }

        public void SendOrderSMS(string OrderNumber, string FName, string Phone, int type, string TrackingURL, string TrackingID)
        {
            try
            {

                String url = "https://2factor.in/API/R1/";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);
                ASCIIEncoding encoding = new ASCIIEncoding();

                string template_name = "";
                if (type == 1)
                    template_name = HttpUtility.UrlEncode("OrderConfirmation");
                if (type == 2)
                    template_name = HttpUtility.UrlEncode("OrderProcessed");
                if (type == 3)
                    template_name = HttpUtility.UrlEncode("OrderDispatched");
                if (type == 4)
                    template_name = HttpUtility.UrlEncode("OrderDelivered");



                string postData = "module=TRANS_SMS";
                postData += "&apikey=c47c40de-e3cf-11ea-9fa5-0200cd936042";
                postData += "&to=" + Phone.Trim();

                if (type == 1)
                    postData += "&from=IVIKRM";
                else
                    postData += "&from=IVIKRM";

                postData += "&templatename=" + template_name;

                if (type != 3)
                {

                    string var1 = HttpUtility.UrlEncode(FName);
                    string var2 = HttpUtility.UrlEncode(OrderNumber);

                    postData += "&var1=" + var1;
                    postData += "&var2=" + var2;
                }
                else

                {
                    string var1 = HttpUtility.UrlEncode(FName);
                    string var2 = HttpUtility.UrlEncode(TrackingURL);
                    string var3 = HttpUtility.UrlEncode(OrderNumber);
                    string var4 = HttpUtility.UrlEncode(TrackingID);

                    postData += "&var1=" + var1;
                    postData += "&var2=" + var2;
                    postData += "&var3=" + var3;
                    postData += "&var4=" + var4;

                }

                //ErrorLogger.Log(postData);

                byte[] data = encoding.GetBytes(postData);

                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;

                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                //Get Response


                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                System.Diagnostics.Debug.Print(responseString);



            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside SendEmail.cs SendOrderSMS action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);


            }
        }


        public void SendSMS(string MobileNo, string msg)
        {
            string urlParameters = "";
            string api_key = "c47c40de-e3cf-11ea-9fa5-0200cd936042";
            string URL = "https://2factor.in/API/V1/" + api_key + "/SMS/+91" + MobileNo.ToString() + "/" + msg.ToString();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
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