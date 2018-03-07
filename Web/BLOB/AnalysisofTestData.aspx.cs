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

public partial class BLOB_AnalysisofTestData : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {
                case "ITEM":
                    Response.Write(JsonConvert.SerializeObject(BLOB.Items("1")));
                    Response.End();
                    break;

                case "CHART":
                    Chart();
                    break;

            }
        }
    }


    public void Chart()
    {

        DataTable DT = BLOB.TonByFact("ID".RequestStr(),
            "StartDate".RequestStr(), "EndDate".RequestStr()
            );

        string[] colors = { "#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000" };

        DT.Columns.Add("color");
        int i = 0;
        foreach (DataRow Dr in DT.Rows)
        {
            if (i == colors.Length)
            { i = 0; }
            Dr["color"] = colors[i];
            i++;
        }


        Response.Write(JsonConvert.SerializeObject(DT));
        Response.End();
    }
}