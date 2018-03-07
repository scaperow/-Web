<%@ Page Language="C#" AutoEventWireup="true" CodeFile="onesearch.aspx.cs" Inherits="report_onesearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script type="text/javascript" src="../js/jquery-1.9.0.min.js"></script>
</head>
<body style="background-color:#fff">
    <form id="form1" runat="server">
        
        <table width="100%" align="right"  >
            <tr>
                <td id="html"  runat="server" align="center" ></td>
            </tr>
        </table>
    </form>
</body>
</html>
<script type="text/javascript">


    $(function () {

        $('#html table tr:first').hide();
        $("#html table tr th:first-child").hide();
        $("#html table tr td:last-child").hide();
        $('#html table tr:eq(1)').find('td').html('<%=LineName%>');

        $('#html table tr').each(function (i, t) {
            var h = $(t).attr('style').replace('height', '');
            if (h != '') {
                try{
                    h = h.replace(':', '');
                    h = h.replace('px;', '');
                    if (parseInt(h) < 5) {
                        //$(t).hide();
                        $(t).height(0);
                        return;
                    }
                }
                catch (ex) { }
            }
        });
    });


</script>
