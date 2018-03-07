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

public partial class report_failReport :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List();
        Chart();
    }


    private void List()
    {
        if ("Act".RequestStr() != "List")
        {
            return;
        }
        string sqlwhere = "and  ReportDate between '" + "StartDate".RequestStr() + "' AND  '" + DateTime.Parse("EndDate".RequestStr()).ToString("yyyy-MM-dd") + "' AND  F_InvalidItem NOT LIKE '%#%' AND AdditionalQualified=0  ";


        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {

            switch ("RPNAME".RequestStr())
            {
                //'其它','钢筋','钢筋焊接','钢筋机械连接','混凝土（标养）','混凝土（同条件）','粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂'
                case "已处理": //已处理
                    sqlwhere += @" AND  (DealResult IS NOT NULL) AND DealResult <> ''  ";
                    break;
                case "未处理": //未处理
                    sqlwhere += @" AND (DealResult='' OR DealResult IS NULL) ";
                    break;
                default:
                    sqlwhere += @" AND m.StatisticsCatlog IN (" + Server.UrlDecode("RPNAME".RequestStr()) + ") ";
                    break;
            }
        }

 

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
                        select d.IndexID from v_invalid_document  d left outer join sys_module m on d.ModelIndex = m.id  

                        where 1=1  {0}   Order By  OrderID ASC

                        SELECT     
                        d.*,convert(varchar,d.ReportDate,23) as ReportDate1,
                        d.F_InvalidItem as F_InvalidItem1,d.F_InvalidItem as F_InvalidItem2
                        FROM v_invalid_document   d
                        left outer join sys_module on  d.ModelIndex = sys_module.id                        
                        INNER JOIN @TempTable t ON d.IndexID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        Order By  OrderID ASC

                        DECLARE @C int
                        select @C= count(d.IndexID)  from v_invalid_document d left outer join sys_module m on d.ModelIndex = m.id  where 1=1  {0}  
                        select @C 
                        ";

            string PageIndex = Request["page"];
            string PageSize = Request["rows"];

            Sql = string.Format(Sql, sqlwhere, PageIndex, PageSize);


            BLL_LoginLog BLL = new BLL_LoginLog();
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


    private void Chart()
    {
        if ("Act".RequestStr() != "Chart")
        {
            return;
        }
        int ntype = 1;
        BLL_Document BLL = new BLL_Document();
        DataTable dt = BLL.GetProcDataTableChartsPara5("spweb_failreport_chart_order", "StartDate".RequestStr(), DateTime.Parse("EndDate".RequestStr()).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, 1);
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
             Response.Write(JsonConvert.SerializeObject(list));
        }
        else
        {
            Response.Write("");
        }
        Response.End();
    
    }

}