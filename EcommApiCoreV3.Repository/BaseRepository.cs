﻿using System;
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
        public string ProductImagePath { get; set; }
        public static string ReportTemplate
        {
            get;
            set;
        }
        public BaseRepository()
        {

            //***********************************TESTING SERVER*******************************************************
            //********************************************************************************************************

            string connectionString = "Data Source=148.72.232.166;Initial Catalog=Ecomm;User ID=sonu;Password=password_1234;";
            Errorlog = @"C:\Project\Mohit\EcommApiCoreV3\EcommApiCoreV3\wwwroot\errorlog";
            ProductImagePath = "http://localhost:56283/ProductImage/";

            //***********************************PRODUCTION SERVER*******************************************************
            //********************************************************************************************************

            //string connectionString = "Data Source=WIN-BFFNG68BKRG\\SQLEXPRESS;Initial Catalog=Ecomm;User ID=sa;Password=vikram!@#2021;";
            //Errorlog = @"C:\inetpub\wwwroot\EcommApiV3\wwwroot\errorlog";
            //ProductImagePath = "http://103.108.220.24/EcommApiV3/ProductImage/";

            con = new SqlConnection(connectionString);

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
