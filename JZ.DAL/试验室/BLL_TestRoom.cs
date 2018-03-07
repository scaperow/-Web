using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace JZ.BLL
{
    public class BLL_TestRoom : JZ.BLL.BOBase
    {
        public DataTable GetTree()
        {
            return GetDataTable(@"SELECT NodeCode ,
          DESCRIPTION ,
          DepType ,
          OrderID FROM dbo.Sys_Tree ORDER BY OrderID");
        }

        public DataTable GetUsersTestRoom(string UserName)
        {
            DataSet ds = new DataSet();
            string SqlText = " SELECT * FROM dbo.sys_users_testroom";
            string[] cols = new string[] { "username" };
            string[] exps = new string[] { "=" };
            object[] valus = new object[] { UserName };

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
    }
}
