<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForIndexMapRight.aspx.cs" Inherits="ForIndexMapRight" %>

<%if ("Act".RequestStr() == "RoomInfo")
  {%> 

   <table width="100%"  class="Info" style="border:solid 1px #a0c6e5;" >


  <tr>
    <td width="20%" align="center" bgcolor="#f5f7fa">单位名称</td>
    <td colspan="3" align="left"><%=RoomInfo("D4") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">中心试验室名称</td>
    <td colspan="3" align="left"><a target="_blank" href="report/TestRoomInfo.aspx?id=<%="RoomID".RequestStr() %>"><%=RoomInfo("D5") %></a></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">母体试验室名称</td>
    <td colspan="3" align="left"><%=RoomInfo("D6") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">试验室通讯地址</td>
    <td colspan="3" align="left"><%=RoomInfo("D7") %></td>
  </tr>
  <tr>
    <td width="20%" align="center" bgcolor="#f5f7fa">总工程师</td>
    <td width="30%" align="center"><%=RoomInfo("K4") %></td>
    <td width="20%" align="center" bgcolor="#f5f7fa">联系电话</td>
    <td align="center"><%=RoomInfo("O4") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">主任姓名</td>
    <td align="center"><%=RoomInfo("K5") %></td>
    <td align="center" bgcolor="#f5f7fa">联系电话</td>
    <td align="center"><%=RoomInfo("O5") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">负责人</td>
    <td align="center"><%=RoomInfo("K6") %></td>
    <td align="center" bgcolor="#f5f7fa">联系电话</td>
    <td align="center"><%=RoomInfo("O6") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">邮编</td>
    <td align="center"><%=RoomInfo("K7") %></td>
    <td align="center" bgcolor="#f5f7fa">传真</td>
    <td align="center"><%=RoomInfo("O7") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">试验室面积(㎡)</td>
    <td align="center"><%=RoomInfo("G20") %></td>
    <td align="center" bgcolor="#f5f7fa">标准养护室面积(㎡)</td>
    <td align="center"><%=RoomInfo("L20") %></td>
  </tr>
  <tr>
    <td align="center" bgcolor="#f5f7fa">主要试验检测项目</td>
    <td colspan="3" align="left"><%=(RoomInfo("A22").Length>200?"<a href=\"#\" title=\""+RoomInfo("A22")+"\">"+RoomInfo("A22").Substring(0,200)+"...</a>":RoomInfo("A22")) %></td>
  </tr>
  </table>
 

  <%} 
   %>

  
<% if ("RoomType".RequestStr() == "@unit_施工单位")
   { %>

 <table width="100%" style="border:solid 1px #a0c6e5;">

  <tr>
    <td colspan="5" align="center" bgcolor="#f5f7fa">资料信息 <%=Obj.ZLAll %></td>
  </tr>
  <tr>
    <td bgcolor="#f5f7fa">&nbsp;</td>
    <td align="center" bgcolor="#f5f7fa">资料数</td>
    <td align="center" bgcolor="#f5f7fa">不合格数</td>
    <td width="20%" rowspan="2" align="center" valign="middle" bgcolor="#f5f7fa">平行率</td>
    <td width="20%" rowspan="2" align="center" valign="middle" bgcolor="#f5f7fa">见证率</td>
  </tr>
  <tr>
    <td width="20%" bgcolor="#f5f7fa">资料总数</td>
    <td width="20%" align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.ZL.IsNullOrEmpty()?"0":Obj.ZL %>份</a></td>
    <td width="20%" align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BZL.IsNullOrEmpty()?"0":Obj.BZL %>份</a></td>
  </tr>
  <tr>
    <td bgcolor="#f5f7fa">混凝土原材</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.HNT.IsNullOrEmpty()?"0":Obj.HNT %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BHNT.IsNullOrEmpty()?"0":Obj.BHNT %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><%=Obj.PXHNT.IsNullOrEmpty()?"0":Obj.PXHNT %></td>
    <td align="center" bgcolor="#f5f7fa"><%=Obj.JZHNT.IsNullOrEmpty()?"0":Obj.JZHNT %></td>
  </tr>
  <tr>
    <td>水泥</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'水泥'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.SN.IsNullOrEmpty()?"0":Obj.SN %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'水泥'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BSN.IsNullOrEmpty()?"0":Obj.BSN %>份</a></td>
    <td align="center"><%=Obj.PXSN.IsNullOrEmpty()?"0":Obj.PXSN %></td>
    <td align="center"><%=Obj.JZSN.IsNullOrEmpty()?"0":Obj.JZSN %></td>
  </tr>
  <tr>
    <td>细骨料</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'细骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.XGL.IsNullOrEmpty()?"0":Obj.XGL %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'细骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BXGL.IsNullOrEmpty()?"0":Obj.BXGL %>份</a></td>
    <td align="center"><%=Obj.PXXGL.IsNullOrEmpty()?"0":Obj.PXXGL %></td>
    <td align="center"><%=Obj.JZXGL.IsNullOrEmpty()?"0":Obj.JZXGL %></td>
  </tr>
  <tr>
    <td>粗骨料</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'粗骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.CGL.IsNullOrEmpty()?"0":Obj.CGL %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'粗骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BCGL.IsNullOrEmpty()?"0":Obj.BCGL %>份</a></td>
    <td align="center"><%=Obj.PXCGL.IsNullOrEmpty()?"0":Obj.PXCGL %></td>
    <td align="center"><%=Obj.JZCGL.IsNullOrEmpty()?"0":Obj.JZCGL %></td>
  </tr>
  <tr>
    <td>粉煤灰</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'粉煤灰'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.FMH.IsNullOrEmpty()?"0":Obj.FMH %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'粉煤灰'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BFMH.IsNullOrEmpty()?"0":Obj.BFMH %>份</a></td>
    <td align="center"><%=Obj.PXFMH.IsNullOrEmpty()?"0":Obj.PXFMH %></td>
    <td align="center"><%=Obj.JZFMH.IsNullOrEmpty()?"0":Obj.JZFMH %></td>
  </tr>
  <tr>
    <td>外加剂</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'外加剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.WJJ.IsNullOrEmpty()?"0":Obj.WJJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'外加剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BWJJ.IsNullOrEmpty()?"0":Obj.BWJJ %>份</a></td>
    <td align="center"><%=Obj.PXWJJ.IsNullOrEmpty()?"0":Obj.PXWJJ %></td>
    <td align="center"><%=Obj.JZWJJ.IsNullOrEmpty()?"0":Obj.JZWJJ %></td>
  </tr>
     <tr>
       <td>矿粉</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'矿粉'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.KF.IsNullOrEmpty()?"0":Obj.KF %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'矿粉'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BKF.IsNullOrEmpty()?"0":Obj.BKF %>份</a></td>
    <td align="center"><%=Obj.PXKF.IsNullOrEmpty()?"0":Obj.PXKF %></td>
    <td align="center"><%=Obj.JZKF.IsNullOrEmpty()?"0":Obj.JZKF %></td>
  </tr>
      <tr>
        <td>速凝剂</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'速凝剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.SNJ.IsNullOrEmpty()?"0":Obj.SNJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'速凝剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BSNJ.IsNullOrEmpty()?"0":Obj.BSNJ %>份</a></td>
    <td align="center"><%=Obj.PXSNJ.IsNullOrEmpty()?"0":Obj.PXSNJ %></td>
    <td align="center"><%=Obj.JZSNJ.IsNullOrEmpty()?"0":Obj.JZSNJ %></td>
  </tr>
      <tr>
        <td>引气剂</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.YQJ.IsNullOrEmpty()?"0":Obj.YQJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BYQJ.IsNullOrEmpty()?"0":Obj.BYQJ %>份</a></td>
    <td align="center"><%=Obj.PXYQJ.IsNullOrEmpty()?"0":Obj.PXYQJ %></td>
    <td align="center"><%=Obj.JZYQJ.IsNullOrEmpty()?"0":Obj.JZYQJ %></td>
  </tr>
  <tr>
    <td bgcolor="#f5f7fa">混凝土抗压</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）','混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.HNTKY.IsNullOrEmpty()?"0":Obj.HNTKY %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）','混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BHNTKY.IsNullOrEmpty()?"0":Obj.BHNTKY %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><%=Obj.PXHNTKY.IsNullOrEmpty()?"0":Obj.PXHNTKY %></td>
    <td align="center" bgcolor="#f5f7fa"><%=Obj.JZHNTKY.IsNullOrEmpty()?"0":Obj.JZHNTKY %></td>
  </tr>
  <tr>
    <td>同条件</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.TTJ.IsNullOrEmpty()?"0":Obj.TTJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BTTJ.IsNullOrEmpty()?"0":Obj.BTTJ %>份</a></td>
    <td align="center"><%=Obj.PXTTJ.IsNullOrEmpty()?"0":Obj.PXTTJ %></td>
    <td align="center"><%=Obj.JZTTJ.IsNullOrEmpty()?"0":Obj.JZTTJ %></td>
  </tr>
  <tr>
    <td>标养</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BY.IsNullOrEmpty()?"0":Obj.BY %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BBY.IsNullOrEmpty()?"0":Obj.BBY %>份</a></td>
    <td align="center"><%=Obj.PXBY.IsNullOrEmpty()?"0":Obj.PXBY %></td>
    <td align="center"><%=Obj.JZBY.IsNullOrEmpty()?"0":Obj.JZBY %></td>
  </tr>
  <tr>
    <td bgcolor="#f5f7fa">钢筋试验数</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋','钢筋焊接','钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.GJ.IsNullOrEmpty()?"0":Obj.GJ %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋','钢筋焊接','钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BGJ.IsNullOrEmpty()?"0":Obj.BGJ %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><%=Obj.PXGJ.IsNullOrEmpty()?"0":Obj.PXGJ %></td>
    <td align="center" bgcolor="#f5f7fa"><%=Obj.JZGJ.IsNullOrEmpty()?"0":Obj.JZGJ %></td>
  </tr>
  <tr>
    <td>原材</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.YC.IsNullOrEmpty()?"0":Obj.YC %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BYC.IsNullOrEmpty()?"0":Obj.BYC %>份</a></td>
    <td align="center"><%=Obj.PXYC.IsNullOrEmpty()?"0":Obj.PXYC %></td>
    <td align="center"><%=Obj.JZYC.IsNullOrEmpty()?"0":Obj.JZYC %></td>
  </tr>
  <tr>
    <td>焊接</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋焊接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.HJ.IsNullOrEmpty()?"0":Obj.HJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋焊接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BHJ.IsNullOrEmpty()?"0":Obj.BHJ %>份</a></td>
    <td align="center"><%=Obj.PXHJ.IsNullOrEmpty()?"0":Obj.PXHJ %></td>
    <td align="center"><%=Obj.JZHJ.IsNullOrEmpty()?"0":Obj.JZHJ %></td>
  </tr>
  <tr>
    <td>接头</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.JT.IsNullOrEmpty()?"0":Obj.JT %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BJT.IsNullOrEmpty()?"0":Obj.BJT %>份</a></td>
    <td align="center"><%=Obj.PXJT.IsNullOrEmpty()?"0":Obj.PXJT %></td>
    <td align="center"><%=Obj.JZJT.IsNullOrEmpty()?"0":Obj.JZJT %></td>
  </tr>
  <tr>
    <td bgcolor="#f5f7fa">其他</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'其它'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.QT.IsNullOrEmpty()?"0":Obj.QT %>份</a></td>

    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'其它'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BQT.IsNullOrEmpty()?"0":Obj.BQT %>份</a></td>
    <td align="center" bgcolor="#f5f7fa">&nbsp;</td>
    <td align="center" bgcolor="#f5f7fa">&nbsp;</td>
  </tr>
  <tr>
    <td>待处理</td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("未处理") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.YCL.IsNullOrEmpty()?"0":Obj.YCL %>份</a></td>
    <td>已处理</td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("已处理") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.DCL.IsNullOrEmpty()?"0":Obj.DCL %>份</a></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="5" align="center" bgcolor="#f5f7fa">设备信息</td>
  </tr>
  <tr>
    <td>设备总数</td>
    <td align="center"><a href="baseInfo/machineSummary.aspx?RPName=ALL&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.SB.IsNullOrEmpty()?"0":Obj.SB %>台</a></td>
    <td>待标定数</td>
    <td align="center"><a href="baseInfo/machineSummary.aspx?RPName=DBD&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.DSB.IsNullOrEmpty()?"0":Obj.DSB %>台</a></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="5" align="center" bgcolor="#f5f7fa">人员信息</td>
  </tr>
  <tr>
    <td>人员总数</td>
    <td align="center"><a href="baseInfo/userSummary.aspx?RPName=ALL&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.RS.IsNullOrEmpty()?"0":Obj.RS %>个</a></td>
    <td>新增人员</td>
    <td align="center"><a href="baseInfo/userSummary.aspx?RPName=ADD&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.XZRS.IsNullOrEmpty()?"0":Obj.XZRS %>个</a></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>人员减少</td>
    <td align="center"><a href="baseInfo/userSummary.aspx?RPName=DEL&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.JS.IsNullOrEmpty()?"0":Obj.JS %>个</a></td>
    <td>登录次数</td>
    <td align="center"><a href="logInfo/loginLog.aspx?Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.DL.IsNullOrEmpty()?"0":Obj.DL %>个</a></td>
    <td>&nbsp;</td>
  </tr>
</table>

<%}
   else if ("Act".RequestStr() != "RoomInfo")
  {%>
<table width="100%" style="border:solid 1px #a0c6e5;">

  <tr>
    <td colspan="3" align="center" bgcolor="#f5f7fa">资料信息 <%=Obj.ZLAll %></td>
  </tr>
  <tr>
    <td width="40%" bgcolor="#f5f7fa">&nbsp;</td>
    <td width="30%" align="center" bgcolor="#f5f7fa">资料数</td>
    <td width="30%" align="center" bgcolor="#f5f7fa">不合格数</td>
  </tr>
  <tr>
    <td width="25%" bgcolor="#f5f7fa">资料总数</td>
    <td width="25%" align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.ZL.IsNullOrEmpty()?"0":Obj.ZL %>份</a></td>
    <td width="25%" align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BZL.IsNullOrEmpty()?"0":Obj.BZL %>份</a></td>

  </tr>
  <tr>
    <td bgcolor="#f5f7fa">混凝土原材</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.HNT.IsNullOrEmpty()?"0":Obj.HNT %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'粗骨料','粉煤灰','矿粉','水泥','速凝剂','外加剂','细骨料','引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BHNT.IsNullOrEmpty()?"0":Obj.BHNT %>份</a></td>

  </tr>
  <tr>
    <td>水泥</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'水泥'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.SN.IsNullOrEmpty()?"0":Obj.SN %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'水泥'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BSN.IsNullOrEmpty()?"0":Obj.BSN %>份</a></td>

  </tr>
  <tr>
    <td>细骨料</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'细骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.XGL.IsNullOrEmpty()?"0":Obj.XGL %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'细骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BXGL.IsNullOrEmpty()?"0":Obj.BXGL %>份</a></td>

  </tr>
  <tr>
    <td>粗骨料</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'粗骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.CGL.IsNullOrEmpty()?"0":Obj.CGL %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'粗骨料'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BCGL.IsNullOrEmpty()?"0":Obj.BCGL %>份</a></td>

  </tr>
  <tr>
    <td>粉煤灰</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'粉煤灰'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.FMH.IsNullOrEmpty()?"0":Obj.FMH %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'粉煤灰'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BFMH.IsNullOrEmpty()?"0":Obj.BFMH %>份</a></td>

  </tr>
  <tr>
    <td>外加剂</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'外加剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.WJJ.IsNullOrEmpty()?"0":Obj.WJJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'外加剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BWJJ.IsNullOrEmpty()?"0":Obj.BWJJ %>份</a></td>

  </tr>
     <tr>
       <td>矿粉</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'矿粉'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.KF.IsNullOrEmpty()?"0":Obj.KF %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'矿粉'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BKF.IsNullOrEmpty()?"0":Obj.BKF %>份</a></td>

  </tr>
      <tr>
        <td>速凝剂</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'速凝剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.SNJ.IsNullOrEmpty()?"0":Obj.SNJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'速凝剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BSNJ.IsNullOrEmpty()?"0":Obj.BSNJ %>份</a></td>

  </tr>
      <tr>
        <td>引气剂</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.YQJ.IsNullOrEmpty()?"0":Obj.YQJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'引气剂'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BYQJ.IsNullOrEmpty()?"0":Obj.BYQJ %>份</a></td>

  </tr>
  <tr>
    <td bgcolor="#f5f7fa">混凝土抗压</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）','混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.HNTKY.IsNullOrEmpty()?"0":Obj.HNTKY %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）','混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BHNTKY.IsNullOrEmpty()?"0":Obj.BHNTKY %>份</a></td>

  </tr>
  <tr>
    <td>同条件</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.TTJ.IsNullOrEmpty()?"0":Obj.TTJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'混凝土（同条件）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BTTJ.IsNullOrEmpty()?"0":Obj.BTTJ %>份</a></td>

  </tr>
  <tr>
    <td>标养</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BY.IsNullOrEmpty()?"0":Obj.BY %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'混凝土（标养）'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BBY.IsNullOrEmpty()?"0":Obj.BBY %>份</a></td>

  </tr>
  <tr>
    <td bgcolor="#f5f7fa">钢筋</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋','钢筋焊接','钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.GJ.IsNullOrEmpty()?"0":Obj.GJ %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋','钢筋焊接','钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BGJ.IsNullOrEmpty()?"0":Obj.BGJ %>份</a></td>

  </tr>
  <tr>
    <td>原材</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.YC.IsNullOrEmpty()?"0":Obj.YC %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BYC.IsNullOrEmpty()?"0":Obj.BYC %>份</a></td>

  </tr>
  <tr>
    <td>焊接</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋焊接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.HJ.IsNullOrEmpty()?"0":Obj.HJ %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋焊接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BHJ.IsNullOrEmpty()?"0":Obj.BHJ %>份</a></td>

  </tr>
  <tr>
    <td>接头</td>
    <td align="center"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.JT.IsNullOrEmpty()?"0":Obj.JT %>份</a></td>
    <td align="center"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'钢筋机械连接'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BJT.IsNullOrEmpty()?"0":Obj.BJT %>份</a></td>

  </tr>
  <tr>
    <td bgcolor="#f5f7fa">其他</td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/Search.aspx?RPName=<%=Server.UrlEncode("'其它'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.QT.IsNullOrEmpty()?"0":Obj.QT %>份</a></td>
    <td align="center" bgcolor="#f5f7fa"><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("'其它'") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.BQT.IsNullOrEmpty()?"0":Obj.BQT %>份</a></td>
  </tr>
  <tr>
    <td >待处理/已处理</td>
    <td align="center" ><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("未处理") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.YCL.IsNullOrEmpty()?"0":Obj.YCL %>份</a></td>
    <td align="center" ><a href="report/failreport.aspx?RPName=<%=Server.UrlEncode("已处理") %>&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.DCL.IsNullOrEmpty()?"0":Obj.DCL %>份</a></td>
  </tr>
  <tr>
    <td colspan="3" align="center" bgcolor="#f5f7fa">设备信息</td>
  </tr>
  <tr>
    <td >设备总数/待标定数</td>
    <td align="center" ><a href="baseInfo/machineSummary.aspx?RPName=ALL&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.SB.IsNullOrEmpty()?"0":Obj.SB %>台</a></td>
    <td align="center"><a href="baseInfo/machineSummary.aspx?RPName=DBD&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.DSB.IsNullOrEmpty()?"0":Obj.DSB %>台</a></td>
  </tr>
  <tr>
    <td colspan="3" align="center" bgcolor="#f5f7fa">人员信息</td>
  </tr>
  <tr>
    <td >人员总数/新增人员</td>
    <td align="center" ><a href="baseInfo/userSummary.aspx?RPName=ALL&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.RS.IsNullOrEmpty()?"0":Obj.RS %>个</a></td>
    <td align="center" ><a href="baseInfo/userSummary.aspx?RPName=ADD&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.XZRS.IsNullOrEmpty()?"0":Obj.XZRS %>个</a></td>
  </tr>
  <tr>
    <td>人员减少/登录次数</td>
    <td align="center"><a href="baseInfo/userSummary.aspx?RPName=DEL&Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.JS.IsNullOrEmpty()?"0":Obj.JS %>个</a></td>
    <td align="center"><a href="logInfo/loginLog.aspx?Type=<%="SearchType".RequestStr() %><%="RoomID".RequestStr()=="ALL"?"":"&num="+ "RoomID".RequestStr()%>" target="_blank"><%=Obj.DL.IsNullOrEmpty()?"0":Obj.DL %>个</a></td>
  </tr>
</table>

<%}%>
