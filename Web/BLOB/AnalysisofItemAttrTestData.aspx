<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AnalysisofItemAttrTestData.aspx.cs" Inherits="BLOB_AnalysisofItemAttrTestData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="../js/jquery-1.9.0.min.js"></script>
    <link href="../css/css.css" rel="stylesheet" />
    <link href="../css/colorbox.css" rel="stylesheet" />
    <link href="../css/initialize.css" rel="stylesheet" />
     <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/serial.js") %>"></script>
    <script src="../js/jquery.colorbox-min.js"></script>
    <style>
        .Item ,.Year{
            display:inline-block;
            border:#ccc 1px solid;
            padding:0 5px;
            color:#fff;
            background:#ccc;
        }
        .Active {
            background:#e76049;
            color:#FFF;
             border:#e76049 1px solid;
        }
    </style>
</head>
<body>
<div class="piece">
        <div class="piece_con">


            <div class="content">

                <div id="divChart">
                    <div id="chartdiv"></div>

                </div>
                
            </div>
        </div>
    </div>
</body>
</html>
 <script type="text/javascript">


     $(function () {

         createChart(1);
     });




     var tongChart;
     function createChart(n) {
         $(".chart_btn").removeClass("bagc_8584");
         $(".list_btn").addClass("bagc_8584");
         $("#divChart").show();
         $("#divList").hide();

         if (tongChart != undefined && tongChart != null) {
             tongChart.clear();
         }

         $.post('AnalysisofItemAttrTestData.aspx', {
             Act: 'Chart',
             Item: '<%="ID".RequestStr()%>',
             Factory: '<%="FID".RequestStr()%>',
             Attr: '<%="ATTR".RequestStr()%>',
             AttrName: '<%="ATTRNAME".RequestStr()%>',
             Model: '<%="MO".RequestStr()%>',
             M: '<%="M".RequestStr()%>',
             StartDate: '<%="StartDate".RequestStr()%>',
             EndDate: '<%="EndDate".RequestStr()%>'
         }, function (d) {

             chartData = eval(d);
             if (chartData.length == 0) {
                 return;
             }

             tongChart = new AmCharts.AmSerialChart();
             tongChart.dataProvider = chartData;
             tongChart.categoryField = "BGRQ";
             tongChart.pathToImages = "../images/";
             var categoryAxis = tongChart.categoryAxis;
             categoryAxis.parseDates = true;
             categoryAxis.minPeriod = "DD";
             categoryAxis.axisColor = "#DADADA";
             categoryAxis.title = "日期";
             categoryAxis.labelRotation = 45;
             categoryAxis.dateFormats = [{
                 period: 'DD',
                 format: 'M月D日'
             }, {
                 period: 'WW',
                 format: 'M月D日'
             }, {
                 period: 'MM',
                 format: 'M月'
             }, {
                 period: 'YYYY',
                 format: 'YYYY年'
             }];
             tongChart.addListener("clickGraphItem", handleClick);

             var valueAxis = new AmCharts.ValueAxis();
             valueAxis.axisColor = "#438EB9";
             valueAxis.dashLength = 1;
             valueAxis.title = "检测值<%="Unit".RequestStr()%>";
             valueAxis.unit = '<%="Unit".RequestStr()%>';


             $('#liZVal').html('');
             if ($('#Model').find("option:selected").val() != '' && chartData[0].ZVal != undefined) {

                 $('#liZVal').html('<font color="green">标准值: (' + chartData[0].ZVal + ')</font>');

                 var reg = RegExp("合格\\S*", "gmi");

                 if (reg.test(chartData[0].ZVal)) {
                     return;
                 }
                 reg = RegExp("^[\\d]+～[\\d]+$", "gmi");
                 var reg1 = RegExp("^[-|+]?\\d+\\.?\\d*～[-|+]?\\d+\\.?\\d*$", "gmi");


                 var guide = new AmCharts.Guide();

                 if (reg.test(chartData[0].ZVal)) {

                     var arr = chartData[0].ZVal.split('～');

                     guide.value = parseInt(arr[0]);
                     guide.lineColor = "#F00";
                     guide.position = "right";
                     guide.dashLength = 0;
                     guide.label = "标准值(" + chartData[0].ZVal + ")";
                     guide.lineThickness = 1;
                     guide.toValue = parseInt(arr[1]);
                     guide.fillColor = '#F00';
                     guide.fillAlpha = 0.2;
                     guide.labelRotation = 1;
                     guide.inside = true;
                     guide.lineAlpha = 1;

                     valueAxis.minimum = parseInt(arr[0]) - 1;
                     valueAxis.maximum = parseInt(arr[1]) + 1;

                 }
                 else if (reg1.test(chartData[0].ZVal)) {

                     var arr = chartData[0].ZVal.split('～');


                     var guide = new AmCharts.Guide();
                     guide.value = Number(arr[0]);
                     guide.lineColor = "#F00";
                     guide.position = "right";
                     guide.dashLength = 0;
                     guide.label = "标准值(" + chartData[0].ZVal + ")";
                     guide.lineThickness = 1;
                     guide.toValue = Number(arr[1]);
                     guide.fillColor = '#F00';
                     guide.fillAlpha = 0.2;
                     guide.labelRotation = 1;
                     guide.inside = true;
                     guide.lineAlpha = 1;
                     valueAxis.minimum = Number(arr[0]) - 1;
                     valueAxis.maximum = Number(arr[1]) + 1;
                 }
                 else {

                     var ZVal = chartData[0].ZVal.replace(/\(\S*\)/g, '');
                     ZVal = chartData[0].ZVal.replace(/^[\d]+～[\d]+$/, '');
                     ZVal = chartData[0].ZVal.replace(/^≥+|≤+|>+|<+/g, '');

                     var max = parseInt(maxsize(chartData, chartData[0].Val, "Val")) + 1;

                     max = max >= parseInt(ZVal) ? max : parseInt(ZVal);

                     max = Math.round(max);

                     var min = minsize(chartData, chartData[0].Val, "Val") - 1;
                     min = min >= parseInt(ZVal) ? parseInt(ZVal) - 1 : min;

                     guide.value = ZVal;
                     guide.lineColor = "#F00";
                     guide.position = "right";
                     guide.dashLength = 0;
                     guide.label = "标准值(" + chartData[0].ZVal + ")";
                     guide.lineThickness = 1;
                     //guide.toValue = max;
                     guide.fillColor = '#F00';
                     guide.fillAlpha = 0.2;
                     guide.labelRotation = 1;
                     guide.inside = true;
                     guide.lineAlpha = 1;

                     valueAxis.minimum = min;
                     valueAxis.maximum = max;
                 }
                 valueAxis.addGuide(guide);
             }
             else {
                 valueAxis.minimum = parseInt(minsize(chartData, 0, "Val")) - 1;
                 valueAxis.maximum = parseInt(maxsize(chartData, 100, "Val")) + 1;

             }


             //valueAxis.precision = 0;//数值后的小数位数
             valueAxis.minMaxMultiplier = 1;//上下刻度的冗余
             valueAxis.autoGridCount = false;//自动生成刻度线
             valueAxis.integersOnly = true;
             valueAxis.gridCount = parseInt(valueAxis.maximum - valueAxis.minimum) + 2;//纵坐标显示几个刻度线
             valueAxis.labelFrequency = 5;//每个几个刻度显示一个label

             tongChart.addValueAxis(valueAxis);


             var graph = new AmCharts.AmGraph();
             graph.type = "smoothedLine";
             //graph.title = "组值";
             graph.bullet = "round";
             graph.bulletSize = 6;
             graph.bulletColor = "#438EB9";
             graph.balloonText = "试验室:[[description]] 报告编号:[[BGBH]] 日期:[[BGRQ]] 检测值:[[Val]]";
             graph.valueField = "Val";
             graph.lineThickness = 0;
             graph.lineColor = "#438EB9";
             graph.showHandOnHover = true;
             tongChart.addGraph(graph);

             var chartScrollbar = new AmCharts.ChartScrollbar();//鼠标拖动时间
             tongChart.addChartScrollbar(chartScrollbar);

             // WRITE
             tongChart.write("chartdiv");

         });
     }

     function minsize(data, m, column) {
         var len = data.length;
         var j = 0;
         if (len > 0) {
             var j = data[0][column];
             for (var i = 1; i < len ; i++) {
                 if (data[i][column] < j) {
                     j = data[i][column];
                 }
             }
         }
         if (m < j) {
             j = m;
         }
         return j;
     }

     function maxsize(data, m, column) {
         var len = data.length;
         var j = 0;
         if (len > 0) {
             var j = data[0][column];
             for (var i = 1; i < len ; i++) {
                 if (data[i][column] > j) {
                     j = data[i][column];
                 }
             }
         }
         if (m > j) {
             j = m;
         }
         return j;
     }

     function handleClick(e) {

         $.colorbox({
             href: "../report/onesearch.aspx?id=" + e.item.dataContext.DataID + "&MName=" + $('a.Item.Active').text() + "&r=" + Math.random(),
             width: 950,
             height: 1240,
             title: function () {
                 return "报告编号：[" + e.item.dataContext.BGBH + "]  报告日期：[" + e.item.dataContext.BGRQ + "] ";
             },
             close: '',
             iframe: true
         });

     }
    </script>
