using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using Newtonsoft.Json;

public partial class ajax_ajaxChart : BasePage
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
        String sTestcode = "";
        String sUnit = "";
        String sModel = "";
        String IsUnit = "";
        String sUser = "";
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
            switch (sType)
            {
                case "qxzlhzbchart":
                    IsUnit = Request.Params["isUnit"];
                    result = qxzlhzbchart("spweb_qxzlhzb_charts", Int32.Parse(IsUnit));
                    break;
                case "failreportsum":
                    //IsUnit = Request.Params["isUnit"];
                    result = failreportsum("spweb_failreport_chart", 1);
                    break;
                case "failreportsumorder":
                    //IsUnit = Request.Params["isUnit"];
                    result = spweb_failreport_chart_order("spweb_failreport_chart_order", 1);
                    break;
                case "qxzlhzbchartpop":
                    sTestcode = Request.Params["sTestcode"];
                    result = qxzlhzbchartpop("spweb_qxzlhzb_charts_pop", sTestcode);
                    break;

                case "loginpop":
                    sTestcode = Request.Params["sTestcode"];
                    result = loginpop("spweb_login_charts_pop", sTestcode);
                    break;
                case "onelogin":
                    sUser = Request.Params["sUser"];
                    sUser = Server.UrlDecode(sUser);//一次解码
                    result = onelogin("spweb_login_one", sUser);
                    break;
                case "qxjqgridpop":
                    sTestcode = Request.Params["sTestcode"];
                    sModel = Request.Params["sModelID"];
                    result = qxjqgridpop("spweb_qxzlhzb_jqgrid_pop", 1, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), sTestcode, sModel);
                    break;
                case "usersummary":
                    result = usersummary("spweb_userSummary_chart", 1);
                    break;
                case "testdata":
                    IsUnit = Request.Params["isUnit"];
                    sTestcode = Request.Params["sTestcode"];
                    result = testdata(sTestcode, IsUnit);
                    break;
                case "loginLog":
                    result = loginLog("spweb_login_charts", 1);
                    break;
                case "usersummarypop":
                    IsUnit = Request.Params["isUnit"];
                    sTestcode = Request.Params["sTestcode"];
                    result = usersummarypop("spweb_userSummary_chart_pop", sTestcode, Int32.Parse(IsUnit));
                    break;
                case "machinesummary":
                    IsUnit = Request.Params["isUnit"];
                    result = machinesummary("spweb_machineSummary_chart", Int32.Parse(IsUnit));
                    break;
                case "machinesummarypop":
                    sTestcode = Request.Params["sTestcode"];
                    result = machinesummarypop("spweb_machineSummary_chart_pop", sTestcode);
                    break;
                case "evaluatedata":
                    result = evaluatedata();
                    break;
                case "evaluatedatapop":
                    result = evaluatedatapop();
                    break;
                case "unpopEvaluatedata":
                    result = unpopEvaluatedata();
                    break;
                case "pxreportpop":
                    sTestcode = Request.Params["sTestcode"];
                    sModel = Request.Params["sModelID"];
                    result = pxreportpop("spweb_pxreport_Chart_pop", 1, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), sTestcode, sModel);
                    break;
                case "parallellinechart":
                    IsUnit = Request.Params["isUnit"];
                    sTestcode = Request.Params["sTestcode"];
                    result = parallellinechart("spweb_pxjzReport_line_chart", sTestcode, Int32.Parse(IsUnit));
                    break;
                case "parallelfailchart":
                    IsUnit = Request.Params["isUnit"];
                    sTestcode = Request.Params["sTestcode"];
                    result = parallellinechart("spweb_pxjzReport_fail_chart", sTestcode, Int32.Parse(IsUnit));
                    break;
                case "tongreport":
                    sModel = Request.Params["sModelID"];
                    sUnit = Request.Params["sUnit"];
                    string sDay = Request.Params["sDay"];
                    result = tongreport("", sUnit, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), sModel, sDay);
                    break;
                case "loginlogpop":
                    sUser = Request.Params["sUser"];
                    sUser = Server.UrlDecode(sUser);//一次解码
                    result = loginlogpop("spweb_loginlogpop", sUser);
                    break;

                case "bsloginlog":
                    result = bsloginlog("spweb_sys_user_login_log");
                    break;
                case "bsloginpop":
                    sUser = Request.Params["sUser"];
                    sUser = Server.UrlDecode(sUser);//一次解码
                    result = bsloginpop("spweb_sys_user_login_log_one", sUser);
                    break;
                default:
                    break;
            }

        }

        Response.Write(result);
    }

    #region 全线资料方法
    private String qxzlhzbchart(string proc, int ntype)
    {
        if (String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            #region
            JZ.BLL.BLL_TestRoom bll_testroom = new JZ.BLL.BLL_TestRoom();
            DataTable dt = bll_testroom.GetTree();
            DataTable dt_user = bll_testroom.GetUsersTestRoom(UserName);
            int n = 0;

            DataTable dtnew = new DataTable();
            dtnew = dt.Clone();

            if (dt_user != null && dt_user.Rows.Count > 0)
            {
                n = dt_user.Rows.Count;
                #region
                if (n == 1)
                {
                    string testroom = dt_user.Rows[0]["testroomcode"].ToString().Trim();
                    if (testroom == "0001")
                    {
                        foreach (DataRow oldDR in dt.Rows)
                        {
                            DataRow newDR = dtnew.NewRow();//新表创建新行                         
                            newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                            dtnew.ImportRow(oldDR);
                        }
                    }
                    if (testroom.Length == 8)//监理一个单位或者中心实验室主任，8位标段编码
                    {
                        foreach (DataRow oldDR in dt.Rows)
                        {
                            DataRow newDR = dtnew.NewRow();//新表创建新行
                            if (oldDR["NodeCode"].ToString().Length > 7)
                            {
                                if (oldDR["NodeCode"].ToString().Substring(0, 8) == testroom)
                                {
                                    newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                    dtnew.ImportRow(oldDR);
                                }
                            }

                        }
                    }
                    if (testroom.Length == 16)
                    {
                        foreach (DataRow oldDR in dt.Rows)
                        {
                            DataRow newDR = dtnew.NewRow();//新表创建新行
                            if (oldDR["NodeCode"].ToString().Length > 7)
                            {
                                if (oldDR["NodeCode"].ToString() == testroom.Substring(0, 8))
                                {
                                    newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                    dtnew.ImportRow(oldDR);
                                }
                                if (oldDR["NodeCode"].ToString() == testroom.Substring(0, 12))
                                {
                                    newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                    dtnew.ImportRow(oldDR);
                                }
                            }
                            if (oldDR["NodeCode"].ToString() == testroom)
                            {
                                newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                dtnew.ImportRow(oldDR);
                            }
                        }
                    }
                }
                #endregion

                if (n > 1)
                {
                    DataRow newDR = dtnew.NewRow();//新表创建新行
                    foreach (DataRow item in dt_user.Rows)
                    {
                        string testroom = item["testroomcode"].ToString().Trim();
                        if (testroom.Length == 8)//监理一个单位或者中心实验室主任，8位标段编码
                        {
                            foreach (DataRow oldDR in dt.Rows)
                            {
                                if (oldDR["NodeCode"].ToString().Length > 7)
                                {
                                    if (oldDR["NodeCode"].ToString().Substring(0, 8) == testroom)
                                    {
                                        newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                        dtnew.ImportRow(oldDR);
                                    }
                                }
                            }
                        }
                        if (testroom.Length == 16)
                        {
                            foreach (DataRow oldDR in dt.Rows)
                            {
                                if (oldDR["NodeCode"].ToString().Length > 7)
                                {
                                    if (oldDR["NodeCode"].ToString() == testroom.Substring(0, 8))
                                    {

                                        newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                        dtnew.ImportRow(oldDR);
                                    }
                                    if (oldDR["NodeCode"].ToString() == testroom.Substring(0, 12))
                                    {
                                        newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                        dtnew.ImportRow(oldDR);
                                    }
                                }
                                if (oldDR["NodeCode"].ToString() == testroom)
                                {
                                    newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                                    dtnew.ImportRow(oldDR);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        for (int j = i + 1; j < dtnew.Rows.Count; j++)
                        {
                            if (dtnew.Rows[i]["NodeCode"].ToString().Trim() == dtnew.Rows[j]["NodeCode"].ToString().Trim())
                            {
                                dtnew.Rows.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            GetDTJson(dtnew);
            #endregion
        }

        BLL_Document BLL = new BLL_Document();
        DataTable dt1 = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, ntype);
        if (dt1 != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                ChartModel trcs = new ChartModel();
                if (ntype == 1)
                {
                    trcs.Description = dr["segment"].ToString() + "\n" + dr["company"].ToString();
                    trcs.Para1 = dr["companycode"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["totalncount"].ToString());
                    trcs.IntNumberMarks = Int32.Parse(dr["counts"].ToString());
                    trcs.DoubleNumber = Double.Parse(dr["prenct"].ToString());

                }
                else if (ntype == 2)
                {
                    trcs.Description = dr["segment"].ToString() + "\n" + dr["company"].ToString() + "\n" + dr["testroom"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["totalncount"].ToString());
                    trcs.IntNumberMarks = Int32.Parse(dr["counts"].ToString());
                    trcs.DoubleNumber = Double.Parse(dr["prenct"].ToString());
                }
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    private void GetDTJson(DataTable dt)
    {
        List<String> selectedTestRoomList = new List<String>();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["NodeCode"].ToString().Length == 16)
                {
                    selectedTestRoomList.Add(dr["NodeCode"].ToString());
                }
            }
        }
        SelectedTestRoomCodes = "'" + String.Join("','", selectedTestRoomList.ToArray()) + "'";
    }

    private String qxzlhzbchartpop(string proc, string testcode)
    {
        BLL_Document BLL = new BLL_Document();
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, 1);

        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["testname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["modelid"].ToString();
                trcs.Para2 = dr["segment"].ToString() + "-" + dr["company"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }


    private String qxjqgridpop(string proc, int ftype, string startdate, string enddate, string testcode, string modelid)
    {
        BLL_Document BLL = new BLL_Document();
        if (modelid.Length > 0)
        {
            DataTable dt = BLL.GetProcDataTableChartsPara6(proc, ftype, startdate, enddate, testcode, modelid);
            if (dt != null)
            {
                List<ChartModel> list = new List<ChartModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    ChartModel trcs = new ChartModel();
                    trcs.Description = dr["chartDate"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["zjCount"].ToString());
                    list.Add(trcs);
                }
                return JsonConvert.SerializeObject(list);
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    #endregion

    #region 人员情况方法
    private String usersummary(string proc, int ntype)
    {
        BLL_UserInfo BLL = new BLL_UserInfo();
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, ntype);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["segment"].ToString() + "\n" + dr["companyname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["companycode"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    private String usersummarypop(string proc, string testcode, int nUnit)
    {
        BLL_UserInfo BLL = new BLL_UserInfo();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, nUnit);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["testname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["Testcode"].ToString();
                trcs.Para2 = dr["Companycode"].ToString();
                trcs.Para3 = dr["company"].ToString();
                trcs.IntNumberMarks = nUnit;
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region 设备情况方法
    private String machinesummary(string proc, int ntype)
    {


        string Sql = @"
select 
 t1.Description as sg , t2.Description, t2.nodecode as Para1, 
COUNT(b1.ID) as IntNumber,
Sum(case when b1.预计下次校验日期<'{0}' then 1 else 0 end)

 as OIntNumber
 from sys_tree t1
 left outer join sys_tree t2 on left(t2.nodecode,8)=t1.nodecode AND len(t2.nodecode)=12
 left outer join v_bs_machineSummary b1 on t2.nodecode= b1.单位编码 AND b1.试验室编码 IN (" + SelectedTestRoomCodes + ") where len(t1.nodecode)=8  group by b1.标段编码,t2.Description,t1.Description, t2.nodecode order by b1.标段编码 asc ";


        BLL_Machine BLL = new BLL_Machine();

        DataTable Dt = BLL.GetDataTable(string.Format(Sql,DateTime.Now.AddDays(1).AddSeconds(-1).ToString()));

        if (Dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in Dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["sg"].ToString()+"\n"+dr["Description"].ToString();
                trcs.IntNumber = Int32.Parse(dr["IntNumber"].ToString());
                trcs.Para1 = dr["Para1"].ToString();
                trcs.IntNumberMarks = Int32.Parse(dr["OIntNumber"].ToString());
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    private String machinesummarypop(string proc, string testcode)
    {
        BLL_Machine BLL = new BLL_Machine();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, 1);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["testname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["companycode"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region 不合格报告方法
    private String evaluatedata()
    {
        BLL_FailReport BLL = new BLL_FailReport();
        string sqlwhere = "";
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
            sqlwhere = " SELECT CompanyCode as companycode,COUNT(1) as ncount,MAX(SectionName) as segment,MAX(CompanyName) AS companyname FROM dbo.biz_all_failreport ";
            sqlwhere += "where ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
            {
                sqlwhere += " and TestRoomCode in (" + SelectedTestRoomCodes + ") ";
            }
            sqlwhere += "   GROUP BY CompanyCode";
        }
        else
        {
            sqlwhere = "SELECT b.CompanyCode AS companycode,COUNT(DISTINCT a.ID) AS ncount,MAX(c.标段名称) AS segment,MAX(c.单位名称) AS companyname FROM dbo.sys_invalid_document a JOIN dbo.sys_document b ON a.ID = b.ID JOIN dbo.v_bs_codeName c ON b.CompanyCode=c.单位编码 JOIN dbo.sys_module d ON b.ModuleID=d.ID AND b.Status>0  ";
            sqlwhere += "where b.BGRQ>='" + StartDate + "' AND b.BGRQ<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'   ";
            if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
            {
                sqlwhere += " and b.TestRoomCode in (" + SelectedTestRoomCodes + ") ";
            }
            sqlwhere += "   GROUP BY  b.CompanyCode";
        }
        DataTable dt = BLL.GetTablelist(sqlwhere);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["segment"].ToString() + "\n" + dr["companyname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["companycode"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    private String evaluatedatapop()
    {
        BLL_FailReport BLL = new BLL_FailReport();
        string sqlwhere = "";
        string testcode = Request.Params["sTestcode"].ToString();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
            sqlwhere = " SELECT  Max(TestRoomName) AS testname,COUNT(1) as ncount,MAX(TestRoomCode) as companycode  FROM dbo.biz_all_failreport ";
            sqlwhere += "where  ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";
            if (!String.IsNullOrEmpty(testcode))
            {
                sqlwhere += " and TestRoomCode in (" + testcode + ")";
            }
            else
            {
                sqlwhere += " 1=2 ";
            }
            sqlwhere += "   GROUP BY TestRoomCode";
        }
        else
        {
            sqlwhere = " SELECT  Max(TestRoomName) AS testname,COUNT(1) as ncount,MAX(TestRoomCode) as companycode  FROM dbo.v_invalid_document ";
            sqlwhere += "where  ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'  AND  F_InvalidItem NOT LIKE '%#%' AND AdditionalQualified=0 ";
            if (!String.IsNullOrEmpty(testcode))
            {
                sqlwhere += " and TestRoomCode in (" + testcode + ")";
            }
            else
            {
                sqlwhere += " 1=2 ";
            }
            sqlwhere += "   GROUP BY TestRoomCode";
        }
        DataTable dt = BLL.GetTablelist(sqlwhere);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["testname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["companycode"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }
    private String unpopEvaluatedata()
    {
        BLL_FailReport BLL = new BLL_FailReport();
        string sqlwhere = "";
        string testcode = Request.Params["sTestcode"].ToString();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
            sqlwhere = " SELECT  Max(TestRoomName) AS testname,COUNT(1) as ncount,MAX(TestRoomCode) as companycode,COUNT(1) as uncounts  FROM dbo.biz_all_failreport ";
            sqlwhere += "where  ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'  AND (SGComment IS NULL OR JLComment IS NULL)  ";
            if (!String.IsNullOrEmpty(testcode))
            {
                sqlwhere += " and TestRoomCode in (" + testcode + ")";
            }
            else
            {
                sqlwhere += " 1=2 ";
            }
            sqlwhere += "   GROUP BY TestRoomCode";
        }
        else
        {
            sqlwhere = " SELECT  Max(TestRoomName) AS testname,COUNT(1) as ncount,MAX(TestRoomCode) as companycode  FROM dbo.v_invalid_document ";
            sqlwhere += "where  ReportDate>='" + StartDate + "' AND ReportDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'  AND  F_InvalidItem NOT LIKE '%#%' AND AdditionalQualified=0  AND (SGComment IS NULL OR JLComment IS NULL) ";
            if (!String.IsNullOrEmpty(testcode))
            {
                sqlwhere += " and TestRoomCode in (" + testcode + ")";
            }
            else
            {
                sqlwhere += " 1=2 ";
            }
            sqlwhere += "   GROUP BY TestRoomCode";
        }
        DataTable dt = BLL.GetTablelist(sqlwhere);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["testname"].ToString();
                trcs.IntNumber = Int32.Parse(dr["ncount"].ToString());
                trcs.Para1 = dr["companycode"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region 平行见证方法
    private String pxreportpop(string proc, int ftype, string startdate, string enddate, string testcode, string modelid)
    {
        BLL_ParallelReport BLL = new BLL_ParallelReport();
        DataTable dt = BLL.GetProcDataTableChartsPara6(proc, ftype, startdate, enddate, testcode, modelid);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["chartDate"].ToString();
                trcs.IntNumber = Int32.Parse(dr["zjCount"].ToString());
                trcs.IntNumberMarks = Int32.Parse(dr["pxjzCount"].ToString());
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }



    private String parallellinechart(string proc, string testcode, int nUnit)
    {
        BLL_ParallelReport BLL = new BLL_ParallelReport();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, nUnit);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["result"].ToString();
                trcs.IntNumber = Int32.Parse(dr["counts"].ToString());
                trcs.Para1 = dr["testroomID"].ToString();
                trcs.Para2 = dr["modelid"].ToString();
                trcs.Para3 = dr["stype"].ToString();
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region 砼强度方法


    private String tongreport(string proc, string type, string startdate, string enddate, string modelid, string days)
    {
        string sqlwhere = "";
        BLL_Betong BLL = new BLL_Betong();
        sqlwhere = " SELECT * FROM biz_tongqiangdu ";
        sqlwhere += "where sDate>='" + StartDate + "' AND sDate<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and LEFT(sTestCode,16) in (" + SelectedTestRoomCodes + ") ";
        }
        if (!String.IsNullOrEmpty(modelid))
        {
            sqlwhere += " and ModelID='" + modelid + "' ";
        }
        if (!String.IsNullOrEmpty(type))
        {
            sqlwhere += " and cType like '%" + type + "%' ";
        }
        if (!String.IsNullOrEmpty(days))
        {
            sqlwhere += " and sAge like '%" + days + "%' ";
        }
        sqlwhere += "   ORDER BY sDate";

        DataSet ds = DbHelperSQL.Query(sqlwhere);
        DataTable dt = new DataTable();
        if (ds != null && ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        if (dt.Rows.Count > 0)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                DateTime date = new DateTime();
                if (DateTime.TryParse(dr["sDate"].ToString(), out date))
                {

                    trcs.DescriptionDate = DateTime.Parse(dr["sDate"].ToString());
                    Double num = 0;
                    if (Double.TryParse(dr["sValue1"].ToString(), out num))
                    {
                        trcs.DoubleNumber = Double.Parse(dr["sValue1"].ToString());
                        trcs.Para4 = dr["segment"].ToString() + "-" + dr["company"].ToString() + "-" + dr["testroom"].ToString() + "\n组值：" + dr["sValue1"].ToString() + "\n施工部位:" + dr["sPlace"].ToString();
                        list.Add(trcs);
                    }
                    if (Double.TryParse(dr["sValue2"].ToString(), out num))
                    {
                        trcs.DoubleNumber = Double.Parse(dr["sValue2"].ToString());
                        trcs.Para4 = dr["segment"].ToString() + "-" + dr["company"].ToString() + "-" + dr["testroom"].ToString() + "\n组值：" + dr["sValue2"].ToString() + "\n施工部位:" + dr["sPlace"].ToString();
                        list.Add(trcs);
                    }
                    if (Double.TryParse(dr["sValue3"].ToString(), out num))
                    {
                        trcs.DoubleNumber = Double.Parse(dr["sValue3"].ToString());
                        trcs.Para4 = dr["segment"].ToString() + "-" + dr["company"].ToString() + "-" + dr["testroom"].ToString() + "\n组值：" + dr["sValue3"].ToString() + "\n施工部位:" + dr["sPlace"].ToString();
                        list.Add(trcs);
                    }
                    if (Double.TryParse(dr["sValue4"].ToString(), out num))
                    {
                        trcs.DoubleNumber = Double.Parse(dr["sValue4"].ToString());
                        trcs.Para4 = dr["segment"].ToString() + "-" + dr["company"].ToString() + "-" + dr["testroom"].ToString() + "\n组值：" + dr["sValue4"].ToString() + "\n施工部位:" + dr["sPlace"].ToString();
                        list.Add(trcs);
                    }
                }
            }
            return JsonConvert.SerializeObject(BLL.GetMinAndAverage(list, type));
        }
        else
        {
            return "";
        }
    }



    #endregion

    #region 登录、修改日志
    private String loginlogpop(string proc, string testcode)
    {
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, 1);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["UserName"].ToString();
                trcs.DescriptionDate = DateTime.Parse(dr["FirstAccessTime"].ToString());
                trcs.DescriptionDate1 = DateTime.Parse(dr["LastAccessTime"].ToString());
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    private String loginLog(string proc, int ntype)
    {
        BLL_LoginLog BLL = new BLL_LoginLog();
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            string SQL = "SELECT  MAX(CompanyName) CompanyName,MAX(SegmentName) SegmentName,COUNT(1) as nTotals, LEFT(TestRoomCode,12) AS CompanyCode  FROM dbo.sys_loginlog a  join sys_tree b ON LEFT(TestRoomCode,12)=b.NodeCode WHERE TestRoomCode IN (" + SelectedTestRoomCodes + ")   AND FirstAccessTime>='" + StartDate + "'  AND FirstAccessTime<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'  GROUP BY  LEFT(TestRoomCode,12) ORDER BY MAX(b.OrderID) ";
            DataSet ds = DbHelperSQL.Query(SQL);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                List<ChartModel> list = new List<ChartModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    ChartModel trcs = new ChartModel();
                    trcs.Description = dr["SegmentName"].ToString() + "\n" + dr["CompanyName"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["nTotals"].ToString());
                    //trcs.IntNumberMarks = Int32.Parse(dr["nUserCounts"].ToString());
                    trcs.Para1 = dr["CompanyCode"].ToString();
                    list.Add(trcs);
                }
                return JsonConvert.SerializeObject(list);
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    private String bsloginlog(string proc)
    {
        int records = 0, pageCount = 0;
        string username = "";
        if (Session["UserName"] != null)
        {
            username = Session["UserName"].ToString();
        }
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        DataTable dt = LineDbHelperSQL.GetDataTableFromProc(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), username, sysBaseLine.ID.ToString(), PageIndex, PageSize, "", "", out pageCount, out records);
        if (dt != null)
        {
            string Data = JsonConvert.SerializeObject(dt);
            return "{\"pageCount\":\"" + pageCount + "\",\"Data\":" + Data + "}";
        }
        else
        {
            return "";
        }
    }

    private String bsloginpop(string proc, string users)
    {
        int records = 0, pageCount = 0;
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        DataTable dt = LineDbHelperSQL.GetDataTableFromProc(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), users, sysBaseLine.ID.ToString(), PageIndex, PageSize, "", "", out pageCount, out records);
        if (dt != null)
        {
            string Data = JsonConvert.SerializeObject(dt);
            return "{\"pageCount\":\"" + pageCount + "\",\"Data\":" + Data + "}";
        }
        else
        {
            return "";
        }
    }

    private String loginpop(string proc, string testcode)
    {
        //sTestcode = Request.Params["sTestcode"];
        //result = loginpop("spweb_login_charts_pop", sTestcode);
        BLL_LoginLog BLL = new BLL_LoginLog();
        testcode = GetSelectTree(testcode, SelectedTestRoomCodes);
        //DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, 1);
        if (!String.IsNullOrEmpty(testcode))
        {
            string SQL = " SELECT UserName,MAX(SegmentName) SegmentName,MAX(CompanyName) CompanyName,MAX(TestRoomName) TestRoomName,COUNT(1) nTotals FROM dbo.sys_loginlog WHERE TestRoomCode IN (" + testcode + ")   AND loginDay>='" + StartDate + "'  AND loginDay<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' GROUP BY UserName ORDER BY COUNT(1) DESC ";
            DataSet ds = DbHelperSQL.Query(SQL);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                List<ChartModel> list = new List<ChartModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    ChartModel trcs = new ChartModel();
                    trcs.Description = dr["UserName"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["nTotals"].ToString());
                    trcs.Para1 = dr["UserName"].ToString();
                    list.Add(trcs);
                }
                return JsonConvert.SerializeObject(list);
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }



    }


    private String onelogin(string proc, string testcode)
    {
        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), testcode, 1);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["loginDay"].ToString();
                trcs.IntNumber = Int32.Parse(dr["nTotals"].ToString());
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    #endregion

    #region 不合格报告统计
    private String failreportsum(string proc, int ntype)
    {
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, 1);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                if (ntype == 1)
                {
                    trcs.Description = dr["segment"].ToString() + "\n" + dr["company"].ToString();
                    trcs.Para1 = dr["companycode"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["totalncount"].ToString());
                    trcs.IntNumberMarks = Int32.Parse(dr["counts"].ToString());
                    trcs.DoubleNumber = Double.Parse(dr["prenct"].ToString());

                }
                else if (ntype == 2)
                {
                    trcs.Description = dr["segment"].ToString() + "\n" + dr["company"].ToString() + "\n" + dr["testroom"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["totalncount"].ToString());
                    trcs.IntNumberMarks = Int32.Parse(dr["counts"].ToString());
                    trcs.DoubleNumber = Double.Parse(dr["prenct"].ToString());
                }
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    private String spweb_failreport_chart_order(string proc, int ntype)
    {
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetProcDataTableChartsPara5(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, 1);
        if (dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                if (ntype == 1)
                {
                    trcs.Description = dr["segment"].ToString() + "\n" + dr["company"].ToString();
                    trcs.Para1 = dr["companycode"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["totalncount"].ToString());
                    trcs.IntNumberMarks = Int32.Parse(dr["counts"].ToString());
                    trcs.DoubleNumber = Double.Parse(dr["prenct"].ToString());
                    trcs.FloatNumber1 = Int32.Parse(dr["uncounts"].ToString());

                }
                else if (ntype == 2)
                {
                    trcs.Description = dr["segment"].ToString() + "\n" + dr["company"].ToString() + "\n" + dr["testroom"].ToString();
                    trcs.IntNumber = Int32.Parse(dr["totalncount"].ToString());
                    trcs.IntNumberMarks = Int32.Parse(dr["counts"].ToString());
                    trcs.DoubleNumber = Double.Parse(dr["prenct"].ToString());
                }
                list.Add(trcs);
            }
            return JsonConvert.SerializeObject(list);
        }
        else
        {
            return "";
        }
    }

    #endregion
    private String testdata(string id, string num)
    {
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetTestData(id,num);
        List<TestData> cells = new List<TestData>();
        if (dt != null&&dt.Rows.Count>0)
        {
            int m = 0;
            DateTime d0 = new DateTime();//第一根开始时间
            foreach (DataRow dr in dt.Rows)
            {
                List<JZTestDataCell> testdatalist;
                try
                {
                    testdatalist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JZTestDataCell>>(dr["RealTimeData"].ToString());
                }
                catch
                {
                   testdatalist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JZTestDataCell>>( BizCommon.JZCommonHelper.GZipDecompressString(dr["RealTimeData"].ToString()));
                }

               

                int n = 0;
                foreach (JZTestDataCell item in testdatalist)
                {
                    if (n==0)
                    {
                        d0 = testdatalist[0].Time;
                    }
                    TimeSpan span = (TimeSpan)(item.Time - d0);
                   TestData model = new TestData();

                   model.Time = Math.Round(span.TotalMilliseconds / 1000, 2);
                   model.Value = item.Value;
                   cells.Add(model);
                    n++;                  
                }
                m++;
            }
            return JsonConvert.SerializeObject(cells);
            
        }
        else
        {
            return "";
        }
    }
}

