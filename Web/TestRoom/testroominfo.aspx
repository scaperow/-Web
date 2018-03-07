<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testroominfo.aspx.cs" Inherits="TestRoom_testroominfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link href="../Plugin/bootstrap/css/bootstrap.css" rel="Stylesheet" />
     <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
     <script type="text/javascript" src="../Plugin/bootstrap/js/bootstrap.js"></script>
</head>
<body>
    <form id="form1" runat="server" style="text-align:center;">


    </form>
</body>
</html>
<script type="text/javascript">



    $(function () {

   

        $("table tr:first").hide();
        $("table tr th:first-child").hide();
        $("table tr td:last-child").hide();
        $('table tr:eq(1)').find('td').html('<%=LineName%>');
        
   

        $('table tr').each(function (i, t) {
            var h = $(t).attr('style').replace('height', '');
            if (h != '') {
                try {
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


        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

            window.location = $(e.target).attr('data')+"&r=" + Math.random();
        });

    });

    </script>
