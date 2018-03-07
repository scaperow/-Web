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

public partial class ajaxJQGrid : BasePage
{
    #region JQGrid传递的参数

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

    public string OrderField
    {
        get
        {
            try
            {
                return Request.QueryString["sidx"].ToString(); //也就是对应前台colModel中的index            
            }
            catch
            {
                return "id";   //这里默认用id，注意跟着自己的数据变动            
            }
        }
    }

    public string OrderType
    {
        get
        {
            try
            {
                return Request.QueryString["sord"].ToString();
            }
            catch
            {
                return "desc";
            }
        }
    }

    public string Search
    {
        get
        {
            try
            {
                //这里可以自行构造，jqGrid本身自带搜索功能，不过传递的是：searchField,searOper,searchString就像分别 "ID" , "equal", "5"
                return Request.QueryString["condition"].ToString();
            }
            catch
            {
                return "";
            }
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        String result = "";
        String sType = Request.Params["sType"];
        string sqlwhere = "";
        String sTestcode = "";
        String sModel = "";
        string sNuit = "";
        String sClassname = "";
        if (!String.IsNullOrEmpty(Request.Params["StartDate"]))
        {
            StartDate = Request.Params["StartDate"];
        }
        if (!String.IsNullOrEmpty(Request.Params["EndDate"]))
        {
            EndDate = Request.Params["EndDate"];
        }
        if (!String.IsNullOrEmpty(sType))
        {
            int records = 0, pageCount = 0;

            String json = "";
            switch (sType)
            {
                case "pxreport":
                    json = pxreport("", 1, out pageCount, out records);
                    break;
                case "jzreport":
                    json = pxreport("", 2, out pageCount, out records);
                    break;
                case "qxzlhzb":
                    json = qxzlhzb("spweb_qxzlhzb", 0, out pageCount, out records);
                    break;
                case "qxzlhzbcharttogrid":
                    sTestcode = Request.Params["sTestcode"];
                    sModel = Request.Params["sModelID"];
                    json = qxzlhzbcharttogrid("spweb_qxzlhzb_charts_pop_grid", 0, sTestcode, sModel, out pageCount, out records);
                    break;
                case "parallelchartgrid":
                    sTestcode = Request.Params["sTestcode"];
                    sModel = Request.Params["sModelID"];
                    sNuit = Request.Params["sNuit"];
                    json = parallelchartgrid("spweb_pxjzReport_ByCode", Int32.Parse(sNuit), sTestcode, sModel, out pageCount, out records);
                    break;
                case "parallelchartgridfail":
                    sTestcode = Request.Params["sTestcode"];
                    sModel = Request.Params["sModelID"];
                    sNuit = Request.Params["sNuit"];
                    json = parallelchartgrid("spweb_pxjzReport_ByCode_fail", Int32.Parse(sNuit), sTestcode, sModel, out pageCount, out records);
                    break;
                case "popfailgrid":
                    json = popfailgrid("biz_all_failreport", "*", sqlwhere, "IndexID", 0, out pageCount, out records);
                    break;
                case "unpopfailgrid":
                    json = unpopfailgrid("biz_all_failreport", "*", sqlwhere, "IndexID", 0, out pageCount, out records);
                    break;
                case "usersummarycharttogrid":
                    sTestcode = Request.Params["sTestcode"];
                    sModel = Request.Params["sCompanycode"];
                    sNuit = Request.Params["sNuit"];
                    sClassname = Request.Params["sClassname"];
                    json = usersummarycharttogrid("v_bs_userSummary", "*",sNuit,sModel,sTestcode,sClassname, "id", 1, out pageCount, out records);
                    break;
                case "usersummary":
                    json = usersummary("spweb_userSummary", 0, out pageCount, out records);
                    break;
                case "failreportsum":
                    json = failreportsum("spweb_failreport", 0, out pageCount, out records);
                    break;
                case "machinesummary":
                    json = machinesummary("v_bs_machineSummary", "*", "", "id", 0, out pageCount, out records);
                    break;
                case "machinesummarygrid":
                    sTestcode = Request.Params["sTestcode"];
                    json = machinesummary("v_bs_machineSummary", "*", " and 试验室编码='"+sTestcode+"' ", "id", 0, out pageCount, out records);
                    break;
                case "evaluatedata":                
                    json = evaluatedata("biz_all_failreport", "*", sqlwhere, "id", 0, out pageCount, out records);
                    break;
                case "loginLog":
                    json = loginLog("sys_loginlog", "*", sqlwhere, "loginDay", 0, out pageCount, out records);
                    break;
                case "operateLog":
                    json = operateLog("sys_operatelog", "*", sqlwhere, "modifiedDate", 0, out pageCount, out records);
                    break;

                case "SMS":
                    json = SMS("v_bs_sms", "*", sqlwhere, "id", 0, out pageCount, out records);
                    break;
                case "ageremindundo":
                    json = ageremindundo("spweb_ageremind", 0, out pageCount, out records);
                    break;
                case "stadiumLog":
                    json = stadiumLog("v_bs_reminder_stadiumData", "*", "", "id", 0, out pageCount, out records);
                    break;
                case "report_Search":
                    json = report_Search("sys_document", "*", sqlwhere, "ID", 0, out pageCount, out records);
                    break;
                default:
                    break;
            }
            result = "{\"total\": \"" + pageCount + "\", \"page\": \"" + PageIndex.ToString() + "\", \"records\": \"" + records + "\", \"rows\" : " + json + "}";

        }

        Response.Write(result);
    }


    #region 平行见证方法
    private String pxreport(String proc, int ftype, out int pageCount, out int records)
    {
        BLL_ParallelReport BLL = new BLL_ParallelReport();
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
        }
        else
        {
            proc = "sp_pxjz_report";
        }

        string Meet = "meet".RequestStr();
        Meet = Meet.IsNullOrEmpty() ? "0" : Meet;
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes.Replace("'", ""), ftype, PageIndex, PageSize, Meet, OrderType, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    private String parallelchartgrid(String proc, int ftype, String testcode, String modelid, out int pageCount, out int records)
    {
        BLL_ParallelReport BLL = new BLL_ParallelReport();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, modelid, ftype, PageIndex, PageSize, OrderField, OrderType, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }

    #endregion

    #region 全线资料方法
    private String qxzlhzb(String proc, int ftype, out int pageCount, out int records)
    {
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, ftype, PageIndex, PageSize, OrderField, OrderType, out pageCount, out records);

        #region 添加总数
        int pageCount1 = 0;
        int records1 = 0;
        string SelectedTestRoomCodes1 = "";

        foreach (DataRow Dr in dt.Rows)
        {
            SelectedTestRoomCodes1 += string.IsNullOrEmpty(SelectedTestRoomCodes1) ? "'" + Dr["testcode"].ToString() + "'" : ",'" + Dr["testcode"].ToString() + "'";
        }

        DataTable dt1 = BLL.GetProcDataTable(proc, DateTime.Parse("1999-1-1").ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes1, ftype, 1, 1000, OrderField, OrderType, out pageCount1, out records1);
        //testcode modelid
        dt.Columns.Add(new DataColumn("ncountA",typeof(string)));
        dt.Columns.Add(new DataColumn("wncountA", typeof(string)));
        DataTable _tempTable = new DataTable();
        foreach (DataRow Dr in dt.Rows)
        {
            dt1.DefaultView.RowFilter = " testcode='" + Dr["testcode"].ToString() + "' AND modelid='" + Dr["modelid"].ToString() + "' ";
            _tempTable = dt1.DefaultView.ToTable();
            Dr["ncountA"] = _tempTable.Rows.Count > 0 ? _tempTable.Rows[0]["ncount"].ToString() : "";
            Dr["wncountA"] = _tempTable.Rows.Count > 0 ? _tempTable.Rows[0]["wncount"].ToString() : "";
            dt1.DefaultView.RowFilter = "";
        }

        #endregion

        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    private String qxzlhzbcharttogrid(String proc, int ftype, String testcode, String modelid, out int pageCount, out int records)
    {
        BLL_Document BLL = new BLL_Document();
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        //if (sysBaseLine.IsActive == 0)
        //{
            testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        //}
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, modelid, ftype, PageIndex, PageSize, OrderField, OrderType, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }

    #endregion

    #region 人员情况方法
    private String usersummarycharttogrid(String name, String fileds, String sNuit, String sModel, String sTestcode, String sClassname, String key, int ftype, out int pageCount, out int records)
    {
        String sqlwhere = "";
        if (sNuit == "1")
        {
            sqlwhere = "and   单位编码='" + sModel + "' AND 试验室编码='" + sTestcode + "'";

        }
        if (sNuit == "2")
        {
            sqlwhere = "and   单位编码='" + sModel + "' AND 技术职称='" + sClassname + "'";

        }
        if (sNuit == "3")
        {
            sqlwhere = "and   单位编码='" + sModel + "' AND 学历='" + sClassname + "'";

        }
        if (sNuit == "4")
        {
            sqlwhere = "and   单位编码='" + sModel + "'";
            if (sClassname == "1-5年")
            {
                sqlwhere += "and   工作年限>=1 and 工作年限<5 ";
            }
            if (sClassname == "5-10年")
            {
                sqlwhere += "and   工作年限>=5 and 工作年限<10 ";
            }
            if (sClassname == "10-15年")
            {
                sqlwhere += "and   工作年限>=10 and 工作年限<15 ";
            }
            if (sClassname == "15-20年")
            {
                sqlwhere += "and   工作年限>=15 and 工作年限<20 ";
            }
            if (sClassname == "20年以上")
            {
                sqlwhere += "and   工作年限>=20 ";
            }
        }
        BLL_UserInfo BLL = new BLL_UserInfo();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, OrderField, OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 人员
    /// </summary>
    /// <param name="proc"></param>
    /// <param name="ftype"></param>
    /// <param name="pageCount"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    private String usersummary(String proc, int ftype, out int pageCount, out int records)
    {
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


        #region For首页
        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {

            switch ("RPNAME".RequestStr())
            {
                case "ADD": //新增
                    sqlwhere += " AND CreatedTime between '" + StartDate + "' and '" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' AND  Status>0 ";
                    break;
                case "DEL": //调减
                    sqlwhere += " AND CreatedTime between '" + StartDate + "' and '" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' AND  Status=0 ";
                    break;
                default :
                    sqlwhere += " AND  Status>0 ";
                    break;
            }
        }
        else
        {
            sqlwhere +=  " AND  Status>0 ";
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

    #endregion

    #region 不合格报告方法
    private String failreportsum(String proc, int ftype, out int pageCount, out int records)
    {
        BLL_FailReport BLL = new BLL_FailReport();
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, ftype, PageIndex, PageSize, OrderField, OrderType, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    /// <summary>
    /// 不合格数据分析 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fileds"></param>
    /// <param name="sqlwhere"></param>
    /// <param name="key"></param>
    /// <param name="ftype"></param>
    /// <param name="pageCount"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    private String evaluatedata(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
            sqlwhere = "and ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
            {
                sqlwhere += " and TestRoomCode in (" + SelectedTestRoomCodes + ") ";
            }
            if (!String.IsNullOrEmpty(Request.Params["sReportCode"]))
            {
                sqlwhere += " and  ReportNumber LIKE '%" + Request.Params["sReportCode"].Trim() + "%'";
            }
            if (!String.IsNullOrEmpty(Request.Params["sReportName"]))
            {
                sqlwhere += " and  ReportName LIKE '%" + Request.Params["sReportName"].Trim() + "%'";
            }
        }
        else
        {
            name = "v_invalid_document";
            sqlwhere = "and ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' AND  F_InvalidItem NOT LIKE '%#%' AND AdditionalQualified=0 ";

            #region  FOR 首页
            if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
            {

                switch ("RPNAME".RequestStr())
                {
                    case "1": //混凝土抗压
                        sqlwhere += @"  AND ModelIndex IN (
                                                                                'A46D053A-0776-467F-9136-0AA45D2021D2',
                                                                                '90973417-6C68-4E90-849B-0B1AEB36A234',
                                                                                'E1E2A6EA-C092-4C8D-920D-170BE5128BC4',
                                                                                'D06E5471-EE69-49E4-9AF4-1BE8AB2E0310',
                                                                                '8A2B5231-8602-4519-8919-1FF649ED2E41',
                                                                                'FA0D1C8C-DE69-4C16-A301-3A861C6B11CD',
                                                                                'D1C56FBD-2EDC-40FF-956F-4DA7178F2DD3',
                                                                                '4500603A-5BE7-4574-BBBA-77B4626A3EA1',
                                                                                '05D0D71B-DEF3-42EE-A16A-79B34DE97E9B',
                                                                                'A14D0408-690C-420C-9D41-7A2FA515C371',
                                                                                'F21C9D4A-CB80-4705-AA7C-81A8BD17DB7D',
                                                                                'C9A1DD95-79BF-4543-924B-94362381E705',
                                                                                'E25F399F-A147-4663-B74A-98A46A39F121',
                                                                                '269EE291-F6E7-4AEA-B4C7-A7D02B7C59DE',
                                                                                '3AB86F48-7A73-46B1-8AA4-D1B55DE1EE8A',
                                                                                'F34C2B8B-DDBE-4C04-BD01-F08B0F479AE8',
                                                                                'A974B39B-EC88-4917-A1D5-F8FBBFBB1F7A',
                                                                                'C72E2DD8-EC76-4663-B400-FB1B7F8F8C2B',
                                                                                '7894EACD-DA8C-4659-9891-FB4D62EB9FF5'
                                                                                )";
                        break;
                    case "2"://钢筋实验
                        sqlwhere += @"  AND ModelIndex IN (
                                                                                'AE9F8E75-773F-4DEE-A3A8-3E871AC8598E',
                                                                                'A98902C1-BB72-4E79-9C74-4746D1709D3B',
                                                                                '68F05EBC-5D34-49C5-9B57-49B688DF24F7',
                                                                                '9B8BD64A-2D9C-4B67-B9DC-53FCF67FD361',
                                                                                '46622671-E829-412C-99E6-587525ED968F',
                                                                                'C7B2620E-7F4C-4586-AEBB-59855B54E522',
                                                                                '3B46BC3A-92DF-4AFC-AA85-AE74FC00F96D',
                                                                                '377A20DA-7E27-4CD3-B9E1-B3C7993CF6EA',
                                                                                'A12AD84C-A7D3-4B42-A9DF-6E80A3E3A0CF',
                                                                                '0A2F0365-D561-4504-B602-98FCC5C3AB94',
                                                                                '4C817CF9-E7F3-422D-975F-C8175E738382'
                                                                                ) ";
                        break;
                    case "3": //混凝土原材
                        sqlwhere += "AND ModelIndex IN (SELECT ID FROM dbo.sys_module WHERE CatlogCode LIKE '0001%') ";
                        break;
                    case "4": //其他
                        sqlwhere += @" AND ModelIndex not IN(SELECT ID FROM dbo.sys_module WHERE CatlogCode LIKE '0001%' or ID in('AE9F8E75-773F-4DEE-A3A8-3E871AC8598E','A98902C1-BB72-4E79-9C74-4746D1709D3B','68F05EBC-5D34-49C5-9B57-49B688DF24F7','9B8BD64A-2D9C-4B67-B9DC-53FCF67FD361','46622671-E829-412C-99E6-587525ED968F','C7B2620E-7F4C-4586-AEBB-59855B54E522','3B46BC3A-92DF-4AFC-AA85-AE74FC00F96D','377A20DA-7E27-4CD3-B9E1-B3C7993CF6EA','A12AD84C-A7D3-4B42-A9DF-6E80A3E3A0CF','0A2F0365-D561-4504-B602-98FCC5C3AB94','4C817CF9-E7F3-422D-975F-C8175E738382','A46D053A-0776-467F-9136-0AA45D2021D2','90973417-6C68-4E90-849B-0B1AEB36A234','E1E2A6EA-C092-4C8D-920D-170BE5128BC4','D06E5471-EE69-49E4-9AF4-1BE8AB2E0310','8A2B5231-8602-4519-8919-1FF649ED2E41','FA0D1C8C-DE69-4C16-A301-3A861C6B11CD','D1C56FBD-2EDC-40FF-956F-4DA7178F2DD3','4500603A-5BE7-4574-BBBA-77B4626A3EA1','05D0D71B-DEF3-42EE-A16A-79B34DE97E9B','A14D0408-690C-420C-9D41-7A2FA515C371','F21C9D4A-CB80-4705-AA7C-81A8BD17DB7D','C9A1DD95-79BF-4543-924B-94362381E705','E25F399F-A147-4663-B74A-98A46A39F121','269EE291-F6E7-4AEA-B4C7-A7D02B7C59DE','3AB86F48-7A73-46B1-8AA4-D1B55DE1EE8A','F34C2B8B-DDBE-4C04-BD01-F08B0F479AE8','A974B39B-EC88-4917-A1D5-F8FBBFBB1F7A','C72E2DD8-EC76-4663-B400-FB1B7F8F8C2B','7894EACD-DA8C-4659-9891-FB4D62EB9FF5')) ";
                        break;
                    case "5": //已处理
                        sqlwhere += @" AND  (DealResult IS NOT NULL) AND DealResult <> '' ";
                        break;
                    case "6": //未处理
                        sqlwhere += @" AND (DealResult='' OR DealResult IS NULL) ";
                        break;

                }
            }

            #endregion

            if (!"NUM".RequestStr().IsNullOrEmpty())
            {
                sqlwhere += " and testroomcode in ('" + "NUM".RequestStr() + "') ";
            }
            else if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
            {
                sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
            }

            if (!String.IsNullOrEmpty(Request.Params["sReportCode"]))
            {
                sqlwhere += " and  ReportNumber LIKE '%" + Request.Params["sReportCode"].Trim() + "%'";
            }
            if (!String.IsNullOrEmpty(Request.Params["sReportName"]))
            {
                sqlwhere += " and  ReportName LIKE '%" + Request.Params["sReportName"].Trim() + "%'";
            }     
        }
        
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "OrderID", "ASC", PageIndex, PageSize, out pageCount, out records);
     
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }



    private String popfailgrid(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
            sqlwhere = "and ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            if (!String.IsNullOrEmpty(Request.Params["sTestcode"].ToString()))
            {
                sqlwhere += " and TestRoomCode='" + Request.Params["sTestcode"].ToString() + "' ";
            }
        }
        else
        {
            name = "v_invalid_document";
            sqlwhere = "and ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' AND  F_InvalidItem NOT LIKE '%#%' AND AdditionalQualified=0 ";
            if (!String.IsNullOrEmpty(Request.Params["sTestcode"].ToString()))
            {
                sqlwhere += " and TestRoomCode='" + Request.Params["sTestcode"].ToString() + "' ";
            }
        }

        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "ReportDate", OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    private String unpopfailgrid(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
            sqlwhere = "and ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'  AND (SGComment IS NULL OR JLComment IS NULL) ";
            if (!String.IsNullOrEmpty(Request.Params["sTestcode"].ToString()))
            {
                sqlwhere += " and TestRoomCode='" + Request.Params["sTestcode"].ToString() + "' ";
            }
        }
        else
        {
            name = "v_invalid_document";
            sqlwhere = "and ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' AND  F_InvalidItem NOT LIKE '%#%' AND AdditionalQualified=0   AND (SGComment IS NULL OR JLComment IS NULL)  ";
            if (!String.IsNullOrEmpty(Request.Params["sTestcode"].ToString()))
            {
                sqlwhere += " and TestRoomCode='" + Request.Params["sTestcode"].ToString() + "' ";
            }
        }

        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "ReportDate", OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    #endregion

    #region 登录、操作日志方法
    private String operateLog(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
        }
        else
        {
            name = "v_operate_log";
        }
        sqlwhere = "and modifiedDate>='" + StartDate + "' AND modifiedDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
        if (!String.IsNullOrEmpty(Request.Params["username"]))
        {
            sqlwhere += " and  modifiedby LIKE '%" + Request.Params["username"].Trim() + "%'";
        }
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "modifiedDate", OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }

    private String loginLog(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        fileds = @"loginDay ,
          ipAddress ,
          macAddress ,
          machineName ,
          osVersion ,
          osUserName ,
          UserName ,
          ProjectName ,
          SegmentName ,
          CompanyName ,
          TestRoomName ,
          TestRoomCode ,
          FirstAccessTime ,
          LastAccessTime";
        sqlwhere = "and FirstAccessTime>='" + StartDate + "' AND FirstAccessTime<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";


        #region For 首页
        if (!"NUM".RequestStr().IsNullOrEmpty())
        {
            sqlwhere += " and testroomcode in ('" + "NUM".RequestStr() + "') ";
        }
        else if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }
       

        #endregion

        if (!String.IsNullOrEmpty(Request.Params["username"]))
        {
            sqlwhere += " and  UserName LIKE '%" + Request.Params["username"].Trim() + "%'";
        }
     
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "loginDay", OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    private String report_Search(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        fileds = @"ID,
 BGBH ,
 WTBH,CONVERT(NVARCHAR(10),BGRQ,120) BGRQ ,CompanyCode,TestRoomCode,SegmentCode,
    '' SegmentName,      ''   CompanyName,'' TestRoomName, '' MName,'' DeviceType ,
ModuleId
            ";



        sqlwhere = " and status>0 and BGRQ>='" + StartDate + "' AND BGRQ<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";

        #region
        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {

            switch ("RPNAME".RequestStr())
            {
                case "1": //混凝土抗压
                    sqlwhere += @" AND m.id IN ('A46D053A-0776-467F-9136-0AA45D2021D2',
                                            '90973417-6C68-4E90-849B-0B1AEB36A234',
                                            'E1E2A6EA-C092-4C8D-920D-170BE5128BC4',
                                            'D06E5471-EE69-49E4-9AF4-1BE8AB2E0310',
                                            '8A2B5231-8602-4519-8919-1FF649ED2E41',
                                            'FA0D1C8C-DE69-4C16-A301-3A861C6B11CD',
                                            'D1C56FBD-2EDC-40FF-956F-4DA7178F2DD3',
                                            '4500603A-5BE7-4574-BBBA-77B4626A3EA1',
                                            '05D0D71B-DEF3-42EE-A16A-79B34DE97E9B',
                                            'A14D0408-690C-420C-9D41-7A2FA515C371',
                                            'F21C9D4A-CB80-4705-AA7C-81A8BD17DB7D',
                                            'C9A1DD95-79BF-4543-924B-94362381E705',
                                            'E25F399F-A147-4663-B74A-98A46A39F121',
                                            '269EE291-F6E7-4AEA-B4C7-A7D02B7C59DE',
                                            '3AB86F48-7A73-46B1-8AA4-D1B55DE1EE8A',
                                            'F34C2B8B-DDBE-4C04-BD01-F08B0F479AE8',
                                            'A974B39B-EC88-4917-A1D5-F8FBBFBB1F7A',
                                            'C72E2DD8-EC76-4663-B400-FB1B7F8F8C2B',
                                            '7894EACD-DA8C-4659-9891-FB4D62EB9FF5') ";
                    break;
                case "2"://钢筋实验
                    sqlwhere += @" AND m.id IN ('AE9F8E75-773F-4DEE-A3A8-3E871AC8598E',
                                            'A98902C1-BB72-4E79-9C74-4746D1709D3B',
                                            '68F05EBC-5D34-49C5-9B57-49B688DF24F7',
                                            '9B8BD64A-2D9C-4B67-B9DC-53FCF67FD361',
                                            '46622671-E829-412C-99E6-587525ED968F',
                                            'C7B2620E-7F4C-4586-AEBB-59855B54E522',
                                            '3B46BC3A-92DF-4AFC-AA85-AE74FC00F96D',
                                            '377A20DA-7E27-4CD3-B9E1-B3C7993CF6EA',
                                            'A12AD84C-A7D3-4B42-A9DF-6E80A3E3A0CF',
                                            '0A2F0365-D561-4504-B602-98FCC5C3AB94',
                                            '4C817CF9-E7F3-422D-975F-C8175E738382') ";
                    break;
                case "3": //混凝土原材
                    sqlwhere += " AND m.id  IN (SELECT ID FROM dbo.sys_module WHERE CatlogCode LIKE '0001%') and d.status>0 and d.bgrq between '" + StartDate + "' and '" + DateTime.Parse(EndDate).ToString("yyyy-MM-dd") + "' ";
                    break;
                case "4": //其他
                    sqlwhere += @" AND m.id not in (SELECT ID FROM dbo.sys_module WHERE CatlogCode LIKE '0001%' or ID in (
                                    'AE9F8E75-773F-4DEE-A3A8-3E871AC8598E',
                                    'A98902C1-BB72-4E79-9C74-4746D1709D3B',
                                    '68F05EBC-5D34-49C5-9B57-49B688DF24F7',
                                    '9B8BD64A-2D9C-4B67-B9DC-53FCF67FD361',
                                    '46622671-E829-412C-99E6-587525ED968F',
                                    'C7B2620E-7F4C-4586-AEBB-59855B54E522',
                                    '3B46BC3A-92DF-4AFC-AA85-AE74FC00F96D',
                                    '377A20DA-7E27-4CD3-B9E1-B3C7993CF6EA',
                                    'A12AD84C-A7D3-4B42-A9DF-6E80A3E3A0CF',
                                    '0A2F0365-D561-4504-B602-98FCC5C3AB94',
                                    '4C817CF9-E7F3-422D-975F-C8175E738382',
                                    'A46D053A-0776-467F-9136-0AA45D2021D2',
                                    '90973417-6C68-4E90-849B-0B1AEB36A234',
                                    'E1E2A6EA-C092-4C8D-920D-170BE5128BC4',
                                    'D06E5471-EE69-49E4-9AF4-1BE8AB2E0310',
                                    '8A2B5231-8602-4519-8919-1FF649ED2E41',
                                    'FA0D1C8C-DE69-4C16-A301-3A861C6B11CD',
                                    'D1C56FBD-2EDC-40FF-956F-4DA7178F2DD3',
                                    '4500603A-5BE7-4574-BBBA-77B4626A3EA1',
                                    '05D0D71B-DEF3-42EE-A16A-79B34DE97E9B',
                                    'A14D0408-690C-420C-9D41-7A2FA515C371',
                                    'F21C9D4A-CB80-4705-AA7C-81A8BD17DB7D',
                                    'C9A1DD95-79BF-4543-924B-94362381E705',
                                    'E25F399F-A147-4663-B74A-98A46A39F121',
                                    '269EE291-F6E7-4AEA-B4C7-A7D02B7C59DE',
                                    '3AB86F48-7A73-46B1-8AA4-D1B55DE1EE8A',
                                    'F34C2B8B-DDBE-4C04-BD01-F08B0F479AE8',
                                    'A974B39B-EC88-4917-A1D5-F8FBBFBB1F7A',
                                    'C72E2DD8-EC76-4663-B400-FB1B7F8F8C2B',
                                    '7894EACD-DA8C-4659-9891-FB4D62EB9FF5')
                                    )  ";
                    break;
               
            }
        }

        #endregion

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
            using (System.Data.SqlClient.SqlCommand Cmd = new System.Data.SqlClient.SqlCommand(Sql,Conn))
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

        #region 过滤 !未使用


        //DataTable Module = BLL.GetDataTable("select Id,Name,DeviceType from sys_module");

        //DataTable Temp;
        //foreach (DataRow Dr in dt.Rows)
        //{
        //    Module.DefaultView.RowFilter = " ID = '" + Dr["ModuleId"].ToString() + "' ";
        //    Temp = Module.DefaultView.ToTable();

        //    Dr["MName"] = Temp.Rows[0]["Name"].ToString();
        //    Dr["DeviceType"] = Temp.Rows[0]["DeviceType"].ToString();
        //    Temp.Clear();
        //    Temp = null;
        //}

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


    private String stadiumLog(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {       
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, OrderField, OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    private String ageremindundo(String proc, int ftype, out int pageCount, out int records)
    {
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, ftype, PageIndex, PageSize, OrderField, OrderType, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }

    private String SMS(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {
        sqlwhere = "and SentTime>='" + StartDate + "' AND SentTime<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
        if (!String.IsNullOrEmpty(Request.Params["username"]))
        {
            sqlwhere += " and  PersonName LIKE '%" + Request.Params["username"].Trim() + "%'";
        }
        if (!String.IsNullOrEmpty(Request.Params["tel"]))
        {
            sqlwhere += " and  SMSPhone LIKE '%" + Request.Params["tel"].Trim() + "%'";
        }
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, OrderField, OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }


    #endregion

    #region 设备情况方法
    private String machinesummary(String name, String fileds, String sqlwhere, String key, int ftype, out int pageCount, out int records)
    {

        if (!"NUM".RequestStr().IsNullOrEmpty())
        {
            sqlwhere += " and 试验室编码 in ('" + "NUM".RequestStr() + "') ";
        }
        else if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and 试验室编码 in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }

        switch ("meet".RequestStr())
        {
            case "1":
                sqlwhere += " AND 预计下次校验日期 < '" + DateTime.Now.AddDays(1).AddSeconds(-1).ToString() + "' ";
                break;
            case "2":
                sqlwhere += " AND 预计下次校验日期 > '" + DateTime.Now.AddDays(1).AddSeconds(-1).ToString() + "' ";
                break;
        }

        #region For首页
        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {

            switch ("RPNAME".RequestStr())
            {
                case "1": //待标定数
                    sqlwhere += " AND 预计下次校验日期 between '" + StartDate + "' and '" + EndDate + "' ";
                    break;
            }
        }
        #endregion
       

        BLL_Machine BLL = new BLL_Machine();
        DataTable dt = BLL.GetDataTablePager(name, fileds, sqlwhere, key, "OrderID", "asc", PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }
    #endregion  
}