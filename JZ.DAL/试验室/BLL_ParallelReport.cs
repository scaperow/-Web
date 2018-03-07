using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
   public class BLL_ParallelReport : JZ.BLL.BOBase
    {
       public DataTable GetProcDataTableChartsPara6(string procname, int ftype, String startdate, String enddate, String testcode, string modelid)
       {
           DataTable dt = GetDataTableFromProcChartsPara6(procname, ftype, startdate, enddate, testcode, modelid);
           return dt;
       }

       public DataTable GetProcDataTable(string procname, String startdate, String enddate, String testcode, String modelid, int ftype, int pageIndex, int pageSize
    , String OrderField, String OrderType, out int pageCount, out int recordsCount)
       {
           DataTable dt = GetDataTableFromProc(procname, startdate, enddate, testcode, modelid, ftype, pageIndex, pageSize, OrderField, OrderType, out pageCount, out recordsCount);
           return dt;
       }

       public DataTable GetProcDataTableChartsPara5(string procname, String startdate, String enddate, string testcode, int type)
       {
           DataTable dt = GetDataTableFromProcChartsPara5(procname, startdate, enddate, testcode, type);
           return dt;
       }

       public DataTable GetProcDataTable(string procname, String startdate, String enddate, String testcode, int ftype, int pageIndex, int pageSize
   , String OrderField, String OrderType, out int pageCount, out int recordsCount)
       {
           DataTable dt = GetDataTableFromProc(procname, startdate, enddate, testcode, ftype, pageIndex, pageSize, OrderField, OrderType, out pageCount, out recordsCount);
           return dt;
       }
    }
}
