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

public partial class logInfo_loginLog :BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {

                case "CHART":
                    Response.Write(Chart());
                    Response.End();
                    break;


            }
        }


    }


    public string Chart()
    {
        string[] colors = { "#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000" };
        int i = 0;

        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr().ToDateTime().AddDays(1).ToString("yyyy-MM-dd");
        string Sql = @"
            SELECT  
            TestRoomCode,left(TestRoomCode,12) as c,
            count(1)  as v,TestRoomName
            from sys_loginlog
            
            WHERE TestRoomCode IN ({0})   AND FirstAccessTime between '{1}' AND '{2}'
            group by TestRoomCode,TestRoomName

            select 
            a.nodecode
            ,b.description+ ' '+ a.Description as Description
            from 
            sys_tree  a
            left outer join sys_tree b on left(a.nodecode,8)=b.nodecode
            where LEN(a.nodecode) = 12
            order by a.orderid asc
";

        Sql = string.Format(Sql, SelectedTestRoomCodes, StartDate, EndDate);

        DataSet Ds = new BLL_Document().GetDataSet(Sql);



        DataTable DTR = Ds.Tables[0];
        DataTable DTC = Ds.Tables[1];

        DataTable OutC = DTR.Clone();

        string Json = string.Empty;

        foreach (DataRow Dr in DTC.Rows)
        {
            DTR.DefaultView.RowFilter = " c = '" + Dr["nodecode"].ToString() + "'";
         

                if (i == colors.Length)
                { i = 0; }
                Json += Json.IsNullOrEmpty() ? "" : ",";
                Json += "{";
                Json += "\"nodecode\":\"" + Dr["nodecode"].ToString() + "\"";
                Json += ",\"Description\":\"" + Dr["Description"].ToString() + "\"";


                foreach (DataRow Drc in DTR.DefaultView.ToTable().Rows)
                {
                    Json += ",\"" + Drc["TestRoomName"].ToString() + "\":\"" + Drc["v"].ToString() + "\"";

                    OutC.DefaultView.RowFilter = " TestRoomName = '" + Drc["TestRoomName"].ToString() + "'";
                    if (OutC.DefaultView.ToTable().Rows.Count == 0)
                    {
                        DataRow DtT = OutC.NewRow();

                        DtT["TestRoomName"] = Drc["TestRoomName"].ToString();
                        DtT["c"] = Dr["nodecode"].ToString();
                        OutC.Rows.Add(DtT);
                    }
                    OutC.DefaultView.RowFilter = "";
                }

                Json += "}";

                i++;
          
        }

        return "{\"Data\":[" + Json + "],\"Test\":" + JsonConvert.SerializeObject(OutC) + "}";

  

    }
}