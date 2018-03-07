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

public partial class BLOB_AnalysisofItemAttrTestData : BasePage
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
        int RecordCount = 0;

        string M = "M".RequestStr();
        M = M.IsNullOrEmpty() ? "" : M.Replace("月份", "");

        string StartDate = M + "-1";
        string EndDate = (M + "-1").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

        if (!"StartDate".RequestStr().IsNullOrEmpty() && !"EndDate".RequestStr().IsNullOrEmpty())
        {
            StartDate = "StartDate".RequestStr();
            EndDate = "EndDate".RequestStr();
        }

        DataTable DT = BLOB.DataAnalysis(out RecordCount, 1, 100000,
            StartDate, EndDate,
            SelectedTestRoomCodes, "Item".RequestStr(), "Factory".RequestStr(), "Attr".RequestStr(), "AttrName".RequestStr(), "Model".RequestStr());

        Response.Write(JsonConvert.SerializeObject(DT));
        Response.End();
    }

  
}