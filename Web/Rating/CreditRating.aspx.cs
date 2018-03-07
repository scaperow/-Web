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



public partial class Rating_CreditRating :  BasePage
{
    #region 参数

    /// <summary>
    /// 当前页
    /// </summary>
    string PageIndex = string.Empty;

    /// <summary>
    /// 页行数
    /// </summary>
    string PageSize = string.Empty;

    /// <summary>
    /// 记录总数
    /// </summary>
    string RecordCount = string.Empty;

    #endregion

    #region 周期
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("ACT".RequestStr() == "DEL")
        {
            Response.Write(Del());
            Response.End();
            return;
        }

        if (Request["sType"] != null)
        {
            if (!String.IsNullOrEmpty(Request.Params["StartDate"]))
            {
                StartDate = Request.Params["StartDate"];
            }
            if (!String.IsNullOrEmpty(Request.Params["EndDate"]))
            {
                EndDate = Request.Params["EndDate"];
            }

            PageIndex = Request["page"];
            PageSize = Request["rows"];
            string json;
            switch (Request["sType"].ToString())
            {
                case "List":
                     json = GetListJson();

                    decimal Temp = Math.Round(decimal.Parse(RecordCount) / decimal.Parse(PageSize), 2);
                    Temp = Math.Ceiling(Temp);
                    string result = "{\"total\": \"" + Temp.ToString() + "\", \"page\": \"" + PageIndex + "\", \"records\": \"" + RecordCount + "\", \"rows\" : " + json + "}";
                    Response.Write(result);
                    Response.End();
                    break;

                case "CHART":
                     json = GetCharJson();
                    Response.Write(json);
                    Response.End();
                    break;
            }


        }
    }
    #endregion

    #region 方法

    /// <summary>
    /// 列表
    /// </summary>
    /// <returns>Json:string</returns>
    public string GetListJson()
    {
       string testroom = "testroom".RequestStr();
        string name = "name".RequestStr();
        string createdby = "createdby".RequestStr();
        string deduct = "deduct".RequestStr();
        string SqlWhere = " AND 1=1 ";

        SqlWhere += testroom.IsNullOrEmpty() ? string.Empty : " AND t3.DESCRIPTION like '%" + testroom + "%' ";
        SqlWhere += name.IsNullOrEmpty() ? string.Empty : " AND a.name like '%" + name + "%' ";
        SqlWhere += createdby.IsNullOrEmpty() ? string.Empty : " AND a.createdby like '%" + createdby + "%' ";
        SqlWhere += deduct.IsNullOrEmpty() ? string.Empty : " AND a.remark like '%" + deduct + "%' ";

        #region 查询语句

        string Sql = @" 
                        DECLARE @Page int
                        DECLARE @PageSize int
                        SET @Page = {0}
                        SET @PageSize = {1}
                        SET NOCOUNT ON
                        DECLARE @TempTable TABLE (IndexId int identity, _keyID int)
                        INSERT INTO @TempTable
                        (
	                        _keyID
                        )
                        select ID from Sys_ReditRating where CreateOn between '{2}' AND '{3}' AND TestRoomCode IN ({4}) AND (ISDeleted <> 0 or ISDeleted is  NULL) Order By  CreateOn Desc
                        
                        SELECT
                        SysID,
                        t1.DESCRIPTION  as '标段',
						 t2.DESCRIPTION  as '单位',
						 t3.DESCRIPTION  as '试验室',
						CompanyType as '单位性质',
                        RType as '评价类别',
						name as '姓名',
						idcard as '身份证',
						job as '职务',
						deduct as '总扣分数',
						remark as '备注',
						createon as '评价时间',
                        createby as '评价人'
                        FROM Sys_ReditRating a
                        left outer join sys_tree t1 on t1.NodeCode = a.SegmentCode 
					    left outer join sys_tree t2 on t2.NodeCode = a.companycode
					    left outer join sys_tree t3 on t3.NodeCode = a.testroomcode 
                        INNER JOIN @TempTable t ON a.ID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        {5}
                        Order By  CreateOn Desc

                        DECLARE @C int
                        select @C= count(ID)  from Sys_ReditRating a left outer join sys_tree t3 on t3.NodeCode = a.testroomcode  where CreateOn between '{2}' AND '{3}' AND TestRoomCode IN ({4})  AND (ISDeleted <> 0 or ISDeleted is  NULL) {5}
                        select @C 
                        ";

        Sql = string.Format(Sql, PageIndex, PageSize, "StartDate".RequestStr(), DateTime.Parse("EndDate".RequestStr()).AddDays(1).ToString("yyyy-MM-dd"), SelectedTestRoomCodes, SqlWhere);
        #endregion

        BLL_Document BLL = new BLL_Document();

        DataSet Ds = BLL.GetDataSet(Sql);
      



        if (Ds.Tables.Count >1)
        {
            DataTable List = Ds.Tables[0];

            RecordCount = Ds.Tables[1].Rows[0][0].ToString();

            return JsonConvert.SerializeObject(List);
        }
        else
        {
            return "";
        }
    }


    public string Del()
    {
        string SysID = "SysID".RequestStr();
        if (SysID.IsNullOrEmpty()) { return ""; }

        string Sql = @" UPDATE  [dbo].[sys_ReditRating] SET IsDeleted=0  WHERE  SysID = '" + SysID + "' ";
        BLL_Document BLL = new BLL_Document();

        return BLL.ExcuteCommand(Sql) > 0 ? "true" : "false";
    }


    public string GetCharJson()
    {

        #region 查询语句

        

        //按照试验室排名 无分数不显示
       string SqlBySYS = @"
                        select 
                        t1.description+'  '+t2.description+'  '+t3.description description,
                        sum(deduct) deduct from [sys_ReditRating] r
                        left outer join sys_Tree t1 on t1.nodecode = r.segmentcode
                        left outer join sys_Tree t2 on t2.nodecode = r.CompanyCode
                        left outer join sys_Tree t3 on t3.nodecode = r.testroomcode
                        where (r.ISDeleted <> 0 or r.ISDeleted is null) and r.testroomcode in ({0})  ANd r.CreateOn >='{1}' AND r.CreateOn<='{2}'
                        group by testroomcode,t1.description,t2.description,t3.description
                        order by deduct desc
                        ";

        //按照人员排名 无分数不显示
       string SqlByPerson = @"
                            select 
                            name as description,
                            sum(deduct) deduct from [sys_ReditRating] r
                            where (r.ISDeleted <> 0 or r.ISDeleted is null) AND Name <>''  and r.testroomcode in ({0})  ANd r.CreateOn >='{1}' AND r.CreateOn<='{2}'
                            group by name
                            order by deduct desc

                            ";
        #endregion

        BLL_Document BLL = new BLL_Document();

       string Sql = "n".RequestStr() == "0" ? SqlByPerson : SqlBySYS;

        #region 事件段数量
        Sql = string.Format(Sql, SelectedTestRoomCodes, "StartDate".RequestStr(), DateTime.Parse("EndDate".RequestStr()).AddDays(1).ToString("yyyy-MM-dd"));

        DataSet Ds = BLL.GetDataSet(Sql);


        #endregion


        if (Ds.Tables.Count > 0)
        {
            foreach (DataRow Dr in Ds.Tables[0].Rows)
            {
                if (Dr["deduct"].ToString() == "" || Dr["deduct"].ToString() == "NULL")
                {
                    Dr["deduct"] = "0";
                }
            }
            DataTable List = Ds.Tables[0];

            

            return JsonConvert.SerializeObject(List);
        }
        else
        {
            return "";
        }
    }

    #endregion
}

