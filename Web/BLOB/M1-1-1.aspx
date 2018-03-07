<%@ Page Language="C#" AutoEventWireup="true" CodeFile="M1-1-1.aspx.cs" Inherits="BLOB_M1_1_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../js/jquery-1.9.0.min.js"></script>

    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>

</head>
<body>
    <form id="form1" runat="server">
  
         <div id="chartdiv" style="width: 100%; height: 510px;"></div>
    </form>
</body>
</html>
<script type="text/javascript">


    $(function () {

        createChart(1);

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

        $.post("M1-1-1.aspx", { Act: 'Chart', ID: '<%="ID".RequestStr()%>', StartDate: '<%="StartDate".RequestStr()%>', EndDate: '<%="EndDate".RequestStr()%>' }, function (d) {

            var popChart = new AmCharts.AmPieChart();
            popChart.dataProvider = eval(d);
            popChart.titleField = "FactoryName";
            popChart.valueField = "val";
            popChart.outlineColor = "#FFFFFF";
            popChart.outlineAlpha = 0.8;
            popChart.outlineThickness = 2;
            popChart.labelsEnabled = true;
            //popChart.colors = ["#FF0000","#FF6600","#CCFF00" ];

            //popChart.addListener("clickSlice", handleClick);
            popChart.depth3D = 15;
            popChart.angle = 30;

            // LEGEND
            var legend = new AmCharts.AmLegend();
            legend.align = "center";
            legend.markerType = "circle";
            popChart.addLegend(legend);

            // WRITE
            popChart.write("chartdiv");

        });





    }

    </script>