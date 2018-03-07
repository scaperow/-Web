using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using JZ.BLL;

public partial class XianLogin_Login : System.Web.UI.Page
{
    protected void btnLogin_Click(object sender, EventArgs e)
    {


        string flag = "";
        string UserName = txtUsername.Value.Trim();
        string Password = txtPass.Value.Trim();

        BLL_Login bll = new BLL_Login("BaseSystem");
        Session["UserName"] = UserName;
        flag = bll.CheckUser_BaseSys(UserName, Password);
        if (flag == "true")
        {
            Session["DoMain"] = "sys";
            bll = new BLL_Login("BaseSystem");
            Response.Redirect("indexMap.aspx");
        }
        else
        {
            Label1.Text = flag;
        }


    }
    protected void btnResert_Click(object sender, EventArgs e)
    {
        txtUsername.Value = "";
        txtPass.Value = "";

    }
    protected void Page_Load(object sender, EventArgs e)
    {



        if (Session["SysBaseLine"] != null)
        {
            Response.Redirect("../indexMap.aspx");
        }
    }


    /// <summary>
    /// 获取地址栏中的二级域名 
    /// </summary>
    /// <returns></returns>
    public string GetUrlDomainName()
    {

        string HostName = HttpContext.Current.Request.Url.Host;

        string[] UserHost = HostName.Split(new[] { '.' });

        string strDomainName = UserHost[0];

        if (String.IsNullOrEmpty(strDomainName))
        {
            strDomainName = "www";
        }
        return strDomainName.ToLower();
    }
}