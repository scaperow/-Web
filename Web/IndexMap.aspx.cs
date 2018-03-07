using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using JZ.BLL.模型;
using Newtonsoft.Json;

[Serializable]
public partial class LineMapEnt
{
    public string TagX = "0";
    public string TagY = "0";
    public string MapCenterX = "0";
    public string MapCenterY = "0";
    public string ZoomLevel = "0";
    public string TitleImg = "";
}

public partial class IndexMap : System.Web.UI.Page
{
    #region 参数
    public string Lines = string.Empty;

    protected string TitleImg = string.Empty;
    protected string Markers1;
    protected string AddOverlays;
    public String UserName
    {
        get
        {
            return Session["UserName"].ToString();
        }
    }

    public String MachineID
    {
        get
        {
            if (Session["MachineID"] == null)
            {
                Session["MachineID"] = "";
            }
            return Session["MachineID"].ToString();
        }
    }

    public String MachineName
    {
        get
        {
            if (Session["MachineName"] == null)
            {
                Session["MachineName"] = "当前监理下所有拌和站";
            }
            return Session["MachineName"].ToString();
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //UpDataUserLine();
        PostHandler();
        InitLinesSites();
        SiteMap();
        if (Session["SysBaseLine"] != null)
        {
            sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
            //Label1.Text = BasePage.CutString(sysBaseLine.LineName, 18);
        }
    }

    public LineMapEnt Site;
    public void SiteMap()
    {
        string DoMain = Session["DoMain"] ==null? "sys":Session["DoMain"].ToString();
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        string Sql = " Select * from Domain_Line_Map where DoMain ='" + DoMain + "'";

        DataSet Ds = LineDbHelperSQL.Query(Sql);

         Site = Newtonsoft.Json.JsonConvert.DeserializeObject<LineMapEnt>(Ds.Tables[0].Rows[0]["Map"].ToString());
    }

    private void UpDataUserLine()
    {
        Session["leftTree"] = "";
        Session["SelectedTestRoomCodes"] = "";

        string strSql = @"SELECT 
          ID ,
          LineName ,
          Description ,
          DataSourceAddress ,
          UserName ,
          PassWord ,
          DataBaseName,IsActive FROM dbo.sys_Line WHERE ID='" + "LineID".RequestStr() + "' AND  ISBHZ in (2,3) ";

        DataSet ds = LineDbHelperSQL.Query(strSql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            BLL_Login bll = new BLL_Login(ds.Tables[0].Rows[0]["ID"].ToString());
            bll.CreateProjectModel_BaseSys(ds.Tables[0].Rows[0]["ID"].ToString());
            Session["leftTree"] = "";
            Session["SelectedTestRoomCodes"] = "";

            ResultInfo RI = (ResultInfo)Session["UserInfo"];
            RI.LineID = ds.Tables[0].Rows[0]["ID"].ToString();
            Session["UserInfo"] = RI;
            new BLL_Login("BaseSystem").CallService("SetUserLineID", "UserID=" + RI.ID + "&LineID=" + RI.LineID + "&Description=" + ds.Tables[0].Rows[0]["Description"].ToString());
        }

    }

    /// <summary>
    /// 获取所有线路站点JSON
    /// </summary>
    public void InitLinesSites()
    {
        DataSet Ds = GetUserLines("");
        string _temp = string.Empty;
        StringBuilder LinesB = new StringBuilder();

        LinesB.Append("{Lines:[");
        int i = 0;
        foreach (DataRow Dr in Ds.Tables[0].Rows)
        {
            MapData dtMap = JsonConvert.DeserializeObject<MapData>(Dr["TestMapJson"].ToString());
            if (dtMap == null)
            {
                continue;
            }
            LinesB.Append("{\"LineID\":\"" + Dr["LineID"].ToString() + "\",\"LineName\":\"" + Dr["LineName"].ToString() + "\"");
          
            #region 线路坐标

            LinesB.Append(",\"X\":\"" + dtMap.MapCenterX + "\",\"Y\":\"" + dtMap.MapCenterY + "\"");
            LinesB.Append(",\"TagX\":\"" + dtMap.TagX + "\",\"TagY\":\"" + dtMap.TagY + "\"");
            LinesB.Append(",\"ZoomLevel\":\"" + dtMap.ZoomLevel + "\",\"TitleImg\":\"" + dtMap.TitleImg + "\"");

            #endregion
            LinesB.Append("}");
            LinesB.Append(i < Ds.Tables[0].Rows.Count - 1 ? "," : "");
            i++;
        }
        LinesB.Append("]}");

        Lines = LinesB.ToString();
    }

    /// <summary>
    /// 请求控制
    /// </summary>
    private void PostHandler()
    {
        if ("ACT".IsRequest())
        {
            Response.Clear();

            switch ("ACT".RequestStr())
            {
                case "UPDATEUSERLINE":
                    UpDataUserLine();
                    break;
                case "SetLine":
                    SetLine("Name".RequestStr(), "Type".RequestStr());
                    break;
                case "GetItems":
                    GetItems();
                    break;
            }

            Response.End();
        }
    }

    /// <summary>
    /// 设置当前线路的Json
    /// </summary>
    /// <param name="LineName"></param>
    private void SetLine(string LineName, string iType)
    {
        DataSet Ds = GetUserLines(" and LineName='" + LineName + "' ");
        string _ConnStr = "server={0};database={1};uid={2};pwd={3};";
        #region 统计语句
        string _Sql = @"SELECT a.ID,a.x,a.y,b.NodeCode,
((SELECT [DESCRIPTION] FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,8))+'-'+(SELECT [DESCRIPTION] FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,12))+'-'+ a.Description) AS SYSName,
(SELECT OrderID FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,8) )AS OrderID
 FROM dbo.sys_engs_ItemInfo a right JOIN dbo.sys_engs_Tree AS b ON a.id=b.RalationID and b.Scdel = 0 WHERE  x IS NOT NULL ORDER BY a.X desc";



        #endregion
        string _temp = string.Empty;
        StringBuilder LinesB = new StringBuilder();
        string SysNameTemp = "";
        int i = 0;
        foreach (DataRow Dr in Ds.Tables[0].Rows)
        {
            LinesB.Append("{Sites:[");
            _temp = string.Format(_ConnStr, Dr["DataSourceAddress"].ToString(), Dr["DataBaseName"].ToString(), Dr["SaUserName"].ToString(), Dr["SaPassWord"].ToString());

            string sql = _Sql;
            
            DataSet ds1 = GetDs(_temp, sql);

           
            _temp = string.Empty;
            i = 0;
            foreach (DataRow Dr1 in ds1.Tables[0].Rows)
            {
                SysNameTemp = Dr1["SYSName"].ToString();

                LinesB.Append("{");
                LinesB.Append("\"TitleImg\":\"" + Dr["DataBaseName"].ToString().Substring(7) + "\",");
                LinesB.Append("\"LineID\":\"" + Dr["LineID"].ToString() + "\",");
                LinesB.Append("\"SYSName\":\"" + Dr1["SYSName"].ToString() + "\",");
                LinesB.Append("\"X\":\"" + Dr1["X"].ToString() + "\",");
                LinesB.Append("\"Y\":\"" + Dr1["Y"].ToString() + "\",");
                LinesB.Append("\"TestCode\":\"" + Dr1["NodeCode"].ToString() + "\"");
                LinesB.Append("}");
                LinesB.Append(i < ds1.Tables[0].Rows.Count - 1 ? "," : "");
                i++;
            }
            LinesB.Append("]");

            LinesB.Append(",\"LineCoordinate\":[" + Dr["LinesJson"].ToString() + "]");
            LinesB.Append("}");
        }

        Response.Write(LinesB);
    }
    /// <summary>
    /// 获取用户线路
    /// </summary>
    public DataSet GetUserLines(string strWhere)
    {
        DataSet Result = new DataSet();
        if (Session["UserName"] != null)
        {
            ResultInfo RI = (ResultInfo)Session["UserInfo"];
            string Line = "'" + ((ResultInfo)Session["UserInfo"]).Line.Replace(",", "','") + "'";
            Line = Line == "''" ? ((ResultInfo)Session["UserInfo"]).LineID : Line;
            string strSql = @"SELECT 
          ID as LineID ,
          LineName ,
          [Description] ,
          DataSourceAddress ,
          UserName as SaUserName ,
          PassWord as SaPassWord ,
          DataBaseName ,

          IsActive,TestMapJson,LinesJson FROM  dbo.sys_Line   WHERE IsActive='1' AND ISBHZ in (2,3) AND Id in (" + Line + ")";


            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql += strWhere;
            }
            Result = LineDbHelperSQL.Query(strSql);
        }
        else
        {
            Response.Redirect("~/login.aspx");
        }

        return Result;
    }

    /// <summary>
    /// 获取实验室列表
    /// </summary>
    private void GetItems()
    {

        //string strSql = "select * from sys_engs_ItemInfo";
        string strSql = "SELECT a.ID,((SELECT [DESCRIPTION] FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,8))+'-'+(SELECT top 1 [DESCRIPTION] FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,12))+'-'+ a.Description) AS [Description],a.x,a.y,a.zoomLevel,b.NodeCode,b.NodeType,(SELECT OrderID FROM dbo.Sys_Tree where NodeCode=SUBSTRING(b.NodeCode,1,8) )AS OrderID FROM dbo.sys_engs_ItemInfo AS a,dbo.sys_engs_Tree AS b WHERE a.id=b.RalationID and b.Scdel = 0 ORDER BY OrderID";
        DataSet Ds = DbHelperSQL.Query(strSql);

        StringBuilder Result = new StringBuilder();
        Result.Append("{\"items\":[");
        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        {
            Result.AppendFormat("{{\"id\":\"{0}\",\"description\":\"{1}\",\"x\":\"{2}\",\"y\":\"{3}\"}}", new string[] { Ds.Tables[0].Rows[i]["id"].ToString(), Ds.Tables[0].Rows[i]["description"].ToString(), Ds.Tables[0].Rows[i]["x"].ToString(), Ds.Tables[0].Rows[i]["y"].ToString() });
            if (i < Ds.Tables[0].Rows.Count - 1)
            {
                Result.Append(",");
            }
        }
        Result.Append("]}");

        Response.Write(Result.ToString());
    }

    protected void lb_logout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("~/login.aspx");
    }


    #region 数据库操作

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="ConnStr"></param>
    /// <param name="Sql"></param>
    /// <returns></returns>
    public DataSet GetDs(string ConnStr, string Sql)
    {
        DataSet Result = new DataSet();
        using (SqlConnection _Conn = new SqlConnection(ConnStr))
        {
            _Conn.Open();
            using (SqlCommand _Com = new SqlCommand(Sql, _Conn))
            {
                using (SqlDataAdapter _Adp = new SqlDataAdapter(_Com))
                {
                    _Adp.Fill(Result);
                }
            }
            _Conn.Close();
        }
        return Result;
    }

    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <param name="storedProcName">存储过程名</param>
    /// <param name="parameters">存储过程参数</param>
    /// <param name="tableName">DataSet结果中的表名</param>
    /// <returns>DataSet</returns>
    public static DataSet RunProcedure(string ConnStr, string storedProcName, IDataParameter[] parameters, string tableName)
    {
        using (SqlConnection connection = new SqlConnection(ConnStr))
        {
            DataSet dataSet = new DataSet();
            connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
            connection.Close();
            return dataSet;
        }
    }

    /// <summary>
    /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
    /// </summary>
    /// <param name="connection">数据库连接</param>
    /// <param name="storedProcName">存储过程名</param>
    /// <param name="parameters">存储过程参数</param>
    /// <returns>SqlCommand</returns>
    private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
    {
        SqlCommand command = new SqlCommand(storedProcName, connection);
        command.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parameter in parameters)
        {
            if (parameter != null)
            {
                // 检查未分配值的输出参数,将其分配以DBNull.Value.
                if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                command.Parameters.Add(parameter);
            }
        }

        return command;
    }
    #endregion
}