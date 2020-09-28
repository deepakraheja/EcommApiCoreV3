using System;
using System.Collections.Generic;
using System.Text;

namespace EcommApiCoreV3.Entities
{
   public class OtpLog
    {
         public int OtpLogId { get; set; }
         public string MobileNo { get; set; }
        public string OTP { get; set; }
        public string SessionId { get; set; }

    }
}
