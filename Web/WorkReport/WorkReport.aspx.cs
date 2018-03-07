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


public partial class WorkReport_WorkReport : BasePage
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

        int RecordCount = 0;

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
            Dr["上"+unit+"资料数量"] = Ds.Tables[1].Compute("sum(ct)", " testroomcode ='" + Dr["NodeCode"].ToString() + "'").ToString();
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


        string Json = JsonConvert.SerializeObject(Rooms);
        Json = "{\"rows\":" + Json + ",\"total\":" + RecordCount + "}";

        Response.Write(Json);
        Response.End();
    }
}