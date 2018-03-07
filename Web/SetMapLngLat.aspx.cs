using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using JZ.BLL;



public partial class SetMapLngLat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PostHandler();
    }

    #region POST Method


    /// <summary>
    /// 请求控制
    /// </summary>
    private void PostHandler()
    {
        if ("ACT".IsRequest())
        {
            Response.Clear();

            switch ("ACT".RequestStr())
            {
                case "GetItems":
                    GetItems();
                    break;
                case "SaveLng":
                    SaveLng();
                    break;
            }

            Response.End();
        }
    }

    /// <summary>
    /// 获取实验室列表
    /// </summary>
    private void GetItems()
    {

        //string strSql = "select * from sys_engs_ItemInfo";
        string strSql = "SELECT a.ID,((SELECT [DESCRIPTION] FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,8))+'-'+(SELECT [DESCRIPTION] FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,12))+'-'+ a.Description) AS [Description],a.x,a.y,a.zoomLevel,b.NodeCode,b.NodeType,(SELECT OrderID FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,8) )AS OrderID FROM dbo.sys_engs_ItemInfo AS a,dbo.sys_engs_Tree AS b WHERE a.id=b.RalationID ORDER BY OrderID";
        DataSet Ds = DbHelperSQL.Query(strSql);

        StringBuilder Result = new StringBuilder();
        Result.Append("{\"items\":[");
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Result.AppendFormat("{{\"id\":\"{0}\",\"description\":\"{1}\",\"x\":\"{2}\",\"y\":\"{3}\"}}", new string[] { Ds.Tables[0].Rows[i]["id"].ToString(), Ds.Tables[0].Rows[i]["description"].ToString(), Ds.Tables[0].Rows[i]["x"].ToString(), Ds.Tables[0].Rows[i]["y"].ToString() });
            if (i < Ds.Tables[0].Rows.Count - 1)
            {
                Result.Append(",");
            }
        }
        Result.Append("]}");

        Response.Write(Result.ToString());
    }

    /// <summary>
    /// 保存坐标
    /// </summary>
    private void SaveLng()
    {

        if ("ID".IsRequest() && "X".IsRequest() && "Y".IsRequest())
        {
            string ID = "ID".RequestStr();
            string X = "X".RequestStr();
            string Y = "Y".RequestStr();

            
            try
            {
                string strSql = "update sys_engs_ItemInfo set x='" + X + "',y='" + Y + "' where id = '" + ID + "'";
                Response.Write(DbHelperSQL.ExecuteSql(strSql) < 1 ? "false" : "true");
            }
            catch
            {
                Response.Write("false");
            }

        }

    }

    #endregion

    

}



