<%@ Page Language="C#" AutoEventWireup="true" CodeFile="unpopEvaluatedata.aspx.cs" Inherits="report_unpopEvaluatedata" %>


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
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajaxLoaderPopPage.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajax_loader.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
    <script type="text/javascript">

        $(function () {

            var code = getQueryString('testcode');
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=unpopEvaluatedata&sTestcode=") %>" + code + "&r=" + Math.random(),
                success: function (msg) {
                    var popChart = new AmCharts.AmPieChart();
                    popChart.dataProvider = eval(msg);
                    popChart.titleField = "Description";
                    popChart.valueField = "IntNumber";
                    popChart.outlineColor = "#FFFFFF";
                    popChart.outlineAlpha = 0.8;
                    popChart.outlineThickness = 2;
                    popChart.labelsEnabled = false;
                    popChart.colors = ["#FF0F00", "#4B0C25", "#FF6600", "#990000", "#FF9E01", "#CA9726", "#FCD202", "#F8FF01", "#57032A", "#B0DE09", "#000000", "#04D215", "#333333", "#0D8ECF", "#999999", "#0D52D1", "#DDDDDD", "#2A0CD0", "#754DEB", "#8A0CCF", "#CD0D74"];


                    popChart.addListener("clickSlice", handleClick);


                    var legend = new AmCharts.AmLegend();
                    legend.align = "center";
                    legend.markerType = "circle";
                    popChart.addLegend(legend);

                    // WRITE
                    popChart.write("chartdiv2");
                }

            });
        });


        function handleClick(data) {
            $.colorbox({
                href: "../baseInfo/unpopfailgrid.aspx?testcode=" + data.dataItem.dataContext.Para1 + "&r=" + Math.random(),
                width: 960,
                height: 500,
                title: function () {
                    return "不合格试验数据分布图 [" + data.dataItem.dataContext.Description + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });
        }

    </script>
</head>
<body>
    <input type="hidden" id="hd_startDate" value="<%= StartDate%>" />
    <input type="hidden" id="hd_endDate" value="<%= EndDate%>" />
    <div id="chartdiv2" style="width: 100%; height: 510px;"></div>

</body>
</html>


