using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using JZ.BLL;

public partial class main : System.Web.UI.MasterPage
{
    public String StartDate
    {
        get
        {
            if (Session["StartDate"] == null)
            {
                Session["StartDate"] = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            }
            return Session["StartDate"].ToString();
        }
        set
        {
            Session["StartDate"] = value;
        }
    }

    public String EndDate
    {
        get
        {
            if (Session["EndDate"] == null)
            {
                Session["EndDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return Session["EndDate"].ToString();
        }
        set
        {
            Session["EndDate"] = value;
        }
    }

    public String UserName
    {
        get
        {
            return Session["UserName"].ToString();
        }
    }

    public String Companyfilter
    {
        get
        {
            if (Session["Companyfilter"] == null)
            {
                Session["Companyfilter"] = "全部单位";
            }
            return Session["Companyfilter"].ToString();
        }
    }

    public String Testroomfilter
    {
        get
        {
            if (Session["Testroomfilter"] == null)
            {
                Session["Testroomfilter"] = "全部试验室";
            }
            return Session["Testroomfilter"].ToString();
        }
    }

    public int i = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            i = 0;
            if (Session["SysBaseLine"] != null)
            {
                sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
                Label1.Text = BasePage.CutString(sysBaseLine.LineName, 18);
            }
            if (Session["UserName"] != null)
            {
                string Line = "'" + ((ResultInfo)Session["UserInfo"]).Line.Replace(",", "','") + "'";
                Line = Line == "''" ? ((ResultInfo)Session["UserInfo"]).LineID : Line;
                string strSql = @"select 
                  ID ,
                  LineName ,
                  Description ,
                  DataSourceAddress ,
                  UserName ,
                  PassWord ,
                  DataBaseName FROM sys_line WHERE ID in(" + Line + ")";
                DataSet ds = LineDbHelperSQL.Query(strSql);

               
                this.rp_list.DataSource = ds;
                this.rp_list.DataBind();
            }
        }
    }
    protected void lb_logout_Click(object sender, EventArgs e)
    {
        if (Session["DoMain"] == "xian")
        {

            Session.Clear();
            Response.Redirect("~/xian/login.aspx");
        }
        if (Session["DoMain"] == "shanghai")
        {

            Session.Clear();
            Response.Redirect("~/shanghai/login.aspx");
        }
        Session.Clear();
        Response.Redirect("~/login.aspx");
    }
    protected void rp_list_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "change")
        {
            string str = e.CommandArgument.ToString();
            string strSql = @"SELECT  
              ID ,
              LineName ,
              Description ,
              DataSourceAddress ,
              UserName ,
              PassWord ,
              DataBaseName,IsActive FROM dbo.sys_Line WHERE ID='" + str + "' ";
            DataSet ds = LineDbHelperSQL.Query(strSql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                BLL_Login bll = new BLL_Login(ds.Tables[0].Rows[0]["ID"].ToString());
                bll.CreateProjectModel_BaseSys(ds.Tables[0].Rows[0]["ID"].ToString());
                Label1.Text = BasePage.CutString(ds.Tables[0].Rows[0]["LineName"].ToString(), 18);

                Session["leftTree"] = "";
                Session["SelectedTestRoomCodes"] = "";

                ResultInfo RI =(ResultInfo)Session["UserInfo"];

                RI.LineID = ds.Tables[0].Rows[0]["ID"].ToString();

                Session["UserInfo"] = RI;

                new BLL_Login("BaseSystem").CallService("SetUserLineID", "UserID="+RI.ID+"&LineID="+RI.LineID+"&Description=" + ds.Tables[0].Rows[0]["Description"].ToString());

            }
        }
    }

}
