<%@ Page Language="C#" AutoEventWireup="true" CodeFile="popmachineSummary.aspx.cs" Inherits="baseInfo_popmachineSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <link href="../css/default.css" rel="stylesheet" />
        <link href="../css/colorbox.css" rel="Stylesheet" />
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/main.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajax_loader.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajaxLoaderPopPage.js") %>"></script>
    <script type="text/javascript">
        
        $(function () {

            var code = getQueryString('testcode');
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("popmachineSummary.aspx?Act=Chart&sTestcode=") %>" + code + "&r=" + Math.random(),
                success: function (msg) {
      
                    var popChart = new AmCharts.AmPieChart();
                    popChart.dataProvider = eval(msg);
                    popChart.titleField = "Description";
                    popChart.valueField = "IntNumber";
                    popChart.outlineColor = "#FFFFFF";
                    popChart.outlineAlpha = 0.8;
                    popChart.outlineThickness = 2;
                    popChart.labelsEnabled = true;
                    popChart.colors = ["#FF0F00", "#4B0C25", "#FF6600", "#990000", "#FF9E01", "#CA9726", "#FCD202", "#F8FF01", "#57032A", "#B0DE09", "#000000", "#04D215", "#333333", "#0D8ECF", "#999999", "#0D52D1", "#DDDDDD", "#2A0CD0", "#754DEB", "#8A0CCF", "#CD0D74"];

                    popChart.addListener("clickSlice", handleClick);

                    // LEGEND
                    var legend = new AmCharts.AmLegend();
                    legend.align = "center";
                    legend.markerType = "circle";
                    popChart.addLegend(legend);

                    // WRITE
                    popChart.write("chartdiv2");
                }

            });
        });

        function handleClick(e) {
            $.colorbox({
                href: "machinegrid.aspx?testcode=" + e.dataItem.dataContext.Para1 + "&r=" + Math.random(),
                width: 800,
                height: 520,
                title: function () {
                    return "[" + e.dataItem.dataContext.Description + "]-设备情况/记录数(条)";
                },
                close: '×',
                size: 12,
                iframe: true
            });
        }

       
    </script>
</head>
<body>
    <div id="chartdiv2" style="width: 100%; height: 510px;"></div>

</body>
</html>

