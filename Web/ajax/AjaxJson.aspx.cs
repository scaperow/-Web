using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using JZ.BLL;
using Yqun.Common.Encoder;

public partial class ajax_AjaxJson : BasePage
{

    #region JQGrid传递的参数

    public int PageSize
    {
        get
        {
            int pagesize = 10;
            if (Request["rows"] != null)
            {
                pagesize = int.Parse(Request["rows"]);
            }
            return pagesize;
        }
    }

    public int PageIndex
    {
        get
        {
            int pageindex = 1;
            if (Request["page"] != null)
            {
                pageindex = int.Parse(Request["page"]);
            }
            return pageindex;
        }
    }

    public string OrderField
    {
        get
        {
            try
            {
                return Request.QueryString["sidx"].ToString(); //也就是对应前台colModel中的index            
            }
            catch
            {
                return "id";   //这里默认用id，注意跟着自己的数据变动            
            }
        }
    }

    public string OrderType
    {
        get
        {
            try
            {
                return Request.QueryString["sord"].ToString();
            }
            catch
            {
                return "desc";
            }
        }
    }

    public string Search
    {
        get
        {
            try
            {
                //这里可以自行构造，jqGrid本身自带搜索功能，不过传递的是：searchField,searOper,searchString就像分别 "ID" , "equal", "5"
                return Request.QueryString["condition"].ToString();
            }
            catch
            {
                return "";
            }
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        string result = "";
        string sType = Request.Params["sType"];
        string sqlwhere = "";
        if (!String.IsNullOrEmpty(Request.Params["StartDate"]))
        {
            StartDate = Request.Params["StartDate"];
        }
        if (!String.IsNullOrEmpty(Request.Params["EndDate"]))
        {
            EndDate = Request.Params["EndDate"];
        }
        if (!string.IsNullOrEmpty(sType))
        {
            int records = 0, pageCount = 0;
            string json = "";
            switch (sType)
            {
                case "SendMSMList":

                    if (!String.IsNullOrEmpty(Request.Params["SelectName"]))
                    {
                        sqlwhere += " and  PersonName LIKE '%" + Request.Params["SelectName"].Trim() + "%'";
                    }
                    if (!String.IsNullOrEmpty(Request.Params["Tel"]))
                    {
                        sqlwhere += " and  CellPhone LIKE '%" + Request.Params["Tel"].Trim() + "%'";
                    }
                    if (!String.IsNullOrEmpty(SelectedTestRoomCodes))
                    {
                        sqlwhere += " and TestRoomCode in (" + SelectedTestRoomCodes + ") ";
                    }
                    json = SendMSMList("sys_sms_receiver", "*,case when IsActive='1' then '是' else '否' end as 'Active'", sqlwhere, "ID", 1, out pageCount, out records);
                    result = "{\"total\": \"" + pageCount + "\", \"page\": \"" + PageIndex.ToString() + "\", \"records\": \"" + records + "\", \"rows\" : " + json + "}";
                    break;
                case "DeleteSendMSMList":
                    result = DeleteSendMSMList(Request.Params["SID"]);
                    break;
                case "getTestRoomList":
                    result = getTestRoomList();
                    break;
                case "SaveSmsReceiver":
                    result = SaveSmsReceiver(
                        Request.Params["RID"],
                        Request.Params["uname"],
                        Request.Params["utel"],
                        Request.Params["active"],
                        Request.Params["temp"],
                        Request.Params["temp1"]);
                    break;
                case "UserManageList":
                    if (!String.IsNullOrEmpty(Request.Params["SelectName"]))
                    {
                        sqlwhere += " and  UserName LIKE '%" + Request.Params["SelectName"].Trim() + "%' and UserName !='张宏伟' ";
                    }
                    sqlwhere += "   and UserName !='张宏伟' ";
                    json = UserManageList("sys_bs_users",
                        " *,case when IsActive='1' then '是' else '否' end as 'Active','' as segment",
                        sqlwhere,
                        "ID",
                        1,
                        out pageCount,
                        out records);
                    result = "{\"total\": \"" + pageCount + "\", \"page\": \"" + PageIndex.ToString() + "\", \"records\": \"" + records + "\", \"rows\" : " + json + "}";
                    break;
                case "DeleteUserManageList":
                    result = DeleteUserManageList(Request.Params["UserName"]);
                    break;
                case "getGCList":
                    result = getGCList(Request.Params["uname"]);
                    break;
                case "SaveSysBsUsers":
                    result = SaveSysBsUsers(
                        Request.Params["RID"],
                        Request.Params["uname"],
                        Request.Params["upwd"],
                        Request.Params["truename"],
                        Request.Params["active"],
                        Request.Params["group"],
                        Request.Params["temp"]);
                    break;
                default:
                    break;
            }
        }
        Response.Write(result);
    }

    #region 短信配置
    private string SendMSMList(string name, string fileds, string sqlwhere, string key, int ftype, out int pageCount, out int records)
    {
        DataTable dt = DbHelperSQL.GetDataTablePager(name, fileds, sqlwhere, key, "ID", OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            return JsonConvert.SerializeObject(dt);
        }
        else
        {
            return "";
        }
    }

    private string DeleteSendMSMList(string SID)
    {
        string SQL = "delete from sys_sms_receiver where ID='" + SID+"'";
        return DbHelperSQL.ExecuteSql(SQL).ToString();
    }

    private string SaveSmsReceiver(string RID, string uname, string utel, string active, string temp, string temp1)
    {
        if (!string.IsNullOrEmpty(uname))
        {
            string SQL = "INSERT INTO sys_sms_receiver(ID,TestRoomCode,PersonName,TestRoom,CellPhone,IsActive)VALUES('{5}','{0}','{1}','{2}','{3}','{4}');";
            StringBuilder str = new StringBuilder();
            string[] roomCode = temp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] roomName = temp1.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < roomCode.Length; i++)
                str.Append(string.Format(SQL, roomCode[i], uname, roomName[i], utel, active, Guid.NewGuid().ToString()));
            int rowcount = DbHelperSQL.ExecuteSql(str.ToString());
            if (roomCode.Length.Equals(rowcount)) return "1";
            return "0";
        }
        else
        {
            //string SQL = "update sys_sms_receiver set CellPhone='{0}',IsActive='{1}' where ID='{2}'";
            //SQL = string.Format(SQL, utel, active, RID);
            //return DbHelperSQL.ExecuteSql(SQL).ToString();
            return "";
        }
    }

    private string getTestRoomList()
    {
        string SQL = "select 标段名称,标段编码,单位名称,COUNT(试验室编码) as [Count] from v_bs_codeName group by  标段名称,单位名称,标段编码 having COUNT(试验室编码)>0;";
        SQL += "select 标段名称,标段编码,单位名称,试验室名称,试验室编码 from v_bs_codeName";
        DataSet ds = DbHelperSQL.Query(SQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string ul_Start = "<ul>";
            string li_Start = "<li><input type=\"checkbox\" value=\"{0}\" onchange=\"jl_ChkChange(this)\"/><span>{1}-{2}</span>&nbsp;&nbsp;";
            string chkitem = "<input type=\"checkbox\" value=\"{0}\" name=\"group\"/><span>{1}</span>&nbsp;";
            string Li_End = "</li>";
            string ul_End = "</ul>";
            StringBuilder str = new StringBuilder();
            str.Append(ul_Start);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str.Append(string.Format(li_Start, ds.Tables[0].Rows[i]["标段编码"].ToString(), ds.Tables[0].Rows[i]["标段名称"].ToString(), ds.Tables[0].Rows[i]["单位名称"].ToString()));
                DataRow[] drs = ds.Tables[1].Select("标段名称='" + ds.Tables[0].Rows[i]["标段名称"].ToString() + "' and 单位名称='" + ds.Tables[0].Rows[i]["单位名称"].ToString() + "'");
                for (int j = 0; j < drs.Length; j++)
                    str.Append(string.Format(chkitem, drs[j]["试验室编码"].ToString(), drs[j]["试验室名称"].ToString()));
                str.Append(Li_End);
            }
            str.Append(ul_End);
            return str.ToString();
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region 用户管理
    private string UserManageList(string name, string fileds, string sqlwhere, string key, int ftype, out int pageCount, out int records)
    {
        DataTable dt = DbHelperSQL.GetDataTablePager(name, fileds, sqlwhere, key, "ID", OrderType, PageIndex, PageSize, out pageCount, out records);
        if (dt != null)
        {
            DataSet ds;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ds = DbHelperSQL.Query("SELECT top 1 segment FROM sys_users_testroom where username='" + dt.Rows[i]["username"].ToString() + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dt.Rows[i]["segment"] = ds.Tables[0].Rows[0][0].ToString();
                }
            }

            DataTable dtnew = new DataTable();
            dtnew = dt.Clone();

            foreach (DataRow oldDR in dt.Rows)
            {

                DataRow newDR = dtnew.NewRow();//新表创建新行
                newDR.ItemArray = oldDR.ItemArray;//旧表结构行赋给新表结构行
                oldDR["Password"] = EncryptSerivce.Dencrypt(oldDR["Password"].ToString());
                dtnew.ImportRow(oldDR);

            }
            return JsonConvert.SerializeObject(dtnew);
        }
        else
        {
            return "";
        }
    }

    private string SaveSysBsUsers(string RID, string uname, string upwd, string truename, string active, string group, string temp)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        //0:操作失败,1:操作成功,2:用户名重复.
        if (!string.IsNullOrEmpty(uname))
        {
            string SQL = "";
            #region 添加线路库的数据
            SQL = "SELECT * FROM dbo.sys_BaseUsers WHERE UserName='{0}'";
            DataSet sys_ds = LineDbHelperSQL.Query(string.Format(SQL, uname));//判断大库里面的use表
            SQL = "SELECT * FROM  dbo.sys_BaseLine_Users WHERE UserName='{0}' AND LineID='{1}'";
            DataSet sys_line_ds = LineDbHelperSQL.Query(string.Format(SQL, uname, sysBaseLine.ID));//判断大库里面的Line权限
            SQL = "SELECT id FROM sys_bs_users where UserName='{0}';";
            DataSet ds = DbHelperSQL.Query(string.Format(SQL, uname));
            if (ds != null && ds.Tables[0].Rows.Count == 0)
            {
                if (sys_ds != null && sys_ds.Tables[0].Rows.Count == 0)
                {
                    #region

                    SQL = @"INSERT dbo.sys_BaseUsers
                                                                        ( UserName ,
                                                                          Password ,
                                                                          IsActive ,
                                                                          TrueName ,
                                                                          LineID ,
                                                                          Descrption ,
                                                                          RoleName
                                                                        )VALUES  ( '{0}' ,
                                                                          '{1}' ,
                                                                          '{2}' ,
                                                                          '{3}' ,
                                                                         '{4}' , 
                                                                         '{5}' , 
                                                                         '{6}'
                                                                        )";
                    LineDbHelperSQL.ExecuteSql(string.Format(SQL, uname, EncryptSerivce.Encrypt(upwd), active, truename, sysBaseLine.ID, sysBaseLine.LineName, group));


                    SQL = @"INSERT dbo.sys_BaseLine_Users
                                                                ( UserName, LineID)
                                                        VALUES  ( '{0}', 
                                                                  '{1}'
                                                                  )";
                    LineDbHelperSQL.ExecuteSql(string.Format(SQL, uname, sysBaseLine.ID));
                    #endregion
                }
                else
                {
                    #region
                    //if ((group == "S" && ((sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "S") || (sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "SS"))) || (group == "SS" && ((sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "S") || (sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "SS"))))
                    if ((group == "S"||group=="SS"||group=="X"||group=="A")&&((sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "S") ||(sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "SS") ||(sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "X") ||(sys_ds.Tables[0].Rows[0]["RoleName"].ToString() == "A") ))
                    {
                        SQL = @"INSERT dbo.sys_BaseLine_Users
                                                                ( UserName, LineID )
                                                        VALUES  ( '{0}', 
                                                                  '{1}'
                                                                  )";
                        LineDbHelperSQL.ExecuteSql(string.Format(SQL, uname, sysBaseLine.ID));
                    }
                    else
                    {
                        return "2";
                    }
                    #endregion
                }
                SQL = "INSERT INTO sys_bs_users(UserName,Password,IsActive,TrueName)VALUES('{0}','{1}','{2}','{3}');";
                int userCount = DbHelperSQL.ExecuteSql(string.Format(SQL, uname, EncryptSerivce.Encrypt(upwd), active, truename));
                if (userCount > 0)
                {
                    StringBuilder str = new StringBuilder();
                    SQL = "INSERT INTO sys_users_testroom(username,testroomcode,segment)VALUES('{0}','{1}','{2}');";
                    string[] roomCode = temp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < roomCode.Length; i++) str.Append(string.Format(SQL, uname, roomCode[i], group));
                    DbHelperSQL.ExecuteSql(str.ToString());
                    return "1";
                }
                return "0";
            }
            else
            {
                SQL = "UPDATE sys_bs_users set Password='{0}',IsActive='{1}',TrueName='{2}' where UserName='{3}';";
                int userCount = DbHelperSQL.ExecuteSql(string.Format(SQL, EncryptSerivce.Encrypt(upwd), active, truename, uname));
                if (userCount > 0)
                {
                    SQL = "delete from sys_users_testroom where username='{0}';";
                    DbHelperSQL.ExecuteSql(string.Format(SQL, uname));
                    StringBuilder str = new StringBuilder();
                    SQL = "INSERT INTO sys_users_testroom(username,testroomcode,segment)VALUES('{0}','{1}','{2}');";
                    string[] roomCode = temp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < roomCode.Length; i++) str.Append(string.Format(SQL, uname, roomCode[i], group));
                    DbHelperSQL.ExecuteSql(str.ToString());

                    #region 修改大库的密码
                    SQL = "SELECT * FROM dbo.sys_BaseUsers WHERE UserName='{0}' ";
                    DataSet sys_ds_edit = LineDbHelperSQL.Query(string.Format(SQL, uname));//判断大库里面的use表
                    if (sys_ds_edit != null && sys_ds_edit.Tables.Count > 0)
                    {
                        SQL = @"UPDATE dbo.sys_BaseUsers SET  Password='{0}' WHERE UserName='{1}'";
                        LineDbHelperSQL.ExecuteSql(string.Format(SQL, EncryptSerivce.Encrypt(upwd), uname));
                    }
                    #endregion
                    return "1";
                }
                return "0";
            }
            #endregion
        }
        else
        {
            return "0";
        }
    }

    private string DeleteUserManageList(string UserName)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        try
        {
            string SQL = "delete from sys_bs_users where username='" + UserName + "';delete from sys_users_testroom where username='" + UserName + "'";
            DbHelperSQL.ExecuteSql(SQL).ToString();
            #region
            SQL = "DELETE dbo.sys_BaseLine_Users WHERE UserName='{0}'  AND LineID='{1}'";
            LineDbHelperSQL.ExecuteSql(string.Format(SQL, UserName, sysBaseLine.ID));
            SQL = "SELECT * FROM  dbo.sys_BaseLine_Users WHERE UserName='{0}'";
            DataSet ds = LineDbHelperSQL.Query(string.Format(SQL, UserName));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                SQL = "SELECT * FROM dbo.sys_BaseUsers WHERE  UserName='{0}'";
                DataSet ds1 = LineDbHelperSQL.Query(string.Format(SQL, UserName));
                if (ds1!=null&&ds1.Tables[0].Rows.Count>0)
                {
                    SQL = "SELECT * FROM  dbo.sys_BaseLine_Users WHERE LineID='{0}' and UserName='{1}'";
                    DataSet ds2 = LineDbHelperSQL.Query(string.Format(SQL, ds1.Tables[0].Rows[0]["LineID"].ToString(), UserName));
                    if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        //此处修改用户表的默认登录库，因为一旦删除了线路表里面的数据，用户登录以后找不到这个线路会报错
                        SQL = "UPDATE dbo.sys_BaseUsers SET LineID='{0}' ,Descrption='{1}' WHERE UserName='{2}'";
                        LineDbHelperSQL.ExecuteSql(string.Format(SQL, ds.Tables[0].Rows[0]["LineID"].ToString(), "",UserName));
                    }
                }
            }
            else
            {
                SQL = "DELETE dbo.sys_BaseUsers WHERE UserName='{0}'";
                LineDbHelperSQL.ExecuteSql(string.Format(SQL, UserName));
            }
            #endregion
            return "1";
        }
        catch
        {
            return "0";
        }
    }

    private string getGCList(string uname)
    {
        string SQL = "select 工程编码,工程名称 from v_bs_codeName group by  工程编码,工程名称 having COUNT(工程编码)>0;";
        SQL += "select 标段名称,标段编码,单位名称,COUNT(试验室编码) as [Count] from v_bs_codeName group by  标段名称,单位名称,标段编码 having COUNT(试验室编码)>0;";
        SQL += "select 标段名称,标段编码,单位名称,试验室名称,试验室编码 from v_bs_codeName;";
        string temp = ",";
        DataSet ds = DbHelperSQL.Query(SQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(uname))
            {
                SQL = "select testroomcode from sys_users_testroom where username='{0}'";
                DataSet Data = DbHelperSQL.Query(string.Format(SQL, uname));
                if (Data != null && Data.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < Data.Tables[0].Rows.Count; i++)
                    {
                        temp += Data.Tables[0].Rows[i][0].ToString() + ",";
                    }
                }
            }
            string ul_Start = "<ul>";
            string li_Start = "<li><input type=\"checkbox\" value=\"{0}\"/><span>{1}-{2}</span>&nbsp;&nbsp;";
            string li_Start_Chked = "<li><input type=\"checkbox\" value=\"{0}\" checked=\"true\"/><span>{1}-{2}</span>&nbsp;&nbsp;";
            string chkitem = "<input type=\"checkbox\" value=\"{0}\"/><span>{1}</span>&nbsp;";
            string chkitem_Chked = "<input type=\"checkbox\" value=\"{0}\" checked=\"true\"/><span>{1}</span>&nbsp;";
            string chkGCitem = "<li><input type=\"checkbox\" value=\"{0}\"/><span>{1}</span>&nbsp;&nbsp;</li>";
            string chkGCitem_Chked = "<li><input type=\"checkbox\" value=\"{0}\" checked=\"true\"/><span>{1}</span>&nbsp;&nbsp;</li>";
            string Li_End = "</li>";
            string ul_End = "</ul>";
            StringBuilder str = new StringBuilder();
            str.Append(ul_Start);
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                if (temp.IndexOf("," + ds.Tables[0].Rows[k]["工程编码"].ToString() + ",") > -1)
                    str.Append(string.Format(chkGCitem_Chked, ds.Tables[0].Rows[k]["工程编码"].ToString(), ds.Tables[0].Rows[k]["工程名称"].ToString()));
                else
                    str.Append(string.Format(chkGCitem, ds.Tables[0].Rows[k]["工程编码"].ToString(), ds.Tables[0].Rows[k]["工程名称"].ToString()));
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    if (temp.IndexOf("," + ds.Tables[1].Rows[i]["标段编码"].ToString() + ",") > -1)
                        str.Append(string.Format(li_Start_Chked, ds.Tables[1].Rows[i]["标段编码"].ToString(), ds.Tables[1].Rows[i]["标段名称"].ToString(), ds.Tables[1].Rows[i]["单位名称"].ToString()));
                    else
                        str.Append(string.Format(li_Start, ds.Tables[1].Rows[i]["标段编码"].ToString(), ds.Tables[1].Rows[i]["标段名称"].ToString(), ds.Tables[1].Rows[i]["单位名称"].ToString()));
                    DataRow[] drs = ds.Tables[2].Select("标段名称='" + ds.Tables[1].Rows[i]["标段名称"].ToString() + "' and 单位名称='" + ds.Tables[1].Rows[i]["单位名称"].ToString() + "'");
                    for (int j = 0; j < drs.Length; j++)
                    {
                        if (temp.IndexOf("," + drs[j]["试验室编码"].ToString() + ",") > -1)
                            str.Append(string.Format(chkitem_Chked, drs[j]["试验室编码"].ToString(), drs[j]["试验室名称"].ToString()));
                        else
                            str.Append(string.Format(chkitem, drs[j]["试验室编码"].ToString(), drs[j]["试验室名称"].ToString()));
                    }
                    str.Append(Li_End);
                }
            }
            str.Append(ul_End);
            return str.ToString();
        }
        else
        {
            return "";
        }
    }
    #endregion
}