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

public partial class baseInfo_documentSummarySearch : BasePage
{
    public int PageSize
    {
        get
        {
            int pagesize = 10;
            if (Request["rows"] != null)
            {
                pagesize = int.Parse(Request["rows"]);
            }
            return pagesize;
        }
    }

    public int PageIndex
    {
        get
        {
            int pageindex = 1;
            if (Request["page"] != null)
            {
                pageindex = int.Parse(Request["page"]);
            }
            return pageindex;
        }
    }
    int pageCount = 0;
    int records = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".RequestStr() == "List")
        {
          string json =  report_Search();
         string result = "{\"total\": \"" + pageCount + "\", \"page\": \"" + PageIndex.ToString() + "\", \"records\": \"" + records + "\", \"rows\" : " + json + "}";
         Response.Write(result);
         Response.End();
        }
    }
    private String report_Search()
    {//testroomid modelid 资料报告编号、报告日期、检测结果、打开报告

        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr() ;
        string testroomid = "testroomid".RequestStr();
        string modelid = "modelid".RequestStr();

       string fileds = @"ID,
 BGBH ,
 WTBH,CONVERT(NVARCHAR(10),BGRQ,120) BGRQ ,CompanyCode,TestRoomCode,SegmentCode,
    '' SegmentName,      ''   CompanyName,'' TestRoomName, '' MName,'' DeviceType ,
ModuleId
            ";



       string sqlwhere = " AND moduleid ='" + modelid + "' and status>0 and BGRQ>='" + StartDate + "' AND BGRQ<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";




      if (!String.IsNullOrEmpty(testroomid))
        {
            sqlwhere += " and testroomcode in ('" + testroomid + "') ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }
        BLL_LoginLog BLL = new BLL_LoginLog();
        // DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "BGRQ", OrderType, PageIndex, PageSize, out pageCount, out records);

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

        records = DSs.Tables[1].Rows[0][0].ToString().Toint();
         pageCount = Tempc.ToString().Toint();
        #endregion



        if (DSs.Tables[0] != null)
        {
            return JsonConvert.SerializeObject(DSs.Tables[0]);
        }
        else
        {
            return "";
        }
    }


}