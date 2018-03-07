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

public partial class WorkReport_FailReport : BasePage
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
        DataSet Ds = new DataSet();
        using (System.Data.SqlClient.SqlConnection Conn = BLL.Connection as System.Data.SqlClient.SqlConnection)
        {
            Conn.Open();
            using (System.Data.SqlClient.SqlCommand Cmd = new System.Data.SqlClient.SqlCommand(Sql, Conn))
            {
                using (System.Data.SqlClient.SqlDataAdapter Adp = new System.Data.SqlClient.SqlDataAdapter(Cmd))
                {
                    Adp.Fill(Ds);
                }
            }
            Conn.Close();
        }


        string Json = JsonConvert.SerializeObject(Ds.Tables[0]);
        Json = "{\"rows\":" + Json + ",\"total\":0}";

        Response.Write(Json);
        Response.End();
    }
}