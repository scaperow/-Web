<%@ Page Language="C#" AutoEventWireup="true" CodeFile="popParallelReport.aspx.cs" Inherits="report_popParallelReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/main.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajax_loader.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajaxLoaderPopPage.js") %>"></script>
    <script type="text/javascript">
        
        $(function () {
            var code = getQueryString('testroomid');
            var model = getQueryString('modelid');
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=pxreportpop&sTestcode=") %>" + code + "&sModelID=" + model + "&r=" + Math.random(),
                success: function (msg) {

                    var popChart = new AmCharts.AmSerialChart();
                    popChart.dataProvider = eval(msg);
                    popChart.categoryField = "Description";
                    popChart.pathToImages = "../images/";//添加拖动的图片
                    popChart.angle = 30;
                    popChart.depth3D = 15;

                    popChart.columnSpacing3D = 1;
                    popChart.columnWidth = 0.6;
                    popChart.equalSpacing = true;

                    var categoryAxis = popChart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#DADADA";
                    categoryAxis.title = "时间";
                    categoryAxis.labelRotation = 45;

                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#438EB9";
                    valueAxis.dashLength = 1;
                    valueAxis.integersOnly = true;
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "试验次数";
                    popChart.addValueAxis(valueAxis);

                    //var graph1 = new AmCharts.AmGraph();
                    //graph1.type = "line";
                    //graph1.title = "自检个数";
                    //graph1.valueField = "IntNumber";
                    //graph1.labelText = "[[IntNumber]]";
                    //graph1.balloonText = "[[category]](自检个数): [[value]]";                
                    //graph1.bullet = "round";
                    //graph1.lineColor = "#555555";//线条
                    //graph1.lineThickness = 2;
                    //graph1.labelPosition = "right";
                    //graph1.showHandOnHover = true;
                    //popChart.addGraph(graph1);

                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "IntNumber";
                    graph.balloonText = "[[category]](自检个数): [[value]]";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.title = "自检个数";
                    graph.fillColors = "#438eb9";//柱状图
                    graph.width = "auto";
                    graph.labelText = "[[IntNumber]]";//设置直接显示数目
                    graph.labelPosition = "top";
                    graph.showHandOnHover = true;
                    graph.labelText = "[[IntNumber]]";
                    popChart.addGraph(graph);

                    var graph2 = new AmCharts.AmGraph();
                    graph2.type = "line";
                    graph2.title = "平行个数";
                    graph2.valueField = "IntNumberMarks";
                    graph2.labelText = "[[IntNumberMarks]]";
                    graph2.balloonText = "[[category]](平行个数): [[value]]";
                    graph2.bullet = "round";
                    graph2.lineColor = "#E76049";//线条
                    graph2.lineThickness = 2;
                    graph2.labelPosition = "left";
                    graph2.showHandOnHover = true;
                    popChart.addGraph(graph2);


 
                    var chartCursor = new AmCharts.ChartCursor();//添加鼠标移动过以后的事件
                    popChart.addChartCursor(chartCursor);

                    var chartScrollbar = new AmCharts.ChartScrollbar();//鼠标拖动时间
                    popChart.addChartScrollbar(chartScrollbar);


                    var legend = new AmCharts.AmLegend();
                    popChart.addLegend(legend);
                    // WRITE
                    popChart.write("chartdiv2");
                }

            });
        });
    </script>
</head>
<body>
    <div id="chartdiv2" style="width: 100%; height: 510px;"></div>

</body>
</html>
