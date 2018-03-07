<%@ Page Language="C#" AutoEventWireup="true" CodeFile="popjqgrid.aspx.cs" Inherits="baseInfo_popjqgrid" %>

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
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=qxjqgridpop&sTestcode=") %>" + code + "&sModelID=" + model + "&r=" + Math.random(),
                success: function (msg) {
                    

                    var popChart = new AmCharts.AmSerialChart();
                    popChart.dataProvider = eval(msg);
                    popChart.categoryField = "Description";
                    popChart.pathToImages = "../images/";//添加拖动的图片



                    var categoryAxis = popChart.categoryAxis;
                    //categoryAxis.parseDates = false;
                    //categoryAxis.dashLength = 1;
                    //categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#438EB9";
                    categoryAxis.title = "日期";
                    categoryAxis.labelRotation = 45;

                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#E76049";
                    valueAxis.dashLength = 1;
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "资料数量";

                    //debugger;

                    valueAxis.minimum = maxminsize(msg, "min", "IntNumber")-1;
                    valueAxis.maximum = maxminsize(msg, "max", "IntNumber")+1;
                    valueAxis.precision = 0;//数值后的小数位数
                    valueAxis.minMaxMultiplier = 1.1;//上下刻度的冗余
                    valueAxis.autoGridCount = false;//自动生成刻度线
                    valueAxis.integersOnly = true;
                    valueAxis.gridCount = gridCount(valueAxis.maximum, valueAxis.minimum);//纵坐标显示几个刻度线
                    valueAxis.labelFrequency = parseInt(labelFrequency(valueAxis.maximum, valueAxis.gridCount));//每个几个刻度显示一个label


                    popChart.addValueAxis(valueAxis);

                    var graph1 = new AmCharts.AmGraph();
                    graph1.valueAxis = valueAxis;
                    graph1.valueField = "IntNumber";
                    graph1.balloonText = "[[category]]\n资料数量: [[value]]";
                    graph1.type = "line";
                    graph1.bullet = "round";
                    graph1.lineColor = "#E76049";//线条
                    graph1.labelText = "[[IntNumber]]";
                    graph1.labelPosition = "left";
                    graph1.lineThickness = 2;
                    graph1.showHandOnHover = true;
                    popChart.addGraph(graph1);


                    var chartCursor = new AmCharts.ChartCursor();//添加鼠标移动过以后的事件
                    popChart.addChartCursor(chartCursor);

                    var chartScrollbar = new AmCharts.ChartScrollbar();//鼠标拖动时间
                    popChart.addChartScrollbar(chartScrollbar);

                    //var legend = new AmCharts.AmLegend();
                    //popChart.addLegend(legend);
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

