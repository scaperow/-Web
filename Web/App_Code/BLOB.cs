using System;
using System.Collections;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

/// <summary>
/// BLOB 的摘要说明
/// </summary>
public class BLOB
{


    #region 参数

    /// <summary>
    /// 获取强度等级
    /// </summary>
    /// <returns></returns>
    public static DataTable QDDJ()
    {
        string Sql = @"select M.QDDJ from 
                    sys_TJ_MainData M
                    join sys_TJ_Item I on I.ItemID = M.ItemID AND  I.Status=1 AND I.ItemType =2
                    ANd I.ItemName in('混凝土抗压(同条件)','混凝土抗压')
                    group by QDDJ";
        BLL_Document BLL = new BLL_Document();
        return BLL.GetDataSet(Sql).Tables[0];
    }

    /// <summary>
    /// 获取原材料
    /// </summary>
    public static DataTable Items()
    {
        string Sql = "SELECT ItemID,ItemName FROM sys_TJ_Item where Status =1";
        BLL_Document BLL = new BLL_Document();
        return BLL.GetDataSet(Sql).Tables[0];
    }

    /// <summary>
    /// 获取原材料半成品
    /// </summary>
    public static DataTable Items(string ItemType)
    {
        string Sql = "SELECT ItemID,ItemName FROM sys_TJ_Item where Status =1 AND ItemType=" + ItemType;
        BLL_Document BLL = new BLL_Document();
        return BLL.GetDataSet(Sql).Tables[0];
    }


    /// <summary>
    /// 获取厂家
    /// </summary>
    public static DataTable Factory(string ItemID)
    {
        string Sql = @" select [sys_TJ_Factory].FactoryID, [sys_TJ_Factory].FactoryName from [dbo].[sys_TJ_MainData] 
                      join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                      join [dbo].[sys_TJ_Factory] on [dbo].[sys_TJ_Factory].FactoryID = [dbo].[sys_TJ_MainData].FactoryID
                      where [sys_TJ_Item_Module].ItemID = '" + ItemID + "'  group by  [sys_TJ_Factory].FactoryID, [sys_TJ_Factory].FactoryName ";
        BLL_Document BLL = new BLL_Document();
        return BLL.GetDataSet(Sql).Tables[0];
    }

    /// <summary>
    /// 获取clomns字段
    /// </summary>
    public static ItemCollection Attribute(string ItemID)
    {
        string Sql = "SELECT Columns FROM [dbo].[sys_TJ_Item] where ItemID = '" + ItemID + "' ";
        BLL_Document BLL = new BLL_Document();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Item[] Temp = serializer.Deserialize<Item[]>(BLL.ExcuteScalar(Sql).ToString());
        ItemCollection Items = new ItemCollection();
        Items.Copy(Temp);

        return Items;

    }

    /// <summary>
    /// 检查指标
    /// </summary>
    /// <param name="ItemID"></param>
    /// <returns></returns>
    public static ItemCollection Attr(string ItemID)
    {
        ItemCollection Attrs = Attribute(ItemID);

   
          Attrs.Remove("厂家名称")  ;
          Attrs.Remove("报告编号")  ;
          Attrs.Remove("报告日期")  ;
          Attrs.Remove("强度等级")  ;
          Attrs.Remove("品种等级") ;
          Attrs.Remove("施工部位") ;
          Attrs.Remove("数量") ;
          Attrs.Remove("组织");
          Attrs.Remove("组值");
          Attrs.Remove("型号");
          Attrs.Remove("标准值");

          Attrs.Remove("代表数量");
          Attrs.Remove("设备型号");
          Attrs.Remove("仪表型号");
          Attrs.Remove("试验人员");
          Attrs.Remove("设备厂家");
          Attrs.Remove("焊接种类");
          Attrs.Remove("级别代号");

          Attrs.Remove("批号");
          Attrs.Remove("公称直径");
          Attrs.Remove("委托编号");
          Attrs.Remove("级别代号");
          Attrs.Remove("级别代号");

        return Attrs;
    }


    /// <summary>
    /// 型号
    /// </summary>
    public static DataTable Model(string ItemID)
    {
        string Sql = @"  select {0} as XH from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          join [dbo].[sys_TJ_Item] on [dbo].[sys_TJ_Item].ItemID = [dbo].[sys_TJ_Item_Module].ItemID 
                          AND [dbo].[sys_TJ_Item_Module].ItemID ='{1}' AND {0} is not null group by {0}";
        BLL_Document BLL = new BLL_Document();
        ItemCollection Attrs = Attribute(ItemID);
        if (!Attrs.ContainsKey("型号"))
        {
            return null;
        }
        return BLL.GetDataSet(string.Format(Sql, Attrs["型号"].BindField, ItemID)).Tables[0];
    }



    /// <summary>
    /// 工程部位
    /// </summary>
    public static DataTable Position(string ItemID)
    {
         string Sql = @"  select SGBW from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          join [dbo].[sys_TJ_Item] on [dbo].[sys_TJ_Item].ItemID = [dbo].[sys_TJ_Item_Module].ItemID 
                          AND [dbo].[sys_TJ_Item_Module].ItemID ='{0}' group by SGBW";

         if (ItemID.IsNullOrEmpty())
         {
             Sql = @"  select SGBW from [dbo].[sys_TJ_MainData] group by SGBW";
         }
        BLL_Document BLL = new BLL_Document();
        return BLL.GetDataSet(string.Format(Sql,  ItemID)).Tables[0];
    }
    #endregion

    #region 原材使用统计

    /// <summary>
    /// 原材 数量统计
    /// </summary>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataTable Raw(string StartDate, string EndDate)
    {

        string Sql = @" select 
                        c.ItemID,c.ItemName,
                        CONVERT (decimal(18,2),sum(ShuLiang))  as val ,count(ShuLiang) as c from 
                        sys_TJ_MainData as a
                        left outer join  Sys_Tree as b on a.CompanyCode = b.NodeCode 
                        left outer join sys_TJ_Item as c on c.ItemID =a.itemid
                        where b.deptype <>'@unit_监理单位' ANd c.itemtype =1   {0}
                        group by c.ItemID,c.ItemName
                        order by val desc ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";
        Sql = string.Format(Sql, Where);

        DataSet Ds = GetDataSet(Sql);



        if (Ds.Tables.Count < 1)
        {
            return new DataTable();
        }

        return Ds.Tables[0];
    }

    /// <summary>
    /// 原材 按照 标段 试验室 统计
    /// </summary>
    /// <param name="ItemID"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataSet RawPro(string ItemID, string StartDate, string EndDate)
    {

        string Sql = @" select SegmentCode,
                            b.Description,
                            CONVERT (decimal(18,2),sum(ShuLiang)) as val, 
                            count(ShuLiang) as c
                            from 
                            sys_TJ_MainData as a
                            left outer join  Sys_Tree as b on a.SegmentCode = b.NodeCode 
                            where   a.ItemID='{1}' {0}
                             
                            group by Description,OrderID,SegmentCode
                            order by OrderID 
                            
                            ";

        string Sql1 = @" 
                            select SegmentCode,
                            b.Description,
                            CONVERT (decimal(18,2),sum(ShuLiang)) as val,
                            count(ShuLiang) as c
                            from 
                            sys_TJ_MainData a
                            left outer join  Sys_Tree as b on a.TestRoomCode = b.NodeCode 
                            where    a.ItemID='{1}' {0}
                            group by Description,SegmentCode
                            order by SegmentCode 


                            select
                            b.Description
                            from 
                            sys_TJ_MainData a
                            left outer join  Sys_Tree as b on a.TestRoomCode = b.NodeCode 
                            where    a.ItemID='{1}' {0}
                            group by Description
                            
                            ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";

        Sql = string.Format(Sql, Where, ItemID);
        Sql1 = string.Format(Sql1, Where, ItemID);



        DataSet Ds = GetDataSet(Sql + Sql1);



        return Ds;
    }

    /// <summary>
    /// 原材 厂家数量
    /// </summary>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataTable RawFactory(string StartDate, string EndDate)
    {

        string Sql = @"  select ItemID, ItemName,count(val) as val
from
( select    sys_TJ_Item.ItemID,
                          ItemName ,
                          FactoryID as val
                          from sys_TJ_Item
                          left outer join sys_TJ_MainData on sys_TJ_Item.ItemID =sys_TJ_MainData.itemid
                            where sys_TJ_Item.itemtype =1 {0}
                          group by ItemName,FactoryID ,sys_TJ_Item.ItemID  ) as a

						 group by ItemName,ItemID
                         ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";

        Sql = string.Format(Sql, Where);

        DataSet Ds = GetDataSet(Sql);



        if (Ds.Tables.Count < 1)
        {
            return new DataTable();
        }

        return Ds.Tables[0];
    }

    /// <summary>
    /// 原材 厂家供货次数
    /// </summary>
    /// <param name="ItemID"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataTable FactPro(string ItemID, string StartDate, string EndDate)
    {

        string Sql = @"  select b.FactoryName,count(itemid) as val
                         from 
                         sys_TJ_MainData  a 
                         left outer join sys_TJ_Factory b on a.FactoryID = b.FactoryID
                         where ItemID = '{1}' and b.FactoryName is not null
                         {0}
                         group by b.FactoryName
                         ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";

        Sql = string.Format(Sql, Where, ItemID);

        DataSet Ds = GetDataSet(Sql);



        if (Ds.Tables.Count < 1)
        {
            return new DataTable();
        }

        return Ds.Tables[0];
    }




    #endregion

    #region 实验数据分析

    /// <summary>
    /// 混凝土
    /// </summary>
    /// <param name="YCItemID"></param>
    /// <param name="QDDJ"></param>
    /// <param name="Attr"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataSet HNTQD(string YCItemID, string QDDJ, string Attr, string YLL, string YLLOffset, string StartDate, string EndDate, string SelectedTestRoomCodes)
    {
        string Sql = @"                            
                        select 
                        *
                        ,ROW_NUMBER() OVER (ORDER BY a.BGRQ ASC) AS OrderID
                        into #tmp
                         from
                        (
                        seleCT 
                        convert(varchar,a.BGRQ,23) as BGRQ,
                        a.BGBH,
                        a.DataID ,
                        (COALESCE(a.f1,0)+COALESCE(a.f2,0)+COALESCE(a.f3,0)+COALESCE(a.f4,0)+COALESCE(a.f5,0))/
                        ((case  when a.f1 is null then 0 else 1 end)+
                        (case  when  a.f2 is null then 0 else 1 end)+
                        (case  when  a.f3 is null then 0 else 1 end)+
                        (case  when  a.f4 is null then 0 else 1 end)+
                        (case  when  a.f5 is null then 0 else 1 end)) as avg 
                        from [dbo].[sys_TJ_MainData] a 
                        join [dbo].[sys_TJ_YCGL] b on a.DataID=b.HNTid
                        where a.qddj='{1}'  {3}  and 
                        (
                        f1 is not null or
                        f5 is not null or
                        f4 is not null or
                        f3 is not null or
                        f2 is not null 
                        ) 
                        and b.ycitemid='{0}'
                        ) as a

                        select * from #tmp 
                        select b.ycDataID as DataID,{2},a.OrderID ,c.BGBH,convert(varchar,c.BGRQ,23) as BGRQ
                        from #tmp a join [dbo].[sys_TJ_YCGL] b
                        on a.Dataid=b.hntid 
                        join [sys_TJ_MainData] c on b.ycdataid=c.DataID
                        where c.ItemID='{0}'
                        and {2} is not null
                        order by a.OrderID  
                         ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND a.BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND a.BGRQ <'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";
        Where += string.IsNullOrEmpty(SelectedTestRoomCodes) ? " " : "  AND a.TestRoomCode in (" + SelectedTestRoomCodes + ") ";
        

        int inyYLL = YLL.Toint();
        int inyYLLOffset = inyYLL == 0 ? 0 : YLLOffset.Toint();


        Where += inyYLL > 0 ? " AND b.YLL >= " + (inyYLL - inyYLLOffset) + " AND b.YLL <= " + (inyYLL + inyYLLOffset) + " " : "  ";

        Sql = string.Format(Sql, YCItemID, QDDJ, Attr, Where);

        DataSet Ds = GetDataSet(Sql);


        return Ds;
    }

    /// <summary>
    /// 原材 厂家供货吨位
    /// </summary>
    /// <param name="ItemID"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataTable TonByFact(string ItemID, string StartDate, string EndDate)
    {

        string Sql = @"  select 
                            a.FactoryID,
                            b.FactoryName,
                            CONVERT (decimal(18,2),sum(ShuLiang)) as val, 
                            count(a.ItemId) as c
                            from sys_TJ_MainData a
                            join sys_TJ_Factory  b on a.FactoryID=b.FactoryID
                            where ItemID='{1}' {0}
                            group by a.FactoryID,b.FactoryName
                            Order by val desc
                         ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <'" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";

        Sql = string.Format(Sql, Where, ItemID);

        DataSet Ds = GetDataSet(Sql);



        if (Ds.Tables.Count < 1)
        {
            return new DataTable();
        }

        return Ds.Tables[0];
    }

    /// <summary>
    /// 原材 监测指标 型号 年/半年
    /// </summary>
    /// <param name="ItemID"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static DataSet RawAttrByModel(string ItemID, string Attr, string AttrName, string Model, string FactoryID, string StartDate, string EndDate)
    {

        string Sql = @"     select 
                            {3},
                            {2},
                            bgrq,
                            StandardValue
                             from [dbo].[sys_TJ_MainData]
                          
                            left outer join sys_TJ_StandardValue on sys_TJ_StandardValue.ItemID = sys_TJ_MainData.ItemID
                            AND (sys_TJ_StandardValue.Model = {3} OR '/' = sys_TJ_StandardValue.Model  )
                            AND sys_TJ_StandardValue.ItemName = '{5}'
                            AND sys_TJ_StandardValue.ModuleID =  sys_TJ_MainData.ModuleID 
                            where sys_TJ_MainData.ItemID ='{1}' {0} AND  FactoryID ='{4}'

                            select {3}
                            from [dbo].[sys_TJ_MainData]
                            where ItemID ='{1}' {0} AND  FactoryID ='{4}'
                            group by {3}
                         ";

        

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <='" + EndDate + "' ";

        Sql = string.Format(Sql, Where, ItemID, Attr, Model, FactoryID, AttrName);


        return GetDataSet(Sql);
    }

    #endregion

    #region 统计查询




    /// <summary>
    /// 计算标准差
    /// </summary>
    /// <param name="Standard"></param>
    /// <param name="Val"></param>
    /// <returns></returns>
    public static string StandardDeviation(string Standard, string Val)
    {
        string Result = string.Empty;
        if (Standard.IsNullOrEmpty() || Val.IsNullOrEmpty())
        {
            return Result;
        }
        //已配置完毕。标准值为”/”、或多于两项及两项以上的、为用户自己填写的均未配置。
        //其中特殊项有：300～350、合格(沸煮法)、(C40及以上≤0.60)≤0.80等格式，已和胡工和田沟通。
        //另符号中包括：≥、≤、>、<。

  
        // ^[\d]+~[\d]+$   
        // ^[合格]+
        // \(\S*\)
        try
        {
            if (Regex.IsMatch(Standard, "\\(\\S*\\)"))
            {
                Standard = Regex.Replace(Standard, "\\(\\S*\\)", "");
            }

            if (Regex.IsMatch(Val, "^[合格]+"))
            {
                Result = "合格";
            }
            else if (Regex.IsMatch(Standard, "^[-|+]?\\d+\\.?\\d*～[-|+]?\\d+\\.?\\d*$"))
            {

                string[] Temp = Standard.Split(new char[] { '～' });
                Result = Val.Todouble(2) < Temp[0].Todouble(2) ?
                     (Temp[0].Todouble(2) - Val.Todouble(2)).ToString()
                    : Val.Todouble(2) > Temp[1].Todouble(2) ? (Val.Todouble(2) - Temp[1].Todouble(2)).ToString() : "合格";

              
               
            }
            else if (Regex.IsMatch(Standard, "^≥+"))
            {
                Standard = Regex.Replace(Standard, "^≥+", "");
                Result = (Val.Todouble(2) - Standard.Todouble(2)).ToString();
            }
            else if (Regex.IsMatch(Standard, "^≤+"))
            {
                Standard = Regex.Replace(Standard, "^≤+", "");
                Result = (Val.Todouble(2) - Standard.Todouble(2)).ToString();
            }
            else if (Regex.IsMatch(Standard, "^>+"))
            {
                Standard = Regex.Replace(Standard, "^>+", "");
                Result = (Val.Todouble(2) - Standard.Todouble(2)).ToString();
            }
            else if (Regex.IsMatch(Standard, "^<+"))
            {
                Standard = Regex.Replace(Standard, "^<+", "");
                Result = (Val.Todouble(2) - Standard.Todouble(2)).ToString();
            }
        }
        catch
        { }
        return Result;
    }


    /// <summary>
    /// 试验检测数据分析 
    /// </summary>
    /// <param name="RecordCount"></param>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="StartDate">起始日期</param>
    /// <param name="EndDate">结束日期</param>
    /// <param name="TestRoom">试验室</param>
    /// <param name="Item">原材</param>
    /// <param name="Factory">厂家</param>
    /// <param name="Attribute">检测指标</param>
    /// <param name="Model">型号</param>
    /// <returns></returns>
    public static DataTable DataAnalysis(out int RecordCount,int PageIndex,int PageSize,
        string StartDate,string EndDate,
        string TestRoom,
        string Item, string Factory, string Attribute, string AttributeName, string Model
        )
    {

        #region SQL
        string Sort = " ORDER BY BGRQ DESC ";

        if (PageSize > 10000)
        {
            Sort = "  ORDER BY BGRQ ASC  ";
        }

        string Clomun = @"
                        sys_tree.description , 
                        BGBH,DataID,
                        CONVERT(char(10),bgrq,23) as BGRQ , 
                        sys_TJ_Item.ItemName as 'YC',
                        FactoryName ,
                        sys_Module.Name as 'MName',
                        {0} as XH ,
                        sys_TJ_StandardValue.StandardValue as ZVal,
                        {1} as Val,
                        '' as BVal
                        ";

        string From = @" sys_TJ_MainData  
                        join sys_TJ_Item on sys_TJ_Item.ItemID = sys_TJ_MainData.ItemID
                        left outer join sys_tree on sys_tree.NodeCode = sys_TJ_MainData.testroomcode
                        left outer join sys_Module on sys_Module.ID = sys_TJ_MainData.ModuleID
                        left outer join sys_TJ_StandardValue on sys_TJ_StandardValue.ItemID = sys_TJ_MainData.ItemID
                        AND (sys_TJ_StandardValue.Model = {0} OR '/' = sys_TJ_StandardValue.Model  )
                        AND sys_TJ_StandardValue.ItemName = '{1}'
                        AND sys_TJ_StandardValue.ModuleID =  sys_TJ_MainData.ModuleID";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <='" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";
        Where += string.IsNullOrEmpty(TestRoom) ? " " : " AND  TestRoomCode in(" + TestRoom + ") ";
        Where += string.IsNullOrEmpty(Item) ? " " : "  AND sys_TJ_MainData.ItemID ='" + Item + "' ";
        Where += string.IsNullOrEmpty(Factory) ? " " : "  AND FactoryID ='" + Factory + "' ";

        ItemCollection Attr = BLOB.Attribute(Item);
        Where += string.IsNullOrEmpty(Model) ? " " : Attr["型号"]==null ? "" : "  AND  " + Attr["型号"].BindField + "  = '" + Model + "' ";
        Where += " AND (" + Attribute + " <> '' AND " + Attribute + " is not null ) ";

        Clomun = string.Format(Clomun, Attr.ContainsKey("型号") ? Attr["型号"].BindField : " ' '", Attribute);

        From = string.Format(From, Attr.ContainsKey("型号") ? Attr["型号"].BindField : " ' '", AttributeName);

        #endregion

        Attr.Clear();
        Attr = null;
        RecordCount = 0;
    
        string Sql = Expand.SqlPage(PageIndex, PageSize, "DataID", From, Where, Sort, Clomun);

        DataSet Ds = GetDataSet(Sql);

        

        if (Ds.Tables.Count < 1)
        {
            return new DataTable();
        }
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());


        foreach (DataRow Dr in Ds.Tables[0].Rows)
        {
            Dr["BVal"] = StandardDeviation(Dr["ZVal"].ToString(), Dr["Val"].ToString());
        }

        return Ds.Tables[0];
    }


    /// <summary>
    /// 厂家 监测指标 合格率
    /// </summary>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="TestRoom"></param>
    /// <param name="Item"></param>
    /// <param name="Factory"></param>
    /// <param name="Attribute"></param>
    /// <returns></returns>
    public static DataTable FactoryQualified(
        string StartDate, string EndDate,
        string TestRoom,
        string Item, string Factory, string Attribute)
    {

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <='" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";
        Where += string.IsNullOrEmpty(TestRoom) ? " " : " AND  TestRoomCode in(" + TestRoom + ") ";
        Where += string.IsNullOrEmpty(Item) ? " " : "  AND sys_TJ_MainData.ItemID ='" + Item + "' ";
        Where += string.IsNullOrEmpty(Factory) ? " " : "  AND FactoryID in(" + Factory + ") ";

        //
        ItemCollection Attr = BLOB.Attribute(Item);

        Where += " AND (" + Attribute + " <> '' AND " + Attribute + " is not null ) ";

        return null;
    }

    /// <summary>
    /// 施工部位的原材料使用情况 
    /// </summary>
    /// <param name="RecordCount"></param>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="TestRoom"></param>
    /// <param name="Item"></param>
    /// <returns></returns>
    public static DataTable ItemPosition(out int RecordCount, int PageIndex, int PageSize,
        string StartDate, string EndDate,
        string TestRoom,
        string Item
        )
    {

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
                        select sys_TJ_MainData.DataID from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          WHERE 1=1 
                          {0} 
 

                        SELECT 
                          sys_TJ_MainData.FactoryName as '生产厂家', 
                          sys_TJ_Item.ItemName as '原材',
                          {3} as '型号',
                          sys_TJ_MainData.SGBW as '工程部位',
                          sys_TJ_MainData.BGRQ as '报告日期',
                          sys_TJ_MainData.BGBH as '报告编号'
                           from  sys_TJ_MainData
                          JOIN sys_TJ_Item_Module on sys_TJ_MainData.ModuleID = sys_TJ_Item_Module.ModuleID 
                          Join sys_TJ_Item on sys_TJ_Item.ItemID = sys_TJ_Item_Module.ItemID
                        INNER JOIN @TempTable t ON sys_TJ_MainData.DataID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                         {0}        
                        Order By  BGRQ Desc

                        DECLARE @C int
                        select @C= count(DataID) from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          where 1=1 
                          {0}
                        select @C 
                        ";


        #endregion


        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <='" + DateTime.Parse(EndDate).AddDays(1).ToShortDateString() + "' ";
        Where += string.IsNullOrEmpty(TestRoom) ? " " : " AND  TestRoomCode in(" + TestRoom + ") ";
        Where += string.IsNullOrEmpty(Item) ? " " : "  AND sys_TJ_MainData.ItemID ='" + Item + "' ";

       ItemCollection Attr = BLOB.Attribute(Item);

        Sql = string.Format(Sql,
            Where,
            PageIndex,
            PageSize,
            Attr.ContainsKey("型号") ? Attr["型号"].BindField : " ' ' "
            );
        Attr.Clear();
        Attr = null;
        RecordCount = 0;
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());
        return Ds.Tables[0];
    }


    /// <summary>
    /// 施工部位的原材料使用情况
    /// </summary>
    /// <param name="RecordCount"></param>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="TestRoom"></param>
    /// <param name="Position"></param>
    /// <returns></returns>
    public static DataTable PositionItem(out int RecordCount, int PageIndex, int PageSize,
        string StartDate, string EndDate,
        string TestRoom,
        string Position
        )
    {

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
                        select sys_TJ_MainData.DataID from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          WHERE 1=1 
                          {0} 
 

                        SELECT 
                          sys_TJ_MainData.FactoryName as '生产厂家', 
                          sys_TJ_Item.ItemName as '原材',
                          {3} as '型号',
                          sys_TJ_MainData.SGBW as '工程部位',
                          sys_TJ_MainData.BGRQ as '报告日期',
                          sys_TJ_MainData.BGBH as '报告编号'
                           from  sys_TJ_MainData
                          JOIN sys_TJ_Item_Module on sys_TJ_MainData.ModuleID = sys_TJ_Item_Module.ModuleID 
                          Join sys_TJ_Item on sys_TJ_Item.ItemID = sys_TJ_Item_Module.ItemID
                        INNER JOIN @TempTable t ON sys_TJ_MainData.DataID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                         {0}        
                        Order By  BGRQ Desc

                        DECLARE @C int
                        select @C= count(DataID) from [dbo].[sys_TJ_MainData] 
                          join [dbo].[sys_TJ_Item_Module] on [dbo].[sys_TJ_MainData].ModuleID = [dbo].[sys_TJ_Item_Module].ModuleID
                          where 1=1 
                          {0}
                        select @C 
                        ";


        #endregion

        

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <='" + DateTime.Parse(EndDate).AddDays(1).ToShortDateString() + "' ";
        Where += string.IsNullOrEmpty(TestRoom) ? " " : " AND  TestRoomCode in(" + TestRoom + ") ";
        Where += string.IsNullOrEmpty(Position) ? " " : "  AND sys_TJ_MainData.SGBW ='" + Position + "' ";

 

        Sql = string.Format(Sql,
            Where,
            PageIndex,
            PageSize,
             " ' ' "
            );

        RecordCount = 0;
        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());
        return Ds.Tables[0];
    }


    /// <summary>
    /// 采集分析
    /// </summary>
    /// <param name="RecordCount"></param>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
    /// <param name="StartDate"></param>
    /// <param name="EndDate"></param>
    /// <param name="TestRoom"></param>
    /// <param name="Item"></param>
    /// <param name="Factory"></param>
    /// <param name="Model"></param>
    /// <param name="Person"></param>
    /// <returns></returns>
    public static DataTable SamplingAnalysis(out int RecordCount, int PageIndex, int PageSize,
        string StartDate, string EndDate,
        string TestRoom,
        string Item, string Factory, string Model, string Person
        )
    {

        #region SQL
        string Sort = " ORDER BY BGRQ DESC ";

        if (PageSize > 10000)
        {
            Sort = "  ORDER BY BGRQ ASC  ";
        }

        string Clomun = @"
                        t1.description+' '+ sys_tree.description as  description, 
                        BGBH,
                        CONVERT(char(10),bgrq,23) as BGRQ , 
                        sys_TJ_Item.ItemName as 'YC',
                        FactoryName ,
                        sys_Module.Name as 'MName',
                        
                        {1} ,
                        {2} as C,
                        {3} as P
                        ,biz_machinelist.col_norm_c6 as XH
                        ";
        //{0} as XH ,
        string From = @" sys_TJ_MainData  
                        join sys_TJ_Item on sys_TJ_Item.ItemID = sys_TJ_MainData.ItemID
                        left outer join sys_tree on sys_tree.NodeCode = sys_TJ_MainData.testroomcode
                        left outer join sys_tree as t1 on t1.NodeCode = left(sys_TJ_MainData.testroomcode,8)
                        left outer join sys_Module on sys_Module.ID = sys_TJ_MainData.ModuleID
                        left outer join  biz_machinelist on biz_machinelist.scpt = {0}
                       ";

        string Where = "  ";

        Where += string.IsNullOrEmpty(StartDate) ? " " : " AND BGRQ >='" + StartDate + "' ";
        Where += string.IsNullOrEmpty(EndDate) ? " " : "  AND BGRQ <='" + DateTime.Parse(EndDate).AddDays(1).ToString("yyyy-MM-dd") + "' ";
        Where += string.IsNullOrEmpty(TestRoom) ? " " : " AND  TestRoomCode in(" + TestRoom + ") ";
        Where += string.IsNullOrEmpty(Item) ? " " : "  AND sys_TJ_MainData.ItemID ='" + Item + "' ";
        Where += string.IsNullOrEmpty(Factory) ? " " : "  AND FactoryID ='" + Factory + "' ";

        ItemCollection Attr = BLOB.Attribute(Item);
        Where += string.IsNullOrEmpty(Model) ? " " : Attr["设备型号"] == null ? "" : "  AND  " + Attr["设备型号"].BindField + "  = '" + Model + "' ";
        Where += string.IsNullOrEmpty(Person) ? " " : Attr["试验员"] == null ? "" : "  AND  " + Attr["试验员"].BindField + "  = '" + Person + "' ";


        string Temp = string.Empty;
        int cc = 1;
        foreach (Item K in Attr)
        {
            if ((K.ItemName.IndexOf("抗拉") > -1) || K.ItemName.IndexOf("强度值") > -1)
            {
                Temp += Temp.IsNullOrEmpty() ? K.BindField + " as  Val" + cc : "  ," + K.BindField + " as  Val" + cc;
                cc++;
            }
        }

        Clomun = string.Format(Clomun,
            Attr.ContainsKey("设备型号") ? Attr["设备型号"].BindField : " ' '",
            !Temp.IsNullOrEmpty() ? Temp : " ' '",
            Attr.ContainsKey("代表数量") ? Attr["代表数量"].BindField : " ' '",
             Attr.ContainsKey("试验人员") ? Attr["试验人员"].BindField : " ' '"
            );

        From =  string.Format(From,Attr.ContainsKey("设备型号") ? Attr["设备型号"].BindField : " ' '");
        #endregion

        Attr.Clear();
        Attr = null;
        RecordCount = 0;

        string Sql = Expand.SqlPage(PageIndex, PageSize, "DataID", From, Where, Sort, Clomun);

        DataSet Ds = GetDataSet(Sql);

        if (Ds.Tables.Count < 1)
        {
            return new DataTable();
        }
        RecordCount = int.Parse(Ds.Tables[1].Rows[0][0].ToString());



        return Ds.Tables[0];
    }



    #endregion


    /// <summary>
    /// 数据查询
    /// </summary>
    /// <param name="Sql"></param>
    /// <returns></returns>
    public static DataSet GetDataSet(string Sql)
    {
        DataSet DS = new DataSet();
        using (SqlConnection _Conn = new BLL_Document().Connection as SqlConnection)
        {
            _Conn.Open();
            using (SqlCommand _Cmd = new SqlCommand(Sql, _Conn))
            {
                using (SqlDataAdapter _Adp = new SqlDataAdapter(_Cmd))
                {
                    _Adp.Fill(DS);
                }
            }
            _Conn.Close();
            _Conn.Dispose();
        }
        return DS;
    }
}

/// <summary>
/// 监测指标
/// </summary>
public class Item
{
    public string ItemName;
    public string BindField;
    public string StandardBindField;
}

[Serializable]
public class ItemCollection : CollectionBase
{
    #region

    public ItemCollection()
    {
       
    }

    public Item this[string ItemName]
    {
        get
        {
            foreach (object i in List)
            {
                if (((Item)i).ItemName == ItemName)
                {
                    return i as Item;
                }
            }
            return null;
        }
  
    }

    public int Add(Item Val)
    {
        return List.Add(Val);
    }

    public void Remove(string ItemName)
    {
        ArrayList Temp = new ArrayList(List);

        for (int i = 0; i < Temp.Count; i++)
        {


            if (((Item)Temp[i]).ItemName == ItemName)
            {
                List.RemoveAt(i);
                break;
            }
        }
    }

    public bool ContainsKey(string ItemName)
    {
        return this[ItemName] != null;
    }


    public void Copy(IEnumerable<Item> arr)
    {
        foreach (Item i in arr)
        {
            this.List.Add(i);
        }
    }

    #endregion

}
