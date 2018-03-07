using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using Newtonsoft.Json;

public partial class ajax_ajaxDefault : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String result = "";
        String sType = Request.Params["sType"];
        if (!String.IsNullOrEmpty(sType))
        {
            switch (sType)
            {
                case "baseSummaryCol":
                    result = GetBaseSummaryColumnData();
                    break;
                default:
                    break;
            }
        }

        Response.Write(result);
    }

    private String GetBaseSummaryColumnData()
    {
        String sModel = Request.Params["sModel"];
        JQGridColumnData data = new JQGridColumnData()
        {
            ColumnNames = new String[] { "标段", "单位", "试验室", "试验名称", "数量" },
            ColumnModels = new List<JQGridColumnModel>()
        };
        data.ColumnModels.Add(new JQGridColumnModel()
        {
            name = "col_norm_d4",
            index = "col_norm_d4",
            width = 200,
            //colspan = true
        });
        data.ColumnModels.Add(new JQGridColumnModel()
        {
            name = "col_norm_d5",
            index = "col_norm_d5",
            width = 200,
            //colspan = true
        });
        data.ColumnModels.Add(new JQGridColumnModel()
        {
            name = "col_norm_d6",
            index = "col_norm_d6",
            width = 200,
            //colspan = false
        });
        data.ColumnModels.Add(new JQGridColumnModel()
        {
            name = "col_norm_d7",
            index = "col_norm_d7",
            width = 200,
            //colspan = false
        });
        data.ColumnModels.Add(new JQGridColumnModel()
        {
            name = "col_norm_d8",
            index = "col_norm_d8",
            width = 60,
            //colspan = false
        });
        return JsonConvert.SerializeObject(data);
    }
}