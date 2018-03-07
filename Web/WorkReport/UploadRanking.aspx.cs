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

public partial class WorkReport_UploadRanking : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if ("Act".IsRequest())
        {
            switch ("Act".RequestStr().ToUpper())
            {
                case "LIST":
                    List();
                    break;

            }
        }
    }



    public void List()
    {

        string StartDate = "StartDate".RequestStr();
        string EndDate = "EndDate".RequestStr().ToDateTime().AddDays(1).ToString("yyyy-MM-dd");


     

        #region SQL



        string Sql = @" 


                        -- 0 施工前三
                        select top 3
						t2.description+' '+t1.description as '单位'
						
						,count(1) as '数量' 
						from sys_document a 
                        join sys_module b on a.moduleid=b.id
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{1}' and '{0}' AND t1.DepType='@unit_施工单位'
						group by t1.description,t2.description
						order by 数量 desc

                        -- 1 施工后三
                        select top 3
						t2.description+' '+t1.description as '单位'
						
						,count(1) as '数量' 
						from sys_document a 
                        join sys_module b on a.moduleid=b.id
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{1}' and '{0}' AND t1.DepType='@unit_施工单位'
						group by t1.description,t2.description
						order by 数量 asc
                        
                        -- 2 监理前三
                        select top 3
						t2.description+' '+t1.description as '单位'
						
						,count(1) as '数量' 
						from sys_document a 
                        join sys_module b on a.moduleid=b.id
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{1}' and '{0}' AND t1.DepType='@unit_监理单位'
						group by t1.description,t2.description
						order by 数量 desc

                        -- 3系统登录次数
                        SELECT 
						top 3
						t2.description+' '+t1.description as '单位'
						,
						COUNT(1) as '数量' 
						FROM dbo.sys_loginlog a
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
						WHERE  FirstAccessTime between '{1}' and '{0}' AND len(testroomcode)>12  
						group by t1.description,t2.description
						order by 数量 desc

                        -- 4 系统登录次数
                        SELECT 
						top 3
						t2.description+' '+t1.description as '单位'
						,
						COUNT(1) as '数量' 
						FROM dbo.sys_loginlog a
						left outer join sys_tree t on t.nodecode = a.testroomcode
						left outer join sys_tree t1 on t1.nodecode = left(t.nodecode,12)
						left outer join sys_tree t2 on t2.nodecode = left(t.nodecode,8)
						WHERE  FirstAccessTime between '{1}' and '{0}' AND len(testroomcode)>12  
						group by t1.description,t2.description
						order by 数量 asc
                  


                        ";


        #endregion




        Sql = string.Format(Sql, EndDate, StartDate);


        BLL_Document BLL = new BLL_Document();
        DataSet Ds = BLL.GetDataSet(Sql);



        DataTable Out = new DataTable();
        Out.Columns.Add(new DataColumn("0"));
        Out.Columns.Add(new DataColumn("1"));
        Out.Columns.Add(new DataColumn("2"));
        Out.Columns.Add(new DataColumn("3"));
        Out.Columns.Add(new DataColumn("4"));
        Out.Columns.Add(new DataColumn("5"));
        Out.Columns.Add(new DataColumn("6"));
        Out.Columns.Add(new DataColumn("7"));
        Out.Columns.Add(new DataColumn("8"));
        Out.Columns.Add(new DataColumn("9"));


        DataRow Dr = Out.NewRow();
        Out.Rows.Add(Dr);
        Dr = Out.NewRow();
        Out.Rows.Add(Dr);
        Dr = Out.NewRow();
        Out.Rows.Add(Dr);
        Dr = Out.NewRow();
        Out.Rows.Add(Dr);

        int ct = 0;
        for (int i = 0; i < Ds.Tables.Count; i++)
        {
            int rt = 0;
            foreach (DataRow tDr in Ds.Tables[i].Rows)
            {
                Out.Rows[rt][ct + 0] = tDr[0].ToString();
                Out.Rows[rt][ct + 1] = tDr[1].ToString();

                Out.Rows[3][ct + 0] = "合计";
                Out.Rows[3][ct + 1] = (Out.Rows[3][ct + 1].ToString().Toint()+tDr[1].ToString().Toint()).ToString();
                rt++;
            }
            ct = ct + 2;
        }


       


        string Json = JsonConvert.SerializeObject(Out);
        Json = "{\"rows\":" + Json + ",\"total\":0}";

        Response.Write(Json);
        Response.End();
    }
}