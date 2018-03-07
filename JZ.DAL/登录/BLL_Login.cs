using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Yqun.Data.DataBase;
using Yqun.Common.Encoder;
using System.Configuration;


namespace JZ.BLL
{
    public class BLL_Login : JZ.BLL.BOBase
    {
        //使用log4net.dll日志接口实现日志记录
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        String projectKey;
        public BLL_Login(string projectKey)
            : base(projectKey)
        {
            this.projectKey = projectKey;
        }

    

        public string CheckUser_BaseSys(string UserName, string Password)
        {
            string Result = CallService("IdentityAuth", "UserName=" + UserName + "&Password=" + Password + "&Site=sys");

            ResultInfo RI = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultInfo>(Result);


            if (RI.Result)
            {
                CreateProjectModel_BaseSys(RI.LineID);
                System.Web.HttpContext.Current.Session["rolename"] = RI.RoleName;
                System.Web.HttpContext.Current.Session["UserInfo"] = RI;
                return "true";
            }
            else
            {
                return RI.Msg;
            }
        }
        
       

        public void CreateProjectModel_BaseSys(string appString)
        {
            if (!String.IsNullOrEmpty(appString))
            {
                DataSet ds = new DataSet();
                string SqlText = "SELECT * FROM  dbo.sys_line WHERE ID='" + appString + "'";
                ds = LineDbHelperSQL.Query(SqlText);
                sys_line sysBaseLine = new sys_line();
                if (ds != null && ds.Tables.Count > 0)
                {
                   
                    #region 保存当前选择线路的值，添加的用户的时候判断是否重复,以及区分新旧线
                  
                    sysBaseLine.ID = new Guid(ds.Tables[0].Rows[0]["ID"].ToString());
                    sysBaseLine.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    sysBaseLine.DataBaseName = ds.Tables[0].Rows[0]["DataBaseName"].ToString();
                    sysBaseLine.DataSourceAddress = ds.Tables[0].Rows[0]["DataSourceAddress"].ToString();
                    sysBaseLine.IPAddress = ds.Tables[0].Rows[0]["IPAddress"].ToString();
                    sysBaseLine.IsActive = System.Convert.ToInt32(ds.Tables[0].Rows[0]["IsActive"].ToString());
                    sysBaseLine.LineName = ds.Tables[0].Rows[0]["LineName"].ToString();
                    sysBaseLine.PassWord = ds.Tables[0].Rows[0]["PassWord"].ToString();
                    sysBaseLine.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    sysBaseLine.Domain = ds.Tables[0].Rows[0]["Domain"].ToString();  
                    #endregion
                }
                 SqlText = @"INSERT dbo.sys_BaseLine_UsersLoginLog
        ( UserName ,
          IpAddress ,
          LoginTime ,
          LineName ,
          Desic ,
          biz1 ,
          biz2
        )
VALUES  ( '" + System.Web.HttpContext.Current.Session["UserName"].ToString() + "' , '" + System.Web.HttpContext.Current.Request.UserHostAddress + "' , '" + DateTime.Now.ToString() + "' ,'" + sysBaseLine.ID + "' , '" + sysBaseLine.LineName + "' ,'' ,'' )";
                 LineDbHelperSQL.ExecuteSql(SqlText);
                System.Web.HttpContext.Current.Session["SysBaseLine"] = sysBaseLine;
            }
        }



        public string CallService(string Method, string Data)
        {
            string Result = string.Empty;
            #region 发送 接收
            try
            {

                byte[] SendBytes = Encoding.UTF8.GetBytes(Data);
                System.Net.HttpWebRequest request = System.Net.HttpWebRequest.Create("http://admin.kingrocket.com/Sevrice/Line.asmx/" + Method) as System.Net.HttpWebRequest;
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



            Result = Result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            Result = Result.Replace("<string xmlns=\"XXSystem.Lines.Service\">", "");
            Result = Result.Replace("</string>", "");
            Result = Result.Replace("\r\n", "");
            return Result;
        }



      


    }

     [Serializable]
    public class ResultInfo
    {

         public bool Result = false;
         public string Msg = string.Empty;
         public string ID = string.Empty;
         public string UserName = string.Empty;
         public string Password = string.Empty;
         public string CurrentSite = string.Empty;
         public string Phone = string.Empty;
         public string PhonePW = string.Empty;
         public string TrueName = string.Empty;
         public string LineID = string.Empty;
         public string RoleName = string.Empty;
         public string GLActive = string.Empty;
         public string CJActive = string.Empty;
         public string SysBSActive = string.Empty;
         public string BhzBSActive = string.Empty;
         public string AppActive = string.Empty;
         public string AdminActive = string.Empty;
         public string Line = string.Empty;
         public string Room = string.Empty;


    }
}
