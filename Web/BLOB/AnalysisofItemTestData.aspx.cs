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

public partial class BLOB_AnalysisofItemTestData : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {
              
                case "ATTR":
                    Response.Write(JsonConvert.SerializeObject(BLOB.Attr("ItemID".RequestStr())));
                    Response.End();
                    break;
                case "CHART":
                    Chart();
                    break;

            }
        }

    }


    public void Chart()
    {

        string As = "AS".RequestStr();

        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr();
  
        string Json = string.Empty;

        ItemCollection Items = BLOB.Attribute("Item".RequestStr());
        if (Items.ContainsKey("型号"))
        {

            DataSet Ds = BLOB.RawAttrByModel("Item".RequestStr(), "Attr".RequestStr(), "AttrName".RequestStr(), Items["型号"].BindField, "FID".RequestStr(), StartDate, EndDate);

            DataTable DT = Ds.Tables[0];
            DataTable TempTB;
            decimal SumV = 0;
            decimal Count = 0;
            int Max = 0;
            int Min = 0;
            int ii = 0;

            #region 按时间段
            string D3 = string.Empty;

            foreach (DataRow Dr in Ds.Tables[1].Rows)
            {
                DT.DefaultView.RowFilter = Items["型号"].BindField + "='" + Dr[0].ToString() +"'";
                TempTB = DT.DefaultView.ToTable();
                Count = TempTB.Rows.Count;
                if (Count > 0)
                {
                    SumV = TempTB.Compute("sum(" + "Attr".RequestStr() + ")", "").Todecimal();
                    //平均值
                    SumV = Math.Round((SumV / Count).Todouble(),2).Todecimal();

                    if (SumV == 0)
                    {
                        continue;
                    }

                    #region 标准差
                    double FC = 0;
                    foreach (DataRow DrFC in TempTB.Rows)
                    {
                        FC += Math.Pow((DrFC[1].Todouble() - SumV.Todouble()), 2);
                    }

                    FC = Math.Sqrt((FC / Count.Todouble()).Todouble()).Todouble();
                    FC = Math.Round(FC, 2);
                    #endregion

                    #region 变异系数

                    double BY = (FC / SumV.Todouble()).Todouble() * 100;
                    BY = Math.Round(BY, 2);
                    #endregion
                    D3 += D3.IsNullOrEmpty() ? "" : ",";
                    D3 += "{\"tit\":\"" + Dr[0].ToString() + "\",\"SV\":\"" + SumV + "\", \"FC\":\"" + FC + "\", \"BY\":\"" + BY + "\",\"ZVal\":\"" + TempTB.Rows[0]["StandardValue"].ToString() + "\"}";
                }
            }
             SumV = 0;
             Count = 0;
             TempTB = null;

             D3 = "[" + D3 + "]";
            #endregion

            #region 按月份计算



            List<DateTime> Months = new List<DateTime>();
    
            DateTime StD = StartDate.ToDateTime();

            while (StD <= EndDate.ToDateTime())
            {
                Months.Add(StD);
                StD = StD.AddMonths(1);
            }
            int i = 0;
            foreach (DateTime nDT in Months)
            {
                #region
                Json += i == 0 ? "" : ",";
                Json += "{";
                Json += "\"M\":\"" + nDT.ToString("yyyy-MM") + "月份\"";

                foreach (DataRow Dr in Ds.Tables[1].Rows)
                {
                    DT.DefaultView.RowFilter = Items["型号"].BindField + "='" + Dr[0].ToString() + "' AND bgrq>='" + nDT.ToString("yyyy-MM-dd") + "-1' AND bgrq <='" + nDT.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + "'";
                    TempTB = DT.DefaultView.ToTable();
                    Count = TempTB.Rows.Count;

                    if (Count > 0)
                    {
                        SumV = TempTB.Compute("sum(" + "Attr".RequestStr() + ")", "").Todecimal();
                        //平均值
                        SumV = Math.Round((SumV / Count).Todouble(), 2).Todecimal();


                        if (SumV == 0)
                        {
                            continue;
                        }

                        #region 标准差
                        double FC = 0;
                        foreach (DataRow DrFC in TempTB.Rows)
                        {
                            FC += Math.Pow((DrFC[1].Todouble() - SumV.Todouble()), 2);
                        }

                        FC = Math.Sqrt((FC / Count.Todouble()).Todouble()).Todouble();
                        FC = Math.Round(FC, 2);
                        #endregion

                        #region 变异系数

                        double BY = (FC / SumV.Todouble()).Todouble() * 100;
                        BY = Math.Round(BY, 2);
                        #endregion

                        Json += ",\"" + Dr[0].ToString() + "\":\"" + SumV + "\"";//bullet
                        Json += ",\"" + Dr[0].ToString() + "-FC\":\"" + FC + "\"";
                        Json += ",\"" + Dr[0].ToString() + "-BY\":\"" + BY + "%\"";

                        if (ii == 0)
                        {
                            Max = (int)SumV + 1;
                            Min = (int)SumV - 1;
                            ii++;


                        }
                        Max = (int)SumV >= Max ? (int)SumV + 1 : Max;
                        Min = (int)SumV <= Min ? (int)SumV - 1 : Min;
                    }
                }

                Json += "}";

                #endregion
                i++;
            }
         

           
            #endregion

            string Ms = string.Empty;
            foreach (DataRow Dr in Ds.Tables[1].Rows)
            {
                if (!Dr[0].ToString().IsNullOrEmpty())
                {
                    Ms += Ms.IsNullOrEmpty() ? "\"" + Dr[0].ToString() + "\"" : ",\"" + Dr[0].ToString() + "\"";
                }
            }

            Json = "{\"D1\":[" + Json + "],\"D2\":[" + Ms + "],\"Max\":" + Max + ",\"Min\":" + Min + ",\"D3\":" + D3 + "}";

            Response.Write(Json);
        }
        Response.End();
    }
}