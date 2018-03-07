using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Web.Script.Serialization;
using JZ.BLL;

public partial class Export_Export :  BasePage
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        Init();

    }

    /// <summary>
    /// 
    /// </summary>
    public void Init()
    {

        StartDate = "StartDate".RequestStr();
        EndDate = "EndDate".RequestStr();
        switch ("sType".RequestStr().ToUpper())
        {
            case "DOCUMENTSEARCH":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("资料查询" + StartDate + "至" + EndDate + ".xls"), DocumentSearch(), null);
                break;
            case "QXZLTJ":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("全线资料统计" + StartDate + "至" + EndDate + ".xls"), QXZLTJ(), null);
                break;
            case "RYQK":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("人员情况.xls"), RYQK(), null);
                break;

            case "SBQK":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("设备情况.xls"), SBQKWITHIMG(), null);
                break;
                
            case "BHGSJFX":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("不合格数据分析.xls"), BHGSJFX(), null);
                break;
            case "BHGSJTJPM":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("不合格数据统计排名.xls"), BHGSJTJPM(), null);
                break;
            case "PXPLFX":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("平行频率分析.xls"), PXPLFX(), null);
                break;
            case "JZPLFX":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("见证频率分析.xls"), JZPLFX(), null);
                break;
            case "XYPJ":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("铁路建设项目信用评价.xls"), XYPJ(), null);
                break;
            case "LOGINLOG":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("客户端登录日志" + StartDate + "至" + EndDate + ".xls"), LOGINLOG(), null);
                break;
            case "WORKREPORT":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("工作汇报表.xls"), WORKREPORT(), null);
                break;
            case "BHGBG":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("不合格报告.xls"), BHGBG(), null);
                break;
            case "SCZLPM":
                ExportToExcel(System.Web.HttpUtility.UrlEncode("上传资料排名.xls"), SCZLPM(), null);
                break;
            default: break;
        }
    }
   
    /// <summary>
    /// 弹出下载框
    /// </summary>
    /// <param name="argResp">弹出页面</param>
    /// <param name="argFileStream">文件流</param>
    /// <param name="strFileName">文件名</param>
    public static void DownloadFile(HttpResponse argResp, StringBuilder argFileStream, string strFileName)
    {
        try
        {
            string asdasdasd = argFileStream.ToString();
            string strResHeader = "attachment; filename=" + Guid.NewGuid().ToString() + ".csv";
            if (!string.IsNullOrEmpty(strFileName))
            {
                strResHeader = "inline; filename=" + strFileName;
            }
            argResp.AppendHeader("Content-Disposition", strResHeader);//attachment说明以附件下载，inline说明在线打开
            argResp.ContentType = "application/ms-excel";
            argResp.ContentEncoding = Encoding.GetEncoding("GB2312"); // Encoding.UTF8;//
            argResp.Write(argFileStream);
            argResp.Flush();
            argResp.SuppressContent = true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #region  导出

    /// <summary>
    /// 导出CSV
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="DataSource"></param>
    public void ExportToCSV(string fileName, DataTable DataSource, Col[] Cols)
    {
        DataGrid exportGrid = GetExportGrid(Cols);
        exportGrid.DataSource = DataSource;
        exportGrid.DataBind();
        this.RenderCSVToStream(exportGrid, fileName);
    }


    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="DataSource"></param>
    public void ExportToExcel(string fileName, DataTable DataSource, Col[] Cols)
    {
        DataGrid exportGrid = GetExportGrid(Cols);
        exportGrid.DataSource = DataSource;
        exportGrid.DataBind();
        this.RenderExcelToStream(exportGrid, fileName);
    }

    /// <summary>
    /// 获取Grid
    /// </summary>
    /// <returns></returns>
    private DataGrid GetExportGrid(Col[] Columns)
    {

        DataGrid child = new DataGrid();
        this.Controls.Add(child);

        child.ID = Guid.NewGuid().ToString() + "_exportGrid";
        if (Columns == null)
            return child;
        child.AutoGenerateColumns = false;
        foreach (Col column in Columns)
        {
            BoundColumn column2 = new BoundColumn
            {
                DataField = column.DataField
            };
            string str = string.IsNullOrEmpty(column.HeaderText) ? column.DataField : column.HeaderText;
            column2.HeaderText = str;
            //column2.DataFormatString = column.DataFormatString;
            //column2.FooterText = column.FooterValue;
            child.Columns.Add(column2);

        }
        return child;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string QuoteText(string input)
    {
        return string.Format("\"{0}\"", input.Replace("\"", "\"\"").Replace("&nbsp;", ""));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="fileName"></param>
    private void RenderCSVToStream(DataGrid grid, string fileName)
    {

        StringBuilder builder = new StringBuilder();


        for (int i = 0; i < grid.Columns.Count; i++)
        {
            builder.AppendFormat("{0}{1}", grid.Columns[i].HeaderText, (i == grid.Columns.Count - 1) ? "" : ",");
        }

        builder.Append("\n");
        foreach (DataGridItem item in grid.Items)
        {
            for (int i = 0; i < item.Cells.Count; i++)
            {

                builder.AppendFormat("{0}{1}", item.Cells[i].Text, (i == item.Cells.Count - 1) ? "" : ",");

            }
            builder.Append("\n");
        }

        //DownloadFile(Response, builder, "1111.cvs");
        //return;

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        HttpContext.Current.Response.ContentType = "application/ms-excel;charset=GB2312;";
        HttpContext.Current.Response.SendResponse(builder.ToString());

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="fileName"></param>
    private void RenderExcelToStream(DataGrid grid, string fileName)
    {

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        HttpContext.Current.Response.ContentType = "application/ms-excel;charset=UTF-8;";
        StringWriter writer = new StringWriter();
        HtmlTextWriter writer2 = new HtmlTextWriter(writer);
        grid.RenderControl(writer2);
        HttpContext.Current.Response.SendResponse(writer.ToString());

    }

    #endregion


    #region 获取DataTable

    int pageCount = 0;
    int records = 0;

    #region 周报/月报

    public DataTable WORKREPORT()
    {

        int Now = (int)DateTime.Now.DayOfWeek;

        string RType = "RType".RequestStr();
        string StartDate = "";
        string EndDate = "";

        string UStartDate = "";
        string UEndDate = "";
        string unit = "周";

        switch (RType)
        {
            case "W":
                StartDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                EndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                UStartDate = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
                UEndDate = StartDate.ToDateTime().AddDays(-1).ToString("yyyy-MM-dd");

                unit = "周";
                break;

            case "M":
                StartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-" + 26);
                EndDate = DateTime.Now.ToString("yyyy-MM-" + 25);

                UStartDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-" + 26);
                UEndDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-" + 25);
                unit = "月";
                break;

        }


        #region SQL

        string SqlRoom = @"
                        select 
                        tsg.DESCRIPTION+' '+t.DESCRIPTION as 单位,

                        t.NodeCode
                        ,''as 上{0}资料数量	
                        ,''as 本{0}资料数量	
                        ,''as 截止本{0}累计数量	
                        ,''as 本{0}资料修改次数	
                        ,''as 上{0}登录次数	
                        ,''as 本{0}系统登录次数	
                        ,''as 截止累计登录次数	
                        ,''as 本{0}不合格报告数量	
                        ,''as 处理完成数量	
                        ,''as 未处理不合格报告数量
                        ,''as 在岗人数	
                        ,''as 离岗人数
                        ,''as 设备数量
                        from sys_Tree t
                       
                        left outer join sys_Tree tsg on tsg.nodecode = left(t.nodecode,8)

                        where len(t.NodeCode)=12
                        order by t.OrderID  asc 
                        ";

        string Sql = @" 
                        -- 1 上周资料
                        select left(testroomcode,12) as testroomcode,count(1) as ct from sys_document a 
                        join sys_module b on a.moduleid=b.id
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{0}' and '{1}' group by a.testroomcode

                        -- 2 本周资料
                        select left(testroomcode,12) as testroomcode,count(1) as ct from sys_document a 
                        join sys_module b on a.moduleid=b.id
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{2}' and '{3}' group by a.testroomcode
                        
                        --3 本周修改资料次数
                        SELECT left(testroomcode,12) as testroomcode,count(1) as ct
                        FROM sys_request_change
                        where RequestTime between  '{2}' and '{3}' 
                        group by TestRoomCode
    
                        -- 4 上周周系统登录次数
                        SELECT left(testroomcode,12) as testroomcode, COUNT(1) as ct  FROM dbo.sys_loginlog WHERE  FirstAccessTime between '{0}' and '{1}' AND len(testroomcode)>12  group by testroomcode

                        -- 5 本周系统登录次数
                        SELECT left(testroomcode,12) as testroomcode, COUNT(1) as ct  FROM dbo.sys_loginlog WHERE  FirstAccessTime between '{2}' and '{3}' AND len(testroomcode)>12  group by testroomcode


                        -- 6 本周不合格资料
                        SELECT left(testroomcode,12) as testroomcode,count(1) as ct,b.StatisticsCatlog  FROM dbo.sys_invalid_document a
                        join sys_module b on a.moduleid=b.id
                        WHERE AdditionalQualified=0  and status>0   and 
                        bgrq between '{2}' and '{3}' AND  F_InvalidItem NOT LIKE '%#%'
                        group by a.testroomcode,b.StatisticsCatlog

                        
                        -- 7 本周已经处理资料
                        SELECT left(testroomcode,12) as testroomcode,count(1) as ct,b.StatisticsCatlog  FROM dbo.sys_invalid_document a
                        join sys_module b on a.moduleid=b.id
                        WHERE AdditionalQualified=0  and status>0   and (DealResult <>'' OR DealResult IS NOT NULL) and
                        bgrq between '{2}' and '{3}' AND  F_InvalidItem NOT LIKE '%#%'
                        group by a.testroomcode,b.StatisticsCatlog

                        -- 8 本周未处理资料
                        SELECT left(testroomcode,12) as testroomcode,count(1) as ct,b.StatisticsCatlog  FROM dbo.sys_invalid_document a
                        join sys_module b on a.moduleid=b.id
                        WHERE AdditionalQualified=0  and status>0   and (DealResult='' OR DealResult IS NULL) and
                        bgrq between '{2}' and '{3}' AND  F_InvalidItem NOT LIKE '%#%'
                        group by a.testroomcode,b.StatisticsCatlog


                        -- 9 截止本月累计数  资料
                        select left(testroomcode,12) as testroomcode,count(1) as ct from sys_document a 
                        join sys_module b on a.moduleid=b.id
                        where status>0 and b.moduletype=1  
                         group by a.testroomcode

                        --10 截止累计登录次
                        SELECT left(testroomcode,12) as testroomcode, COUNT(1) as ct  FROM dbo.sys_loginlog WHERE   len(testroomcode)>12  group by testroomcode
                        
                        -- 11 人员
                        SELECT 
                        left(testroomcode,12) as testroomcode,
                        sum(case  when Status>0  then 1 else 0 end) as ct, --在岗人数
                        sum(case  when Status=0  then 1 else 0 end) as lct --离岗人数
                        FROM dbo.sys_document WHERE ModuleID='08899BA2-CC88-403E-9182-3EF73F5FB0CE'
                        group by testroomcode
                        
                        --12 设备
                        SELECT left(testroomcode,12) as testroomcode,COUNT(1) as ct FROM sys_document where ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') and Status>0 group by TestRoomCode 
                        ";


        #endregion


        SqlRoom = string.Format(SqlRoom, unit);


        Sql = string.Format(Sql, UStartDate, UEndDate, StartDate, EndDate);

        Sql = SqlRoom + Sql;
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);

        DataTable Rooms = Ds.Tables[0];

        foreach (DataRow Dr in Rooms.Rows)
        {
            Dr["上" + unit + "资料数量"] = Ds.Tables[1].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["本" + unit + "资料数量"] = Ds.Tables[2].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["截止本" + unit + "累计数量"] = Ds.Tables[9].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["本" + unit + "资料修改次数"] = Ds.Tables[3].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["上" + unit + "登录次数"] = Ds.Tables[4].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["本" + unit + "系统登录次数"] = Ds.Tables[5].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["截止累计登录次数"] = Ds.Tables[10].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["本" + unit + "不合格报告数量"] = Ds.Tables[6].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["处理完成数量"] = Ds.Tables[7].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["未处理不合格报告数量"] = Ds.Tables[8].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();


            Dr["在岗人数"] = Ds.Tables[11].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["离岗人数"] = Ds.Tables[11].Compute("sum(lct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
            Dr["设备数量"] = Ds.Tables[12].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
        }
        Rooms.Columns.Remove("NodeCode");

        DataRow All = Rooms.NewRow();
        All["单位"] = "合计";
        All["上" + unit + "资料数量"] = Ds.Tables[1].Compute("sum(ct)", "").ToString();
        All["本" + unit + "资料数量"] = Ds.Tables[2].Compute("sum(ct)", "").ToString();
        All["截止本" + unit + "累计数量"] = Ds.Tables[9].Compute("sum(ct)", " ").ToString();
        All["本" + unit + "资料修改次数"] = Ds.Tables[3].Compute("sum(ct)", "").ToString();
        All["上" + unit + "登录次数"] = Ds.Tables[4].Compute("sum(ct)", "").ToString();
        All["本" + unit + "系统登录次数"] = Ds.Tables[5].Compute("sum(ct)", "").ToString();
        All["截止累计登录次数"] = Ds.Tables[10].Compute("sum(ct)", " ").ToString();
        All["本" + unit + "不合格报告数量"] = Ds.Tables[6].Compute("sum(ct)", "").ToString();
        All["处理完成数量"] = Ds.Tables[7].Compute("sum(ct)", "").ToString();
        All["未处理不合格报告数量"] = Ds.Tables[8].Compute("sum(ct)", "").ToString();
        All["在岗人数"] = Ds.Tables[11].Compute("sum(ct)", " ").ToString();
        All["离岗人数"] = Ds.Tables[11].Compute("sum(lct)", " ").ToString();
        All["设备数量"] = Ds.Tables[12].Compute("sum(ct)", " ").ToString();




        Rooms.Rows.Add(All);

        return Rooms;
    }

        /// <summary>
    /// 不合格数据 周报/月报
    /// </summary>
    /// <returns></returns>
    public DataTable BHGBG()
    {

        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr().ToDateTime().ToString("yyyy-MM-dd");


        #region SQL


        string Sql = @" --sass
                        SELECT     
                        d.*,convert(varchar,d.ReportDate,23) as ReportDate1,
                        d.F_InvalidItem as F_InvalidItem1,d.F_InvalidItem as F_InvalidItem2,
						case  
						when AdditionalQualified =0 then '未合格' 
						 when AdditionalQualified =1 then '已合格'
						  end as st
                        FROM v_invalid_document   d
                        left outer join sys_module on  d.ModelIndex = sys_module.id     
						where F_InvalidItem NOT LIKE '%#%' 
						and ReportDate between '{0}' AND '{1}'

                        ";


        #endregion




        Sql = string.Format(Sql, StartDate, EndDate);

        BLL_Document BLL = new BLL_Document();
        DataSet Ds = Sql.GetData();
       
       
        DataTable dt = Ds.Tables[0];

       
        dt.Columns.Remove("CompanyCode");
        dt.Columns.Remove("TestRoomCode");
        dt.Columns.Remove("AdditionalQualified");
        dt.Columns.Remove("DeviceType");
        dt.Columns.Remove("WTBH");
         dt.Columns.Remove("OrderID");
         dt.Columns.Remove("ReportDate1");
         dt.Columns.Remove("F_InvalidItem1");
         dt.Columns.Remove("F_InvalidItem2");
        			

        dt.Columns["ReportDate"].ColumnName = "日期";
        dt.Columns["ReportName"].ColumnName = "试验报告";
        dt.Columns["ReportNumber"].ColumnName = "报告编码";
        dt.Columns["F_InvalidItem"].ColumnName = "不合格项目";
        dt.Columns["SGComment"].ColumnName = "原因分析";
        dt.Columns["JLComment"].ColumnName = "监理意见";
        dt.Columns["SectionName"].ColumnName = "标段";
        dt.Columns["CompanyName"].ColumnName = "单位";
        dt.Columns["TestRoomName"].ColumnName = "试验室";
        dt.Columns["DealResult"].ColumnName = "处理情况";
        dt.Columns["st"].ColumnName = "状态";
        dt.Columns.Add("标准值");
        dt.Columns.Add("误差值");

        dt.Columns["状态"].SetOrdinal(0);
        dt.Columns["日期"].SetOrdinal(1);
        dt.Columns["标段"].SetOrdinal(2);
        dt.Columns["单位"].SetOrdinal(3);
        dt.Columns["试验室"].SetOrdinal(4);
        dt.Columns["试验报告"].SetOrdinal(5);
        dt.Columns["不合格项目"].SetOrdinal(6);
        dt.Columns["标准值"].SetOrdinal(7);
        dt.Columns["误差值"].SetOrdinal(8);
        dt.Columns["原因分析"].SetOrdinal(9);
        dt.Columns["监理意见"].SetOrdinal(10);
        dt.Columns["处理情况"].SetOrdinal(11);
        dt.Columns["报告编码"].SetOrdinal(12);

        string[] Temp = null;
        string[] Temp1 = null;
        string _XM = string.Empty;
        string _SZ = string.Empty;
        string _WCZ = string.Empty;

        foreach (DataRow Dr in dt.Rows)
        {
            Temp = Dr["不合格项目"].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            if (Temp == null) { continue; }
            foreach (string V in Temp)
            {
                if (V.IsNullOrEmpty()) { break; }
                Temp1 = V.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                _XM += Temp1.Length >=1 ?  Temp1[0]+"<br />":"";
                _SZ += Temp1.Length >= 2?  Temp1[1] + "<br />":"";
                _WCZ += Temp1.Length >= 3 ? Temp1[2] + "<br />" : "";
            }
            Dr["不合格项目"] = _XM;
            Dr["标准值"] = _SZ;
            Dr["误差值"] = _WCZ;
            Temp = null;
            Temp1 = null;
             _XM = string.Empty;
             _SZ = string.Empty;
             _WCZ = string.Empty;
        }

        return dt;
    }

    
   /// <summary>
    /// 上传资料排名
    /// </summary>
    /// <returns></returns>
    public DataTable SCZLPM()
    {

        int Now = (int)DateTime.Now.DayOfWeek;

        string RType = "RType".RequestStr();
        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr().ToDateTime().AddDays(1).ToString("yyyy-MM-dd");



        #region SQL



        string Sql = @" 


                        -- 0 施工前三
                        select top 3
						t1.description+' '+t2.description+' '+t.description as '单位'
						
						,count(1) as '数量' 
						from sys_document a 
                        join sys_module b on a.moduleid=b.id
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{1}' and '{0}' AND t1.DepType='@unit_施工单位'
						group by t.description,t1.description,t2.description
						order by 数量 desc

                        -- 1 施工后三
                        select top 3
						t1.description+' '+t2.description+' '+t.description as '单位'
						
						,count(1) as '数量' 
						from sys_document a 
                        join sys_module b on a.moduleid=b.id
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{1}' and '{0}' AND t1.DepType='@unit_施工单位'
						group by t.description,t1.description,t2.description
						order by 数量 asc
                        
                        -- 2 监理前三
                        select top 3
						t1.description+' '+t2.description+' '+t.description as '单位'
						
						,count(1) as '数量' 
						from sys_document a 
                        join sys_module b on a.moduleid=b.id
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{1}' and '{0}' AND t1.DepType='@unit_监理单位'
						group by t.description,t1.description,t2.description
						order by 数量 desc

                        -- 3系统登录次数
                        SELECT 
						top 3
						t1.description+' '+t2.description+' '+t.description as '单位'
						,
						COUNT(1) as '数量' 
						FROM dbo.sys_loginlog a
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
						WHERE  FirstAccessTime between '{1}' and '{0}' AND len(testroomcode)>12  
						group by t.description,t1.description,t2.description
						order by 数量 desc

                        -- 4 系统登录次数
                        SELECT 
						top 3
						t1.description+' '+t2.description+' '+t.description as '单位'
						,
						COUNT(1) as '数量' 
						FROM dbo.sys_loginlog a
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
						WHERE  FirstAccessTime between '{1}' and '{0}' AND len(testroomcode)>12  
						group by t.description,t1.description,t2.description
						order by 数量 asc

                        ";


        #endregion




        Sql = string.Format(Sql, EndDate, StartDate);


        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);



        DataTable Out = new DataTable();
        Out.Columns.Add(new DataColumn("0"));
        Out.Columns.Add(new DataColumn("1"));
        Out.Columns.Add(new DataColumn("2"));
        Out.Columns.Add(new DataColumn("3"));
        Out.Columns.Add(new DataColumn("4"));
        Out.Columns.Add(new DataColumn("5"));
        Out.Columns.Add(new DataColumn("6"));
        Out.Columns.Add(new DataColumn("7"));
        Out.Columns.Add(new DataColumn("8"));
        Out.Columns.Add(new DataColumn("9"));


        DataRow Dr = Out.NewRow();
        Out.Rows.Add(Dr);
        Dr = Out.NewRow();
        Out.Rows.Add(Dr);
        Dr = Out.NewRow();
        Out.Rows.Add(Dr);
        Dr = Out.NewRow();
        Out.Rows.Add(Dr);

        int ct = 0;
        for (int i = 0; i < Ds.Tables.Count; i++)
        {
            int rt = 0;
            foreach (DataRow tDr in Ds.Tables[i].Rows)
            {
                Out.Rows[rt][ct + 0] = tDr[0].ToString();
                Out.Rows[rt][ct + 1] = tDr[1].ToString();

                Out.Rows[3][ct + 0] = "合计";
                Out.Rows[3][ct + 1] = (Out.Rows[3][ct + 1].ToString().Toint() + tDr[1].ToString().Toint()).ToString();
                rt++;
            }
            ct = ct + 2;
        }

        Out.Columns[0].ColumnName = "施工单位周期前三名";
        Out.Columns[2].ColumnName = "施工单位周期后三名";
        Out.Columns[4].ColumnName = "监理单位周期前三名";
        Out.Columns[6].ColumnName = "登陆系统次数周期前三名";
        Out.Columns[8].ColumnName = "登陆系统次数周期后三名";

        Out.Columns[1].ColumnName = "施工单位周期前三名数量";
        Out.Columns[3].ColumnName = "施工单位周期后三名数量";
        Out.Columns[5].ColumnName = "监理单位周期前三名数量";
        Out.Columns[7].ColumnName = "登陆系统次数周期前三名数量";
        Out.Columns[9].ColumnName = "登陆系统次数周期后三名数量";
        return Out;
    }
    #endregion

    public DataTable LOGINLOG()
    {

        string Sql = @"select  UserName ,
          SegmentName ,
          CompanyName ,
          TestRoomName  ,
         Count(id) as 登录次数 
		 
		 from sys_loginlog
            where 1=1 
        {0}
		 group by  UserName ,
          SegmentName ,
          CompanyName ,
          TestRoomName 
    order by SegmentName,CompanyName
";



        string fileds = @"
          UserName ,
          SegmentName ,
          CompanyName ,
          TestRoomName  ,
         Count(id) as C ";
        string sqlwhere = "and FirstAccessTime>='" + StartDate + "' AND FirstAccessTime<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";


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
        Sql = string.Format(Sql, sqlwhere);
       DataTable dt = BLL.GetDataTable(Sql);

  
       // DataTable dt = BLL.GetDataTablePager("sys_loginlog"+" Group by  UserName , SegmentName , CompanyName , TestRoomName  ", fileds, sqlwhere, "loginDay", "loginDay", "0", 1, 11111111, out pageCount, out records);

        dt.Columns["UserName"].ColumnName = "用户名";
        dt.Columns["SegmentName"].ColumnName = "标段";
        dt.Columns["CompanyName"].ColumnName = "单位";
        dt.Columns["TestRoomName"].ColumnName = "试验室";
       
        return dt;
    }



    /// <summary>
    /// 获取资料查询列表
    /// </summary>
    /// <returns></returns>
    public DataTable DocumentSearch()
    {
        string sqlwhere = string.Empty;
        BLL_LoginLog BLL = new BLL_LoginLog();

        #region 查询条件
         sqlwhere = " and status>0 and BGRQ>='" + StartDate + "' AND BGRQ<'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "'";

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
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }
       
        #endregion

        #region  查询脚本

        string Sql = @" 
                        SELECT     
                        sys_tree.description 标段
                        ,t2.description   单位
                        ,t1.description 试验室 
                        ,sys_module.name as 模版名称
                        ,BGBH 报告编号
                        ,WTBH 委托编号
                        ,CONVERT(NVARCHAR(10),BGRQ,120) 报告日期   
                        FROM sys_document  
                        left outer join sys_module on sys_document.ModuleId = sys_module.id
                        left outer join sys_tree on sys_document.SegmentCode = sys_tree.nodecode
                        left outer join sys_tree t1 on sys_document.TestRoomCode = t1.nodecode
                        left outer join sys_tree t2 on sys_document.CompanyCode = t2.nodecode                        
                        where 1=1  
                        ";
        #endregion

        DataSet DS = (Sql + sqlwhere).GetData();

        return DS.Tables[0] != null ? DS.Tables[0] : null;
    }



    /// <summary>
    /// 全线资料统计
    /// </summary>
    /// <returns></returns>
    public DataTable QXZLTJ()
    {
        
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetProcDataTable("spweb_qxzlhzb", StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, 0, 1, 100000, "segment", "ASC", out pageCount, out records);

        #region 添加总数
        int pageCount1 = 0;
        int records1 = 0;
        string SelectedTestRoomCodes1 = "";

     
        foreach (DataRow Dr in dt.Rows)
        {
            if (SelectedTestRoomCodes1.IndexOf("'" + Dr["testcode"].ToString() + "'") >= 0)
            {
                continue;
            }
            SelectedTestRoomCodes1 += string.IsNullOrEmpty(SelectedTestRoomCodes1) ? "'" + Dr["testcode"].ToString() + "'" : ",'" + Dr["testcode"].ToString() + "'";
        }
        

        DataTable dt1 = BLL.GetProcDataTable("spweb_qxzlhzb", DateTime.Parse("1999-1-1").ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes1, 0, 1, 100000, "segment", "ASC", out pageCount1, out records1);

        dt.Columns.Add(new DataColumn("ncountA", typeof(string)));
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

        dt.Columns.Remove("id");
        dt.Columns.Remove("project");
        dt.Columns.Remove("testcode");
        dt.Columns.Remove("modelid");

        dt.Columns["segment"].ColumnName = "标段";
        dt.Columns["company"].ColumnName = "单位";
        dt.Columns["testroom"].ColumnName = "试验室";
        dt.Columns["testname"].ColumnName = "试验名称";
        dt.Columns["ncountA"].ColumnName = "总录入数";
        dt.Columns["ncount"].ColumnName = "时间段内录入数";
        dt.Columns["wncountA"].ColumnName = "不合格报告总数";
        dt.Columns["wncount"].ColumnName = "时间段内不合格报告数";

        dt.Columns["总录入数"].SetOrdinal(4);
        dt.Columns["时间段内录入数"].SetOrdinal(5);
        dt.Columns["不合格报告总数"].SetOrdinal(6);
        dt.Columns["时间段内不合格报告数"].SetOrdinal(7);

        return dt;
    }

    /// <summary>
    /// 人员情况
    /// </summary>
    /// <returns></returns>
    public DataTable RYQK()
    {
        BLL_UserInfo BLL = new BLL_UserInfo();
        DataTable dt = BLL.GetProcDataTable("spweb_userSummary", StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, 0, 1, 100000, "标段名称", "ASC", out pageCount, out records);

        dt.Columns.Remove("id");
        dt.Columns.Remove("userid");
        dt.Columns.Remove("试验室编码");
        dt.Columns.Remove("num");
        return dt;
    }

    /// <summary>
    /// 设备情况
    /// </summary>
    /// <returns></returns>
    public DataTable SBQK()
    {
       
        string sqlwhere = string.Empty;
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and 试验室编码 in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            sqlwhere += "and 1=0";
        }
        BLL_Machine BLL = new BLL_Machine();
        DataTable dt = BLL.GetDataTablePager("v_bs_machineSummary", "*", sqlwhere, "id", "OrderID", "asc", 1, 100000, out pageCount, out records);
        dt.Columns.Remove("rowNum");
        dt.Columns.Remove("ID");
        dt.Columns.Remove("试验室编码");
        dt.Columns.Remove("单位编码");
        dt.Columns.Remove("标段编码");
        dt.Columns.Remove("备注");
        dt.Columns.Remove("OrderID");
        dt.Columns.Remove("购置日期");

        dt.Columns["标段名称"].SetOrdinal(0);
        dt.Columns["单位名称"].SetOrdinal(1);
        return dt;
    }

    /// <summary>
    /// 设备情况
    /// </summary>
    /// <returns></returns>
    public DataTable SBQKWITHIMG()
    {
        string sqlwhere = string.Empty;
        if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            sqlwhere += " and testroomcode in (" + SelectedTestRoomCodes + ") ";
        }

        string Sql = @"

                         select  
                    id,
                    t1.description as '标段名称',
                    t2.description as '单位名称',
                    t3.description as '试验室名称',
                    Ext2 as '管理编号',
                    Ext1 as '设备名称',
                    Ext3 as '生产厂家',
                    Ext4 as '规格型号',
                    Ext9 as '数量',
                    Ext11 as '检定情况',
                    Ext12 as '检定证书编号',
                    Ext13 as '上次校验日期',
                    Ext14 as '预计下次校验日期',
                    Ext15 as '检定周期'
                     from  sys_document 
                     join Sys_Tree t1 on t1.NodeCode = sys_document.SegmentCode
                     join Sys_Tree t2 on t2.NodeCode = sys_document.CompanyCode
                     join Sys_Tree t3 on t3.NodeCode = sys_document.TestRoomCode
                       
                        WHERE ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') 
                         {0}        
                        Order By  t1.OrderID  asc

                         ";

        Sql = string.Format(Sql, sqlwhere);
        BLL_Machine BLL = new BLL_Machine();
        DataTable dt = BLL.GetDataTable(Sql);
        


        dt.Columns["标段名称"].SetOrdinal(0);
        dt.Columns["单位名称"].SetOrdinal(1);
        return dt;
    }

    /// <summary>
    /// 不合格数据分析
    /// </summary>
    /// <returns></returns>
    public DataTable BHGSJFX()
    {

        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        string sqlwhere = string.Empty;
        string name = "biz_all_failreport";
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


        BLL_LoginLog BLL = new BLL_LoginLog();
        DataTable dt = BLL.GetDataTablePager(name, "*", sqlwhere, "IndexID", "SectionName", "ASC", 1, 100000, out pageCount, out records);

     

        dt.Columns.Remove("rowNum");
        dt.Columns.Remove("CompanyCode");
        dt.Columns.Remove("TestRoomCode");
        dt.Columns.Remove("AdditionalQualified");
        dt.Columns.Remove("DeviceType");
        dt.Columns.Remove("WTBH");

        dt.Columns["ReportDate"].ColumnName = "日期";
        dt.Columns["ReportName"].ColumnName = "试验报告";
        dt.Columns["ReportNumber"].ColumnName = "报告编码";
        dt.Columns["F_InvalidItem"].ColumnName = "不合格项目";
        dt.Columns["SGComment"].ColumnName = "原因分析";
        dt.Columns["JLComment"].ColumnName = "监理意见";
        dt.Columns["SectionName"].ColumnName = "标段";
        dt.Columns["CompanyName"].ColumnName = "单位";
        dt.Columns["TestRoomName"].ColumnName = "试验室";
        dt.Columns["DealResult"].ColumnName = "处理情况";
        dt.Columns.Add("标准值");
        dt.Columns.Add("误差值");

        dt.Columns["日期"].SetOrdinal(0);
        dt.Columns["标段"].SetOrdinal(1);
        dt.Columns["单位"].SetOrdinal(2);
        dt.Columns["试验室"].SetOrdinal(3);
        dt.Columns["试验报告"].SetOrdinal(4);
        dt.Columns["不合格项目"].SetOrdinal(5);
        dt.Columns["标准值"].SetOrdinal(6);
        dt.Columns["误差值"].SetOrdinal(7);
        dt.Columns["原因分析"].SetOrdinal(8);
        dt.Columns["监理意见"].SetOrdinal(9);
        dt.Columns["处理情况"].SetOrdinal(10);
        dt.Columns["报告编码"].SetOrdinal(11);

        string[] Temp = null;
        string[] Temp1 = null;
        string _XM = string.Empty;
        string _SZ = string.Empty;
        string _WCZ = string.Empty;

        foreach (DataRow Dr in dt.Rows)
        {
            Temp = Dr["不合格项目"].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            if (Temp == null) { continue; }
            foreach (string V in Temp)
            {
                if (V.IsNullOrEmpty()) { break; }
                Temp1 = V.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                _XM += Temp1.Length >=1 ?  Temp1[0]+"<br />":"";
                _SZ += Temp1.Length >= 2?  Temp1[1] + "<br />":"";
                _WCZ += Temp1.Length >= 3 ? Temp1[2] + "<br />" : "";
            }
            Dr["不合格项目"] = _XM;
            Dr["标准值"] = _SZ;
            Dr["误差值"] = _WCZ;
            Temp = null;
            Temp1 = null;
             _XM = string.Empty;
             _SZ = string.Empty;
             _WCZ = string.Empty;
        }

        return dt;
    }


    

    /// <summary>
    /// 不合格数据统计排名
    /// </summary>
    /// <returns></returns>
    public DataTable BHGSJTJPM()
    {

 
        BLL_FailReport BLL = new BLL_FailReport();
        DataTable dt = BLL.GetProcDataTable("spweb_failreport", StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, 0, 1, 100000, "segment", "ASC", out pageCount, out records);

        dt.Columns.Remove("id");
        dt.Columns.Remove("project");
        dt.Columns.Remove("testcode");
 
        dt.Columns["segment"].ColumnName = "标段";
        dt.Columns["company"].ColumnName = "单位";
        dt.Columns["testroom"].ColumnName = "试验室";
        dt.Columns["totalncount"].ColumnName = "试验资料总数";
        dt.Columns["counts"].ColumnName = "不合格资料总数";
        dt.Columns["prenct"].ColumnName = "不合格比例(%)";

       
        return dt;
    }


    /// <summary>
    /// 平行频率分析
    /// </summary>
    /// <returns></returns>
    public DataTable PXPLFX()
    {
        // json = pxreport("", 1, out pageCount, out records);
        string proc = "";
        BLL_ParallelReport BLL = new BLL_ParallelReport();
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
        }
        else
        {
            proc = "sp_pxjz_report";
        }
        string meet = "meet".RequestStr();
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes.Replace("'", ""), 1, 1, 100000, meet, "ASC", out pageCount, out records);
        dt.Columns.Remove("id");
        dt.Columns.Remove("modelID");
        dt.Columns.Remove("testroomID");
        dt.Columns.Remove("pxqulifty");


        dt.Columns["modelName"].ColumnName = "试验名称";
        dt.Columns["condition"].ColumnName = "标准见证频率(%)";
        dt.Columns["zjCount"].ColumnName = "施工单位资料总数";
        dt.Columns["pxCount"].ColumnName = "见证次数";
        dt.Columns["frequency"].ColumnName = "见证频率(%)";
        dt.Columns["result"].ColumnName = "是否满足要求";
        dt.Columns["segment"].ColumnName = "标段";
        dt.Columns["jl"].ColumnName = "监理单位";
        dt.Columns["sg"].ColumnName = "施工单位";
        dt.Columns["testroom"].ColumnName = "施工单位试验室";

        dt.Columns["标段"].SetOrdinal(0);
        dt.Columns["监理单位"].SetOrdinal(1);
        dt.Columns["施工单位"].SetOrdinal(2);
        dt.Columns["施工单位试验室"].SetOrdinal(3);
        dt.Columns["试验名称"].SetOrdinal(4);
        dt.Columns["标准见证频率(%)"].SetOrdinal(5);
        dt.Columns["施工单位资料总数"].SetOrdinal(6);
        dt.Columns["见证次数"].SetOrdinal(7);
        dt.Columns["见证频率(%)"].SetOrdinal(8);
        dt.Columns["是否满足要求"].SetOrdinal(9);
        return dt;
    }

    /// <summary>
    /// 见证频率分析
    /// </summary>
    /// <returns></returns>
    public DataTable JZPLFX()
    {
        //  json = pxreport("", 2, out pageCount, out records);
        string proc = "";
        BLL_ParallelReport BLL = new BLL_ParallelReport();
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 0)
        {
        }
        else
        {
            proc = "sp_pxjz_report";
        }
        string meet = "meet".RequestStr();
        DataTable dt = BLL.GetProcDataTable(proc, StartDate, DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes.Replace("'", ""), 2, 1, 100000, meet, "ASC", out pageCount, out records);

     
            dt.Columns.Remove("id");
            dt.Columns.Remove("modelID");
            dt.Columns.Remove("testroomID");
            dt.Columns.Remove("pxqulifty");


            dt.Columns["modelName"].ColumnName = "试验名称";
            dt.Columns["condition"].ColumnName = "标准见证频率(%)";
            dt.Columns["zjCount"].ColumnName = "施工单位资料总数";
            dt.Columns["pxCount"].ColumnName = "见证次数";
            dt.Columns["frequency"].ColumnName = "见证频率(%)";
            dt.Columns["result"].ColumnName = "是否满足要求";
            dt.Columns["segment"].ColumnName = "标段";
            dt.Columns["jl"].ColumnName = "监理单位";
            dt.Columns["sg"].ColumnName = "施工单位";
            dt.Columns["testroom"].ColumnName = "施工单位试验室";

            dt.Columns["标段"].SetOrdinal(0);
            dt.Columns["监理单位"].SetOrdinal(1);
            dt.Columns["施工单位"].SetOrdinal(2);
            dt.Columns["施工单位试验室"].SetOrdinal(3);
            dt.Columns["试验名称"].SetOrdinal(4);
            dt.Columns["标准见证频率(%)"].SetOrdinal(5);
            dt.Columns["施工单位资料总数"].SetOrdinal(6);
            dt.Columns["见证次数"].SetOrdinal(7);
            dt.Columns["见证频率(%)"].SetOrdinal(8);
            dt.Columns["是否满足要求"].SetOrdinal(9);

        return dt;
    }

    /// <summary>
    /// 信用评价
    /// </summary>
    /// <returns></returns>
    public DataTable XYPJ()
    {

        #region 查询语句
        
        string Sql = @" 
                        DECLARE @Page int
                        DECLARE @PageSize int
                        SET @Page = {0}
                        SET @PageSize = {1}
                        SET NOCOUNT ON
                        DECLARE @TempTable TABLE (IndexId int identity, _keyID int)
                        INSERT INTO @TempTable
                        (
	                        _keyID
                        )
                        select ID from Sys_ReditRating where CreateOn between '{2}' AND '{3}' AND TestRoomCode IN ({4}) Order By  CreateOn Desc

                        SELECT
                        segmentcode  as '标段',
						companycode as '单位',
						testroomcode as '试验室',
						CompanyType as '单位性质',
						name as '姓名',
						idcard as '身份证',
						job as '职务',
						deduct as '总扣分数',
						remark as '备注',
						createon as '评价时间'
                        FROM Sys_ReditRating
                        INNER JOIN @TempTable t ON Sys_ReditRating.ID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        Order By  CreateOn Desc

                        DECLARE @C int
                        select @C= count(ID)  from Sys_ReditRating where CreateOn between '{2}' AND '{3}' AND TestRoomCode IN ({4}) 
                        select @C 
                        ";

        Sql = string.Format(Sql, 1, 100000, DateTime.Parse(StartDate).AddDays(-1).ToString("yyyy-MM-dd"), DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes);
        #endregion

        BLL_Document BLL = new BLL_Document();



        #region 事件段数量
        Sql = string.Format(Sql, SelectedTestRoomCodes, DateTime.Parse(StartDate).AddDays(-1).ToString("yyyy-MM-dd"), DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd"));

        DataSet Ds = BLL.GetDataSet(Sql);

        #endregion


        if (Ds.Tables.Count > 1)
        {
            DataTable List = Ds.Tables[0];

            #region 替换编码-》名称 防止SQL混乱

            string SqlTemp = "select nodecode,description from sys_tree ";

            DataTable TabTemp = BLL.GetDataTable(SqlTemp);

            foreach (DataRow Dr in List.Rows)
            {
                try
                {
                    TabTemp.DefaultView.RowFilter = " nodecode = '" + Dr["标段"].ToString() + "'";
                    Dr["标段"] = TabTemp.DefaultView.ToTable().Rows[0]["description"].ToString();
                    TabTemp.DefaultView.RowFilter = "";

                    TabTemp.DefaultView.RowFilter = " nodecode = '" + Dr["单位"].ToString() + "'";
                    Dr["单位"] = TabTemp.DefaultView.ToTable().Rows[0]["description"].ToString();
                    TabTemp.DefaultView.RowFilter = "";

                    TabTemp.DefaultView.RowFilter = " nodecode = '" + Dr["试验室"].ToString() + "'";
                    Dr["试验室"] = TabTemp.DefaultView.ToTable().Rows[0]["description"].ToString();
                    TabTemp.DefaultView.RowFilter = "";
                }
                catch
                { }


                TabTemp.DefaultView.RowFilter = "";
            }
            TabTemp.Clear();
            #endregion

            return List;
        }
        else
        {
            return null;
        }
        return null;
    }

    #endregion
}


public class Col
{
    private string _DataField = string.Empty;

    public string DataField
    {
        get { return _DataField; }
        set { _DataField = value; }
    }

    private string _HeaderText = string.Empty;

    public string HeaderText
    {
        get { return _HeaderText; }
        set { _HeaderText = value; }
    }
}


//解决编译报错
namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}

/// <summary>
/// 扩展
/// </summary>
public static class HttpResponseExtensions
{
    internal static void SendResponse(this HttpResponse response, string text)
    {
        try
        {
            response.Clear();
            response.Write(text);
            response.Flush();
            response.SuppressContent = true;
        }
        catch (Exception)
        {
        }
    }


    public static DataSet GetData(this string SqlText)
    {
        DataSet Ds = new DataSet();
        BLL_LoginLog BLL = new BLL_LoginLog();
        using (System.Data.SqlClient.SqlConnection Conn = BLL.Connection as System.Data.SqlClient.SqlConnection)
        {
            Conn.Open();
            using (System.Data.SqlClient.SqlCommand Cmd = new System.Data.SqlClient.SqlCommand(SqlText, Conn))
            {
                using (System.Data.SqlClient.SqlDataAdapter Adp = new System.Data.SqlClient.SqlDataAdapter(Cmd))
                {
                    Adp.Fill(Ds);
                }
                Cmd.Dispose();
            }
            Conn.Close();
            Conn.Dispose();
        }
        return Ds;
    }
}