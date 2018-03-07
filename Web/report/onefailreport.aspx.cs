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
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

public partial class report_onefailreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String id = Request.Params["id"];
            if (!String.IsNullOrEmpty(id))
            {
                BLL_FailReport bll = new BLL_FailReport();
                DataTable dt = new DataTable();
                sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
                if (sysBaseLine.IsActive == 0)
                {
                    dt = bll.GetFailInfo("biz_all_failreport", id);
                }
                else
                {
                    dt = bll.GetFailInfo("v_invalid_document", id);
                }

                lbl_biaoduan.Text = dt.Rows[0]["SectionName"].ToString();
                lbl_danwei.Text = dt.Rows[0]["CompanyName"].ToString();
                lbl_shiyanshi.Text = dt.Rows[0]["TestRoomName"].ToString();
                lbl_shiyanbaogao.Text = dt.Rows[0]["ReportName"].ToString();
                lbl_baogaobianhao.Text = dt.Rows[0]["ReportNumber"].ToString();
                lbl_baogaoriqi.Text = dt.Rows[0]["ReportDate"].ToString();
                string str = dt.Rows[0]["F_InvalidItem"].ToString();

                StringBuilder sb = new StringBuilder();

                String[] Items = str.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String Item in Items)
                {
                    String[] substrings = Item.Split(',');                  
                    if (substrings.Length > 0)
                    {
                        sb.Append(" <tr>");
                        for (int i = 0; i < substrings.Length; i++)
                        {
                            sb.Append("<td   style=\"text-align: center;\">" + substrings[i] + "</td>");
                        }
                        sb.Append(" </tr>");
                    }
                }
                Literal1.Text = sb.ToString();
                lbl_yuanyinfenxi.Text = dt.Rows[0]["SGComment"].ToString();
                lbl_jianliyijian.Text = dt.Rows[0]["JLComment"].ToString();
                lbl_chulijiegou.Text = dt.Rows[0]["DealResult"].ToString();
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }
                else
                {
                    DataTable dt1 = bll.GetImageInfo(id);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        StringBuilder sb1 = new StringBuilder();

                        foreach (DataRow item in dt1.Rows)
                        {

                            string pathname = item["ImgName"].ToString();
                            string fullPath = Server.MapPath("../content/ ") + pathname;
                            if (!File.Exists(fullPath))
                            {
                                object o = item["ImgContent"];
                                var _tempMemoryStream = new MemoryStream((byte[])o);
                                if (_tempMemoryStream.Length > 0)
                                {
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(_tempMemoryStream);
                                    img.Save(fullPath);
                                  
                                }
                                else
                                {
                                    _tempMemoryStream.Close();
                                    continue;
                                }
                                _tempMemoryStream.Close();
                            }
                            else
                            {

                            }
                            sb1.Append("<p><a class=\"group4\"  href=\"../content/" + pathname + "\" title=\"" + item["ImgRemark"].ToString() + "\">" + item["ImgName"].ToString() + "</a></p>");
                        }

                        Literal2.Text = sb1.ToString();

                    }
                }
            }
        }
    }

}