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


public partial class baseInfo_popmachineSummary : BasePage
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

            }
        }
    }

    public void Chart()
    {
        string Sql = @"
                    select count(b1.id) as IntNumber ,b1.testroomcode ,t1.description as Description
			from sys_document b1 
			left outer join sys_tree  t1 on t1.nodecode = b1.testroomcode
			 where b1.Status>0   and  b1.CompanyCode IN ('{0}') 
			 AND b1.ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') 
			  group by b1.testroomcode,t1.description
";


        BLL_Machine BLL = new BLL_Machine();

        DataTable Dt = BLL.GetDataTable(string.Format(Sql,  "sTestcode".RequestStr()));

        if (Dt != null)
        {
            List<ChartModel> list = new List<ChartModel>();
            foreach (DataRow dr in Dt.Rows)
            {
                ChartModel trcs = new ChartModel();
                trcs.Description = dr["Description"].ToString();
                trcs.IntNumber = Int32.Parse(dr["IntNumber"].ToString());
                trcs.Para1 = dr["testroomcode"].ToString();
           
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