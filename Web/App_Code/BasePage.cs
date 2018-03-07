using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using JZ.BLL;

/// <summary>
/// BasePage 的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public String UserName
    {
        get
        {
            return Session["UserName"].ToString();
        }
    }

    public String Project
    {
        get
        {
            return Session["Project"].ToString();
        }
    }

    public ProjectModel ProjectModel
    {
        get
        {
            return Session["ProjectModel"] as ProjectModel;
        }
    }

    public String SelectedTestRoomCodes
    {
        get
        {
            if (Session["SelectedTestRoomCodes"] == null)
            {
                return "";
            }
            return Session["SelectedTestRoomCodes"].ToString();
        }
        set
        {
            Session["SelectedTestRoomCodes"] = value;
        }
    }

    public String StartDate
    {
        get
        {
            if (Session["StartDate"] == null)
            {
                Session["StartDate"] = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");                
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

    protected override void OnLoad(EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("~/login.aspx");
        }
        base.OnLoad(e);
    }

    /// <summary>
    /// 获取当前要点击的单位下的被选择的试验室
    /// </summary>
    /// <param name="testcode"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string GetSelectTree(string testcode, string s)
    {
        List<String> testlist = new List<String>();

        if (s.Contains(","))
        {
            string[] arr = s.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Contains("" + testcode + ""))
                {
                    testlist.Add(arr[i]);
                }
            }
            testcode = String.Join(",", testlist.ToArray());
        }
        else
        {
            testcode = s;
        }
        return testcode;
    }

    #region 截取字符长度
    /// <summary>
    /// 截取字符长度
    /// </summary>
    /// <param name="inputString">字符</param>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string CutString(string inputString, int len)
    {
        if (string.IsNullOrEmpty(inputString))
            return "";
        ASCIIEncoding ascii = new ASCIIEncoding();
        int tempLen = 0;
        string tempString = "";
        byte[] s = ascii.GetBytes(inputString);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }

            try
            {
                tempString += inputString.Substring(i, 1);
            }
            catch
            {
                break;
            }

            if (tempLen > len)
                break;
        }
        //如果截过则加上半个省略号 
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
        if (mybyte.Length > len)
            tempString += "…";
        return tempString;
    }

    public static string CutString(object objString, int len)
    {
        string inputString = objString.ToString();
        if (string.IsNullOrEmpty(inputString))
            return "";
        ASCIIEncoding ascii = new ASCIIEncoding();
        int tempLen = 0;
        string tempString = "";
        byte[] s = ascii.GetBytes(inputString);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }

            try
            {
                tempString += inputString.Substring(i, 1);
            }
            catch
            {
                break;
            }

            if (tempLen > len)
                break;
        }
        //如果截过则加上半个省略号 
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
        if (mybyte.Length > len)
            tempString += "…";
        return tempString;
    }
    #endregion

}