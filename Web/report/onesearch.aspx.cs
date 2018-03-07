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


public partial class report_onesearch : BasePage
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
            
                    LoadReport( sysBaseLine.DataSourceAddress, sysBaseLine.DataBaseName, sysBaseLine.UserName, sysBaseLine.PassWord, id);
                }
            }
        }

    }


    #region 线路名称


    public void GetLineName()
    {
        //SELECT TOP 1 HigWayClassification  FROM dbo.sys_engs_ProjectInfo WHERE Scdel=0

        BLL_Document Bll = new BLL_Document();

        LineName = Bll.ExcuteScalar("SELECT TOP 1 HigWayClassification  FROM dbo.sys_engs_ProjectInfo WHERE Scdel=0").ToString();

    }


    #endregion

    #region EXCEL to  HTML
    public List<FarPoint.Win.Spread.Cell> dataCells = new List<FarPoint.Win.Spread.Cell>();
    public DataTable dtCellStyle { get; set; }

    public void LoadReport(string DSource, string DName, string UID, string PWD,string DocumentID)
    {

        try
        {
            
            string SheetXML = "";
            string sheetData = "";
            string SheetName = "";
            string SheetID = "";
            string ReportIndex = "0";
            JZDocument sheetDataAreaCells;
            List<JZCell> Cells = new List<JZCell>();

            #region 查询数据

            DataTable _TempTable = new DataTable();
            using (SqlConnection _Conn = new SqlConnection("Data Source=" + DSource + ";Initial Catalog=" + DName + ";User ID=" + UID + ";Pwd=" + PWD))
            {
                _Conn.Open();
                string _TempStr = "Select m.reportsheetid,d.Data from [dbo].[sys_document] d left outer  join [dbo].[sys_module] m on d.ModuleId = m.ID where d.id = '{0}'";
                using (SqlCommand _Cmd = new SqlCommand(string.Format(_TempStr, DocumentID), _Conn))
                {
                    using (SqlDataAdapter _Adp = new SqlDataAdapter(_Cmd))
                    {
                        _Adp.Fill(_TempTable);
                        if (_TempTable.Rows.Count > 0)
                        {
                            sheetData = _TempTable.Rows[0]["Data"].ToString();
                            //ReportIndex = Convert.ToInt32(_TempTable.Rows[0]["reportsheetid"].ToString());
                            ReportIndex =_TempTable.Rows[0]["reportsheetid"].ToString();


                            _TempTable.Clear();
                            _TempTable = new System.Data.DataTable();
                            sheetDataAreaCells = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(sheetData);
                            _TempStr = "select Name ,SheetXML ,SheetData  from  sys_sheet where ID = '{0}'";
                            _Cmd.CommandText = string.Format(_TempStr, ReportIndex);

                            foreach (JZSheet Jzs in sheetDataAreaCells.Sheets)
                            {
                                if (Jzs.ID.ToString() == ReportIndex)
                                {
                                    SheetName = Jzs.Name;
                                    SheetID = Jzs.ID.ToString();
                                    Cells = Jzs.Cells;
                                }
                            }
                            _Adp.SelectCommand = _Cmd;
                            _Adp.Fill(_TempTable);

                            SheetXML = _TempTable.Rows[0]["SheetXML"].ToString();
                        }
                        else
                        {
                            throw new Exception("占无报告数据");
                        }
                        
                    }
                }
                _Conn.Close();
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
            SheetView.SheetName = SheetName;

            if (sheetDataAreaCells != null)
            {

                //for (int i = 0; i < SheetView.RowCount; i++)
                //{
                //    for (int j = 0; j < SheetView.ColumnCount; j++)
                //    {
                //        if (SheetView.Cells[i, j].CellType!=null && SheetView.Cells[i, j].CellType.ToString() == "下拉框")
                //        {
                //            SheetView.Cells[i, j].CellType = null;
                //        }
                //    }
                //}
                //SheetView.Columns.Remove(16, 4);

                foreach (JZCell cell in Cells)
                {
                    dataCells.Add(SheetView.Cells[cell.Name]);

                    SheetView.Cells[cell.Name].Value = cell.Value;
                }
            }

            WinSp.Sheets.Add(SheetView);

            //dtCellStyle = ModuleHelperClient.GetCellStyleBySheetID(sheetID);
            //for (int i = 0; i < dtCellStyle.Rows.Count; i++)
            //{
            //    if (dtCellStyle.Rows[i]["CellStyle"] != null)
            //    {
            //        JZCellStyle CurrentCellStyle = Newtonsoft.Json.JsonConvert.DeserializeObject<JZCellStyle>(dtCellStyle.Rows[i]["CellStyle"].ToString());
            //        if (CurrentCellStyle != null)
            //        {
            //            string strCellName = dtCellStyle.Rows[i]["CellName"].ToString();
            //            FarPoint.Win.Spread.Cell cell = SheetView.Cells[strCellName];
            //            cell.ForeColor = CurrentCellStyle.ForColor;
            //            cell.BackColor = CurrentCellStyle.BackColor;
            //            cell.Font = new Font(CurrentCellStyle.FamilyName, CurrentCellStyle.FontSize, CurrentCellStyle.FontStyle);
            //        }
            //    }
            //}

            WinSp.LoadFormulas(true);


            UpdateChart(WinSp);
            UpdateEquation(WinSp);


            

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

            string FileName = SitePath + "/" + SheetID+DateTime.Now.ToString("yyMMddhhmmss") + ".html";

            WinSp.ActiveSheet.SaveHtml(FileName);
           

            FileInfo FHtml = new System.IO.FileInfo(FileName);
            FileStream Fs= FHtml.OpenRead();
            byte[] Buuff = new byte[Fs.Length];
            Fs.Read(Buuff, 0, (int)Fs.Length);
            Fs.Close();
            Fs.Dispose();

            string HTML = ASCIIEncoding.UTF8.GetString(Buuff);
            html.InnerHtml = HTML;

            //WinSp.SaveExcel(SitePath + "/" + SheetID + ".xls");
            File.Delete(FileName);
            #endregion

            #region Save Excel
            //WinSp.SaveExcel(SitePath + "/222.xls",FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);



            //fp1.OpenExcel(SitePath + "/" + SheetID + ".xls");
            //fp1.CommandBar.Visible = false;
            //fp1.Sheets[0].PageSize = fp1.Sheets[0].Rows.Count;

            //File.Delete(SitePath + "/" + SheetID + ".xlsx");
            //fp1.Visible = true;
            #endregion
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
            //fp1.Visible = false;
        }
        finally
        {
            
        }

     

        //Response.Write(stopwatch.ElapsedMilliseconds.ToString());
    }

    public void UpdateChart(FarPoint.Win.Spread.FpSpread FpSpread)
    {
        foreach (FarPoint.Win.Spread.SheetView Sheet in FpSpread.Sheets)
        {
            //支持嵌入的图表
            int RowCount = Sheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Style);
            int ColumnCount = Sheet.GetLastNonEmptyColumn(FarPoint.Win.Spread.NonEmptyItemFlag.Style);
            for (int i = 0; i <= RowCount; i++)
            {
                for (int j = 0; j <= ColumnCount; j++)
                {
                    if (Sheet.Cells[i, j].CellType is ChartCellType)
                    {
                        ChartCellType ChartType = Sheet.Cells[i, j].CellType as ChartCellType;
                        Rectangle r = FpSpread.GetCellRectangle(0, 0, i, j);
                        ChartType.ChartSize = r.Size;
                        ChartType.ActiveSheet = Sheet;
                        ChartType.UpdateChart();
                    }
                }
            }

            //支持浮动的图表
            foreach (IElement Element in Sheet.DrawingContainer.ContainedObjects)
            {
                if (Element is ChartShape)
                {
                    ChartShape Shape = Element as ChartShape;
                    Shape.ActiveSheet = Sheet;
                    Shape.Locked = false;
                    Shape.UpdateChart();
                }
            }
        }
    }

    public void UpdateEquation(FarPoint.Win.Spread.FpSpread FpSpread)
    {
        foreach (FarPoint.Win.Spread.SheetView Sheet in FpSpread.Sheets)
        {
            //支持浮动的公式
            foreach (IElement Element in Sheet.DrawingContainer.ContainedObjects)
            {
                if (Element is EquationShape)
                {
                    EquationShape Shape = Element as EquationShape;
                    Shape.Locked = false;
                }
            }
        }
    }

    #endregion
}