<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestRoomInfo.aspx.cs" Inherits="report_TestRoomInfo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script type="text/javascript" src="../js/jquery-1.9.0.min.js"></script>
        <style>

        .Info  {
            border: solid 0px rgb(208,215,229);
    
         
            background:rgb(208,215,229);
           
        }
            .Info TH, .Info TD {
                background: #fff;
            }

    </style>
</head>
<body style="background-color:#fff">
    <form id="form1" runat="server">
 
<table width="100%" border="0" align="right"  class="Info" cellpadding="5" cellspacing="1">
  <tr>
    <th colspan="10"><h1><%=LineName %></h1></th>
  </tr>

  <tr>
    <td width="10%">标段</td>
    <td colspan="9"><%=RoomInfo("D3") %></td>
  </tr>
  <tr>
    <td>单位名称</td>
    <td colspan="5"><%=RoomInfo("D4") %></td>
    <td>总工程师</td>
    <td><%=RoomInfo("K4") %></td>
    <td>联系电话</td>
    <td><%=RoomInfo("O4") %></td>
  </tr>
  <tr>
    <td>中心试验室名称</td>
    <td colspan="5"><%=RoomInfo("D5") %></td>
    <td>主任名称</td>
    <td><%=RoomInfo("K5") %></td>
    <td>联系电话</td>
    <td><%=RoomInfo("O5") %></td>
  </tr>
  <tr>
    <td>母体试验室名称</td>
    <td colspan="5"><%=RoomInfo("D6") %></td>
    <td>负责人姓名</td>
    <td><%=RoomInfo("K6") %></td>
    <td>联系电话</td>
    <td><%=RoomInfo("O6") %></td>
  </tr>
  <tr>
    <td>试验室通讯地址</td>
    <td colspan="5"><%=RoomInfo("D7") %></td>
    <td>邮编</td>
    <td><%=RoomInfo("K7") %></td>
    <td>传真</td>
    <td><%=RoomInfo("O7") %></td>
  </tr>
  <tr>
    <td rowspan="10">试验室主要人员基本情况</td>
    <td>姓名</td>
    <td>年龄</td>
    <td>性别</td>
    <td>所学专业及学历</td>
    <td>技术职称</td>
    <td>工作年限</td>
    <td>岗位</td>
    <td>证书编号</td>
    <td>发证单位</td>
  </tr>
  <tr>
    <td><%=RoomInfo("B9") %></td>
    <td><%=RoomInfo("C9") %></td>
    <td><%=RoomInfo("E9") %></td>
    <td><%=RoomInfo("F9") %></td>
    <td><%=RoomInfo("G9") %></td>
    <td><%=RoomInfo("I9") %></td>
    <td><%=RoomInfo("J9") %></td>
    <td><%=RoomInfo("L9") %></td>
    <td><%=RoomInfo("N9") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B10") %></td>
    <td><%=RoomInfo("C10") %></td>
    <td><%=RoomInfo("E10") %></td>
    <td><%=RoomInfo("F10") %></td>
    <td><%=RoomInfo("G10") %></td>
    <td><%=RoomInfo("I10") %></td>
    <td><%=RoomInfo("J10") %></td>
    <td><%=RoomInfo("L10") %></td>
    <td><%=RoomInfo("N10") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B11") %></td>
    <td><%=RoomInfo("C11") %></td>
    <td><%=RoomInfo("E11") %></td>
    <td><%=RoomInfo("F11") %></td>
    <td><%=RoomInfo("G11") %></td>
    <td><%=RoomInfo("I11") %></td>
    <td><%=RoomInfo("J11") %></td>
    <td><%=RoomInfo("L11") %></td>
    <td><%=RoomInfo("N11") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B12") %></td>
    <td><%=RoomInfo("C12") %></td>
    <td><%=RoomInfo("E12") %></td>
    <td><%=RoomInfo("F12") %></td>
    <td><%=RoomInfo("G12") %></td>
    <td><%=RoomInfo("I12") %></td>
    <td><%=RoomInfo("J12") %></td>
    <td><%=RoomInfo("L12") %></td>
    <td><%=RoomInfo("N12") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B13") %></td>
    <td><%=RoomInfo("C13") %></td>
    <td><%=RoomInfo("E13") %></td>
    <td><%=RoomInfo("F13") %></td>
    <td><%=RoomInfo("G13") %></td>
    <td><%=RoomInfo("I13") %></td>
    <td><%=RoomInfo("J13") %></td>
    <td><%=RoomInfo("L13") %></td>
    <td><%=RoomInfo("N13") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B14") %></td>
    <td><%=RoomInfo("C14") %></td>
    <td><%=RoomInfo("E14") %></td>
    <td><%=RoomInfo("F14") %></td>
    <td><%=RoomInfo("G14") %></td>
    <td><%=RoomInfo("I14") %></td>
    <td><%=RoomInfo("J14") %></td>
    <td><%=RoomInfo("L14") %></td>
    <td><%=RoomInfo("N14") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B15") %></td>
    <td><%=RoomInfo("C15") %></td>
    <td><%=RoomInfo("E15") %></td>
    <td><%=RoomInfo("F15") %></td>
    <td><%=RoomInfo("G15") %></td>
    <td><%=RoomInfo("I15") %></td>
    <td><%=RoomInfo("J15") %></td>
    <td><%=RoomInfo("L15") %></td>
    <td><%=RoomInfo("N15") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B16") %></td>
    <td><%=RoomInfo("C16") %></td>
    <td><%=RoomInfo("E16") %></td>
    <td><%=RoomInfo("F16") %></td>
    <td><%=RoomInfo("G16") %></td>
    <td><%=RoomInfo("I16") %></td>
    <td><%=RoomInfo("J16") %></td>
    <td><%=RoomInfo("L16") %></td>
    <td><%=RoomInfo("N16") %></td>
  </tr>
  <tr>
    <td><%=RoomInfo("B17") %></td>
    <td><%=RoomInfo("C17") %></td>
    <td><%=RoomInfo("E17") %></td>
    <td><%=RoomInfo("F17") %></td>
    <td><%=RoomInfo("G17") %></td>
    <td><%=RoomInfo("I17") %></td>
    <td><%=RoomInfo("J17") %></td>
    <td><%=RoomInfo("L17") %></td>
    <td><%=RoomInfo("N17") %></td>
  </tr>
  <tr>
    <td rowspan="3">设备状况</td>
    <td colspan="4">主要仪器设备</td>
    <td colspan="2" rowspan="2"><p>试验室面积(m2)</p></td>
    <td colspan="3" rowspan="2">标准养护室面积(m2)</td>
  </tr>
  <tr>
    <td colspan="2">数量台/套</td>
    <td colspan="2">总价值(万元)</td>
  </tr>
  <tr>
    <td colspan="2"><%=RoomInfo("B20") %></td>
    <td colspan="2"><%=RoomInfo("E20") %></td>
    <td colspan="2"><%=RoomInfo("G20") %></td>
    <td colspan="3"><%=RoomInfo("L20") %></td>
  </tr>
  <tr>
    <td colspan="10">主要试验检测项目:</td>
  </tr>
  <tr>
    <td colspan="10"><%=RoomInfo("A22") %></td>
  </tr>
</table>


    </form>
</body>
</html>


