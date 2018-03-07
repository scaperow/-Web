using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using Yqun.Common.Encoder;

public partial class sys_password :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        if (Session["UserName"] != null)
        {
            lbl_username.Text = Session["UserName"].ToString();
            DataSet ds = LineDbHelperSQL.Query("SELECT * FROM sys_User WHERE UserName='" + Session["UserName"].ToString() + "'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lbl_old.Text = EncryptSerivce.Dencrypt(ds.Tables[0].Rows[0]["Password"].ToString());
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int n = LineDbHelperSQL.ExecuteSql("UPDATE sys_User SET Password='" + EncryptSerivce.Encrypt(txt_new.Text.Trim()) + "' WHERE UserName='" + lbl_username.Text.Trim() + "'");
        if (n > 0)
        {
            Label1.Text = "密码修改成功!";
        }
        Bind();
    }
}