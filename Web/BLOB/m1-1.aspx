<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="m1-1.aspx.cs" Inherits="BLOB_m1_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/serial.js") %>"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>原材使用统计</span>
               
                <ul class="searchbar_01 clearfix">

                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1); createChart1();" /></li>

                </ul>
            </h2>
            <div class="content">

                <div id="divChart">
                    <div id="chartdiv"></div>
                    <div id="chartdiv1" class="chartdiv"></div>
                </div>
                
            </div>
        </div>
    </div>
    <script type="text/javascript">


        $(function () {
        
            createChart(1);
            createChart1();
        });

    
    

     



        var chart;
        function createChart(n) {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();

            if (chart != undefined && chart != null) {
                chart.clear();
            }

            $.post("M1-1.aspx", {Act:'Chart'  ,StartDate : $('#txt_startDate').val(),  EndDate : $('#txt_endDate').val()}, function (d) {
          
                // SERIAL CHART
                chart = new AmCharts.AmSerialChart();
                chart.dataProvider = eval(d);
                chart.categoryField = "ItemName";
                chart.rotate = true;


                chart.fontSize = 14;
                chart.startDuration = 1;
                chart.plotAreaFillAlphas = 0.2;
                chart.addListener("clickGraphItem", handleClick1);

                // the following two lines makes chart 3D
                chart.depth3D = 20;
                chart.angle = 30;

                // AXES
                // category
                var categoryAxis = chart.categoryAxis;
                categoryAxis.labelRotation = 90;
                categoryAxis.dashLength = 5;
                categoryAxis.gridPosition = "start";

                // value
                var valueAxis = new AmCharts.ValueAxis();
                //valueAxis.title = "使用总量(吨)";
                valueAxis.unit = "(吨)";
                valueAxis.dashLength = 5;
                chart.addValueAxis(valueAxis);

                // GRAPH
                var graph = new AmCharts.AmGraph();
                graph.labelText = "[[val]] 吨 ";
                graph.valueField = "val";
                graph.colorField = 'color';
                graph.fillColors = "#438eb9";
                graph.balloonText = "[[category]]: [[value]] 吨  资料:[[c]] 份";
                graph.type = "column";
                graph.lineAlpha = 0;
                graph.fillAlphas = 1;
                chart.addGraph(graph);

                // CURSOR
                var chartCursor = new AmCharts.ChartCursor();
                chartCursor.cursorAlpha = 0;
                chartCursor.zoomable = false;
                chartCursor.categoryBalloonEnabled = false;
                chart.addChartCursor(chartCursor);

                chart.creditsPosition = "top-right";

                chart.write("chartdiv");

            });


          


        }



        var chart1;
        function createChart1() {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();

            if (chart1 != undefined && chart1 != null) {
                chart1.clear();
            }
   
            $.post("M1-1.aspx", { Act: 'Chart1', StartDate: $('#txt_startDate').val(), EndDate: $('#txt_endDate').val() }, function (d) {
               
                // SERIAL CHART
                chart1 = new AmCharts.AmSerialChart();
                chart1.dataProvider = eval(d);
                chart1.categoryField = "ItemName";
                chart1.fontSize = 14;
                chart1.startDuration = 1;
                chart1.plotAreaFillAlphas = 0.2;
    
                chart1.addListener("clickGraphItem", handleClick);

                // the following two lines makes chart 3D
                chart1.depth3D = 20;
                chart1.angle = 30;

                // AXES
                // category
                var categoryAxis = chart1.categoryAxis;
                categoryAxis.labelRotation = 90;
                categoryAxis.dashLength = 5;
                categoryAxis.gridPosition = "start";
                categoryAxis.labelRotation = 45;

                // value
                var valueAxis = new AmCharts.ValueAxis();
                valueAxis.title = "厂家数量";
                valueAxis.dashLength = 5;
                chart1.addValueAxis(valueAxis);

                // GRAPH
                var graph = new AmCharts.AmGraph();
                graph.valueField = "val";
                graph.colorField = 'color';
                graph.labelText = "[[val]] 个";
                graph.balloonText = "[[category]]: [[value]]";
                graph.type = "column";
                graph.lineAlpha = 0;
                graph.fillAlphas = 1;

                chart1.addGraph(graph);

                // CURSOR
                var chartCursor = new AmCharts.ChartCursor();
                chartCursor.cursorAlpha = 0;
                chartCursor.zoomable = false;
                chartCursor.categoryBalloonEnabled = false;
                chart1.addChartCursor(chartCursor);

                chart1.creditsPosition = "top-right";

                chart1.write("chartdiv1");

            });





        }


        function handleClick(e) {

            openPopChart(e.item.dataContext.ItemID, e.item.dataContext.ItemName);

        }



        function openPopChart(id, name) {
            $.colorbox({
                href: "m1-1-1.aspx?ID=" + id + "&r=" + Math.random() + "&StartDate=" + $('#hd_startDate').val() + "&EndDate=" + $('#hd_endDate').val(),
                width: 900,
                height: 620,
                title: function () {
                    return name + " [厂家 " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });
        }


        function handleClick1(e) {

            openPopChart1(e.item.dataContext.ItemID, e.item.dataContext.ItemName);

        }
        function openPopChart1(id, name) {
            $.colorbox({
                href: "m1-1-2.aspx?ID=" + id + "&r=" + Math.random() + "&StartDate=" + $('#hd_startDate').val() + "&EndDate=" + $('#hd_endDate').val(),
                width: 900,
                height: 620,
                title: function () {
                    return name + " [ " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });
        }

    </script>
</asp:Content>
