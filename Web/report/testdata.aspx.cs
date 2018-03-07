using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using JZ.BLL;
using Newtonsoft.Json;

public partial class report_testdata :BasePage
{
    public string _ID = string.Empty;
    public string _DeviceType = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["DeviceType"] != null)
        {
            _DeviceType = Request["DeviceType"].ToString() ;
            _DeviceType = _DeviceType == "1" ? "块" : (_DeviceType == "2" ? "根" : "");
        }
    }

    public string GetCount()
    {
        string Result = "0";
        BLL_Document BLL = new BLL_Document();
        DataTable _Data = BLL.GetDataTable("SELECT DataID FROM dbo.sys_test_data WHERE DataID='" + Request["id"].ToString() + "' AND Status=1 order by SerialNumber asc");
        Result = _Data.Rows.Count.ToString();
        return Result;
    }

    
}