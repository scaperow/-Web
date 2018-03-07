using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using FarPoint.Web.Spread;
using FarPoint.Win;
using FarPoint.Win.Spread;
using System.IO;
using System.Text;
using BizCommon;
using BizFunctionInfos;
using BizComponents;
using Newtonsoft;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;
using FarPoint.CalcEngine;

public partial class report_TestRoomInfo : BasePage
{
    #region

    public string LineName = string.Empty;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            GetLineName();

            String id = Request.Params["id"];
            string MName = Request.Params["MName"];
            if (!String.IsNullOrEmpty(id))
            {
                if (Session["SysBaseLine"] != null)
                {
                    StringBuilder sb1 = new StringBuilder();
                    JZ.BLL.sys_line sysBaseLine = Session["SysBaseLine"] as JZ.BLL.sys_line;

                    LoadReport(sysBaseLine.DataSourceAddress, sysBaseLine.DataBaseName, sysBaseLine.UserName, sysBaseLine.PassWord, id);
                }
            }
        }

    }


    #region 线路名称


    public void GetLineName()
    {

        BLL_Document Bll = new BLL_Document();

        LineName =Bll.ExcuteScalar("SELECT TOP 1 HigWayClassification  FROM dbo.sys_engs_ProjectInfo WHERE Scdel=0").ToString();

    }


    public JZDocument sheetDataAreaCells;


    public void LoadReport(string DSource, string DName, string UID, string PWD, string TestRoomCode)
    {

        try
        {
            string _TempStr = @"
                            Select m.reportsheetid,d.Data 
                            from [dbo].[sys_document] d 
                            join [dbo].[sys_module] m on d.ModuleId = m.ID AND m.ID ='E77624E9-5654-4185-9A29-8229AAFDD68B'
                            where testroomcode ='{0}' ";


            #region 查询数据

            DataSet Ds = new DataSet();
            using (SqlConnection _Conn = new SqlConnection("Data Source=" + DSource + ";Initial Catalog=" + DName + ";User ID=" + UID + ";Pwd=" + PWD))
            {
                _Conn.Open();
                using (SqlCommand _Cmd = new SqlCommand(string.Format(_TempStr, TestRoomCode), _Conn))
                {
                    using (SqlDataAdapter _Adp = new SqlDataAdapter(_Cmd))
                    {
                        _Adp.Fill(Ds);
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            sheetDataAreaCells = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(Ds.Tables[0].Rows[0]["Data"].ToString());
                        }
                        else
                        {
                            throw new Exception("占无报告数据");
                        }

                    }
                }
                _Conn.Close();
            }


            #endregion
  

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
 
        }


    }


    public string RoomInfo(string Cell)
    {
        try
        {
            return JZCommonHelper.GetCellValue(sheetDataAreaCells, sheetDataAreaCells.Sheets[0].ID, Cell).ToString();
        }
        catch
        {
            return "";
        }
    }

    #endregion
}