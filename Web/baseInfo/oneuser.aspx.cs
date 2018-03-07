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


public partial class baseInfo_oneuser : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String id = Request.Params["id"];
            if (!String.IsNullOrEmpty(id))
            {
                sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
                if (sysBaseLine.IsActive == 0)
                {
                    #region
                    BLL_UserInfo bll = new BLL_UserInfo();
                    DataTable dt = bll.GetUserInfo(id);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lbl_xingming.Text = dt.Rows[0]["col_norm_D6"].ToString();
                        lbl_xingbie.Text = dt.Rows[0]["col_norm_G6"].ToString();
                        lbl_nianling.Text = dt.Rows[0]["col_norm_K6"].ToString();
                        lbl_jishuzhicheng.Text = dt.Rows[0]["col_norm_D7"].ToString();
                        lbl_zhiwu.Text = dt.Rows[0]["col_norm_G7"].ToString();
                        lbl_gongzuonianxian.Text = dt.Rows[0]["col_norm_K7"].ToString();
                        lbl_zhuanye.Text = dt.Rows[0]["col_norm_D8"].ToString();
                        lbl_xueli.Text = dt.Rows[0]["col_norm_G8"].ToString();


                        try
                        {
                            lbl_biyeshijian.Text = Convert.ToDateTime(dt.Rows[0]["col_norm_K8"].ToString()).ToShortDateString();
                        }
                        catch
                        {

                            lbl_biyeshijian.Text = dt.Rows[0]["col_norm_K8"].ToString();
                        }

                        lbl_lianxidianhua.Text = dt.Rows[0]["col_norm_D9"].ToString();
                        lbl_biyexuexiao.Text = dt.Rows[0]["col_norm_G9"].ToString();

                        //dt.Rows[0]["col_norm_L6"].ToString()

                        #region
                        StringBuilder sb1 = new StringBuilder();
                        if (!string.IsNullOrEmpty(dt.Rows[0]["col_norm_L6"].ToString()))
                        {
                            string pathname = dt.Rows[0]["ID"].ToString();
                            string fullPath = Server.MapPath("../userphoto/ ") + pathname + ".jpg";
                            if (!File.Exists(fullPath))
                            {
                                object o = dt.Rows[0]["col_norm_L6"];
                                var _tempMemoryStream = new MemoryStream((byte[])o);
                                System.Drawing.Image img = System.Drawing.Image.FromStream(_tempMemoryStream);
                                img.Save(fullPath);
                                _tempMemoryStream.Close();
                            }
                            else
                            {

                            }
                            sb1.Append("<img  src=\"../userphoto/" + pathname + ".jpg\" />");
                        }
                        else
                        {
                            sb1.Append("<img  src=\"../images/nohead.png\" />");
                        }
                        Literal3.Text = sb1.ToString();
                        #endregion


                        #region
                        if (!String.IsNullOrEmpty(dt.Rows[0]["col_norm_B14"].ToString()))
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 14; i < 28; i++)
                            {
                                if (!String.IsNullOrEmpty(dt.Rows[0]["col_norm_B" + i + ""].ToString()))
                                {
                                    sb.Append("<tr><td colspan=\"2\">" + dt.Rows[0]["col_norm_B" + i + ""].ToString() + "</td>");
                                    sb.Append("<td colspan=\"3\">" + dt.Rows[0]["col_norm_E" + i + ""].ToString() + "</td>");
                                    sb.Append("<td>" + dt.Rows[0]["col_norm_K" + i + ""].ToString() + "</td>");
                                    sb.Append("<td>" + dt.Rows[0]["col_norm_M" + i + ""].ToString() + "</td></tr>");
                                }
                            }
                            Literal1.Text = sb.ToString();
                        }
                        else
                        {
                            tr_gzjl.Style["display"] = "none";
                            tr_gzjl_mx.Style["display"] = "none";
                        }

                        if (!String.IsNullOrEmpty(dt.Rows[0]["col_norm_B31"].ToString()))
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 31; i < 37; i++)
                            {
                                if (!String.IsNullOrEmpty(dt.Rows[0]["col_norm_B" + i + ""].ToString()))
                                {
                                    sb.Append("<tr><td colspan=\"2\">" + dt.Rows[0]["col_norm_B" + i + ""].ToString() + "</td>");
                                    sb.Append("<td colspan=\"4\">" + dt.Rows[0]["col_norm_E" + i + ""].ToString() + "</td>");


                                    try
                                    {

                                        string[] Temp = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(dt.Rows[0]["col_norm_K" + i + ""].ToString());

                                        sb.Append("<td>" + Temp[0] + "<img src='data:image/gif;base64," + Temp[1] + "' /></td></tr>");
                                    }
                                    catch
                                    {
                                        sb.Append("<td>" + dt.Rows[0]["col_norm_K" + i + ""].ToString() + "' /></td></tr>");
                                    }
                                  
                                }
                            }
                            Literal2.Text = sb.ToString();
                        }
                        else
                        {
                            tr_pxjl.Style["display"] = "none";
                            tr_pxjl_mx.Style["display"] = "none";
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    string SQL = "SELECT * FROM dbo.sys_document WHERE ID='" + id + "'";

                    DataSet ds = DbHelperSQL.Query(SQL);
                    string str = ds.Tables[0].Rows[0]["Data"].ToString();
                    JZDocument doc = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(str);
                    if (doc != null)
                    {
                     
                        #region
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D6") != null)
                        {
                            lbl_xingming.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D6").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G6") != null)
                        {
                            lbl_xingbie.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G6").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K6") != null)
                        {
                            lbl_nianling.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K6").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D7") != null)
                        {
                            lbl_jishuzhicheng.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D7").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G7") != null)
                        {
                            lbl_zhiwu.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G7").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K7") != null)
                        {
                            lbl_gongzuonianxian.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K7").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D8") != null)
                        {
                            lbl_zhuanye.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D8").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G8") != null)
                        {
                            lbl_xueli.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G8").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K8") != null)
                        {
                            lbl_biyeshijian.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K8").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D9") != null)
                        {
                            lbl_lianxidianhua.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D9").ToString();
                        }
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G9") != null)
                        {
                            lbl_biyexuexiao.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G9").ToString();
                        }
                         if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D10") != null)
                        {
                            lbl_idcard.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D10").ToString();
                        }
                         if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D11") != null)
                         {
                             lbl_dgrq.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D11").ToString();
                         }
                         if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K11") != null)
                         {
                             lbl_lgrq.Text = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K11").ToString();
                         }
                        #endregion

                        #region
                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B14") != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 14; i < 28; i++)
                            {
                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B" + i + "") != null)
                                {
                                    sb.Append("<tr><td colspan=\"2\">" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B" + i + "").ToString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<tr><td colspan=\"2\"></td>");
                                }
                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "E" + i + "") != null)
                                {

                                    sb.Append("<td colspan=\"3\">" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "E" + i + "").ToString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td colspan=\"3\"></td>");
                                }

                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i + "") != null)
                                {

                                    sb.Append("<td>" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i + "").ToString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td></td>");
                                }

                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "M" + i + "") != null)
                                {

                                    sb.Append("<td>" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "M" + i + "").ToString() + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td></td></tr>");
                                }
                            }
                            Literal1.Text = sb.ToString();
                        }
                        else
                        {
                            tr_gzjl.Style["display"] = "none";
                            tr_gzjl_mx.Style["display"] = "none";
                        }


                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B31") != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 31; i < 37; i++)
                            {
                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B" + i + "") != null)
                                {
                                    sb.Append("<tr><td colspan=\"2\">" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B" + i + "").ToString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<tr><td colspan=\"2\"></td>");
                                }
                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "E" + i + "") != null)
                                {

                                    sb.Append("<td colspan=\"4\">" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "E" + i + "").ToString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td colspan=\"4\"></td>");
                                }

                                if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i + "") != null)
                                {
                                    try
                                    {
                                        string[] Temp = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i + "").ToString());

                                        System.Drawing.Image  img = BizCommon.JZCommonHelper.StringToBitmap(Temp[1]);

                                        sb.Append("<td>" + Temp[0] + "&nbsp;&nbsp;&nbsp;&nbsp;<a title=\"点击查看大图\" href=\"javascript:Show('" + id + "','" + doc.Sheets[0].ID.ToString() + "',"+i+")\"><img src='data:image/gif;base64," + Temp[1] + "' height=\"30px;\" /></a></td>");
                                    }
                                    catch
                                    {
                                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i + "").ToString() != "[null,null,null]")
                                        {
                                            sb.Append("<td>" + JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i + "").ToString() + "</td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td></td>");
                                            
                                        }
                                    }
                                }
                                else
                                {
                                    sb.Append("<td></td>");
                                }
                            }
                            Literal2.Text = sb.ToString();
                        }
                        else
                        {
                            tr_pxjl.Style["display"] = "none";
                            tr_pxjl_mx.Style["display"] = "none";
                        }

                        #endregion

                        #region
                        StringBuilder sb1 = new StringBuilder();


                        if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "L6") != null)
                        {
                            string pathname = id;
                            string fullPath = Server.MapPath("../userphoto/ ") + pathname + ".jpg";
                            if (!File.Exists(fullPath))
                            {
                                object o = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "L6");
                                System.Drawing.Image img = null;
                                byte[] bitmapBytes = System.Convert.FromBase64String(o.ToString());
                                using (MemoryStream memoryStream = new MemoryStream(bitmapBytes))
                                {
                                    img = System.Drawing.Image.FromStream(memoryStream);
                                    img.Save(fullPath);
                                    memoryStream.Close();  
                                }
                            }
                            //else
                            //{
                            //    sb1.Append("<img width=\"130px\" height=\"165px\" src=\"../images/nohead.png\" />");
                            //}
                            sb1.Append("<img  src=\"../userphoto/" + pathname + ".jpg\" />");
                        }
                        else
                        {
                            sb1.Append("<img  src=\"../images/nohead.png\" />");
                        }
                        Literal3.Text = sb1.ToString();
                        #endregion
                    }
                }
            }
        }



    }


    /// <summary>
    /// 字节数组转换成图片
    /// </summary>
    /// <param name="streamByte"></param>
    /// <returns></returns>
    private System.Drawing.Image ReturnPhoto(byte[] streamByte)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        return img;
    }
    /// <summary>
    /// 字符串转化成字节数组
    /// </summary>
    /// <param name="condtion"></param>
    /// <returns></returns>
    private byte[] stringtoByte(string condtion)
    {
        byte[] buffer = Convert.FromBase64String(condtion);//拿到byte[]，你可以为所欲为了
        return buffer; //类型转换很重要
    }



}