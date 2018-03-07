using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft;

public partial class baseInfo_userSummary : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {
                case "LIST":
                    List();
                    break;

            }
        }
    }


    public void List()
    {


        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr().ToDateTime().ToString("yyyy-MM-dd");
        string PageIndex = "page".RequestStr();
        string PageSize = "rows".RequestStr();
        string zglg = "zglg".RequestStr();

       



        BLL_UserInfo BLL = new BLL_UserInfo();
        string sqlwhere = " AND 1=1 ";
        if (!"NUM".RequestStr().IsNullOrEmpty())
        {
            sqlwhere += " and 试验室编码 in ('" + "NUM".RequestStr() + "') ";
        }
        else if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and 试验室编码 in (" + SelectedTestRoomCodes + ") ";
        }

        //Ext21 到岗日  Ext22 离岗日期

        if (zglg.Contains("1"))
        {
            sqlwhere += " AND   Status>0  ";
        }
        if (zglg.Contains("0"))
        {
            sqlwhere += " AND  Status=0  ";//(Ext22 <> '' OR Ext22 is not null ) AND 
        }

     

        #region For首页
        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {

            switch ("RPNAME".RequestStr())
            {
                case "ADD": //新增
                    sqlwhere += " AND CreatedTime between '" + StartDate + "' and '" + EndDate + "' AND  Status>0 ";
                    break;
                case "DEL": //调减
                    sqlwhere += " AND CreatedTime between '" + StartDate + "' and '" + EndDate + "' AND  Status=0 ";
                    break;
                default:
                    sqlwhere += " AND  Status>0 ";
                    break;
            }
        }

        #endregion

        #region  使用脚本分页

        string Sql = @" 
                        DECLARE @Page int
                        DECLARE @PageSize int
                        SET @Page = {1}
                        SET @PageSize = {2}
                        SET NOCOUNT ON
                        DECLARE @TempTable TABLE (IndexId int identity, _keyID varchar(50))
                        INSERT INTO @TempTable
                        (
	                        _keyID
                        )
                        SELECT ID
						FROM dbo.sys_document a JOIN dbo.v_bs_codeName b 
						ON a.ModuleID='08899BA2-CC88-403E-9182-3EF73F5FB0CE'  
						{0}
						AND a.TestRoomCode=b.试验室编码  
						JOIN dbo.Sys_Tree c ON  LEFT(a.TestRoomCode,12)=c.NodeCode
                        Order By  c.OrderID ASC


					    SELECT ID,TestRoomCode,b.标段名称,b.单位名称, b.试验室名称  
						,Ext1 姓名,Ext2 性别,Ext3 年龄,Ext4 技术职称,Ext5 职务,Ext6 工作年限,Ext7 联系电话,Ext8 学历,Ext9 毕业学校,Ext10 专业,1   num 
						FROM dbo.sys_document a JOIN dbo.v_bs_codeName b 
						ON a.ModuleID='08899BA2-CC88-403E-9182-3EF73F5FB0CE'  
						{0}
						AND a.TestRoomCode=b.试验室编码  
						JOIN dbo.Sys_Tree c ON  LEFT(a.TestRoomCode,12)=c.NodeCode
                        INNER JOIN @TempTable t ON a.ID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        Order By  OrderID,TestRoomCode ASC

                        DECLARE @C int
                        select @C= count(ID)  from dbo.sys_document a JOIN dbo.v_bs_codeName b 
						ON a.ModuleID='08899BA2-CC88-403E-9182-3EF73F5FB0CE'  
						{0}
						AND a.TestRoomCode=b.试验室编码  
						JOIN dbo.Sys_Tree c ON  LEFT(a.TestRoomCode,12)=c.NodeCode
  
                        select @C 
                        ";

        Sql = string.Format(Sql, sqlwhere, PageIndex, PageSize);

        //DataSet DS = BLL.GetDataSet(Sql);

        DataSet DSs = new DataSet();
        using (System.Data.SqlClient.SqlConnection Conn = BLL.Connection as System.Data.SqlClient.SqlConnection)
        {
            Conn.Open();
            using (System.Data.SqlClient.SqlCommand Cmd = new System.Data.SqlClient.SqlCommand(Sql, Conn))
            {
                using (System.Data.SqlClient.SqlDataAdapter Adp = new System.Data.SqlClient.SqlDataAdapter(Cmd))
                {
                    Adp.Fill(DSs);
                }
            }
            Conn.Close();
        }

        decimal Tempc = Math.Round(decimal.Parse(DSs.Tables[1].Rows[0][0].ToString()) / decimal.Parse(PageSize.ToString()), 2);
        Tempc = Math.Ceiling(Tempc);

        int records = DSs.Tables[1].Rows[0][0].ToString().Toint();
        int pageCount = Tempc.ToString().Toint();
        #endregion

        string result = "{\"total\": \"" + pageCount + "\", \"page\": \"" + PageIndex.ToString() + "\", \"records\": \"" + records + "\", \"rows\" : " + Newtonsoft.Json.JsonConvert.SerializeObject(DSs.Tables[0]) + "}";

        if (DSs.Tables[0] != null)
        {
            Response.Write(result);
            Response.End();
        }
        else
        {
            Response.Write("");
            Response.End();
        }
    }
}

