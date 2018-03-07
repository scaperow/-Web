using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    public class BLL_UserInfo : JZ.BLL.BOBase
    {
        public DataTable GetTestPersonnelFiles(string TestCode, int len)
        {
            StringBuilder sql_select = new StringBuilder();
            sql_select.Append("SELECT * FROM  [dbo].[biz_norm_试验人员档案]  WHERE LEFT(SCPT," + len + ")='" + TestCode + "'");
            return GetDataTable(sql_select.ToString());
        }

        public DataTable GetUserInfo(string id)
        {
            StringBuilder sql_select = new StringBuilder();
            sql_select.Append("SELECT * FROM  [dbo].[biz_norm_试验人员档案]  WHERE id='"+id+"'");
            return GetDataTable(sql_select.ToString());
        }

        public DataTable GetProcDataTableChartsPara5(string procname, String startdate, String enddate, string testcode, int type)
        {
            DataTable dt = GetDataTableFromProcChartsPara5(procname, startdate, enddate, testcode, type);
            return dt;
        }

        public DataTable GetDataTablePager(String tablename, String fileds, String sqlWhere, String key, String orderField, String orderType, int pageIndex, int pageSize
            , out int pageCount, out int recordsCount)
        {
            DataTable dt = GetPagedDataTable(tablename, fileds, sqlWhere, key, orderField, orderType,
                pageIndex, pageSize, out pageCount, out recordsCount);

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
