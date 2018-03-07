using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using FarPoint.Web.Spread;
using FarPoint.Win;
using FarPoint.Win.Spread;
using System.IO;
using System.Text;
using System.Data;
using BizCommon;

public partial class TestRoom_FPDocument : BasePage
{
    #region

    public string LineName = string.Empty;

    public int SheetCount = 0;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        GetLineName();

        LoadReport();
   
    }



    #region 线路名称


    public void GetLineName()
    {

        BLL_Document Bll = new BLL_Document();
        JZ.BLL.sys_line sysBaseLine = Session["SysBaseLine"] as JZ.BLL.sys_line;
        LineName = sysBaseLine.LineName;

       
    }


    #endregion

    #region EXCEL to  HTML



    public void LoadReport()
    {
        string DocumentID = "ID".RequestStr();
        string SheetIndex = "SI".RequestStr();
        string IsTab = "IT".RequestStr();

        string SheetXML = "";
        string sheetData = "";
        string SheetName = "";
        string SheetID = "";
        string ReportSheetID = "";
        string IsRs = "IsRs".RequestStr();

        IsRs = IsRs.IsNullOrEmpty() ? "false" : IsRs;

        SheetIndex = SheetIndex.IsNullOrEmpty() ? "0" : SheetIndex;

        try
        {


            JZDocument sheetDataAreaCells;
            List<JZCell> Cells = new List<JZCell>();

            #region 查询数据

            DataTable _TempTable = new DataTable();

            string _TempStr = "Select m.reportsheetid,d.Data from [dbo].[sys_document] d left outer  join [dbo].[sys_module] m on d.ModuleId = m.ID where d.id = '" + DocumentID + "'";
            BLL_Document Bll = new BLL_Document();
            _TempTable = Bll.GetDataTable(_TempStr);


            if (_TempTable.Rows.Count > 0)
            {

                sheetData = _TempTable.Rows[0]["Data"].ToString();
                sheetDataAreaCells = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(sheetData);
                SheetCount = sheetDataAreaCells.Sheets.Count;


                if (IsRs == "true")
                {
                    
                    ReportSheetID = _TempTable.Rows[0]["reportsheetid"].ToString();
                    foreach (JZSheet Jzs in sheetDataAreaCells.Sheets)
                    {
                        if (Jzs.ID.ToString() == ReportSheetID)
                        {
                            SheetName = Jzs.Name;
                            SheetID = Jzs.ID.ToString();
                            Cells = Jzs.Cells;
                            break;
                        }
                    }
                }
                else
                {
                    ReportSheetID = sheetDataAreaCells.Sheets[SheetIndex.Toint()].ID.ToString();

                    SheetName = sheetDataAreaCells.Sheets[SheetIndex.Toint()].Name;
                    SheetID = ReportSheetID;
                    Cells = sheetDataAreaCells.Sheets[SheetIndex.Toint()].Cells;
                }


                _TempStr = "select SheetXML ,SheetData  from  sys_sheet where ID = '{0}'";
                SheetXML = Bll.ExcuteScalar(string.Format(_TempStr, ReportSheetID)).ToString();
            }
            else
            {
                throw new Exception("占无报告数据");
            }

            _TempTable.Clear();
            _TempTable.Dispose();

            #endregion

            #region 创建WIN组件
            FarPoint.Win.Spread.FpSpread WinSp = new FarPoint.Win.Spread.FpSpread();
            WinSp.Sheets.Clear();
            SheetXML = JZCommonHelper.GZipDecompressString(SheetXML);
            int a = SheetXML.Length;
            FarPoint.Win.Spread.SheetView SheetView = Serializer.LoadObjectXml(typeof(FarPoint.Win.Spread.SheetView), SheetXML, "SheetView") as FarPoint.Win.Spread.SheetView;
            SheetView.SheetName = "Document";

            if (sheetDataAreaCells != null)
            {

                foreach (JZCell cell in Cells)
                {
                    //
                    if (SheetView.Cells[cell.Name].CellType.ToString() == "图片")
                    {

                        SheetView.Cells[cell.Name].ResetCellType();

                        SheetView.Cells[cell.Name].Value = "<img src='data:image/gif;base64," + cell.Value.ToString().Replace("\r\n", "") + "'  />"; 
                    }
                    else if (SheetView.Cells[cell.Name].CellType.ToString() == "文本")
                    {

                        SheetView.Cells[cell.Name].Value = cell.Value;
                    }
                    else if (SheetView.Cells[cell.Name].CellType.ToString() == "长文本")
                    {
                        SheetView.Cells[cell.Name].ResetCellType();
                        SheetView.Cells[cell.Name].Value = cell.Value;
                    }
                    else if (SheetView.Cells[cell.Name].CellType.ToString() == "超链接")
                    {
                        SheetView.Cells[cell.Name].ResetCellType();
                        if (cell.Value != null)
                        {
                            try
                            {
                                string[] Temp = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(cell.Value.ToString());
                                SheetView.Cells[cell.Name].Value = Temp[0] + "["+Temp[2]+"]<img src='data:image/gif;base64," + Temp[1].Replace("\r\n", "") + "'  />"; 
                            }
                            catch
                            { }
                        }
                    }
                }
            }

            WinSp.Sheets.Add(SheetView);



            WinSp.LoadFormulas(true);

            #endregion

            #region 临时文件
            string SitePath = Server.MapPath("~/Temp");
            DirectoryInfo Dir = new System.IO.DirectoryInfo(SitePath);
            if (!Dir.Exists)
            {
                Dir.Create();
            }
            #endregion

            #region Save HTML

            string FileName = SitePath + "/" + SheetID + DateTime.Now.ToString("yyMMddhhmmss") + ".html";


            WinSp.ActiveSheet.SaveHtml(FileName);


            FileInfo FHtml = new System.IO.FileInfo(FileName);
            FileStream Fs = FHtml.OpenRead();
            byte[] Buuff = new byte[Fs.Length];
            Fs.Read(Buuff, 0, (int)Fs.Length);
            Fs.Close();
            Fs.Dispose();

            string HTML = ASCIIEncoding.UTF8.GetString(Buuff);
       


            File.Delete(FileName);

            #endregion


            #region show Element
             



            StringBuilder Nav = new StringBuilder();
            StringBuilder Cont = new StringBuilder();
            Nav.Append("<ul class=\"nav nav-tabs\">");
            Cont.Append("<div class=\"tab-content\">");
            if (IsRs == "false" && IsTab=="true")
            {
                for (int i = 0; i < SheetCount; i++)
                {

                    Nav.Append("<li class=\"" + (i == SheetIndex.Toint() ? "active" : "") + "\" ><a data=\"FPDocument.aspx?IT=true&ID=" + DocumentID + "&IsRs=" + IsRs + "&SI=" + i + "\" href=\"#t" + i + "\" data-toggle=\"tab\" >第" + (i + 1).ToString() + "</a></li>");


                    Cont.Append("<div class=\"tab-pane CPane " + (i == SheetIndex.Toint() ? "active" : "") + " \" id=\"t" + i.ToString() + "\">" + HTML + "</div>");
                

                }
                Nav.Append("</ul>");
                Cont.Append("</div>");
                form1.InnerHtml = Nav.ToString() + Cont.ToString();
            }
            else
            {
                form1.InnerHtml = HTML;
            }


            #endregion

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());

        }



    }


    #endregion
}