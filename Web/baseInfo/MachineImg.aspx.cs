using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using System.Text;
using System.IO;
using BizCommon;
using System.Runtime.Serialization.Formatters.Binary;

public partial class baseInfo_MachineImg : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String id = Request.Params["ID"];
            if (!String.IsNullOrEmpty(id))
            {
                string SQL = "SELECT * FROM dbo.sys_document WHERE ID='" + id + "'";

                DataSet ds = DbHelperSQL.Query(SQL);
                string str = ds.Tables[0].Rows[0]["Data"].ToString();
                JZDocument doc = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(str);
                if (doc != null)
                {
                    
                        try
                        {
                            string Temp = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "AI".RequestStr()).ToString();

                            Response.Write("<img src='data:image/gif;base64," + Temp + "'  />");

                        }
                        catch
                        {
                            Response.Write("<div style=\"width:400px;height:300px;line-height:300px; text-align:center;\">无图片或图片有误</div>");
                        }
                   
                }
            }
        }
    }
}