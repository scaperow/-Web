using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JZ.BLL;
using JZ.BLL.模型;
using Newtonsoft.Json;
using BizCommon;

public partial class ForIndexMapRight : System.Web.UI.Page
{
    public RightData Obj = new RightData();

    protected void Page_Load(object sender, EventArgs e)
    {
        switch ("Act".RequestStr())
        {
            case "Rooms":
                Rooms();
                break;
            case "Room":
                Room();
                break;
            case "RoomInfo":
                RoomInfo();
                break;
        }
    }


    #region

    /// <summary>
    /// 获取试验室列表
    /// </summary>
    public void Rooms()
    {
        string Sql = @"
                        select 
                        tsg.DESCRIPTION+' '+tcm.DESCRIPTION+' '+t.DESCRIPTION as DESCRIPTION,
                        tcm.DepType, 
                        t.NodeCode,
                        eif.x,eif.y 
                        from sys_Tree t
                        left outer join sys_engs_Tree et on t.NodeCode=et.NodeCode AND et.Scdel = 0
                        left outer join sys_engs_ItemInfo eif on et.RalationID=eif.id
                        left outer join sys_Tree tsg on tsg.nodecode = left(t.nodecode,8)
                        left outer join sys_Tree tcm on tcm.nodecode = left(t.nodecode,12)
                        where t.deptype = '@folder'
                        order by tsg.OrderID  asc , tcm.DepType desc
                        ";

        DataSet Ds = GetDs( Sql);
        StringBuilder Rooms = new StringBuilder();
        //Rooms.Append("<div class=\"easyui-accordion\" style=\"width:100%; margin:0px;\">");

        Rooms.Append("<div id=\"ALL\" title=\"全线资料统计\"  style=\"overflow:auto;padding:10px;\"></div>");

        foreach (DataRow Dr in Ds.Tables[0].Rows)
        {
            Rooms.Append("<div DepType=\"" + Dr["DepType"].ToString() + "\" X=\"" + Dr["x"].ToString() + "\" Y=\"" + Dr["y"].ToString() + "\"  id=\"" + Dr["NodeCode"].ToString() + "\" title=\"" + Dr["DESCRIPTION"].ToString() + "\"  style=\"overflow:auto;padding:10px;\"></div>");
        }

       // Rooms.Append("</div>");

        Response.Write(Rooms.ToString());
        Response.End();
    }

    /// <summary>
    /// 获取试验室数据
    /// </summary>
    public void Room()
    {
        #region SQL
        string Sql = @"
                        -- 0 资料
                        select testroomcode,count(1) as ct,b.StatisticsCatlog  from sys_document a 
                        join sys_module b on a.moduleid=b.id
                        where status>0 and b.moduletype=1  and  a.bgrq between 
                        '{0}' and '{1}' group by a.testroomcode,b.StatisticsCatlog
                        
                        -- 1 不合格资料
                        SELECT testroomcode,count(1) as ct,b.StatisticsCatlog  FROM dbo.sys_invalid_document a
                        join sys_module b on a.moduleid=b.id
                        WHERE AdditionalQualified=0  and status>0   and 
                        bgrq between '{0}' and '{1}' AND  F_InvalidItem NOT LIKE '%#%'
                        group by a.testroomcode,b.StatisticsCatlog
               
     
                   
                        -- 2 平行
                        SELECT COUNT(1) ct,a.testroomcode,d.StatisticsCatlog 
                        FROM dbo.sys_document a JOIN dbo.sys_px_relation b ON a.ID=b.SGDataID 
                        JOIN dbo.sys_document c ON  b.PXDataID=c.ID join sys_module d on a.moduleID=d.id
                        where a.status>0 and d.moduletype=1 AND a.bgrq between  '{0}' and '{1}'  AND c.bgrq between  '{0}' and '{1}' 
                        group by a.testroomcode,d.StatisticsCatlog

                        -- 3 见证
                        select testroomcode,count(1) as ct,b.StatisticsCatlog   from sys_document a 
                        join sys_module b on a.moduleid=b.id
                        where status>0 and b.moduletype=1  and a.bgrq between 
                        '{0}' and '{1}' and a.trytype='见证' group by a.testroomcode,b.StatisticsCatlog

                        -- 4 未处理资料
                        SELECT testroomcode,count(1) as ct,b.StatisticsCatlog  FROM dbo.sys_invalid_document a
                        join sys_module b on a.moduleid=b.id
                        WHERE AdditionalQualified=0  and status>0   and (DealResult='' OR DealResult IS NULL) and
                        bgrq between '{0}' and '{1}' AND  F_InvalidItem NOT LIKE '%#%'
                        group by a.testroomcode,b.StatisticsCatlog

                        -- 5 已经处理资料
                        SELECT testroomcode,count(1) as ct,b.StatisticsCatlog  FROM dbo.sys_invalid_document a
                        join sys_module b on a.moduleid=b.id
                        WHERE AdditionalQualified=0  and status>0   and (DealResult IS NOT NULL) and
                        bgrq between '{0}' and '{1}' AND  F_InvalidItem NOT LIKE '%#%'
                        group by a.testroomcode,b.StatisticsCatlog
                        
                        -- 6 登录次数
                        SELECT testroomcode, COUNT(1) as ct  FROM dbo.sys_loginlog WHERE  FirstAccessTime between '{0}' and '{1}' AND len(testroomcode)>12  group by testroomcode

                        -- 7 人员
                        SELECT 
                        testroomcode,
                        sum(case  when Status>0  then 1 else 0 end) as ct,
                        sum(case  when Status=0  AND  CreatedTime between '{0}' and '{1}'  then 1 else 0 end) as dct,
                        sum(case  when Status>0 AND  CreatedTime between '{0}' and '{1}'  then 1 else 0 end) as act
                        FROM dbo.sys_document WHERE ModuleID='08899BA2-CC88-403E-9182-3EF73F5FB0CE'
                        group by testroomcode

                        #ALLSB#

                        #DSB#

                         -- 10 资料总数
                        select testroomcode,count(1) as ct  from sys_document a 
                        join sys_module b on a.moduleid=b.id
                        where status>0 and b.moduletype=1  group by a.testroomcode
                        ";


        string ALLSB = " SELECT TestRoomCode,COUNT(1) as ct FROM sys_document where ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') and Status>0 group by TestRoomCode ";
        string DSB = " SELECT TestRoomCode,COUNT(1) as ct FROM sys_document where ModuleID in('A0C51954-302D-43C6-931E-0BAE2B8B10DB') AND Ext22 < '" + DateTime.Now.AddDays(1).AddSeconds(-1).ToString() + "' AND Status>0  group by TestRoomCode ";


        Sql = Sql.Replace("#ALLSB#", ALLSB);
        Sql = Sql.Replace("#DSB#", DSB);

        #endregion

        #region 本月 上月 本年

        switch ("SearchType".RequestStr())
        {
            case "1":
                Sql = string.Format(Sql, DateTime.Now.ToString("yyyy-MM-01"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                break;
            case "2":
                Sql = string.Format(Sql, DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01"), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1));
                break;
            case "3":
                Sql = string.Format(Sql, DateTime.Now.ToString("yyyy-01-01"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                break;
            default:
                Sql = string.Format(Sql, DateTime.Now.ToString("yyyy-MM-01"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                break;
        }
        #endregion

        DataSet Ds = GetDs(Sql);

       

        #region 过滤 全线/试验室
        if ("RoomID".RequestStr() != "ALL")
        {
            Ds.Tables[0].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb0 = Ds.Tables[0].DefaultView.ToTable();

            Ds.Tables[1].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb1 = Ds.Tables[1].DefaultView.ToTable();

            Ds.Tables[2].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb2 = Ds.Tables[2].DefaultView.ToTable();

            Ds.Tables[3].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb3 = Ds.Tables[3].DefaultView.ToTable();

            Ds.Tables[4].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb4 = Ds.Tables[4].DefaultView.ToTable();

            Ds.Tables[5].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb5 = Ds.Tables[5].DefaultView.ToTable();

            Ds.Tables[6].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb6 = Ds.Tables[6].DefaultView.ToTable();

            Ds.Tables[7].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb7 = Ds.Tables[7].DefaultView.ToTable();

            Ds.Tables[8].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb8 = Ds.Tables[8].DefaultView.ToTable();

            Ds.Tables[9].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb9 = Ds.Tables[9].DefaultView.ToTable();

            Ds.Tables[10].DefaultView.RowFilter = "testroomcode='" + "RoomID".RequestStr() + "'";
            DataTable Tb10 = Ds.Tables[10].DefaultView.ToTable();

            Ds.Tables.Clear();

            Ds.Tables.Add(Tb0);
            Ds.Tables.Add(Tb1);
            Ds.Tables.Add(Tb2);
            Ds.Tables.Add(Tb3);
            Ds.Tables.Add(Tb4);
            Ds.Tables.Add(Tb5);
            Ds.Tables.Add(Tb6);
            Ds.Tables.Add(Tb7);
            Ds.Tables.Add(Tb8);
            Ds.Tables.Add(Tb9);
            Ds.Tables.Add(Tb10);
        }
        else
        {
            
        }

        #endregion

        Obj.ZLAll = "总数:"+Ds.Tables[10].Compute("sum(ct)", "").ToString()+"份";
        //总数
        Obj.ZL = Ds.Tables[0].Compute("sum(ct)", "").ToString();
        Obj.BZL = Ds.Tables[1].Compute("sum(ct)", "").ToString();


        #region 混凝土原材
        //混凝土原材
        Obj.HNT = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂')").ToString();
        Obj.BHNT = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂')").ToString(); 
        


        //水泥
        Obj.SN = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('水泥')").ToString();
        Obj.BSN = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('水泥')").ToString();

        Obj.PXSN = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('水泥')").ToString();
        Obj.JZSN = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('水泥')").ToString();
        Obj.PXSN = (Obj.SN.Toint() == 0) ? "0.00%" : ((Obj.PXSN.Todouble() * 100 / Obj.SN.Todouble()).ToString("F2") + "%");
        Obj.JZSN = (Obj.SN.Toint() == 0) ? "0.00%" : ((Obj.JZSN.Todouble() * 100 / Obj.SN.Todouble()).ToString("F2") + "%");

        //细骨料  
        Obj.XGL = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('细骨料')").ToString();
        Obj.BXGL = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('细骨料')").ToString();

        Obj.PXXGL = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('细骨料')").ToString();
        Obj.JZXGL = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('细骨料')").ToString();
        Obj.PXXGL = (Obj.XGL.Toint() == 0) ? "0.00%" : ((Obj.PXXGL.Todouble() * 100 / Obj.XGL.Todouble()).ToString("F2") + "%");
        Obj.JZXGL = (Obj.XGL.Toint() == 0) ? "0.00%" : ((Obj.JZXGL.Todouble() * 100 / Obj.XGL.Todouble()).ToString("F2") + "%");

        //粗骨料 
        Obj.CGL = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('粗骨料')").ToString();
        Obj.BCGL = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('粗骨料')").ToString();

        Obj.PXCGL = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('粗骨料')").ToString();
        Obj.JZCGL = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('粗骨料')").ToString();
        Obj.PXCGL = (Obj.CGL.Toint() == 0) ? "0.00%" : ((Obj.PXCGL.Todouble() * 100 / Obj.CGL.Todouble()).ToString("F2") + "%");
        Obj.JZCGL = (Obj.CGL.Toint() == 0) ? "0.00%" : ((Obj.JZCGL.Todouble() * 100 / Obj.CGL.Todouble()).ToString("F2") + "%");

        //粉煤灰 
        Obj.FMH = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('粉煤灰')").ToString();
        Obj.BFMH = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('粉煤灰')").ToString();

        Obj.PXFMH = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('粉煤灰')").ToString();
        Obj.JZFMH = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('粉煤灰')").ToString();
        Obj.PXFMH = (Obj.FMH.Toint() == 0) ? "0.00%" : ((Obj.PXFMH.Todouble() * 100 / Obj.FMH.Todouble()).ToString("F2") + "%");
        Obj.JZFMH = (Obj.FMH.Toint() == 0) ? "0.00%" : ((Obj.JZFMH.Todouble() * 100 / Obj.FMH.Todouble()).ToString("F2") + "%");

        //外加剂 
        Obj.WJJ = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('外加剂')").ToString();
        Obj.BWJJ = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('外加剂')").ToString();

        Obj.PXWJJ = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('外加剂')").ToString();
        Obj.JZWJJ = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('外加剂')").ToString();
        Obj.PXWJJ = (Obj.WJJ.Toint() == 0) ? "0.00%" : ((Obj.PXWJJ.Todouble() * 100 / Obj.WJJ.Todouble()).ToString("F2") + "%");
        Obj.JZWJJ = (Obj.WJJ.Toint() == 0) ? "0.00%" : ((Obj.JZWJJ.Todouble() * 100 / Obj.WJJ.Todouble()).ToString("F2") + "%");

        //矿粉
        Obj.KF = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('矿粉')").ToString();
        Obj.BKF = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('矿粉')").ToString();

        Obj.PXKF = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('矿粉')").ToString();
        Obj.JZKF = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('矿粉')").ToString();
        Obj.PXKF = (Obj.KF.Toint() == 0) ? "0.00%" : ((Obj.PXKF.Todouble() * 100 / Obj.KF.Todouble()).ToString("F2") + "%");
        Obj.JZKF = (Obj.KF.Toint() == 0) ? "0.00%" : ((Obj.JZKF.Todouble() * 100 / Obj.KF.Todouble()).ToString("F2") + "%");

        //速凝剂
        Obj.SNJ = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('速凝剂')").ToString();
        Obj.BSNJ = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('速凝剂')").ToString();

        Obj.PXSNJ = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('速凝剂')").ToString();
        Obj.JZSNJ = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('速凝剂')").ToString();
        Obj.PXSNJ = (Obj.SNJ.Toint() == 0) ? "0.00%" : ((Obj.PXSNJ.Todouble() * 100 / Obj.SNJ.Todouble()).ToString("F2") + "%");
        Obj.JZSNJ = (Obj.SNJ.Toint() == 0) ? "0.00%" : ((Obj.JZSNJ.Todouble() * 100 / Obj.SNJ.Todouble()).ToString("F2") + "%");

        //引气剂
        Obj.YQJ = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('引气剂')").ToString();
        Obj.BYQJ = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('引气剂')").ToString();

        Obj.PXYQJ = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('引气剂')").ToString();
        Obj.JZYQJ = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('引气剂')").ToString();
        Obj.PXYQJ = (Obj.YQJ.Toint() == 0) ? "0.00%" : ((Obj.PXYQJ.Todouble() * 100 / Obj.YQJ.Todouble()).ToString("F2") + "%");
        Obj.JZYQJ = (Obj.YQJ.Toint() == 0) ? "0.00%" : ((Obj.JZYQJ.Todouble() * 100 / Obj.YQJ.Todouble()).ToString("F2") + "%");

        #endregion

        #region 混凝土抗压

        //混凝土抗压
        Obj.HNTKY = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('混凝土（标养）','混凝土（同条件）')").ToString();
        Obj.BHNTKY = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('混凝土（标养）','混凝土（同条件）')").ToString();



        //同条件
        Obj.TTJ = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('混凝土（同条件）')").ToString();
        Obj.BTTJ = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('混凝土（同条件）')").ToString();

        Obj.PXTTJ = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('混凝土（同条件）')").ToString();
        Obj.JZTTJ = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('混凝土（同条件）')").ToString();
        Obj.PXTTJ = (Obj.TTJ.Toint() == 0) ? "0.00%" : ((Obj.PXTTJ.Todouble() * 100 / Obj.TTJ.Todouble()).ToString("F2") + "%");
        Obj.JZTTJ = (Obj.TTJ.Toint() == 0) ? "0.00%" : ((Obj.JZTTJ.Todouble() * 100 / Obj.TTJ.Todouble()).ToString("F2") + "%");

        //标养
        Obj.BY = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('混凝土（标养）')").ToString();
        Obj.BBY = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('混凝土（标养）')").ToString();

        Obj.PXBY = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('混凝土（标养）')").ToString();
        Obj.JZBY = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('混凝土（标养）')").ToString();
        Obj.PXBY = (Obj.BY.Toint() == 0) ? "0.00%" : ((Obj.PXBY.Todouble() * 100 / Obj.BY.Todouble()).ToString("F2") + "%");
        Obj.JZBY = (Obj.BY.Toint() == 0) ? "0.00%" : ((Obj.JZBY.Todouble() * 100 / Obj.BY.Todouble()).ToString("F2") + "%");

        #endregion

        #region 钢筋

        //钢筋
        Obj.GJ = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('钢筋','钢筋焊接','钢筋机械连接')").ToString();
        Obj.BGJ = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('钢筋','钢筋焊接','钢筋机械连接')").ToString();

        //原材
        Obj.YC = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('钢筋')").ToString();
        Obj.BYC = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('钢筋')").ToString();

        Obj.PXYC = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('钢筋')").ToString();
        Obj.JZYC = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('钢筋')").ToString();
        Obj.PXYC = (Obj.YC.Toint() == 0) ? "0.00%" : ((Obj.PXYC.Todouble() * 100 / Obj.YC.Todouble()).ToString("F2") + "%");
        Obj.JZYC = (Obj.YC.Toint() == 0) ? "0.00%" : ((Obj.JZYC.Todouble() * 100 / Obj.YC.Todouble()).ToString("F2") + "%");

        //焊接
        Obj.HJ = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('钢筋焊接')").ToString();
        Obj.BHJ = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('钢筋焊接')").ToString();

        Obj.PXHJ = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('钢筋焊接')").ToString();
        Obj.JZHJ = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('钢筋焊接')").ToString();
        Obj.PXHJ = (Obj.HJ.Toint() == 0) ? "0.00%" : ((Obj.PXHJ.Todouble() * 100 / Obj.HJ.Todouble()).ToString("F2") + "%");
        Obj.JZHJ = (Obj.HJ.Toint() == 0) ? "0.00%" : ((Obj.JZHJ.Todouble() * 100 / Obj.HJ.Todouble()).ToString("F2") + "%");

        //接头
        Obj.JT = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('钢筋机械连接')").ToString();
        Obj.BJT = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('钢筋机械连接')").ToString();

        Obj.PXJT = Ds.Tables[2].Compute("sum(ct)", "StatisticsCatlog in('钢筋机械连接')").ToString();
        Obj.JZJT = Ds.Tables[3].Compute("sum(ct)", "StatisticsCatlog in('钢筋机械连接')").ToString();
        Obj.PXJT = (Obj.JT.Toint() == 0) ? "0.00%" : ((Obj.PXJT.Todouble()*100 / Obj.JT.Todouble()).ToString("F2") + "%");
        Obj.JZJT = (Obj.JT.Toint() == 0) ? "0.00%" : ((Obj.JZJT.Todouble() * 100 / Obj.JT.Todouble()).ToString("F2") + "%");
        #endregion

        #region 其他
        //其他
        Obj.QT = Ds.Tables[0].Compute("sum(ct)", "StatisticsCatlog in('其它')").ToString();
        Obj.BQT = Ds.Tables[1].Compute("sum(ct)", "StatisticsCatlog in('其它')").ToString();

        Obj.DCL = Ds.Tables[5].Compute("sum(ct)", "StatisticsCatlog in('其它','钢筋','钢筋焊接','钢筋机械连接','混凝土（标养）','混凝土（同条件）','粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂')").ToString();
        Obj.YCL = Ds.Tables[4].Compute("sum(ct)", "StatisticsCatlog in('其它','钢筋','钢筋焊接','钢筋机械连接','混凝土（标养）','混凝土（同条件）','粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂')").ToString();
        #endregion

        #region 设备
        Obj.SB = Ds.Tables[8].Compute("sum(ct)", "").ToString();
        Obj.DSB = Ds.Tables[9].Compute("sum(ct)", "").ToString();
        #endregion

        #region 人员
        Obj.RS = Ds.Tables[7].Compute("sum(ct)", "").ToString();
        Obj.XZRS = Ds.Tables[7].Compute("sum(act)", "").ToString();
        Obj.JS = Ds.Tables[7].Compute("sum(dct)", "").ToString();
        Obj.DL = Ds.Tables[6].Compute("sum(ct)", "").ToString();
        #endregion
       

    }

    public JZDocument doc;
    public void RoomInfo()
    {
        string Sql = "select data from sys_document where moduleid ='E77624E9-5654-4185-9A29-8229AAFDD68B' AND testroomcode='"+"RoomID".RequestStr()+"'";
        DataSet Ds = GetDs(Sql);
        if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
        {
            doc = Newtonsoft.Json.JsonConvert.DeserializeObject<JZDocument>(Ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            doc = new JZDocument();
        }

    }

    public string RoomInfo(string Cell)
    {
        try
        {
            return JZCommonHelper.GetCellValue(doc, doc.Sheets[0].ID, Cell).ToString();
        }
        catch {
            return "";
        }
    }

    #endregion

    #region 数据库操作

    /// <summary>
    /// 获取连接字符串
    /// </summary>
    public string ConnStr
    {
        get
        {
            DataSet Ds = GetUserLines(" and ID='" + "LID".RequestStr() + "' ");
            string _ConnStr = "server={0};database={1};uid={2};pwd={3};";


            foreach (DataRow Dr in Ds.Tables[0].Rows)
            {
                _ConnStr = string.Format(_ConnStr, Dr["DataSourceAddress"].ToString(), Dr["DataBaseName"].ToString(), Dr["SaUserName"].ToString(), Dr["SaPassWord"].ToString());
            }

            return _ConnStr;
        }
    }


    /// <summary>
    /// 获取用户线路
    /// </summary>
    public DataSet GetUserLines(string strWhere)
    {
        DataSet Result = new DataSet();
        if (Session["UserName"] != null)
        {
            string strSql = @"SELECT 
          ID as LineID ,
          LineName ,
          [Description] ,
          DataSourceAddress ,
          UserName as SaUserName ,
          PassWord as SaPassWord ,
          DataBaseName ,
          IsActive,TestMapJson,LinesJson FROM  dbo.sys_Line  WHERE 1=1  ";

            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql += strWhere;
            }
            Result = LineDbHelperSQL.Query(strSql);
        }
        else
        {
            return null;
        }

        return Result;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="ConnStr"></param>
    /// <param name="Sql"></param>
    /// <returns></returns>
    public DataSet GetDs( string Sql)
    {
        DataSet Result = new DataSet();
        using (SqlConnection _Conn = new SqlConnection(ConnStr))
        {
            _Conn.Open();
            using (SqlCommand _Com = new SqlCommand(Sql, _Conn))
            {
                using (SqlDataAdapter _Adp = new SqlDataAdapter(_Com))
                {
                    _Adp.Fill(Result);
                }
            }
            _Conn.Close();
        }
        return Result;
    }

    #endregion


}


public class RightData
{
    public string ZLAll = " ";

    public string ZL="0";
    public string BZL="0";

    public string HNT="0";
    public string BHNT="0";
    public string JZHNT="0";
    public string PXHNT="0";

    //矿粉
    public string KF = "0";
    public string BKF = "0";
    public string JZKF = "0";
    public string PXKF = "0";
    //速凝剂
    public string SNJ = "0";
    public string BSNJ = "0";
    public string JZSNJ = "0";
    public string PXSNJ = "0";
    //引气剂
    public string YQJ = "0";
    public string BYQJ = "0";
    public string JZYQJ = "0";
    public string PXYQJ = "0";

    public string SN="0";
    public string BSN="0";
    public string JZSN="0";
    public string PXSN="0";

    public string XGL="0";
    public string BXGL="0";
    public string JZXGL="0";
    public string PXXGL="0";

    public string CGL="0";
    public string BCGL="0";
    public string JZCGL="0";
    public string PXCGL="0";

    public string FMH="0";
    public string BFMH="0";
    public string JZFMH="0";
    public string PXFMH="0";

    public string WJJ="0";
    public string BWJJ="0";
    public string JZWJJ="0";
    public string PXWJJ="0";

    public string HNTKY="0";
    public string BHNTKY="0";
    public string JZHNTKY="0";
    public string PXHNTKY="0";

    public string TTJ="0";
    public string BTTJ="0";
    public string JZTTJ="0";
    public string PXTTJ="0";

    public string BY="0";
    public string BBY="0";
    public string JZBY="0";
    public string PXBY="0";


    public string GJ="0";
    public string BGJ="0";
    public string JZGJ="0";
    public string PXGJ="0";

    public string YC="0";
    public string BYC="0";
    public string JZYC="0";
    public string PXYC="0";

    public string HJ="0";
    public string BHJ="0";
    public string JZHJ="0";
    public string PXHJ="0";

    public string JT="0";
    public string BJT="0";
    public string JZJT="0";
    public string PXJT="0";

    public string QT="0";
    public string BQT="0";
    public string DCL="0";
    public string YCL="0";


    public string SB="0";
    public string DSB="0";

    public string RS="0";
    public string XZRS="0";
    public string JS="0";
    public string DL="0";

}
