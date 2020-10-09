using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
    public class Users
    {
        public int UserID { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public string LoginId { get; set; }
        public string password { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public int UserType { get; set; } = 0;
        public string email { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public int IsApproval { get; set; } = 0;
        public int ApprovedBy { get; set; } = 0;
        public string ApprovedDate { get; set; }
        public string ApprovedByUserName { get; set; }
        public string NewPassword { get; set; }
        public string XMLFilePath { get; set; }
        public string Subject { get; set; }
        public string Token { get; set; }
        public string AdditionalDiscount { get; set; }
        public string OrderID { get; set; }
        public string OrderDate { get; set; }
        public string OrderDetails { get; set; }
        public string DeliveryAddress { get; set; }
        public string Link { get; set; }
        public string LoginURL { get; set; }
        public bool IsChecked { get; set; }
        public int AgentId { get; set; } = 0;
        public string BusinessType { get; set; }
        public string Industry { get; set; }
        public string BusinessLicenseType { get; set; }
        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string BusinessName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsAgent { get; set; } = false; 
        public int StatusId { get; set; } = 0;   
        public string BusinessPhone { get; set; }
        public bool IsVIPMember { get; set; } = false;
        public bool IsAgentCustomer { get; set; } = false;
         
        public string AadharCard { get; set; }
        public string GUID { get; set; } = "";

    }
}
