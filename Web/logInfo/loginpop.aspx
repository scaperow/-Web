<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loginpop.aspx.cs" Inherits="logInfo_loginpop" %>

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
            $("#hid_testcode").val(code);
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=loginpop&sTestcode=") %>" + code + "&r=" + Math.random(),
                success: function (msg) {
                    // SERIAL CHART    
                    chart = new AmCharts.AmSerialChart();
                    chart.dataProvider = eval(msg);
                    chart.categoryField = "Description";
                    //chart.startDuration = 1;
                    chart.angle = 30;
                    chart.depth3D = 15;

                    chart.columnSpacing3D = 1;
                    chart.columnWidth = 0.6;
                    chart.equalSpacing = true;

                    chart.addListener("clickGraphItem", handleClick);


                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#438eb9";
                    //categoryAxis.title = "单位";

                    categoryAxis.labelRotation = 45;


                    //// value                
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.dashLength = 1;
                    valueAxis.axisThickness = 2;
                    valueAxis.axisColor = "#438eb9";//左边
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "登录次数";
                    valueAxis.integersOnly = true;
                    chart.addValueAxis(valueAxis);

           

                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "IntNumber";
                    graph.balloonText = "[[category]](登录总次数): [[value]]";
                    graph.labelText = "[[IntNumber]]";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.fillColors = "#438eb9";
                    graph.labelPosition = "right";
                    graph.showHandOnHover = true;
                    chart.addGraph(graph);

                    // WRITE
                    chart.write("chartdiv");

                }

            });
        });


        function handleClick(e) {
            $.colorbox({
                href: "onelogin.aspx?username=" + e.item.dataContext.Description + "&r=" + Math.random(),
                width: 750,
                height: 500,
                title: function () {
                    return "" + e.item.dataContext.Description + "[" + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]-登录日志";
                },
                close: '×',
                size: 12,
                iframe: true
            });
        }



    </script>
</head>
<body>
       <input type="hidden" id="hd_startDate" value="<%= StartDate%>"/>
        <input type="hidden" id="hd_endDate" value="<%= EndDate%>"/>

    <input type="hidden" id="hid_testcode" />

    <div id="chartdiv" style="width: 100%; height: 510px;"></div>

</body>
</html>
