<%@ Page Language="C#" AutoEventWireup="true" CodeFile="onelogin.aspx.cs" Inherits="logInfo_onelogin" %>

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
            var id = $("#hd_name").val();

            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=onelogin&sUser=") %>" + id + "&r=" + Math.random(),
                success: function (msg) {

                    var popChart = new AmCharts.AmSerialChart();
                    popChart.dataProvider = eval(msg);
                    popChart.categoryField = "Description";
                    popChart.pathToImages = "../images/";//添加拖动的图片



                    var categoryAxis = popChart.categoryAxis;           
                    categoryAxis.axisColor = "#438EB9";
                    categoryAxis.title = "日期";
                    categoryAxis.labelRotation = 45;

                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#E76049";
                    valueAxis.dashLength = 1;
                    valueAxis.integersOnly = true;
                    valueAxis.title = "登录次数";

                    popChart.addValueAxis(valueAxis);

                    var graph1 = new AmCharts.AmGraph();
                    graph1.valueAxis = valueAxis;
                    graph1.valueField = "IntNumber";
                    graph1.balloonText = "[[category]]\登录次数: [[value]]";
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
                    popChart.write("chartdiv2");
                }

            });
        });
    </script>
</head>
<body>
    <div id="chartdiv2" style="width: 100%; height: 380px;"></div>
    <input type="hidden" id="hd_name" value="<%= Request.Params["username"].ToString()%>"/>
</body>
</html>
