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
using System.Web.Script.Serialization;

public partial class BLOB_SetZVal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {
                case "ITEM":
                    Response.Write(JsonConvert.SerializeObject(Items()));
                    Response.End();
                    break;
                case "FACTORY":
                    Response.Write(JsonConvert.SerializeObject(Factory("ItemID".RequestStr())));
                    Response.End();
                    break;
                case "MODEL":
                    Response.Write(JsonConvert.SerializeObject(Model("ItemID".RequestStr())));
                    Response.End();
                    break;
                case "MODULE":
                    Response.Write(JsonConvert.SerializeObject(Module("ItemID".RequestStr())));
                    Response.End();
                    break;
                case "ATTR":
                    Response.Write(JsonConvert.SerializeObject(Attr("ItemID".RequestStr())));
                    Response.End();
                    break;
                case "LIST":
                    List();
                    break;
                case "SAVE":
                    SAVE();
                    break;
                case "DEL":
                    DEL();
                    break;
            }
        }
    }



    #region 参数

    /// <summary>
    /// 获取原材料
    /// </summary>
    public  DataTable Items()
    {
        string Sql = "SELECT ItemID,ItemName FROM sys_TJ_Item where Status =1";
        BLL_Document BLL = new BLL_Document();
        return GetDataSet(Sql).Tables[0];
    }

    /// <summary>
    /// 获取模板
    /// </summary>
    /// <param name="ItemID"></param>
    /// <returns></returns>
    public  DataTable Module(string ItemID)
    {
        string Sql = @"  SELECT [sys_TJ_Item_Module].ModuleID,[sys_module].Name
                          FROM [sys_TJ_Item_Module]
                          join
                          [dbo].[sys_module] on [sys_TJ_Item_Module].ModuleId = [sys_module].ID
                          where 
                           [dbo].[sys_TJ_Item_Module].ItemID ='{0}' ";

        return GetDataSet(string.Format(Sql, ItemID)).Tables[0];
    }

    /// <summary>
    /// 获取厂家
    /// </summary>
    public  DataTable Factory(string ItemID)
    {
        string Sql = @" select [sys_TJ_Factory].FactoryID, [sys_TJ_Factory].FactoryName from [dbo].[sys_TJ_MainData] 
                      join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                      join [dbo].[sys_TJ_Factory] on [dbo].[sys_TJ_Factory].FactoryID = [dbo].[sys_TJ_MainData].FactoryID
                      where [sys_TJ_Item_Module].ItemID = '" + ItemID + "'  group by  [sys_TJ_Factory].FactoryID, [sys_TJ_Factory].FactoryName ";

        return GetDataSet(Sql).Tables[0];
    }

    /// <summary>
    /// 获取clomns字段
    /// </summary>
    public  ItemCollection Attribute(string ItemID)
    {
        string Sql = "SELECT Columns FROM [dbo].[sys_TJ_Item] where ItemID = '" + ItemID + "' ";
       
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Item[] Temp = serializer.Deserialize<Item[]>(ExcuteScalar(Sql).ToString());
        ItemCollection Items = new ItemCollection();
        Items.Copy(Temp);

        return Items;

    }

    /// <summary>
    /// 检查指标
    /// </summary>
    /// <param name="ItemID"></param>
    /// <returns></returns>
    public  ItemCollection Attr(string ItemID)
    {
        ItemCollection Attrs = Attribute(ItemID);


        Attrs.Remove("厂家名称");
        Attrs.Remove("报告编号");
        Attrs.Remove("报告日期");
        Attrs.Remove("强度等级");
        Attrs.Remove("品种等级");
        Attrs.Remove("施工部位");
        Attrs.Remove("数量");
        Attrs.Remove("组织");
        Attrs.Remove("组值");
        Attrs.Remove("型号");
        Attrs.Remove("标准值");

        return Attrs;
    }


    /// <summary>
    /// 型号
    /// </summary>
    public  DataTable Model(string ItemID)
    {
        string Sql = @"  select {0} as XH from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          join [dbo].[sys_TJ_Item] on [dbo].[sys_TJ_Item].ItemID = [dbo].[sys_TJ_Item_Module].ItemID 
                          AND [dbo].[sys_TJ_Item_Module].ItemID ='{1}' group by {0}";
        ItemCollection Attrs = Attribute(ItemID);
        if (!Attrs.ContainsKey("型号"))
        {
            return null;
        }


        Sql = @"SELECT code,description as XH
  FROM [SYGLDB_DaRui].[dbo].[sys_dictionary]
  where scdel = 0 and code is not null
 
  order by code asc";
        DataSet Ds = new DataSet();// AND code = ''

        using (SqlConnection _Conn = new SqlConnection("Data Source=112.124.99.146;Initial Catalog=SYGLDB_DaRui;User ID=sygldb_kingrocket_f;Pwd=wdxlzyn@#830"))
        {
            using (SqlCommand _Cmd = new SqlCommand(Sql, _Conn))
            {
                using (SqlDataAdapter _Adp = new SqlDataAdapter(_Cmd))
                {
                    _Adp.Fill(Ds);
                }
            }

            _Conn.Close();
            _Conn.Dispose();
        }
        //return GetDataSet(string.Format(Sql, Attrs["型号"].BindField, ItemID)).Tables[0];

        return Ds.Tables[0];
    }

    /// <summary>
    /// 工程部位
    /// </summary>
    public  DataTable Position(string ItemID)
    {
        string Sql = @"  select SGBW from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          join [dbo].[sys_TJ_Item] on [dbo].[sys_TJ_Item].ItemID = [dbo].[sys_TJ_Item_Module].ItemID 
                          AND [dbo].[sys_TJ_Item_Module].ItemID ='{0}' group by SGBW";

        if (ItemID.IsNullOrEmpty())
        {
            Sql = @"  select SGBW from [dbo].[sys_TJ_MainData] group by SGBW";
        }

        return GetDataSet(string.Format(Sql, ItemID)).Tables[0];
    }
    #endregion

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
                        select IndexID from sys_TJ_StandardValue
                          WHERE 1=1 
                          {0} 
                        

                        
                        SELECT sys_module.Name as MName, sys_TJ_Item.ItemName as YC, sys_TJ_StandardValue.* from sys_TJ_StandardValue
                        JOIN sys_TJ_Item on sys_TJ_Item.ItemID = sys_TJ_StandardValue.ItemID
                        left outer join sys_module on sys_module.id = sys_TJ_StandardValue.ModuleID
                        INNER JOIN @TempTable t ON sys_TJ_StandardValue.IndexID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                         {0}        
     

                        DECLARE @C int
                        select @C= count(IndexID) from sys_TJ_StandardValue 
                          where 1=1 
                          {0}
                        

                        select @C 
                        ";

        #endregion

        string PageIndex = "page".RequestStr();
        string PageSize = "rows".RequestStr();
        string Item = "Item".RequestStr();
        string Attr = "Attr".RequestStr();
        string AttrName = "AttrName".RequestStr();
        string Model = "Model".RequestStr();

        string Where = "  ";


        Where += string.IsNullOrEmpty(Item) ? " " : "  AND sys_TJ_StandardValue.ItemID ='" + Item + "' ";
        Where += string.IsNullOrEmpty(AttrName) ? " " : "  AND sys_TJ_StandardValue.ItemName ='" + AttrName + "' ";
        Where += string.IsNullOrEmpty(Model) ? " " : "  AND sys_TJ_StandardValue.Model ='" + Model + "' ";

        Sql = string.Format(Sql,
            Where,
            PageIndex,
            PageSize
            );
       
        RecordCount = 0;
       
        DataSet Ds = GetDataSet(Sql);

        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());



        string Json = JsonConvert.SerializeObject(Ds.Tables[0]);
        Json = "{\"rows\":" + Json + ",\"total\":" + RecordCount + "}";

        Response.Write(Json);
        Response.End();
    }


    public void SAVE()
    {

        string ItemID = Request["Item"].ToString();
        string ItemName = Request["AttrName"].ToString();
        string Model = Request["Model"].ToString();
        string StandardValue = Request["ZVal"].ToString();
        string ModuleID = Request["ModuleID"].ToString();

        string Sql = @"

                INSERT INTO [dbo].[sys_TJ_StandardValue]
                           (
                             [IndexID]
                           ,[ItemID]
                           ,[ItemName]
                           ,[Model]
                           ,[StandardValue],[ModuleID])
                     VALUES
                           ('{0}'
                           ,'{1}'
                           ,'{2}'
                           ,'{3}'
                                ,'{4}','{5}'
                           )

";

        Sql = string.Format(Sql, Guid.NewGuid().ToString(), ItemID, ItemName, Model, StandardValue, ModuleID);


        Response.Write(Execute(Sql).ToString());
        Response.End();
    }

    public void DEL()
    {
        string IndexID = Request["IndexID"].ToString();

        string Sql = " delete from sys_TJ_StandardValue where IndexID = '"+IndexID+"' ";

        int Temp = Execute(Sql);

        Response.Write(Temp > 0 ? "删除成功" : "删除失败");
        Response.End();
    }

    public DataSet GetDataSet(string Sql)
    {
        DataSet Ds = new DataSet();

        using (SqlConnection _Conn = new SqlConnection("Data Source=112.124.99.146;Initial Catalog=SYGLDB_Demo;User ID=sygldb_kingrocket_f;Pwd=wdxlzyn@#830"))
        {
            using (SqlCommand _Cmd = new SqlCommand(Sql, _Conn))
            {
                using (SqlDataAdapter _Adp = new SqlDataAdapter(_Cmd))
                {
                    _Adp.Fill(Ds);
                }
            }

            _Conn.Close();
            _Conn.Dispose();
        }

        return Ds;
    }

    public object ExcuteScalar(string Sql)
    {
        object Result = null;

        using (SqlConnection _Conn = new SqlConnection("Data Source=112.124.99.146;Initial Catalog=SYGLDB_Demo;User ID=sygldb_kingrocket_f;Pwd=wdxlzyn@#830"))
        {
            _Conn.Open();
            using (SqlCommand _Cmd = new SqlCommand(Sql, _Conn))
            {
                Result = _Cmd.ExecuteScalar();
            }

            _Conn.Close();
            _Conn.Dispose();
        }

        return Result;
    }

    public int Execute(string Sql)
    {
        int Result = 0;

        using (SqlConnection _Conn = new SqlConnection("Data Source=112.124.99.146;Initial Catalog=SYGLDB_Demo;User ID=sygldb_kingrocket_f;Pwd=wdxlzyn@#830"))
        {
            _Conn.Open();
            using (SqlCommand _Cmd = new SqlCommand(Sql, _Conn))
            {
                Result = _Cmd.ExecuteNonQuery();
            }

            _Conn.Close();
            _Conn.Dispose();
        }

        return Result;
    }
}