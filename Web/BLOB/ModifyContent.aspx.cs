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
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;


public partial class BLOB_ModifyContent : BasePage
{
    #region 参数


    public SYS_ModifyOBJ Obj = new SYS_ModifyOBJ();


    public string PersonType = "JL";

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
        //设置 是监理 还是业主
        PersonType = "rolename".SessionStr().ToString() == "A" ? "YZ" :
            "rolename".SessionStr().ToString() == "D" ? "JL" : "";

        if (!"ID".RequestStr().IsNullOrEmpty())
        {
            GetObj();
        }



        if (Request["ACT"] != null && Request["ACT"].ToString() == "EDIT")
        {
            Response.Write(Edit());
            Response.End();
        }




    }
    #endregion


    #region 方法




    /// <summary>
    /// 添加
    /// </summary>
    public string Edit()
    {
        string Result = "";
        try
        {

            string KMID = Request["KMID"].ToString();
            string Content = Request["Content"].ToString();
            string Person = UserName;
            //rolename


            string Sql = @"

                UPDATE [dbo].[sys_KeyModify]
                          SET 
                           [YZUserName] = '{0}'
                          ,[YZContent] = '{1}'
                          ,[YZOPTime] = '{2}'
                          ,Status={4}
                   WHERE KMID = '{3}'

";

            Sql = string.Format(Sql, Person, Content, DateTime.Now.ToString(), KMID, "Type".RequestStr());
            BLL_Document BLL = new BLL_Document();
            


            Result = BLL.ExcuteCommand(Sql) > 0 ? "true" : "false";

            try
            {
                #region 发短信
                if ("Type".RequestStr() == "2" && Result == "true")
                {


                    sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
                    DataSet DsTemp = LineDbHelperSQL.Query("SELECT [SMSState] FROM [sys_line] where ID ='" + sysBaseLine.ID + "'");
                    if (DsTemp.Tables.Count > 0 && DsTemp.Tables[0].Rows.Count > 0 && DsTemp.Tables[0].Rows[0][0].ToString() == "1")
                    {

                        string SMSSql = @"
                                 SELECT sys_module.name,sys_KeyModify.BGBH,sys_KeyModify.ModifyItem,sys_KeyModify.YzUserName,
                        t1.DESCRIPTION +' '+sys_tree.DESCRIPTION as DESCRIPTION,TestRoomCode
                         FROM sys_KeyModify
                        left outer join sys_tree on sys_KeyModify.testroomcode = sys_tree.nodecode
                        left outer join sys_tree as t1 on Left(sys_KeyModify.testroomcode,8) = t1.nodecode
                        left outer join sys_module on sys_module.id = sys_KeyModify.moduleid
                        where sys_KeyModify.KMID = '" + KMID + "'";

                        DataSet Ds = BLL.GetDataSet(SMSSql);

                        string SmsContent = "{0} {1} {2} {3} {4}拒绝";
                        if (Ds.Tables[0] != null && Ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow Dr = Ds.Tables[0].Rows[0];

                            ModifyItem[] Temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ModifyItem[]>(Dr["ModifyItem"].ToString());

                            SmsContent = string.Format(SmsContent
                                , Dr["DESCRIPTION"].ToString()
                                , Dr["name"].ToString()
                                , Dr["BGBH"].ToString()
                                , Temp[0].Description + Temp[0].CurrentValue + "修改为" + Temp[0].OriginalValue
                                , Dr["YzUserName"].ToString()
                                );



                            string CPS = @"  SELECT CellPhone
                        FROM [SYGLDB_ZhengXu].[dbo].[sys_sms_receiver]
                        where TestRoomCode ='" + Ds.Tables[0].Rows[0]["TestRoomCode"].ToString() + "' and CellPhone <>'' ANd CellPhone is not null and IsActive=1";

                            string MS = "";
                            Ds = BLL.GetDataSet(CPS);
                            if (Ds.Tables[0] != null && Ds.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow Dr1 in Ds.Tables[0].Rows)
                                {
                                    if (!Dr1[0].ToString().IsNullOrEmpty())
                                    {
                                        MS += MS.IsNullOrEmpty() ? Dr1[0].ToString() : "," + Dr1[0].ToString();
                                    }
                                }
                            }
                            if (!MS.IsNullOrEmpty())
                            {
                                string SMSResult = SendSMS("Mobile=" + MS + "&Content=" + SmsContent + "&Stime=" + DateTime.Now.ToString() + "&Extno=1");
                            }

                        }
                    }
                }
                #endregion
            }
            catch { }

        }
        catch
        {
            Result = "false";
        }
        return Result;
    }



    /// <summary>
    /// 获取数据库实体
    /// </summary>
    public void GetObj()
    {
        string Sql = @" select * from sys_KeyModify where KMID = '" + "ID".RequestStr() + "' ";


        BLL_Document BLL = new BLL_Document();

        using (IDataReader Read = BLL.ExcuteReader(Sql))
        {

            try
            {
                Read.Read();
                Obj.KMID = Read["KMID"].ToString();

                Obj.Person = Read["YZUserName"].ToString();
                Obj.Content = Read["YZContent"].ToString();
                Obj.ModifyTime = Read["YZOPTime"].ToString();

            }
            catch { }

            Read.Close();
            Read.Dispose();
        }

    }





    public string SendSMS(string PostData)
    {
        string Result = string.Empty;
        #region 发送 接收
        try
        {
            byte[] SendBytes = Encoding.UTF8.GetBytes(PostData);
            System.Net.HttpWebRequest request = System.Net.HttpWebRequest.Create("http://115.29.206.137:8081/myservice.asmx/SendSMS") as System.Net.HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = SendBytes.Length;

            using (System.IO.Stream newStream = request.GetRequestStream())
            {
                newStream.Write(SendBytes, 0, SendBytes.Length);
                newStream.Close();
            }

            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
            {

                using (System.IO.StreamReader readStream = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    Result = readStream.ReadToEnd();
                    readStream.Close();
                    readStream.Dispose();
                }
                response.Close();

            }
        }
        catch
        { }
        #endregion

        return Result;
    }

    #endregion
}


/// <summary>
/// 实体
/// </summary>
public class SYS_ModifyOBJ
{
    public string KMID;
    public string Content;
    public string Person;
    public string ModifyTime;

}

public class ModifyItem
{
    public string Description;
    public string CellPosition;
    public string OriginalValue;
    public string CurrentValue;

}