<%@ Page Language="C#" AutoEventWireup="true" CodeFile="oneuser.aspx.cs" Inherits="baseInfo_oneuser" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<link href="../css/css.css" rel="stylesheet" />--%>

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
     <link href="../css/colorbox.css" rel="Stylesheet" />
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
</head>
<body>
    <form id="form1" runat="server">

        <table>


            <tbody>
                <tr>
                    <th>姓名</th>
                    <td>
                        <asp:Label ID="lbl_xingming" runat="server" Text=""></asp:Label>
                    </td>
                    <th>性别</th>
                    <td>
                        <asp:Label ID="lbl_xingbie" runat="server" Text=""></asp:Label></td>
                    <th>年龄</th>
                    <td>
                        <asp:Label ID="lbl_nianling" runat="server" Text=""></asp:Label></td>
                     <td rowspan="5" style="text-align:center;">
                   <asp:Literal ID="Literal3" runat="server"></asp:Literal>

                         

                </td>
                   
                </tr>
                <tr>
                    <th>技术职称</th>
                    <td>
                        <asp:Label ID="lbl_jishuzhicheng" runat="server" Text=""></asp:Label></td>
                    <th>职 务</th>
                    <td>
                        <asp:Label ID="lbl_zhiwu" runat="server" Text=""></asp:Label></td>
                    <th>从事本工种年限</th>
                    <td>
                        <asp:Label ID="lbl_gongzuonianxian" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>所学专业</th>
                    <td>
                        <asp:Label ID="lbl_zhuanye" runat="server" Text=""></asp:Label></td>
                    <th>学 历</th>
                    <td>
                        <asp:Label ID="lbl_xueli" runat="server" Text=""></asp:Label></td>
                    <th>毕业时间</th>
                    <td>
                        <asp:Label ID="lbl_biyeshijian" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>联系电话</th>
                    <td>
                        <asp:Label ID="lbl_lianxidianhua" runat="server" Text=""></asp:Label></td>
                    <th>毕业学校</th>
                    <td colspan="3">
                        <asp:Label ID="lbl_biyexuexiao" runat="server" Text=""></asp:Label></td>

                </tr>
                 <tr>
                    <th>身份证号</th>
                    <td colspan="5">
                        <asp:Label ID="lbl_idcard" runat="server" Text=""></asp:Label>

                    </td>
                    

                </tr>
               <tr>
                    <th>到岗日期</th>
                    <td colspan="2">
                        <asp:Label ID="lbl_dgrq" runat="server" Text=""></asp:Label>

                    </td>
                    
                    <th>离岗日期</th>
                    <td colspan="3">
                        <asp:Label ID="lbl_lgrq" runat="server" Text=""></asp:Label>

                    </td>
                </tr>






            </tbody>
        </table>

        <table>

            <tr id="tr_gzjl" runat="server">
                <th colspan="7" style="text-align: center;">工作经历</th>

            </tr>
            <tr id="tr_gzjl_mx" runat="server">
                <th colspan="2" style="text-align: center;">年 月 至 年 月</th>
                <th colspan="3" style="text-align: center;">参加过施工的工程项目名称</th>
                <th style="text-align: center;">担任何职</th>
                <th style="text-align: center;">备注</th>
            </tr>

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </table>

        <table>


            <tr id="tr_pxjl" runat="server">
                <th colspan="7" style="text-align: center;">培训经历</th>

            </tr>
            <tr id="tr_pxjl_mx" runat="server">
                <th colspan="2" style="text-align: center;">年 月 至 年 月</th>
                <th colspan="4" style="text-align: center;">培训内容</th>
                <th style="text-align: center;">取得资格证书种类及编号</th>


            </tr>
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
        </table>

    </form>
</body>
</html>
<script type="text/javascript">

    function Show(id, sid,i)
    {
        //$.colorbox({
        //    href: "oneuserJDZS.aspx?id="+id+"&sid="+sid+"&r=" + Math.random(),
        //    width: 850,
        //    height: 620,
        //    title: function () {
        //        return "";
        //    },
        //    close: '',
        //    iframe: true
        //});

        $.colorbox({
            href: "oneuserJDZS.aspx?i="+i+"&id=" + id + "&sid=" + sid + "&r=" + Math.random(),

            title: function () {
                return '';
            }, close: ' '
        });
    }

</script>