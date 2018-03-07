<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m1-1-2.aspx.cs" Inherits="BLOB_m1_1_2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../js/jquery-1.9.0.min.js"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>

    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
</head>
<body>
    <form id="form1" runat="server">
  
         <div id="chartdiv" style="width: 100%; height: 510px;"></div>
    </form>
</body>
</html>
<script type="text/javascript">


    $(function () {

   
        Chart();
    });


    var chart;


    var ChartData = <%= ChartData%>;
   
    function Chart()
    {
        //$.post("M1-1-2.aspx", { Act: 'Chart', ID: '<%="ID".RequestStr()%>', StartDate: '<%="StartDate".RequestStr()%>', EndDate: '<%="EndDate".RequestStr()%>' }, function (d) {
           
           
          
            // SERIALL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = ChartData.Data;
            chart.categoryField = "Description";
            chart.plotAreaBorderAlpha = 0.2;
            chart.rotate = true;
            chart.depth3D = 20;
            chart.angle = 30;
            chart.startDuration = 1;
            // AXES
            // Category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.gridAlpha = 0.1;
            categoryAxis.axisAlpha = 0;
            categoryAxis.gridPosition = "start";

            // value
            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.title = "使用量 (吨)";
            valueAxis.unit = "(吨)";
            valueAxis.stackType = "regular";
            valueAxis.gridAlpha = 0.1;
            valueAxis.axisAlpha = 0;
            chart.addValueAxis(valueAxis);

            var color= ["#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000" ];

            // GRAPHS
            // firstgraph
            var c = 0;
            for (var i = 0; i < ChartData.Test.length; i++) {
                var graph = new AmCharts.AmGraph();
                graph.title = ChartData.Test[i].Description;
                graph.labelText = "[[value]] 吨";
                graph.valueField = ChartData.Test[i].Description;
                graph.type = "column";
                graph.lineAlpha = 0;
                graph.fillAlphas = 1;
                graph.lineColor = color[c];
                graph.fillColors =  color[c];
               
                graph.balloonText =  "[[title]]: [["+ChartData.Test[i].Description+"t]][[value]]吨 [["+ChartData.Test[i].Description+"t]]资料[["+ChartData.Test[i].Description+"c]]份";

               
                
                chart.addGraph(graph);
                
                c++;
            }
          
            // LEGEND
            var legend = new AmCharts.AmLegend();
            legend.position = "right";
            legend.borderAlpha = 0.3;
            legend.horizontalGap = 10;
            legend.switchType = "v";
            //chart.addLegend(legend);

            chart.creditsPosition = "top-right";

            // WRITE
            chart.write("chartdiv");
       // });

    }


    </script>
