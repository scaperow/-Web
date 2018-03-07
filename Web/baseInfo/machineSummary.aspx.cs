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


public partial class baseInfo_MachineSummary : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {

                case "CHART":
                    Chart();
                    break;
                case "LIST":
                    List();
                    break;
            }
        }
    }


    public void Chart()
    {
        string Sql = @"
                    select 
                    t1.Description as sg ,
                    t2.Description,
                    t2.nodecode as Para1, 
                    COUNT(b1.ID) as IntNumber,
                    Sum(case when b1.Ext22<'{0}' then 1 else 0 end) as OIntNumber
                    from sys_tree t1
                    left outer join sys_tree t2 on left(t2.nodecode,8)=t1.nodecode AND len(t2.nodecode)=12
                    left outer join sys_document b1 on t2.nodecode= b1.CompanyCode AND b1.TestRoomCode IN ({1}) 
                    AND b1.ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') 
                    where len(t1.nodecode)=8  AND  b1.Status>0  
                    group by t1.OrderID,t2.Description,t1.Description, t2.nodecode 
                    order by t1.OrderID asc 
";


        BLL_Machine BLL = new BLL_Machine();

        DataTable Dt = BLL.GetDataTable(string.Format(Sql, DateTime.Now.AddDays(1).AddSeconds(-1).ToString(), SelectedTestRoomCodes ));

        if (Dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in Dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["sg"].ToString() + "\n" + dr["Description"].ToString();
                trcs.IntNumber = Int32.Parse(dr["IntNumber"].ToString());
                trcs.Para1 = dr["Para1"].ToString();
                trcs.IntNumberMarks = Int32.Parse(dr["OIntNumber"].ToString());
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

    public void List()
    {
        int RecordCount = 0;

        string Sql = @"DECLARE @Page int
                        DECLARE @PageSize int
                        SET @Page = {1}
                        SET @PageSize = {2}
                        SET NOCOUNT ON
                        DECLARE @TempTable TABLE (IndexId int identity, _keyID varchar(200))
                        INSERT INTO @TempTable
                        (
	                        _keyID
                        )
                        select id from  sys_document join Sys_Tree t1 on t1.NodeCode = sys_document.SegmentCode   where ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') {0}  order by t1.OrderID asc

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
                    Ext21 as '上次校验日期',
                    Ext22 as '预计下次校验日期',
                    Ext15 as '检定周期'
                     from  sys_document 
                     join Sys_Tree t1 on t1.NodeCode = sys_document.SegmentCode
                     join Sys_Tree t2 on t2.NodeCode = sys_document.CompanyCode
                     join Sys_Tree t3 on t3.NodeCode = sys_document.TestRoomCode
                        INNER JOIN @TempTable t ON sys_document.id = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        AND ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') 
                         {0}        
                        Order By  t1.OrderID  asc

                        DECLARE @C int
                        select @C=Count(id) from  sys_document   where ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') {0}
                        select @C ";

      
        //A14 I14

        string Where = " AND Status>0  ";


        switch ("meet".RequestStr())
        {
            case "1":
                Where += " AND Ext22 <'" + DateTime.Now.AddDays(1).AddSeconds(-1).ToString() + "' ";
                break;
            case "2":
                Where += " AND Ext22 >'" + DateTime.Now.AddDays(1).AddSeconds(-1).ToString() + "' ";
                break;
        }

        #region For首页
        if (!"NUM".RequestStr().IsNullOrEmpty())
        {
            Where += " and TestRoomCode in ('" + "NUM".RequestStr() + "') ";
        }
        else if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
        {
            Where += " and TestRoomCode in (" + SelectedTestRoomCodes + ") ";
        }
        else
        {
            Where += "and 1=1";
        }



        if (!string.IsNullOrEmpty("RPNAME".RequestStr()))
        {

            switch ("RPNAME".RequestStr())
            {
                case "1": //待标定数
                    Where += " AND Ext22 <'" + DateTime.Now.AddDays(1).AddSeconds(-1).ToString() + "' ";
                    break;
            }
        }
        #endregion

        Sql = string.Format(Sql,
           Where,
           "page".RequestStr(),
           "rows".RequestStr());

        RecordCount = 0;
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());


        decimal Tempc = Math.Round(decimal.Parse(Ds.Tables[1].Rows[0][0].ToString()) / decimal.Parse("rows".RequestStr()), 2);
        Tempc = Math.Ceiling(Tempc);

        string Json = JsonConvert.SerializeObject(Ds.Tables[0]); 
        int pageCount = Tempc.ToString().Toint();
        Json = "{\"total\": \"" + pageCount + "\", \"page\": \"" + "page".RequestStr() + "\", \"records\": \"" + RecordCount + "\", \"rows\" : " + Json + "}";

        Response.Write(Json);
        Response.End();
    }


}