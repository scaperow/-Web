<%@ Page Language="C#" AutoEventWireup="true" CodeFile="parallellinechart.aspx.cs" Inherits="report_parallellinechart" %>

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
            BindCharts("chartdiv1", 1, "试验室分布图");
            BindCharts("chartdiv2", 2, "平行质量指数分布图");
        });

        function BindCharts(type, n, title) {
            var code = getQueryString('testcode');
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=parallellinechart&sTestcode=") %>" + code + "&IsUnit=" + n + "&r=" + Math.random(),
                success: function (msg) {
                    var popChart = new AmCharts.AmPieChart();
                    popChart.addTitle(title, 12);


                    popChart.dataProvider = eval(msg);
                    popChart.titleField = "Description";
                    popChart.valueField = "IntNumber";
                    popChart.outlineColor = "#FFFFFF";
                    popChart.outlineAlpha = 0.8;
                    popChart.outlineThickness = 2;

                    popChart.labelsEnabled = false;
                    popChart.colors = ["#FF0F00", "#4B0C25", "#FF6600", "#990000", "#FF9E01", "#CA9726", "#FCD202", "#F8FF01", "#57032A", "#B0DE09", "#000000", "#04D215", "#333333", "#0D8ECF", "#999999", "#0D52D1", "#DDDDDD", "#2A0CD0", "#754DEB", "#8A0CCF", "#CD0D74"];
                    popChart.addListener("clickSlice", handleClick);

                    //LEGEND
                    var legend = new AmCharts.AmLegend();
                    legend.align = "center";
                    legend.markerType = "square";
                    //legend.position = "right";

                    popChart.addLegend(legend);

                    // WRITE
                    popChart.write(type);
                }

            });
        }

        function handleClick(e) {
            $.colorbox({
                href: "parallelchartgrid.aspx?testroomid=" + e.dataItem.dataContext.Para1 + "&modelid=" + e.dataItem.dataContext.Para2 + "&sNuit=" + e.dataItem.dataContext.Para3 + "&r=" + Math.random(),
                width: 700,
                height: 500,
                title: function () {
                    return "试验模板平行频率及指数 [" + e.dataItem.dataContext.Description + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '×',
                size: 12,
                iframe: true
            });
        }
    </script>
</head>
<body >

       <input type="hidden" id="hd_startDate" value="<%= StartDate%>"/>
        <input type="hidden" id="hd_endDate" value="<%= EndDate%>"/>

    <div style=" overflow-y:hidden; overflow-x:hidden;">

    <table style=" font-size:12px; font-family:'Microsoft Yahei','Tahoma','SimSun';">

        <tr>
            <td style="border-right: solid 1px #ADADAD; width:400px; ">
               
                <div id="chartdiv1" style="width: 100%; height: 500px;"></div>
            </td>

            <td style="width:400px; "> 
                <div id="chartdiv2" style="width: 100%; height: 500px;"></div>
            </td>
        </tr>

        

    </table>
</div>
</body>
</html>
