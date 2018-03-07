using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
   public class BLL_Betong : JZ.BLL.BOBase
    {
       public DataTable GetDataTableTongCharts(string procname, string type, String startdate, String enddate, String testcode, string modelid,string marks)
       {
           DataTable dt = GetDataTableTongChartsProc(procname, type, startdate, enddate, testcode, modelid,marks);
           return dt;
       }

       public List<ChartModel> GetMinAndAverage(List<ChartModel> list, string type)
       {
           //C20、C30、C40、C50、C60、C15、C25、C35

           if (type.Contains('C'))
           {
               type = type.Replace("C", "");
               type = type.Substring(0, 2);
               Double cu = 0;
               Double min = 0;
               int num = list.Count;
               int nType = Int32.Parse(type);
               Double ValueCu = 0;
               Double ValueMin = 0;
               if (num < 10)
               {
                   if (nType < 60)
                   {
                       cu = 1.15;
                       min = 0.95;
                   }
                   else
                   {
                       cu = 1.10;
                       min = 0.95;
                   }
                   ValueCu = Calculate(cu, nType);
                   ValueMin = Calculate(min, nType);
               }
               else if (num > 9 && num < 15)
               {
                   cu = 1.15;
                   min = 0.90;
                   ValueCu = Calculate(list, cu) + nType;
                   ValueMin = Calculate(min, nType);
               }
               else if (num > 14 && num < 20)
               {
                   cu = 1.05;
                   min = 0.85;
                   ValueCu = Calculate(list, cu) + nType;
                   ValueMin = Calculate(min, nType);
               }
               else if (num > 20)
               {
                   cu = 0.95;
                   min = 0.85;
                   ValueCu = Calculate(list, cu) + nType;
                   ValueMin = Calculate(min, nType);
               }

               Double sum = 0;
               foreach (var item in list)
               {
                   sum += item.DoubleNumber;
               }

               sum = Math.Round((sum / list.Count), 2);

               foreach (var item in list)
               {
                   item.Para1 = Math.Round(ValueCu, 2).ToString();
                   item.Para2 = ValueMin.ToString();
                   item.Para3 = sum.ToString();
               }

               return list;
           }
           else
           {
               return null;
           }
       }

       public Double Calculate(double r, int n)
       {
           return r * n;
       }

       public Double Calculate(List<ChartModel> list, Double r)
       {
           Double sum1 = 0;
           Double sum2 = 0;
           foreach (var item in list)
           {
               sum1 = sum1 + item.DoubleNumber * item.DoubleNumber;
               sum2 += item.DoubleNumber;
           }

           sum2 = (sum2 / list.Count) * (sum2 / list.Count) * list.Count;
           return Math.Sqrt((sum1 - sum2) / (list.Count - 1)) * r;
       }

    }
}
