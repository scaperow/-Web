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

public partial class BLOB_OperateLog : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {

                case "SHEET":
                    Sheet();
                    break;
                case "LIST":
                    List();
                    break;
            }
        }
    }

    public void List()
    {

        int RecordCount = 0;

        DataSet DS = LineDbHelperSQL.Query("SELECT [Account] FROM [Sys_Users]");
        string Users = string.Empty;

        if (DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow Dr in DS.Tables[0].Rows)
            {
                Users += Users.IsNullOrEmpty() ? "'" + Dr[0].ToString() + "'" : ",'" + Dr[0].ToString() + "'";
            }
        }

        #region SQL



        string Sql = @" 
                        DECLARE @Page int
                        DECLARE @PageSize int
                        SET @Page = {1}
                        SET @PageSize = {2}
                        SET NOCOUNT ON
                        DECLARE @TempTable TABLE (IndexId int identity, _keyID varchar(200))
                        INSERT INTO @TempTable
                        (
	                        _keyID
                        )
                         SELECT ID  FROM sys_operate_log 

                          WHERE 1=1 
                          {0} 
                           Order by modifiedDate DESC

        
                         SELECT  sys_operate_log.ID,
                            modifiedby as 'YH',
                            t2.description as 'BD',
                            t3.description as 'DW',
                            t1.description as 'SYS',
                            modifiedDate as 'CZRQ',
                            optType as 'CZLX',
                            sys_module.name as 'MB',
                            DataName as 'BGRQ',
                            BGBH as 'BGBH',
                            modifyitem as 'XGRZ'
                              FROM sys_operate_log
                              left outer join sys_module  on sys_module.id = sys_operate_log.moduleID
                              left outer join sys_tree t1 on t1.nodecode = sys_operate_log.testroomcode
                              left outer join sys_tree t2 on t2.nodecode = left(sys_operate_log.testroomcode,8)
                              left outer join sys_tree t3 on t3.nodecode = left(sys_operate_log.testroomcode,12)
                        INNER JOIN @TempTable t ON sys_operate_log.ID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                         {0}        
                        Order By  modifiedDate Desc

                        DECLARE @C int
                        select @C= count(ID) from sys_operate_log
                         
                          where 1=1 
                          {0}
                        select @C 
                        ";


        #endregion



        string Where = "  ";

        Where += string.IsNullOrEmpty("StartDate".RequestStr()) ? " " : " AND modifiedDate >='" + "StartDate".RequestStr() + "' ";
        Where += string.IsNullOrEmpty("EndDate".RequestStr()) ? " " : "  AND modifiedDate <='" + DateTime.Parse("EndDate".RequestStr()).AddDays(1).ToShortDateString() + "' ";
        Where += string.IsNullOrEmpty(SelectedTestRoomCodes) ? " " : " AND  TestRoomCode in(" + SelectedTestRoomCodes + ") ";
        Where += string.IsNullOrEmpty("Person".RequestStr()) ? " " : " AND modifiedby like'%" + "Person".RequestStr() + "%' ";
        Where += Users.IsNullOrEmpty() ? "" : " AND modifiedby in (" + Users + ")";


        Sql = string.Format(Sql,
            Where,
            "page".RequestStr(),
            "rows".RequestStr());

        RecordCount = 0;
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());



        string Json = JsonConvert.SerializeObject(Ds.Tables[0]);
        Json = "{\"rows\":" + Json + ",\"total\":" + RecordCount + "}";

        Response.Write(Json);
        Response.End();
    }

    public void Sheet()
    {

        string Sql = "select id,name from sys_sheet where ID in (" + "SheetID".RequestStr() + ")";

       
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);
        



        string Json = JsonConvert.SerializeObject(Ds.Tables[0]);
       

        Response.Write(Json);
        Response.End();
    }
}