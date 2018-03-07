using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

//解决编译报错
namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}

public static class Expand
{

   

    #region

    /// <summary>
    /// 是否Session
    /// </summary>
    /// <param name="Para"></param>
    /// <returns></returns>
    public static bool IsSession(this string Para)
    {

        return System.Web.HttpContext.Current.Session[Para] == null
                             ? false
                             : true;
    }

    /// <summary>
    /// 获取Request值
    /// </summary>
    /// <param name="Para">键</param>
    /// <returns>值</returns>
    public static object SessionStr(this string Para)
    {

        return System.Web.HttpContext.Current.Session[Para] == null
            ? null
            : System.Web.HttpContext.Current.Session[Para];
    }

    /// <summary>
    /// 是否有Request请求
    /// </summary>
    /// <param name="Para"></param>
    /// <returns></returns>
    public static bool IsRequest(this string Para)
    {
        return System.Web.HttpContext.Current.Request[Para] == null
                       ? false
                       : true;
    }


    /// <summary>
    /// 是否有Request请求
    /// </summary>
    /// <param name="Para"></param>
    /// <returns></returns>
    public static bool IsRequest(this string Para, bool IsForm)
    {
        if (IsForm)
        {
            return System.Web.HttpContext.Current.Request.Form[Para] == null
                           ? false
                           : true;
        }
        else
        {
            return IsRequest(Para);
        }
    }


    /// <summary>
    /// 获取Request值
    /// </summary>
    /// <param name="Para">键</param>
    /// <returns>值</returns>
    public static string RequestStr(this string Para)
    {

        return System.Web.HttpContext.Current.Request[Para] == null
            ? null
            : System.Web.HttpContext.Current.Request[Para];
    }

    /// <summary>
    /// 获取Request值
    /// </summary>
    /// <param name="Para">键</param>
    /// <returns>值</returns>
    public static string RequestStr(this string Para, bool IsForm)
    {
        if (IsForm)
        {
            return System.Web.HttpContext.Current.Request.Form[Para] == null
          ? null
          : System.Web.HttpContext.Current.Request.Form[Para];
        }
        else
        {
            return RequestStr(Para);
        }
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="Para"></param>
    /// <returns></returns>
    public static string MD5(this string Para)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Para, "MD5");
    }



    /// <summary>
    /// 是否为空
    /// </summary>
    /// <param name="Obj"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string Obj)
    {
        return string.IsNullOrEmpty(Obj);
    }

    /// <summary>
    /// 获取虚拟路径
    /// </summary>
    /// <param name="Url"></param>
    /// <returns></returns>
    public static string ServerUrl(this string Url)
    {
        return string.IsNullOrEmpty(Url) ? "" : new System.Web.UI.Control().ResolveUrl(Url);
    }


    #endregion

    #region 类型转换


    /// <summary>
    /// 转换Bool
    /// </summary>
    /// <param name="Para"></param>
    /// <returns></returns>
    public static bool ToBool(this string Para)
    {
        bool Result = false;
        bool.TryParse(Para, out Result);

        return Result;
    }

    /// <summary>
    /// 转换返回Int32类型
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static Int32 Toint(this object num)
    {
        int number = 0;
        num = num == null ? "" : num;
        int.TryParse(num.ToString(), out number);

        return number;
    }

    /// <summary>
    /// 转换为浮点数
    /// </summary>
    /// <param name="num"></param>
    /// <param name="ditits"></param>
    /// <returns></returns>
    public static double Todouble(this object num, int ditits)
    {
        double number = 0;
        num = num == null ? "" : num;
        double.TryParse(num.ToString(), out number);

        //number = Math.Round(number, ditits);
        if (number < 0)
        {
            return Math.Round(number + 5 / Math.Pow(10, ditits + 1), ditits, MidpointRounding.AwayFromZero);
        }
        else
        {
            return Math.Round(number, ditits, MidpointRounding.AwayFromZero);
        }

    }

    /// <summary>
    /// 转换为浮点数
    /// </summary>
    /// <param name="num"></param>
    /// <param name="ditits"></param>
    /// <returns></returns>
    public static double Todouble(this object num)
    {
        double number = 0;
        num = num == null ? "" : num;
        double.TryParse(num.ToString(), out number);

        return number;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static decimal Todecimal(this object num)
    {
        decimal number = 0;
        num = num == null ? "" : num;
        decimal.TryParse(num.ToString(), out number);

        Math.Round(number, 1);
        return number;
    }
    /// <summary>
    /// 转换类型
    /// </summary>
    /// <param name="v"></param>
    /// <returns>v/DateTime.MinValue</returns>
    public static DateTime ToDateTime(this string v)
    {
        DateTime Result = DateTime.MinValue;

        DateTime.TryParse(v, out Result);

        return Result;
    }

    #endregion

    #region WebConfig值

    /// <summary>
    /// 获取AppSettings
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    public static string GetAppSettings(this string Key)
    {
        string Result = string.Empty;

        Result = (System.Configuration.ConfigurationManager.AppSettings[Key] == null)
            ? Result
        : System.Configuration.ConfigurationManager.AppSettings[Key].ToString();

        return Result;
    }
    #endregion

    #region 页面脚本执行

    /// <summary>
    /// 弹出提示
    /// </summary>
    /// <param name="Msg"></param>
    public static void Alert(this string Msg, System.Web.UI.Page Page)
    {
        ExeScript("alert('" + Msg + "');", Page);

    }


    /// <summary>
    /// 执行页面脚本
    /// </summary>
    /// <param name="Script"></param>
    public static void ExeScript(this string Script, System.Web.UI.Page Page)
    {
        ExeScript(Script, Page, true);

    }

    /// <summary>
    /// 执行页面脚本
    /// </summary>
    /// <param name="Script"></param>
    public static void ExeScript(this string Script, System.Web.UI.Page Page, bool AddTag)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), Script, AddTag);
    }


    /// <summary>
    /// 弹出提示
    /// </summary>
    /// <param name="Msg"></param>
    public static void Alert(this string Msg, UpdatePanel Panel, System.Web.UI.Page Page)
    {
        ExeScript("alert('" + Msg + "');", Panel, Page);

    }

    /// <summary>
    /// 执行页面脚本
    /// </summary>
    /// <param name="Script"></param>
    public static void ExeScript(this string Script, UpdatePanel Panel, System.Web.UI.Page Page)
    {
        ExeScript(Script, Panel, Page, true);

    }

    /// <summary>
    /// 执行页面脚本
    /// </summary>
    /// <param name="Script"></param>
    public static void ExeScript(this string Script, UpdatePanel Panel, System.Web.UI.Page Page, bool AddTag)
    {


        ScriptManager.RegisterStartupScript(Panel, Page.GetType(), Guid.NewGuid().ToString(), Script, AddTag);


    }

    #endregion

    #region 数值比较

    /// <summary>
    /// 数字在两者之间（包含s e）
    /// </summary>
    /// <param name="o"></param>
    /// <param name="s"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public static bool IsBetween(this int o, int s, int e)
    {
        bool Result = false;

        if (o >= s && o <= e)
            Result = true;

        return Result;
    }

    /// <summary>
    /// 数字在两者之间（不包含s e）
    /// </summary>
    /// <param name="o"></param>
    /// <param name="s"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public static bool IsIn(this int o, int s, int e)
    {
        bool Result = false;

        if (o > s && o < e)
            Result = true;

        return Result;
    }

    #endregion

    #region 字符串

    /// <summary>
    /// 分割字符串，判断是否包含相同字符串
    /// </summary>
    /// <param name="Str"></param>
    /// <param name="Split"></param>
    /// <param name="Str1"></param>
    /// <param name="Split1"></param>
    /// <returns></returns>
    public static bool IsSplitIn(this string Str, string Split, string Str1, string Split1)
    {
        bool Result = false;

        if (!Str.IsNullOrEmpty() && !Str1.IsNullOrEmpty())
        {
            foreach (string _str in Str.Split(new string[] { Split }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (string _str1 in Str1.Split(new string[] { Split1 }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.Equals(_str, _str1))
                    {
                        Result = true;
                        return Result;

                    }
                }
            }
        }

        return Result;
    }

    /// <summary>
    /// 判断 是否等去其中一个
    /// </summary>
    /// <param name="Str"></param>
    /// <param name="Val"></param>
    /// <returns></returns>
    public static bool IsEqualsOr(this string Str,params string[] Val)
    {
        bool Result = false;

        foreach (string v in Val)
        {
            if (v == Str)
            {
                Result = true;
                break;
            }
        }
        return Result;
    }
    #endregion


    #region DataTable

    /// <summary>
    /// DataTable转换JSON DataTime类型列转换为String类型
    /// </summary>
    /// <param name="Table"></param>
    /// <param name="TableName"></param>
    /// <param name="Sort"></param>
    /// <returns></returns>
    public static string ToJson(this DataTable Table, string Sort )
    {
        string Result = string.Empty;
        try
        {
            if (Table.Rows.Count <= 0)
            {
                throw new Exception();
            }
            string XML = string.Empty;
            bool IsHasDatac = false;

            #region 检查是否存在日期格式
            foreach (DataColumn Dc in Table.Columns)
            {
                if (Dc.DataType == typeof(System.DateTime))
                {
                    IsHasDatac = true;
                    break;
                }
            }
            #endregion

            #region 生成XML
            if (IsHasDatac)
            {
                DataTable T = Table.Clone();
                foreach (DataColumn Dc in T.Columns)
                {
                    if (Dc.DataType == typeof(System.DateTime))
                    {
                        Dc.DataType = typeof(String);
                    }
                }

                T.Load(Table.CreateDataReader());
                System.IO.StringWriter W = new System.IO.StringWriter();
                if (!string.IsNullOrEmpty(Sort) && Table.Rows.Count > 0)
                {
                    T.DefaultView.Sort = Sort;
                }

                T.TableName = T.TableName.IsNullOrEmpty() ? "Temp" : T.TableName;
                T.DefaultView.ToTable().WriteXml(W);
                XML = W.ToString();
                W.Close();
                W.Dispose();
                T.Clear();
                T.Dispose();
            }
            else
            {
                System.IO.StringWriter W = new System.IO.StringWriter();
                if (!string.IsNullOrEmpty(Sort) && Table.Rows.Count > 0)
                {
                    Table.DefaultView.Sort = Sort;
                }

                Table.TableName = Table.TableName.IsNullOrEmpty() ? "Temp" : Table.TableName;
                Table.DefaultView.ToTable().WriteXml(W);
                XML = W.ToString();
                W.Close();
                W.Dispose();
            }
            #endregion

            #region 替换 格式
            XML = XML.Replace("<DocumentElement>", "<data>")
                .Replace("</DocumentElement>", "</data>")
                .Replace("<DocumentElement />", "");


            XML = XML.Replace("<Table>", String.Concat("<rows>")).Replace("</Table>", String.Concat("</rows>"));
            System.Xml.XmlDocument XMLD = new System.Xml.XmlDocument();
            XMLD.LoadXml(XML);
            Result = XmlToJSONParser.XmlToJSON(XMLD);
            Result = Result.Replace(@"\", @"\\");
            #endregion
        }
        catch
        {

        }
        return Result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Table"></param>
    /// <param name="Sort"></param>
    /// <returns></returns>
    public static string ToJsonForEasyUI(this DataTable Table, int total, string Sort)
    {
        string Json = Table.ToJson(Sort);

        Json = Json.IsNullOrEmpty() ? Json : Json.Substring(14, Json.Length - 16);

        if (total == 1 || Table.Rows.Count == 1)
        {
            Json = "[" + Json + "]";
        }
        Json = Json.IsNullOrEmpty() ? "[]" : Json;
        return "{\"rows\":" + Json + ",\"total\":" + total + "}";
    }

 

    #endregion

    #region Dictorary

    public static string ToJson(this Dictionary<string,string> Obj)
    {
        string Result = string.Empty;
        
        foreach (KeyValuePair<string, string> K in Obj)
        {
            Result += string.IsNullOrEmpty(Result) ? "{\"Key\":\"" + K.Key + "\",\"Val\":\"" + K.Value + "\"}" : ",{\"Key\":\"" + K.Key + "\",\"Val\":\"" + K.Value + "\"}";
        }
        Result = "[" + Result + "]";
        return Result;
    }
    #endregion


    public static string SqlPage(
        int Page
        ,int PageSize
        , string Key
        , string From
        ,string Where
        ,string Sort
        , string Cloumn
        )
    {

        string Temp = string.Format(_SqlPage,
            Page.ToString()
            , PageSize.ToString()
            , Key
            , From
            , Where
            , Sort
            , Cloumn);

        return Temp;
    }

    private static string _SqlPage= @" 
                        DECLARE @Page int
                        DECLARE @PageSize int
                        SET @Page = {0}
                        SET @PageSize = {1}
                        SET NOCOUNT ON
                        DECLARE @TempTable TABLE (IndexId int identity, _keyID varchar(200))
                        INSERT INTO @TempTable
                        (
	                        _keyID
                        )
                        select 
                        {2} 
                        from 
                        {3} 
                        WHERE 1=1 
                        {4} 
                        {5}

                        Select 
                        {6} 
                        FROM 
                        {3} 
                        INNER JOIN @TempTable t ON sys_TJ_MainData.DataID = t._keyID
                        WHERE t.IndexId BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
                        {4}        
                        {5}

                        DECLARE @C int
                        select @C= count({2}) 
                        from 
                        {3} 
                        where 1=1 
                        {4}
                        select @C ";

}


