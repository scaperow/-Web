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

public partial class BLOB_m5 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {
                case "ITEM":
                    Response.Write(JsonConvert.SerializeObject(BLOB.Items("2")));
                    Response.End();
                    break;
                case "FACTORY":
                    Response.Write(JsonConvert.SerializeObject(BLOB.Factory("ItemID".RequestStr())));
                    Response.End();
                    break;
                case "MODEL":
                    Response.Write(JsonConvert.SerializeObject(BLOB.Model("ItemID".RequestStr())));
                    Response.End();
                    break;
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
        int RecordCount = 0;

        DataTable DT = BLOB.SamplingAnalysis(out RecordCount, 1, 100000,
            "StartDate".RequestStr(), "EndDate".RequestStr(),
            SelectedTestRoomCodes, "Item".RequestStr(), "Factory".RequestStr(),  "Model".RequestStr(),"Person".RequestStr());

        Response.Write(JsonConvert.SerializeObject(DT));
        Response.End();
    }

    public void List()
    {
        int RecordCount = 0;

        DataTable DT = BLOB.SamplingAnalysis(out RecordCount, "page".RequestStr().Toint(), "rows".RequestStr().Toint(),
            "StartDate".RequestStr(), "EndDate".RequestStr(),
            SelectedTestRoomCodes, "Item".RequestStr(), "Factory".RequestStr(), "Model".RequestStr(), "Person".RequestStr());

        string Json = JsonConvert.SerializeObject(DT);
        Json = "{\"rows\":" + Json + ",\"total\":" + RecordCount + "}";

        Response.Write(Json);
        Response.End();
    }
}