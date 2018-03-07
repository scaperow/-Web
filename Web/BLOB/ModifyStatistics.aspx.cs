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


public partial class BLOB_ModifyStatistics : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {


                case "LIST":
                    List();
                    break;
            }
        }
    }



    public void List()
    {

        int RecordCount = 0;

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
                         SELECT KMID  FROM sys_KeyModify 

                          WHERE 1=1 
                          {0} 
                            Order by ModifyTime DESC

        
                          SELECT sys_module.name,sys_KeyModify.*,
t1.DESCRIPTION +' '+sys_tree.DESCRIPTION as DESCRIPTION
 FROM sys_KeyModify
left outer join sys_tree on sys_KeyModify.testroomcode = sys_tree.nodecode
left outer join sys_tree as t1 on Left(sys_KeyModify.testroomcode,8) = t1.nodecode
left outer join sys_module on sys_module.id = sys_KeyModify.moduleid
                        INNER JOIN @TempTable t ON sys_KeyModify.KMID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                         {0}        
                        Order By  ModifyTime Desc

                        DECLARE @C int
                        select @C= count(DataID) from sys_KeyModify
                         
                          where 1=1 
                          {0}
                        select @C 
                        ";


        #endregion
 


        string Where = "  ";

        Where += string.IsNullOrEmpty("StartDate".RequestStr()) ? " " : " AND modifytime >='" + "StartDate".RequestStr() + "' ";
        Where += string.IsNullOrEmpty( "EndDate".RequestStr()) ? " " : "  AND modifytime <='" + DateTime.Parse( "EndDate".RequestStr()).AddDays(1).ToShortDateString() + "' ";
        Where += string.IsNullOrEmpty(SelectedTestRoomCodes) ? " " : " AND  TestRoomCode in(" + SelectedTestRoomCodes + ") ";




        Sql = string.Format(Sql,
            Where,
            "page".RequestStr(),
            "rows".RequestStr() );
       
        RecordCount = 0;
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());



        string Json = JsonConvert.SerializeObject(Ds.Tables[0]);
        Json = "{\"rows\":" + Json + ",\"total\":" + RecordCount + "}";

        Response.Write(Json);
        Response.End();
    }
}