using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using System.Text;
using System.Data;

public partial class report_betonReport : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Literal1.Text = sb.ToString();
            string SQL = "SELECT  ModelID,ModelName FROM dbo.biz_tongqiangdu GROUP BY ModelID,ModelName,OrderID ORDER BY OrderID ";
            DataSet ds = DbHelperSQL.Query(SQL);
            #region 绑定模板
            if (ds != null && ds.Tables.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                int n = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (n == 0)
                    {
                        sb.Append("<option value=\"" + dr["ModelID"].ToString() + "\"  selected=\"selected\">" + dr["ModelName"].ToString() + "</option>");
                        Label1.Text = dr["ModelName"].ToString();
                    }
                    else
                    {
                        sb.Append("<option value=\"" + dr["ModelID"].ToString() + "\">" + dr["ModelName"].ToString() + "</option>");
                    }
                    n++;
                   
                }
                Literal1.Text = sb.ToString();
            }
            #endregion
            SQL = "SELECT sage  FROM biz_tongqiangdu GROUP BY  sage ORDER BY sAge ";
            ds = DbHelperSQL.Query(SQL);
            #region 绑定龄期天数
 if (ds != null && ds.Tables.Count > 0)
 {
     StringBuilder sb = new StringBuilder();
     int n = 0;
     foreach (DataRow dr in ds.Tables[0].Rows)
     {
         if (n == 0)
         {
             sb.Append("<option value=\"" + dr["sAge"].ToString() + "\"  selected=\"selected\">" + dr["sAge"].ToString() + "天</option>");
             Label2.Text = dr["sAge"].ToString() + "天";
         }
         else
         {
             sb.Append("<option value=\"" + dr["sAge"].ToString() + "\">" + dr["sAge"].ToString() + "天</option>");
         }
         n++;

     }
     Literal2.Text = "   <option value=\"28\">28天</option> <option value=\"56\">56天</option>";
 }
 #endregion

 SQL = "SELECT cType FROM biz_tongqiangdu GROUP BY  cType ORDER BY cType ";
 ds = DbHelperSQL.Query(SQL);
 #region 绑定龄期天数
 if (ds != null && ds.Tables.Count > 0)
 {
     StringBuilder sb = new StringBuilder();
     int n = 0;
     foreach (DataRow dr in ds.Tables[0].Rows)
     {
         if (n == 0)
         {
             sb.Append("<option value=\"" + dr["cType"].ToString() + "\"  selected=\"selected\">" + dr["cType"].ToString() + "</option>");
             Label3.Text = dr["cType"].ToString();
         }
         else
         {
             sb.Append("<option value=\"" + dr["cType"].ToString() + "\">" + dr["cType"].ToString() + "</option>");
         }
         n++;

     }
     Literal3.Text = sb.ToString();
 }
 #endregion
        }
    }
}