using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
  public  class BLL_FailReport: JZ.BLL.BOBase
    {
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


      public DataTable GetDataTablePager(String tablename, String fileds, String sqlWhere, String key, String orderField, String orderType, int pageIndex, int pageSize
          , out int pageCount, out int recordsCount)
      {
          DataTable dt = GetPagedDataTable(tablename, fileds, sqlWhere, key, orderField, orderType,
              pageIndex, pageSize, out pageCount, out recordsCount);

          return dt;
      }


      public DataTable GetProcDataTable(string procname, String startdate, String enddate, String testcode, String modelid, int ftype, int pageIndex, int pageSize
    , String OrderField, String OrderType, out int pageCount, out int recordsCount)
      {
          DataTable dt = GetDataTableFromProc(procname, startdate, enddate, testcode, modelid, ftype, pageIndex, pageSize, OrderField, OrderType, out pageCount, out recordsCount);
          return dt;
      }

      public DataTable GetFailInfo(string tablename,string id)
      {
          StringBuilder sql_select = new StringBuilder();
          sql_select.Append("SELECT * FROM  "+tablename+" WHERE IndexID='" + id + "'");
          return GetDataTable(sql_select.ToString());
      }

      public DataTable GetImageInfo(string id)
      {
          StringBuilder sql_select = new StringBuilder();
          sql_select.Append("SELECT * FROM dbo.sys_biz_Image WHERE ImgDataID='" + id + "'");
          return GetDataTable(sql_select.ToString());
      }

      public DataTable GetTablelist(string sql)
      {
          return GetDataTable(sql);
      }

    }
}
