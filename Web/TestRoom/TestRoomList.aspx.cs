using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using JZ.BLL;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class TestRoom_TestRoomList : BasePage
{
    #region 参数



    #endregion

    #region 周期
    protected void Page_Load(object sender, EventArgs e)
    {

        switch ("Act".RequestStr())
        {
            case "List":
                Response.Write(GetTestRoom());
                Response.End();

                break;

        }
    }
    #endregion

    #region 方法



    public string GetTestRoom()
    {
        StringBuilder Result = new StringBuilder();
        string SqlCmd = @"SELECT NodeCode ,
                              DESCRIPTION ,OrderID,DepType,
                              left(NodeCode,len(NodeCode)-4) as ParentID
                              FROM dbo.Sys_Tree ORDER BY OrderID";

        DataTable DT = new BLL_Document().GetDataTable(SqlCmd);

        DT.DefaultView.RowFilter = " ParentID = '' ";

        if (DT.DefaultView.ToTable().Rows.Count > 0)
        {
            DT.DefaultView.RowFilter = " ParentID = '" + DT.DefaultView.ToTable().Rows[0]["NodeCode"] + "' ";

            Result.Append("[");
            int i = 0;
            foreach (DataRow Dr in DT.DefaultView.ToTable().Rows)
            {
                if (TestRoomHasChildren(DT.DefaultView, Dr["NodeCode"].ToString()))
                {
                    Result.Append(i == 0 ? "" : ",");
                    Result.Append(TestRoomChildren(DT.DefaultView, Dr["NodeCode"].ToString(), Dr["DESCRIPTION"].ToString()));
                    i++;
                }
            }
            Result.Append("]");
        }
        DT.DefaultView.RowFilter = " ";
        return Result.ToString();
    }

    /// <summary>
    /// 获取下级试验室
    /// </summary>
    /// <param name="Dv"></param>
    /// <param name="ParentID"></param>
    /// <param name="ParentName"></param>
    /// <returns></returns>
    public string TestRoomChildren(DataView Dv, string ParentID, string ParentName)
    {

        StringBuilder Result = new StringBuilder();
        Dv.RowFilter = " ParentID = '" + ParentID + "' ";
        Dv.Sort = " OrderID ASC";
        int r = 0;

        foreach (DataRow Dr in Dv.ToTable().Rows)
        {
            Result.Append(r == 0 ? "" : ",");
            Result.Append("{");
            Result.Append("\"id\":\"" + Dr["NodeCode"].ToString() + "\",\"text\":\"" + ParentName + "  " + Dr["DESCRIPTION"].ToString() + "\",");
            int i = 0;
            foreach (DataColumn Dc in Dv.ToTable().Columns)
            {
                Result.Append(i == 0 ? "" : ",");
                Result.Append("\"" + Dc.ColumnName + "\":\"" + Dr[Dc.ColumnName].ToString() + "\"");
                i++;
            }
            if (TestRoomHasChildren(Dv, Dr["NodeCode"].ToString()))
            {
                Result.Append(",\"state\":\"closed\",\"children\":[");
                Result.Append(TestRoomChildren(Dv, Dr["NodeCode"].ToString(), ""));
                Result.Append("]");
            }
            Result.Append("}");
            r++;
        }

        Dv.RowFilter = "  ";
        return Result.ToString();
    }

    /// <summary>
    /// 判断是否有下级
    /// </summary>
    /// <param name="Dv"></param>
    /// <param name="ParentID"></param>
    /// <returns></returns>
    public bool TestRoomHasChildren(DataView Dv, string ParentID)
    {
        Dv.RowFilter = " ParentID = '" + ParentID + "' ";
        return Dv.ToTable().Rows.Count > 0;
    }




    #endregion
}