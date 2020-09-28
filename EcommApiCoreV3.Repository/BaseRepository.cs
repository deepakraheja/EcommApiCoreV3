using System;
using System.Data;
using System.Data.SqlClient;


namespace EcommApiCoreV3.Repository
{
    public abstract class BaseRepository : IDisposable
    {
        protected IDbConnection con;
        protected string ConnectionString { get; set; }
        protected string UsesmtpSSL { get; set; }
        protected string EnableSsl { get; set; }
        protected string enableMail { get; set; }
        protected string mailServer { get; set; }
        protected string userId { get; set; }
        protected string password { get; set; }
        protected string authenticate { get; set; }
        protected string fromEmailID { get; set; }
        protected string UserName { get; set; }
        protected static string Errorlog { get; set; }
        protected string filePdfPath { get; set; }

        public static string ReportTemplate
        {
            get;
            set;
        }
        public BaseRepository()
        {
            string connectionString = "Data Source=148.72.232.166;Initial Catalog=Ecomm;User ID=sonu;Password=password_1234;";
            //string connectionString = "Data Source=INSTANCE-1\\SQLEXPRESS;Initial Catalog=Ecomm;User ID=sa;Password=password_1234;";
            con = new SqlConnection(connectionString);


            Errorlog = @"C:\Project\Mohit\EcommApiCoreV3\EcommApiCoreV3\wwwroot\errorlog";
            //errorlog = @"C:\inetpub\wwwroot\EcommApi\wwwroot\errorlog";

           
            //**********************email setting*******************************

            //UsesmtpSSL = "true";
            //EnableSsl = "false";
            //enableMail = "1";
            //mailServer = "mailgateway.cscgrp.com";
            //userId = "support@protatech.com";
            //password = "C$c@aA07";
            //authenticate = "true";

            //fromEmailID = "jobs@csc-usa.com";
            //ReportTemplate = @"D:\inetpub\wwwWISHESSAPIv2\wwwroot\ReportTemplate\";
        }


        public void Dispose()
        {
            con.Close();
            //throw new NotImplementedException();
        }
    }
}
