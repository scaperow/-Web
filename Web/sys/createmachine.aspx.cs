using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BizCommon;
using JZ.BLL;

public partial class sys_createmachine : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        sys_line sysBaseLine = System.Web.HttpContext.Current.Session["SysBaseLine"] as sys_line;
        if (sysBaseLine.IsActive == 1)
        {
            string SQL = "SELECT * FROM dbo.sys_document WHERE  ModuleID='BA23C25D-7C79-4CB3-A0DC-ACFA6C285295' ";
            DataSet ds = DbHelperSQL.Query(SQL);
            SQL = " TRUNCATE TABLE biz_machinelist ";
            DbHelperSQL.Query(SQL);

            DateTime d_min = DateTime.Parse(SqlDateTime.MinValue.ToString());
            DateTime d_max = DateTime.Parse(SqlDateTime.MaxValue.ToString());
        


            if (ds != null)
            {
                List<biz_machinelist> list = new List<biz_machinelist>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string str = dr["Data"].ToString();
                    JZDocument doc = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(str);
                    if (doc != null)
                    {                    
                        #region
                        for (int i = 6; i < 32; i++)
                        {
                            biz_machinelist biz = new biz_machinelist();
                            biz.SCTS = DateTime.Parse(dr["CreatedTime"].ToString());
                            biz.SCPT = dr["TestRoomCode"].ToString();
                            //int n=0;
                            //if (Int32.TryParse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "A" + i).ToString(),out n))
                            //{
                            //    biz.col_norm_A6 = Int32.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "A" + i).ToString());
                            //}
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B" + i) != null)
                            {
                                biz.col_norm_B6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "B" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "C" + i) != null)
                            {
                                biz.col_norm_C6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "C" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D" + i) != null)
                            {
                                biz.col_norm_D6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "D" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "E" + i) != null)
                            {
                                biz.col_norm_E6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "E" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "F" + i) != null)
                            {
                                biz.col_norm_F6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "F" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G" + i) != null)
                            {
                                biz.col_norm_G6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "G" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "H" + i) != null)
                            {
                                DateTime date=new DateTime();
                                if (DateTime.TryParse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "H" + i).ToString(), out date) == false)
                                {
                                    biz.col_norm_H6 = null;
                                }
                                else
                                {
                                    if (DateTime.Compare(d_min, DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "H" + i).ToString())) < 0 && DateTime.Compare(d_max, DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "H" + i).ToString())) >= 0)
                                    {
                                        biz.col_norm_H6 = DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "H" + i).ToString());
                                    }
                                    else
                                    {
                                        biz.col_norm_H6 = null;
                                    }

                                   
                                }                              
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "I" + i) != null)
                            {
                                biz.col_norm_I6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "I" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "J" + i) != null)
                            {
                                biz.col_norm_J6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "J" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i) != null)
                            {
                                biz.col_norm_K6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "K" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "L" + i) != null)
                            {
                                biz.col_norm_L6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "L" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "M" + i) != null)
                            {
                                biz.col_norm_M6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "M" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "N" + i) != null)
                            {
                                
                                DateTime date = new DateTime();
                                if (DateTime.TryParse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "N" + i).ToString(), out date) == false)
                                {
                                    biz.col_norm_N6 = null;
                                }
                                else
                                {
                                   //biz.col_norm_N6 = DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "N" + i).ToString());
                                    if (DateTime.Compare(d_min, DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "N" + i).ToString())) < 0 && DateTime.Compare(d_max, DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "N" + i).ToString())) >= 0)
                                    {
                                        biz.col_norm_N6 = DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "N" + i).ToString());
                                    }
                                    else
                                    {
                                        biz.col_norm_N6 = null;
                                    }
                                }


                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "O" + i) != null)
                            {
                               
                                DateTime date = new DateTime();
                                if (DateTime.TryParse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "O" + i).ToString(), out date) == false)
                                {
                                    biz.col_norm_O6 = null;
                                }
                                else
                                {
                                    //biz.col_norm_O6 = DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "O" + i).ToString());
                                    if (DateTime.Compare(d_min, DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "O" + i).ToString())) < 0 && DateTime.Compare(d_max, DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "O" + i).ToString())) >= 0)
                                    {
                                        biz.col_norm_O6 = DateTime.Parse(JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "O" + i).ToString());
                                    }
                                    else
                                    {
                                        biz.col_norm_O6 = null;
                                    }
                                }

                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "P" + i) != null)
                            {
                                biz.col_norm_P6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "P" + i).ToString();
                            }
                            if (JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "Q" + i) != null)
                            {
                                biz.col_norm_Q6 = JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, "Q" + i).ToString();
                            }
                            if (biz.col_norm_B6 == null && biz.col_norm_C6 == null && biz.col_norm_D6 == null && biz.col_norm_E6 == null && biz.col_norm_F6 == null )
                            {

                            }
                            else
                            {
                                list.Add(biz);
                            }
                        }
                        #endregion
                       
                    }
                }

                #region
                foreach (var item in list)
                {
                    long n = 0;
                    n = Add(item);
                }
                #endregion

                SQL = "DELETE dbo.biz_machinelist WHERE  col_norm_B6='' AND col_norm_C6='' AND col_norm_D6='' AND col_norm_E6='' AND col_norm_F6='' AND col_norm_G6='' ";
                DbHelperSQL.Query(SQL);
                Label1.Text = "执行成功";
            }
        }
    }

    public long Add(biz_machinelist model)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into biz_machinelist(");
        strSql.Append("SCTS,SCPT,SCCT,SCCS,col_norm_A6,col_norm_B6,col_norm_C6,col_norm_D6,col_norm_E6,col_norm_F6,col_norm_G6,col_norm_H6,col_norm_I6,col_norm_J6,col_norm_K6,col_norm_L6,col_norm_M6,col_norm_N6,col_norm_O6,col_norm_P6,col_norm_Q6)");
        strSql.Append(" values (");
        strSql.Append("@SCTS,@SCPT,@SCCT,@SCCS,@col_norm_A6,@col_norm_B6,@col_norm_C6,@col_norm_D6,@col_norm_E6,@col_norm_F6,@col_norm_G6,@col_norm_H6,@col_norm_I6,@col_norm_J6,@col_norm_K6,@col_norm_L6,@col_norm_M6,@col_norm_N6,@col_norm_O6,@col_norm_P6,@col_norm_Q6)");
        strSql.Append(";select @@IDENTITY");
        SqlParameter[] parameters = {
					new SqlParameter("@SCTS", SqlDbType.DateTime),
					new SqlParameter("@SCPT", SqlDbType.VarChar,50),
					new SqlParameter("@SCCT", SqlDbType.VarChar,50),
					new SqlParameter("@SCCS", SqlDbType.VarChar,50),
					new SqlParameter("@col_norm_A6", SqlDbType.Int,4),
					new SqlParameter("@col_norm_B6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_C6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_D6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_E6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_F6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_G6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_H6", SqlDbType.DateTime),
					new SqlParameter("@col_norm_I6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_J6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_K6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_L6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_M6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_N6", SqlDbType.DateTime),
					new SqlParameter("@col_norm_O6", SqlDbType.DateTime),
					new SqlParameter("@col_norm_P6", SqlDbType.NVarChar,500),
					new SqlParameter("@col_norm_Q6", SqlDbType.NVarChar,500)};
        parameters[0].Value = model.SCTS;
        parameters[1].Value = model.SCPT;
        parameters[2].Value = model.SCCT;
        parameters[3].Value = model.SCCS;
        parameters[4].Value = model.col_norm_A6;
        parameters[5].Value = model.col_norm_B6;
        parameters[6].Value = model.col_norm_C6;
        parameters[7].Value = model.col_norm_D6;
        parameters[8].Value = model.col_norm_E6;
        parameters[9].Value = model.col_norm_F6;
        parameters[10].Value = model.col_norm_G6;
        parameters[11].Value = model.col_norm_H6;
        parameters[12].Value = model.col_norm_I6;
        parameters[13].Value = model.col_norm_J6;
        parameters[14].Value = model.col_norm_K6;
        parameters[15].Value = model.col_norm_L6;
        parameters[16].Value = model.col_norm_M6;
        parameters[17].Value = model.col_norm_N6;
        parameters[18].Value = model.col_norm_O6;
        parameters[19].Value = model.col_norm_P6;
        parameters[20].Value = model.col_norm_Q6;

        object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
        if (obj == null)
        {
            return 0;
        }
        else
        {
            return Convert.ToInt64(obj);
        }
    }
}