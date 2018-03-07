using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    public class BLL_Document : JZ.BLL.BOBase
    {
        public DataTable GetProcDataTableChartsPara5(string procname, String startdate, String enddate, string testcode, int type)
        {
            DataTable dt = GetDataTableFromProcChartsPara5(procname, startdate, enddate, testcode, type);
            return dt;
        }

        public DataTable GetProcDataTableChartsPara6(string procname, int ftype, String startdate, String enddate, String testcode, string modelid)
        {
            DataTable dt = GetDataTableFromProcChartsPara6(procname, ftype, startdate, enddate, testcode, modelid);
            return dt;
        }


        public DataTable GetProcDataTable(string procname, String startdate, String enddate, String testcode, int ftype, int pageIndex, int pageSize
           , String OrderField, String OrderType, out int pageCount, out int recordsCount)
        {
            DataTable dt = GetDataTableFromProc(procname, startdate, enddate, testcode, ftype, pageIndex, pageSize, OrderField, OrderType, out pageCount, out recordsCount);
            return dt;
        }

        public DataTable GetProcDataTable(string procname, String startdate, String enddate, String testcode, String modelid, int ftype, int pageIndex, int pageSize
      , String OrderField, String OrderType, out int pageCount, out int recordsCount)
        {
            DataTable dt = GetDataTableFromProc(procname, startdate, enddate, testcode, modelid, ftype, pageIndex, pageSize, OrderField, OrderType, out pageCount, out recordsCount);
            return dt;
        }


        public DataTable GetDocument(string id)
        {
            DataSet ds = new DataSet();
            string SqlText = " SELECT ID,CreatedTime,Data,ModuleID FROM  dbo.sys_document ";
            string[] cols = new string[] { "ID" };
            string[] exps = new string[] { "=" };
            object[] valus = new object[] { id };

            ds = GetDataSet(SqlText, cols, exps, valus);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetTestData(string id,string num)
        {
            DataSet ds = new DataSet();
            string SqlText = "  SELECT RealTimeData,SerialNumber FROM dbo.sys_test_data  WHERE  DataID='" + id + "' AND Status=1 and SerialNumber="+num+" ORDER BY SerialNumber  ";
            //string[] cols = new string[] { "DataID",  "Status" };
            //string[] exps = new string[] { "=","=" };
            //object[] valus = new object[] { id,1 };

            ds = GetDataSet(SqlText);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}
