using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

using System.Data.Common;
using System.ComponentModel;
using System.Reflection;
using System.Data;

namespace EcommReporting
{
    /// <summary>
    /// Summary description for Reporthandler
    /// </summary>
    public class Reporthandler : IHttpHandler
    {

        string GUID = "";

        public void ProcessRequest(HttpContext context)
        {
            string LogPath = context.Server.MapPath("") + @"\ReportErrorlog\" + DateTime.Now.ToString("MM-dd-yyyy hh-mm") + "-logfile.txt";
            try
            {

                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");

                GUID = context.Request.QueryString["id"];

                DataSet dsReport = new DataSet();

                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);

                dsReport = objSql.ExecuteDataSet("p_GetNewInvoiceOrderByGUID");

                // Create Report DataSource
                ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables[0]);

                ReportViewer viewer = new ReportViewer();
                //string ApplicantForm = @"C:\ApplicantForm\";

                string ReportDownlaod = context.Server.MapPath("") + @"\ReportDownlaod\";
                //string imagePath;
                byte[] bytes;
                //string fileName = "";

                viewer.ProcessingMode = ProcessingMode.Local;

                if (Convert.ToString(dsReport.Tables[0].Rows[0]["BusinessState"]) == "Delhi")
                {

                    if (Convert.ToInt32(dsReport.Tables[0].Rows[0]["InvoiceNo"]) > 0)
                    {
                        viewer.LocalReport.ReportPath = context.Server.MapPath("") + @"\Reports\Local\" + "LocalGSTInvoice" + ".rdlc";
                    }
                    else
                    {
                        viewer.LocalReport.ReportPath = context.Server.MapPath("") + @"\Reports\Local\" + "LocalGSTOrder" + ".rdlc";

                    }
                }
                else
                {
                    if (Convert.ToInt32(dsReport.Tables[0].Rows[0]["InvoiceNo"]) > 0)
                    {
                        viewer.LocalReport.ReportPath = context.Server.MapPath("") + @"\Reports\Central\" + "CentralGSTInvoice" + ".rdlc";
                    }
                    else
                    {
                        viewer.LocalReport.ReportPath = context.Server.MapPath("") + @"\Reports\Central\" + "CentralGSTOrder" + ".rdlc";

                    }

                }

                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.Refresh();

                // Variables
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);

                viewer.LocalReport.DataSources.Add(rds); // Add datasource here

                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                //Now that you have all the bytes representing the PDF report, buffer it and send it to the client.

                string path = ReportDownlaod;
                string FileName = DateTime.Now.ToString("MMddyyyyhhmmss") + ".pdf";

                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);

                }
                using (FileStream stream = new FileStream((path + "/" + FileName), FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                context.Response.Buffer = false;
                context.Response.Clear();
                context.Response.ContentType = mimeType;
                //context.Response.AddHeader("content-disposition", "attachment; filename=" + RName + "." + extension);
                context.Response.BinaryWrite(bytes); // create the file
                context.Response.Flush(); // sen
                //context.Response.End();
                System.IO.File.Delete(path + "/" + FileName);


            }
            catch (Exception exx)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(exx.Message.ToString());


                string str = exx.Message.ToString();
                StreamWriter log;
                if (!System.IO.File.Exists(LogPath))
                //}
                {
                    log = new StreamWriter(LogPath);
                }
                else
                {
                    log = System.IO.File.AppendText(LogPath);
                }
                log.Close();
                using (StreamWriter writer = new StreamWriter(LogPath, true))
                {
                    writer.WriteLine("Report handler realted:  " + exx.Message.ToString());
                    writer.Close();
                }

            }

        }


        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Clear();

            if (e.ReportPath == "SubReportInvoiceHsn")
            {
                DataTable dtbl = new DataTable("DataTable1");
                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);
                dtbl = objSql.ExecuteDataSet("p_GetSubInvoiceGroupByGUID").Tables[0];
                e.DataSources.Add(new ReportDataSource("DataSet1", dtbl));
            }

            if (e.ReportPath == "AmountWords")
            {
                DataTable dtbl = new DataTable("DataTable1");
                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);
                dtbl = objSql.ExecuteDataSet("p_GetAmoutnintoWordsByGUID").Tables[0];
                e.DataSources.Add(new ReportDataSource("DataSet1", dtbl));
            }

            if (e.ReportPath == "SubReportGst")
            {
                DataTable dtbl = new DataTable("DataTable1");
                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);
                dtbl = objSql.ExecuteDataSet("p_GetSubInvoiceGSTByGUID").Tables[0];
                e.DataSources.Add(new ReportDataSource("DataSet1", dtbl));
            }

            if (e.ReportPath == "CentralSubReportInvoiceHsn")
            {
                DataTable dtbl = new DataTable("DataTable1");
                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);
                dtbl = objSql.ExecuteDataSet("p_GetSubInvoiceGroupByGUID").Tables[0];
                e.DataSources.Add(new ReportDataSource("DataSet1", dtbl));
            }

            if (e.ReportPath == "CentralAmountWords")
            {
                DataTable dtbl = new DataTable("DataTable1");
                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);
                dtbl = objSql.ExecuteDataSet("p_GetAmoutnintoWordsByGUID").Tables[0];
                e.DataSources.Add(new ReportDataSource("DataSet1", dtbl));
            }

            if (e.ReportPath == "CentralSubReportGst")
            {
                DataTable dtbl = new DataTable("DataTable1");
                SQL objSql = new SQL();
                objSql.AddParameter("@GUID", DbType.String, ParameterDirection.Input, 0, GUID);
                dtbl = objSql.ExecuteDataSet("p_GetSubInvoiceGSTByGUID").Tables[0];
                e.DataSources.Add(new ReportDataSource("DataSet1", dtbl));
            }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    public class SQL : IDisposable
    {

        SqlCommand cmd = null;
        SqlConnection sqlCon = null;
        string sqlSTR = ConfigurationManager.ConnectionStrings["sqlCon"].ConnectionString;

        public SQL()
        {
            InitialzeConnection();
        }

        private void InitialzeConnection()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(sqlSTR);

            if (sqlCon.State != ConnectionState.Closed)
                sqlCon.Close();
            sqlCon.Open();
            cmd = new SqlCommand();
            cmd.Connection = sqlCon;

        }
        public void Dispose()
        {
            sqlCon.Dispose();
            cmd.Dispose();
        }

        /// <summary>
        /// Adds the parameter to the sql Command object
        /// </summary>
        /// <param name="pName">Parameter name</param>
        /// <param name="type">Type</param>
        /// <param name="direction">Direction</param>
        /// <param name="size">Size</param>
        /// <param name="value">Value</param>
        public void AddParameter(string pName, DbType type, ParameterDirection direction, int size, object value)
        {
            DbParameter p = new SqlParameter();
            p.DbType = type;
            p.ParameterName = pName;
            p.Direction = direction;
            p.Size = size;
            p.Value = value;
            cmd.Parameters.Add(p);
        }


        public DataSet ExecuteDataSet(string pName)
        {
            SetCMDName(pName);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            OutputParameters();
            Dispose();
            return ds;
        }

        DbParameter[] param = null;
        public DbParameter[] OutParams
        {
            get { return param; }
        }
        private DbParameter[] OutputParameters()
        {
            param = new DbParameter[cmd.Parameters.Count];
            int i = 0;
            foreach (DbParameter dp in cmd.Parameters)
            {
                if (dp.Direction == ParameterDirection.Output)
                {
                    param[i] = dp;
                }
                i++;
            }
            return param;
        }

        private void SetCMDName(string pName)
        {
            cmd.CommandText = pName;
            cmd.CommandType = CommandType.StoredProcedure;
        }

        /// <Statemary>
        /// handles the communication with the database, Constructs the list and returns to the calling layer
        /// </Statemary>
        /// <returns>Returns the List of the entity</returns>
        public IDataReader ExecuteReader(string pName)
        {
            SetCMDName(pName);
            IDataReader dr = cmd.ExecuteReader();
            OutputParameters();
            return dr;
        }

        public int ExecuteNonQuery(string pName)
        {
            SetCMDName(pName);
            int value = cmd.ExecuteNonQuery();
            OutputParameters();
            Dispose();
            return value;
        }

        /// <Statemary>
        /// handles the communication with the database, Constructs the list and returns to the calling layer
        /// </Statemary>
        /// <returns>Returns the List of the entity</returns>
        public object ExecuteScalar(string pName)
        {
            SetCMDName(pName);
            object retvalue = cmd.ExecuteScalar();
            OutputParameters();
            Dispose();
            return retvalue;
        }



        public List<T> ContructList<T>(DataSet dr)
        {
            List<T> objList = new List<T>();
            foreach (DataTable dt in dr.Tables)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T objT;
                    Type t = typeof(T);
                    BindingFlags bflags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
                    ConstructorInfo cInfo = typeof(T).GetConstructor(bflags, null, new Type[0] { }, null);
                    if (cInfo != null)
                    {
                        objT = (T)cInfo.Invoke(null);
                    }
                    else
                        objT = Activator.CreateInstance<T>();



                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                    for (int col = 0; col <= dt.Columns.Count - 1; col++)
                    {
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.Name.ToUpper() == dt.Columns[col].ColumnName.ToUpper() && !string.IsNullOrEmpty(Convert.ToString(row[col])))
                            {
                                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                                if (underlyingType == null)
                                {
                                    prop.SetValue(objT, Convert.ChangeType(row[col], prop.PropertyType));

                                }
                                else
                                {
                                    prop.SetValue(objT, Convert.ChangeType(row[col], underlyingType));


                                }
                            }
                        }
                    }

                    objList.Add(objT);
                }
            }
            return objList;


        }

        public List<T> ContructList<T>(IDataReader dr)
        {
            List<T> objList = new List<T>();
            while (dr.Read())
            {
                //T objT = default(T);
                T objT = Activator.CreateInstance<T>();
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                for (int col = 0; col <= dr.FieldCount - 1; col++)
                {
                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (prop.Name.ToUpper() == dr.GetName(col).ToUpper() && !string.IsNullOrEmpty(Convert.ToString(dr.GetValue(col))))
                        {
                            prop.SetValue(objT, Convert.ChangeType(dr.GetValue(col), prop.PropertyType));
                        }
                    }
                }
                objList.Add(objT);

            }
            return objList;
        }
    }
    public class SQLHelper
    {

        /// <summary>
        /// on the basis of type passed in "T", construct list from dataset
        /// </summary>

        public static List<T> ContructList<T>(DataSet ds)
        {
            List<T> objList = new List<T>();
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T objT;
                    Type t = typeof(T);
                    BindingFlags bflags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
                    ConstructorInfo cInfo = typeof(T).GetConstructor(bflags, null, new Type[0] { }, null);
                    if (cInfo != null)
                    {
                        objT = (T)cInfo.Invoke(null);
                    }
                    else
                        objT = Activator.CreateInstance<T>();



                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                    for (int col = 0; col <= dt.Columns.Count - 1; col++)
                    {
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.Name.ToUpper() == dt.Columns[col].ColumnName.ToUpper() && !string.IsNullOrEmpty(Convert.ToString(row[col])))
                            {
                                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                                if (underlyingType == null)
                                {
                                    prop.SetValue(objT, Convert.ChangeType(row[col], prop.PropertyType));

                                }
                                else
                                {
                                    prop.SetValue(objT, Convert.ChangeType(row[col], underlyingType));
                                }
                            }
                        }
                    }

                    objList.Add(objT);
                }
            }
            return objList;
        }


        /// <summary>
        /// on the basis of type passed in "T", construct class from dataset
        /// </summary>

        public static T ContructClass<T>(DataSet ds)
        {
            T objT;

            Type t = typeof(T);
            BindingFlags bflags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            ConstructorInfo cInfo = typeof(T).GetConstructor(bflags, null, new Type[0] { }, null);
            objT = (T)cInfo.Invoke(null);
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (cInfo != null)
                    {
                        objT = (T)cInfo.Invoke(null);
                    }
                    else
                        objT = Activator.CreateInstance<T>();

                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                    for (int col = 0; col <= dt.Columns.Count - 1; col++)
                    {
                        foreach (PropertyDescriptor prop in properties)
                        {
                            if (prop.Name.ToUpper() == dt.Columns[col].ColumnName.ToUpper() && !string.IsNullOrEmpty(Convert.ToString(row[col])))
                            {
                                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                                if (underlyingType == null)
                                {
                                    prop.SetValue(objT, Convert.ChangeType(row[col], prop.PropertyType));

                                }
                                else
                                {
                                    prop.SetValue(objT, Convert.ChangeType(row[col], underlyingType));


                                }
                            }
                        }
                    }
                }
            }

            return objT;

        }

    }

}