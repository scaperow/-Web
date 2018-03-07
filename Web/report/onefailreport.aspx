<%@ Page Language="C#" AutoEventWireup="true" CodeFile="onefailreport.aspx.cs" Inherits="report_onefailreport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
            <link href="../css/colorbox.css" rel="Stylesheet" />
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
<style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            /*background: #f1f1f1;*/
            font: 14px Arial, Helvetica, sans-serif;
            color: #555;
            line-height: 150%;
            text-align: left;
        }

        table, td
        {
            font: 100% Arial, Helvetica, sans-serif;
        }

        table
        {
            width: 100%;
            border-collapse: collapse;
            margin: 1em 0;
        }

        th, td
        {
            text-align: left;
            padding: .5em;
            border: 1px solid #fff;
        }

        th
        {
            background: #438EB9;
            color: #fff;
        }

        td
        {
            background: #e5f1f4;
        }
    </style>

    		<script>
    		    $(document).ready(function () {    		       
    		        $(".group4").colorbox({
    		            //rel: 'group4',
    		            //slideshow: true,
    		            width: 600,
    		            height: 380,
    		            close: '×'
    		        });
    		       
    		    });
		</script>
</head>
<body>
    <form id="form1" runat="server">
        <table>


            <tbody>
                <tr>
                    <th style="width:10%;">标段</th>
                    <td>
                        <asp:Label ID="lbl_biaoduan" runat="server" Text=""></asp:Label>
                    </td>
                    <th style="width:10%;">单位</th>
                    <td>
                        <asp:Label ID="lbl_danwei" runat="server" Text=""></asp:Label></td>
                    <th style="width:10%;">试验室</th>
                    <td>
                        <asp:Label ID="lbl_shiyanshi" runat="server" Text=""></asp:Label></td>
                    <%-- <td rowspan="4">
                  

                </td>--%>
                </tr>
                <tr>
                    <th>试验报告</th>
                    <td>
                        <asp:Label ID="lbl_shiyanbaogao" runat="server" Text=""></asp:Label></td>
                    <th>报告编号</th>
                    <td>
                        <asp:Label ID="lbl_baogaobianhao" runat="server" Text=""></asp:Label></td>
                    <th>报告日期</th>
                    <td>
                        <asp:Label ID="lbl_baogaoriqi" runat="server" Text=""></asp:Label></td>
                </tr>
                
                

            </tbody>
        </table>

        <table>
            <tr id="tr_gzjl_mx" runat="server">
                <th   style="text-align: center;">不合格项目</th>
                <th   style="text-align: center;">标准规定值</th>
                <th style="text-align: center;">实测值</th>
            </tr>
         
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </table>

        <table>


            <tbody>
                <tr>
                    <th style="width:10%;">原因分析</th>
                    <td>
                        <asp:Label ID="lbl_yuanyinfenxi" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>监理意见</th>
                    <td>
                        <asp:Label ID="lbl_jianliyijian" runat="server" Text=""></asp:Label></td>                    
                </tr>
                  <tr>
                    <th>处理结果</th>
                    <td>
                        <asp:Label ID="lbl_chulijiegou" runat="server" Text=""></asp:Label></td>                    
                </tr>
                
                

            </tbody>
        </table>

        <table>


            <tbody>
                <tr>
                    <th style="text-align: center;">相关图片</th>                    
                </tr>
                
                <tr>

                    <td>
		<%--<p><a class="group4"  href="../content/ohoopee1.jpg" title="相关图片1">相关图片1</a></p>
		<p><a class="group4"  href="../content/ohoopee2.jpg" title="相关图片2">相关图片2</a></p>
		<p><a class="group4"  href="../content/ohoopee3.jpg" title="相关图片3">相关图片3</a></p>--%>

                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
          
                    </td>
                </tr>
                

            </tbody>
        </table>
    </form>
</body>
</html>
