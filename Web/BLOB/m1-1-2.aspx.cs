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

public partial class BLOB_m1_1_2 : BasePage
{
    public string ChartData = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {

                case "CHART":
                   
                    break;


            }
        }

        Chart();
    }

    public void Chart()
    {
        string[] colors = { "#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000" };
        int i = 0;

        DataSet Ds = BLOB.RawPro("ID".RequestStr(),
            "StartDate".RequestStr(), "EndDate".RequestStr()
            );

     

        DataTable DT = Ds.Tables[0];
        DataTable DTC = Ds.Tables[1];

  

        string Json = string.Empty;

        foreach (DataRow Dr in DT.Rows)
        {
            if (i == colors.Length)
            { i = 0; }
            Json += Json.IsNullOrEmpty() ? "" : ",";
            Json += "{";
            Json += "\"SegmentCode\":\"" + Dr["SegmentCode"].ToString() + "\"";
            Json += ",\"Description\":\"" + Dr["Description"].ToString() + "\"";

            DTC.DefaultView.RowFilter = "SegmentCode = '" + Dr["SegmentCode"].ToString() + "'";
            foreach (DataRow Drc in DTC.DefaultView.ToTable().Rows)
            {
                Json += ",\"" + Drc["Description"].ToString() + "\":\"" + Drc["val"].ToString() + "\"";
                Json += ",\"" + Drc["Description"].ToString() + "c\":\"" + Drc["c"].ToString() + "\"";

                Json += ",\"" + Drc["Description"].ToString() + "t\":\"" + (Dr["Description"].ToString().IndexOf("JL")>-1?"平行":"") + "\"";
            }
            //Json += ",\"color\":\"" + colors[i] + "\"";
            Json += "}";

            i++;
        }

        Json = "{Data:[" + Json + "],Test:" + JsonConvert.SerializeObject(Ds.Tables[2]) + "}";



        //DT.Columns.Add("color");
       
        //foreach (DataRow Dr in DT.Rows)
        //{
        //    if (i == colors.Length)
        //    { i = 0; }
        //    Dr["color"] = colors[i];
        //    i++;
        //}

        ChartData = Json;
        //Response.Write(Json);
        //Response.End();
    }
}