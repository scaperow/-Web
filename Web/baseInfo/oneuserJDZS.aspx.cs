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

public partial class baseInfo_oneuserJDZS :  BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String id = Request.Params["id"];
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
                                string[] Temp = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(JZCommonHelper.GetCellValue(doc,new Guid(Request.Params["sid"].ToString()), "K" + "i".RequestStr() + "").ToString());

                               // System.Drawing.Image img = BizCommon.JZCommonHelper.StringToBitmap(Temp[1]);

                                Response.Write("<img src='data:image/gif;base64," + Temp[1] + "'  />");
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