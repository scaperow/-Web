using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Newtonsoft.Json;
using JZ.BLL;
using System.Data;

public partial class report_Search : BasePage
{
    


    protected void Page_Load(object sender, EventArgs e)
    {
        List();
    }

    private void List()
    {
        if ("Act".RequestStr() != "List")
        {
            return;
        }
        string sqlwhere = " and status>0 and BGRQ between '" + "StartDate".RequestStr() + "' AND '" + DateTime.Parse("EndDate".RequestStr()).AddDays(1).ToString("yyyy-MM-dd") + "'";


        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {
            sqlwhere += @" AND m.StatisticsCatlog IN (" + Server.UrlDecode("RPNAME".RequestStr()) + ") ";
        }

        if (!String.IsNullOrEmpty(Request.Params["bgmc"]))
        {
            sqlwhere += " and  m.name   LIKE '%" + Request.Params["bgmc"].Trim() + "%'";
        }
        if (!String.IsNullOrEmpty(Request.Params["wtbh"]))
        {
            sqlwhere += " and  WTBH LIKE '%" + Request.Params["wtbh"].Trim() + "%'";
        }
        if (!String.IsNullOrEmpty(Request.Params["bgbh"]))
        {
            sqlwhere += " and  BGBH LIKE '%" + Request.Params["bgbh"].Trim() + "%'";
        }
        if (!"NUM".RequestStr().IsNullOrEmpty())
        {
            sqlwhere += " and testroomcode in ('" + "NUM".RequestStr() + "') ";
        }
        else if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
        }

        BLL_LoginLog BLL = new BLL_LoginLog();

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
                        select d.ID from sys_document  d left outer join sys_module m on d.ModuleId = m.id
                        where 1=1  {0}   Order By  BGRQ DESC

                        SELECT     
                        sys_document.ID,
	                    BGBH ,
	                    WTBH,CONVERT(NVARCHAR(10),BGRQ,120) BGRQ ,CompanyCode,TestRoomCode,SegmentCode,
                        sys_tree.description SegmentName, t2.description   CompanyName,t1.description TestRoomName
	                    ,sys_module.DeviceType ,
	                    ModuleId,  
                        sys_module.name as MName
                        FROM sys_document  
                        left outer join sys_module on sys_document.ModuleId = sys_module.id
                        left outer join sys_tree on sys_document.SegmentCode = sys_tree.nodecode
                        left outer join sys_tree t1 on sys_document.TestRoomCode = t1.nodecode
                        left outer join sys_tree t2 on sys_document.CompanyCode = t2.nodecode 
 
                        INNER JOIN @TempTable t ON sys_document.ID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        Order By  BGRQ DESC

                        DECLARE @C int
                        select @C= count(d.ID)  from sys_document d left outer join sys_module m on d.ModuleId = m.id  where 1=1  {0}  
                        select @C 
                        ";

        string PageIndex = Request["page"];
        string PageSize = Request["rows"];

        Sql = string.Format(Sql, sqlwhere, PageIndex, PageSize);


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

 
        int records = DSs.Tables[1].Rows[0][0].ToString().Toint();
        
        #endregion


        if (DSs.Tables[0] != null)
        {
            Response.Write(DSs.Tables[0].ToJsonForEasyUI(records, ""));
           
        }

        Response.End();
    }
}