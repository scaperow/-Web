using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using JZ.BLL;
using System.Data;
using System.Data.SqlClient;

public partial class Rating_CreaditRatingEdit : BasePage
{
    #region 参数

    public string ACT = "ADD";

    public SYS_ReditRating Obj = new SYS_ReditRating();

    #endregion

    #region 周期
    protected void Page_Load(object sender, EventArgs e)
    {

        PageInit();
    }

    /// <summary>
    /// 页面初始化
    /// </summary>
    public void PageInit()
    {
        if (!"ID".RequestStr().IsNullOrEmpty())
        {
            GetObj();
            ACT = "EDIT";
        }


        if (Request["ACT"] != null && Request["ACT"].ToString() == "ADD")
        {
            Response.Write(Add());
            Response.End();
        }

        if (Request["ACT"] != null && Request["ACT"].ToString() == "EDIT")
        {
            Response.Write(Edit());
            Response.End();
        }

        if (Request["ACT"] != null && Request["ACT"].ToString() == "GetTESTROOM")
        {
            Response.Write(GetTestRoom());
            Response.End();
        }

        if (Request["ACT"] != null && Request["ACT"].ToString() == "GetPerson")
        {
            Response.Write(GetPerson());
            Response.End();
        }
        
    }
    #endregion


    #region 方法

    /// <summary>
    /// 添加
    /// </summary>
    public string Add()
    {
        string Result = "";
        try
        {

            string TestRoomCode = Request["TestRoomCode"].ToString();
            string CompanyType = Request["CompanyType"].ToString();
            string Name = "Name".RequestStr();
            string IDCard = Request["IDCard"].ToString();
            string Job = Request["Job"].ToString();
            string Deduct = Request["Deduct"].ToString();
            string Remark = Request["Remark"].ToString();
            string RType = Request["RType"].ToString();
            string CreateBy = Session["UserName"].ToString();
            string Sql = @"

                INSERT INTO [dbo].[sys_ReditRating]
                           ([SysID]
                           ,[SegmentCode]
                           ,[CompanyCode]
                           ,[TestRoomCode]
                           ,[CompanyType]
                           ,[RType] 
                           ,[Name]
                           ,[IDCard]
                           ,[Job]
                           ,[Deduct]
                           ,[Remark]
                           ,[CreateOn],[CreateBy]
                            ,[ISDeleted])
                     VALUES
                           ('{0}'
                           ,'{1}'
                           ,'{2}'
                           ,'{3}'
                           ,'{4}'
                           ,'{5}'
                           ,'{6}'
                           ,'{7}'
                           ,'{8}'
                           ,'{9}'
                           ,'{10}','{11}','{12}',1)

";

            Sql = string.Format(Sql, Guid.NewGuid().ToString(), TestRoomCode.Substring(0, 8), TestRoomCode.Substring(0, 12), TestRoomCode, CompanyType, RType, Name, IDCard, Job, Deduct, Remark, DateTime.Now.ToString(), CreateBy);

            BLL_Document BLL = new BLL_Document();


       
            Result = BLL.ExcuteCommand(Sql) > 0 ? "true" : "false";

        }
        catch
        {
            Result = "false";
        }
        return Result;
    }


    /// <summary>
    /// 添加
    /// </summary>
    public string Edit()
    {
        string Result = "";
        try
        {

            string TestRoomCode = Request["TestRoomCode"].ToString();
            string CompanyType = Request["CompanyType"].ToString();
            string Name = "Name".RequestStr();
            string IDCard = Request["IDCard"].ToString();
            string Job = Request["Job"].ToString();
            string Deduct = Request["Deduct"].ToString();
            string Remark = Request["Remark"].ToString();
            string SysID = Request["SysID"].ToString();
            string RType = Request["RType"].ToString();
            string CreateBy = Session["UserName"].ToString();
            string Sql = @"

                UPDATE [dbo].[sys_ReditRating]
                          SET 
                           [SegmentCode] = '{0}'
                          ,[CompanyCode] = '{1}'
                          ,[TestRoomCode] = '{2}'
                          ,[CompanyType] = '{3}'
                          ,[RType] = '{11}'
                          ,[Name] = '{4}'
                          ,[IDCard] = '{5}'
                          ,[Job] = '{6}'
                          ,[Deduct] = '{7}'
                          ,[Remark] = '{8}'
,[CreateBy] = '{9}'
                   WHERE SysID = '{10}'

";

            Sql = string.Format(Sql, TestRoomCode.Substring(0, 8), TestRoomCode.Substring(0, 12), TestRoomCode, CompanyType, Name, IDCard, Job, Deduct, Remark, CreateBy, SysID, RType);

            BLL_Document BLL = new BLL_Document();



            Result = BLL.ExcuteCommand(Sql) > 0 ? "true" : "false";

        }
        catch
        {
            Result = "false";
        }
        return Result;
    }

    /// <summary>
    /// 获取试验室
    /// </summary>
    /// <returns></returns>
    public string GetTestRoom()
    {
        string Result = string.Empty;

 

        string Sql = @"
                    select 
                    t.nodecode,
                    s.description+'-'+c.description +'-'+t.description as description
                    from Sys_tree  t
                    left outer join Sys_tree s
                    on left(t.nodecode,8) = s.nodecode
                    left outer join Sys_tree c
                    on left(t.nodecode,12) = c.nodecode
                    where t.NodeCode in ({0})
                    order by c.description asc
                    ";

        BLL_Document BLL = new BLL_Document();

        DataTable List = BLL.GetDataTable(string.Format(Sql, SelectedTestRoomCodes));

        Result = JsonConvert.SerializeObject(List);

        return Result;
    }

    /// <summary>
    /// 获取数据库实体
    /// </summary>
    public void GetObj()
    {
        string Sql = @" select * from [dbo].[sys_ReditRating] where SysID = '" + "ID".RequestStr() + "' ";


        BLL_Document BLL = new BLL_Document();

        using (IDataReader Read = BLL.ExcuteReader(Sql))
        {

            try
            {
                Read.Read();
                Obj.SysID = Read["SysID"].ToString();
                Obj.Name = Read["Name"].ToString();
                Obj.SegmentCode = Read["SegmentCode"].ToString();
                Obj.CompanyCode = Read["CompanyCode"].ToString();
                Obj.TestRoomCode = Read["TestRoomCode"].ToString();
                Obj.IDCard = Read["IDCard"].ToString();
                Obj.Job = Read["Job"].ToString();
                Obj.Deduct = Read["Deduct"].ToString();
                Obj.Remark = Read["Remark"].ToString();
                Obj.CompanyType = Read["CompanyType"].ToString();
                Obj.CreateOn = Read["CreateOn"].ToString();
                Obj.RType = Read["RType"].ToString();
                Obj.CreateBy = Session["UserName"].ToString();
            }
            catch { }

            Read.Close();
            Read.Dispose();
        }

    }

    /// <summary>
    /// 获取人员信息
    /// </summary>
    public string GetPerson()
    {
  
        string TestRoomCode = "TestRoomCode".RequestStr();

        string Sql = @" SELECT Ext1 Name,Ext5 Job,Ext11 IDCard
						FROM dbo.sys_document a JOIN dbo.v_bs_codeName b 
						ON a.ModuleID='08899BA2-CC88-403E-9182-3EF73F5FB0CE'  
						AND a.TestRoomCode=b.试验室编码  
						JOIN dbo.Sys_Tree c ON  LEFT(a.TestRoomCode,12)=c.NodeCode
                        where a.TestRoomCode = '{0}'
                        Order By  OrderID,TestRoomCode ASC";
        Sql = string.Format(Sql, TestRoomCode);
        BLL_Document Bll = new BLL_Document();

        DataTable DT = Bll.GetDataTable(Sql);

        return JsonConvert.SerializeObject(DT);
       
    }

    #endregion
}


/// <summary>
/// 信誉评价实体
/// </summary>
public class SYS_ReditRating
{
    private string _SysID;

    public string SysID
    {
        get { return _SysID; }
        set { _SysID = value; }
    }

    private string _SegmentCode;

    public string SegmentCode
    {
        get { return _SegmentCode; }
        set { _SegmentCode = value; }
    }

    private string _CompanyCode;

    public string CompanyCode
    {
        get { return _CompanyCode; }
        set { _CompanyCode = value; }
    }

    private string _TestRoomCode;

    public string TestRoomCode
    {
        get { return _TestRoomCode; }
        set { _TestRoomCode = value; }
    }

    private string _CompanyType;

    public string CompanyType
    {
        get { return _CompanyType; }
        set { _CompanyType = value; }
    }

    private string _Name;

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    private string _IDCard;

    public string IDCard
    {
        get { return _IDCard; }
        set { _IDCard = value; }
    }

    private string _Job;

    public string Job
    {
        get { return _Job; }
        set { _Job = value; }
    }

    private string _Deduct;

    public string Deduct
    {
        get { return _Deduct; }
        set { _Deduct = value; }
    }

    private string _Remark;

    public string Remark
    {
        get { return _Remark; }
        set { _Remark = value; }
    }

    private string _CreateOn;

    public string CreateOn
    {
        get { return _CreateOn; }
        set { _CreateOn = value; }
    }

    private string _CreateBy;

    public string CreateBy
    {
        get { return _CreateBy; }
        set { _CreateBy = value; }
    }

    private string _RType;

    public string RType
    {
        get { return _RType; }
        set { _RType = value; }
    }
}