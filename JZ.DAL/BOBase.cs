using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace JZ.BLL
{
    public class BOBase : Yqun.Data.DataBase.DataService
    {
        public BOBase()
        {
            bool DataISAttach = false;
            bool DataIntegratedLogin = false;
            string DataAdapterType = "sqlclient";
            string DataBaseType = "mssqlserver2k5";
            string DataSource = @"";
            string DataInstance = "";
            string DataUserName = "";
            string DataPassword = "";
            sys_line projectModel = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
            if (projectModel != null)
            {
                DataSource = projectModel.DataSourceAddress;
                DataInstance = projectModel.DataBaseName;
                DataUserName = projectModel.UserName;
                DataPassword = projectModel.PassWord;
            }
            CreateInstance(DataAdapterType, DataBaseType, DataIntegratedLogin, DataSource, DataInstance, DataUserName, DataPassword, DataISAttach, "");
        }

        public BOBase(String appKey)
        {
            bool DataISAttach = false;
            bool DataIntegratedLogin = false;
            string DataAdapterType = "sqlclient";
            string DataBaseType = "mssqlserver2k5";
            string DataSource = @"";
            string DataInstance = "";
            string DataUserName = "";
            string DataPassword = "";
            String appString = ConfigurationManager.AppSettings[appKey];
            if (!String.IsNullOrEmpty(appString))
            {
                String[] arr = appString.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 4)
                {
                    DataSource = arr[0];
                    DataInstance = arr[1];
                    DataUserName = arr[2];
                    DataPassword = arr[3];
                }
            }
            CreateInstance(DataAdapterType, DataBaseType, DataIntegratedLogin, DataSource, DataInstance, DataUserName, DataPassword, DataISAttach, "");
        }

        public BOBase(string _DataAdapterType,
            string _DataBaseType,
            bool _ISIntegrated,
            string _DataSource,
            string _Instance,
            string _UserName,
            string _Password,
            bool _ISAttach,
            string _AppRoot
            )
            : base(_DataAdapterType, _DataBaseType, _ISIntegrated,
            _DataSource, _Instance, _UserName, _Password, _ISAttach, _AppRoot)
        {

        }
        /// <summary>
        /// 通用分页
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="returnFields"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="key"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordsCount"></param>
        /// <returns></returns>
        public DataTable GetPagedDataTable(String tableName, String returnFields, String sqlWhere, String key, 
            String orderName, String orderType, Int32 pageIndex, Int32 pageSize, 
             out int pageCount, out int recordsCount)
        {
            int sordid = 1;
            if (orderType.ToLower().ToString() == "asc")
            {
                sordid = 0;
            }
            DataTable tb = new DataTable();
            SqlConnection con = null;
            try
            {
                con = Connection as SqlConnection;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("spweb_pager", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p1 = new SqlParameter("@tblName", SqlDbType.NVarChar);
                p1.Value = tableName;
                cmd.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter("@fldName", SqlDbType.NVarChar);
                p2.Value = returnFields;
                cmd.Parameters.Add(p2);

                SqlParameter p3 = new SqlParameter("@pageSize", SqlDbType.Int);
                p3.Value = pageSize;
                cmd.Parameters.Add(p3);

                SqlParameter p4 = new SqlParameter("@page", SqlDbType.Int);
                p4.Value = pageIndex;
                cmd.Parameters.Add(p4);

                SqlParameter p5 = new SqlParameter("@pageCount", SqlDbType.Int);
                p5.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p5);

                SqlParameter p6 = new SqlParameter("@Counts", SqlDbType.Int);
                p6.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p6);

                SqlParameter p7 = new SqlParameter("@fldSort", SqlDbType.NVarChar);
                p7.Value = orderName;
                cmd.Parameters.Add(p7);

                SqlParameter p8 = new SqlParameter("@Sort", SqlDbType.Bit);
                p8.Value = sordid;
                cmd.Parameters.Add(p8);

                SqlParameter p9 = new SqlParameter("@strCondition", SqlDbType.NVarChar);
                p9.Value = sqlWhere;
                cmd.Parameters.Add(p9);

                SqlParameter p10 = new SqlParameter("@ID", SqlDbType.NVarChar);
                p10.Value = key;
                cmd.Parameters.Add(p10);

                SqlParameter p11 = new SqlParameter("@Dist", SqlDbType.Bit);
                p11.Value = 0;
                cmd.Parameters.Add(p11);

                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(tb);

                recordsCount = Convert.ToInt32(p6.Value);
                pageCount = Convert.ToInt32(p5.Value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }


            return tb;
          
        }


        public DataTable GetDataTableFromProc(String procname, String startdate, String enddate, String testcode, int ftype, Int32 pageIndex, Int32 pageSize, String orderName, String orderType, out int pageCount, out int recordsCount)
        {
            int sordid = 1;
            if (orderType.ToLower().ToString() == "asc")
            {
                sordid = 0;
            }
       
            DataTable tb = new DataTable();
            SqlConnection con = null;
            try
            {
                con = Connection as SqlConnection;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand(procname, con);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
                p1.Value = startdate;
                cmd.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
                p2.Value = enddate;
                cmd.Parameters.Add(p2);

                SqlParameter p3 = new SqlParameter("@pageSize", SqlDbType.Int);
                p3.Value = pageSize;
                cmd.Parameters.Add(p3);

                SqlParameter p4 = new SqlParameter("@page", SqlDbType.Int);
                p4.Value = pageIndex;
                cmd.Parameters.Add(p4);

                SqlParameter p5 = new SqlParameter("@pageCount", SqlDbType.Int);
                p5.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p5);

                SqlParameter p6 = new SqlParameter("@Counts", SqlDbType.Int);
                p6.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p6);


                SqlParameter p7 = new SqlParameter("@fldSort", SqlDbType.VarChar);
                p7.Value = orderName;
                cmd.Parameters.Add(p7);

                SqlParameter p8 = new SqlParameter("@Sort", SqlDbType.Bit);
                p8.Value = sordid;
                cmd.Parameters.Add(p8);

                SqlParameter p9 = new SqlParameter("@testcode", SqlDbType.VarChar);
                p9.Value =testcode ;
                cmd.Parameters.Add(p9);

                SqlParameter p10 = new SqlParameter("@ftype", SqlDbType.TinyInt);
                p10.Value = ftype;
                cmd.Parameters.Add(p10);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(tb);

                recordsCount = Convert.ToInt32(p6.Value);
                pageCount = Convert.ToInt32(p5.Value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }


            return tb;

        }

        public DataTable GetDataTableFromProc(String procname, String startdate, String enddate, String testcode,String modelid, int ftype, Int32 pageIndex, Int32 pageSize, String orderName, String orderType, out int pageCount, out int recordsCount)
        {
            int sordid = 1;
            if (orderType.ToLower().ToString() == "asc")
            {
                sordid = 0;
            }

            DataTable tb = new DataTable();
            SqlConnection con = null;
            try
            {
                con = Connection as SqlConnection;
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand(procname, con);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
                p1.Value = startdate;
                cmd.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
                p2.Value = enddate;
                cmd.Parameters.Add(p2);

                SqlParameter p3 = new SqlParameter("@pageSize", SqlDbType.Int);
                p3.Value = pageSize;
                cmd.Parameters.Add(p3);

                SqlParameter p4 = new SqlParameter("@page", SqlDbType.Int);
                p4.Value = pageIndex;
                cmd.Parameters.Add(p4);

                SqlParameter p5 = new SqlParameter("@pageCount", SqlDbType.Int);
                p5.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p5);

                SqlParameter p6 = new SqlParameter("@Counts", SqlDbType.Int);
                p6.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(p6);


                SqlParameter p7 = new SqlParameter("@fldSort", SqlDbType.VarChar);
                p7.Value = orderName;
                cmd.Parameters.Add(p7);

                SqlParameter p8 = new SqlParameter("@Sort", SqlDbType.Bit);
                p8.Value = sordid;
                cmd.Parameters.Add(p8);

                SqlParameter p9 = new SqlParameter("@testcode", SqlDbType.VarChar);
                p9.Value = testcode;
                cmd.Parameters.Add(p9);

                SqlParameter p10 = new SqlParameter("@ftype", SqlDbType.TinyInt);
                p10.Value = ftype;
                cmd.Parameters.Add(p10);

                SqlParameter p11 = new SqlParameter("@modelID", SqlDbType.VarChar);
                p11.Value = modelid;
                cmd.Parameters.Add(p11);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(tb);

                recordsCount = Convert.ToInt32(p6.Value);
                pageCount = Convert.ToInt32(p5.Value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
            }


            return tb;

        }

     public DataTable GetDataTableFromProcCharts(String procname, String startdate, String enddate)
     {
       
         DataTable tb = new DataTable();
         SqlConnection con = null;
         try
         {
             con = Connection as SqlConnection;
             if (con.State != ConnectionState.Open)
             {
                 con.Open();
             }

             SqlCommand cmd = new SqlCommand(procname, con);
             cmd.CommandType = CommandType.StoredProcedure;

             SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
             p1.Value = startdate;
             cmd.Parameters.Add(p1);

             SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
             p2.Value = enddate;
             cmd.Parameters.Add(p2);         

             SqlDataAdapter adap = new SqlDataAdapter(cmd);
             adap.Fill(tb);

           
         }
         catch (Exception ex)
         {

             throw ex;
         }
         finally
         {
             con.Close();
         }


         return tb;

     }

     //TODO:Charts待定
     public DataTable GetDataTableFromProcCharts(String procname, String startdate, String enddate,String testcode)
     {

         DataTable tb = new DataTable();
         SqlConnection con = null;
         try
         {
             con = Connection as SqlConnection;
             if (con.State != ConnectionState.Open)
             {
                 con.Open();
             }

             SqlCommand cmd = new SqlCommand(procname, con);
             cmd.CommandType = CommandType.StoredProcedure;

             SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
             p1.Value = startdate;
             cmd.Parameters.Add(p1);

             SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
             p2.Value = enddate;
             cmd.Parameters.Add(p2);

             SqlParameter p3 = new SqlParameter("@testcode", SqlDbType.VarChar);
             p3.Value = testcode;
             cmd.Parameters.Add(p3);

             SqlDataAdapter adap = new SqlDataAdapter(cmd);
             adap.Fill(tb);


         }
         catch (Exception ex)
         {

             throw ex;
         }
         finally
         {
             con.Close();
         }


         return tb;

     }

        /// <summary>
        /// 底层五个参数
        /// </summary>
        /// <param name="procname">存储过程名字</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="testcode">试验室编码</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
     public DataTable GetDataTableFromProcChartsPara5(String procname, String startdate, String enddate, String testcode,int type)
     {

         DataTable tb = new DataTable();
         SqlConnection con = null;
         try
         {
             con = Connection as SqlConnection;
             if (con.State != ConnectionState.Open)
             {
                 con.Open();
             }
             SqlCommand cmd = new SqlCommand(procname, con);
             cmd.CommandType = CommandType.StoredProcedure;
             SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
             p1.Value = startdate;
             cmd.Parameters.Add(p1);
             SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
             p2.Value = enddate;
             cmd.Parameters.Add(p2);
             SqlParameter p3 = new SqlParameter("@testcode", SqlDbType.VarChar);
             p3.Value = testcode;
             cmd.Parameters.Add(p3);
             SqlParameter p4 = new SqlParameter("@ftype", SqlDbType.Int);
             p4.Value = type;
             cmd.Parameters.Add(p4);

             SqlDataAdapter adap = new SqlDataAdapter(cmd);
             adap.Fill(tb);


         }
         catch (Exception ex)
         {

             throw ex;
         }
         finally
         {
             con.Close();
         }


         return tb;

     }


     public DataTable GetDataTableFromProcChartsPara6(String procname, int ftype, String startdate, String enddate, String testcode, String modelid)
     {

         DataTable tb = new DataTable();
         SqlConnection con = null;
         try
         {
             con = Connection as SqlConnection;
             if (con.State != ConnectionState.Open)
             {
                 con.Open();
             }

             SqlCommand cmd = new SqlCommand(procname, con);
             cmd.CommandType = CommandType.StoredProcedure;

             SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
             p1.Value = startdate;
             cmd.Parameters.Add(p1);

             SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
             p2.Value = enddate;
             cmd.Parameters.Add(p2);

             SqlParameter p3 = new SqlParameter("@testcode", SqlDbType.VarChar);
             p3.Value = testcode;
             cmd.Parameters.Add(p3);

             SqlParameter p4 = new SqlParameter("@modelID", SqlDbType.VarChar);
             p4.Value = modelid;
             cmd.Parameters.Add(p4);

             SqlParameter p5 = new SqlParameter("@ftype", SqlDbType.TinyInt);
             p5.Value = ftype;
             cmd.Parameters.Add(p5);

             SqlDataAdapter adap = new SqlDataAdapter(cmd);
             adap.Fill(tb);


         }
         catch (Exception ex)
         {

             throw ex;
         }
         finally
         {
             con.Close();
         }


         return tb;

     }


     public DataTable GetDataTableTongChartsProc(String procname, String type, String startdate, String enddate, String testcode, String modelid,string marks)
     {

         DataTable tb = new DataTable();
         SqlConnection con = null;
         try
         {
             con = Connection as SqlConnection;
             if (con.State != ConnectionState.Open)
             {
                 con.Open();
             }

             SqlCommand cmd = new SqlCommand(procname, con);
             cmd.CommandType = CommandType.StoredProcedure;

             SqlParameter p1 = new SqlParameter("@startdate", SqlDbType.VarChar);
             p1.Value = startdate;
             cmd.Parameters.Add(p1);

             SqlParameter p2 = new SqlParameter("@enddate", SqlDbType.VarChar);
             p2.Value = enddate;
             cmd.Parameters.Add(p2);

             SqlParameter p3 = new SqlParameter("@testcode", SqlDbType.VarChar);
             p3.Value = testcode;
             cmd.Parameters.Add(p3);

             SqlParameter p4 = new SqlParameter("@modelID", SqlDbType.VarChar);
             p4.Value = modelid;
             cmd.Parameters.Add(p4);

             SqlParameter p5 = new SqlParameter("@type", SqlDbType.VarChar);
             p5.Value = type;
             cmd.Parameters.Add(p5);
             SqlParameter p6 = new SqlParameter("@mark", SqlDbType.VarChar);
             p6.Value = marks;
             cmd.Parameters.Add(p6);

             SqlDataAdapter adap = new SqlDataAdapter(cmd);
             adap.Fill(tb);
         }
         catch (Exception ex)
         {

             throw ex;
         }
         finally
         {
             con.Close();
         }
         return tb;

     }

    }
}
