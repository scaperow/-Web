<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testdata.aspx.cs" Inherits="report_testdata" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/default.css" rel="stylesheet" />
    <link href="../css/colorbox.css" rel="Stylesheet" />
    <link href="../Plugin/bootstrap/css/bootstrap.css" rel="Stylesheet" />

    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/main.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajax_loader.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajaxLoaderPopPage.js") %>"></script>

    <script type="text/javascript" src="../Plugin/bootstrap/js/bootstrap.js"></script>
    <style>

        .CPane {
            border: #ddd 1px solid;
            border-top:none;
            position:relative;
            height:370px;
            display:block;
        }

        .Loading {
            position: fixed;
            background: #000;
            filter: alpha(opacity=50);
            opacity: .5;
            width:100%;
            height:100%;
            z-index:9999999;
            color:#FFF;
            font-size:14px;
            text-align:center;
            line-height:400px;
        }
    </style>
    <script type="text/javascript">
       


        var _Charts = [];

        var Count =<%=GetCount()%>;

        $(function () {

            if(Count <=0)
            {
                $('#Loaderr').show();
                return;
            }
                for (var i = 0; i < Count; i++) {
                    $('.nav').append('  <li class="' + (i == 0 ? 'active' : '') + '"><a data="' + i + '" href="#t' + i.toString() + '" data-toggle="tab" >第' + (i + 1).toString() + '<%=_DeviceType%></a></li>');
                    $('.tab-content').append('<div class="tab-pane CPane ' + (i == 0 ? 'active' : '') + '" id="t' + i.toString() + '"></div>');
                    if (i == 0) {
                        BindCharts("t" + i.toString(),(i + 1), '第' + (i + 1).toString() + '<%=_DeviceType%>')
                        _Charts[i] = true;
                    }
                }
                $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                 
                    var id = parseInt($(e.target).attr('data'));
                    if (!_Charts[id]) {
                        BindCharts("t" + id.toString(), (id + 1), '第' + (id + 1).toString() + '<%=_DeviceType%>')
                        _Charts[id] = true;
                    }
                })
         
                //$('.Loading').hide();
        });





        function BindCharts(type, n, title) {
            var code = getQueryString('id');
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=testdata&sTestcode=") %>" + code + "&isUnit=" + n + "&r=" + Math.random(),
                success: function (msg) {



                    var popChart = new AmCharts.AmSerialChart();
                    popChart.dataProvider = eval(msg);
                    popChart.categoryField = "Time";
                    //popChart.pathToImages = "../images/";//添加拖动的图片
                    popChart.addTitle(title, 12);


                    var categoryAxis = popChart.categoryAxis;
                    //categoryAxis.parseDates = true; // as our data is date-based, we set parseDates to true
                    //categoryAxis.minPeriod = "ss"; // our data is yearly, so we set minPeriod to YYYY         

                    categoryAxis.axisColor = "#438EB9";
                    //categoryAxis.title = "时间";
                    //categoryAxis.labelRotation = 15;

                    var valueAxis = new AmCharts.ValueAxis();
                    //valueAxis.minimum = 0;
                    valueAxis.axisColor = "#E76049";
                    valueAxis.dashLength = 1;
                    valueAxis.integersOnly = true;
                    //valueAxis.title = title;

                    popChart.addValueAxis(valueAxis);

                    graph1 = new AmCharts.AmGraph();
                    //graph1.colorField = "color";
                    graph1.lineColor = "#CB1B04";
                


                    graph1.balloonText = "力值: [[value]]";
                    graph1.bullet = "round";
                    graph1.bulletSize = 2;
                    //graph1.connect = false;
                    graph1.lineThickness = 2;
                    graph1.valueField = "Value";
                    popChart.addGraph(graph1);

                    

                    //var chartCursor = new AmCharts.ChartCursor();//添加鼠标移动过以后的事件
                    //popChart.addChartCursor(chartCursor);

                    //var chartScrollbar = new AmCharts.ChartScrollbar();//鼠标拖动时间
                    //popChart.addChartScrollbar(chartScrollbar);
                    popChart.write(type);
                    $('.Loading').hide();
                }
            });
        }
    </script>
</head>
<body>
   
    <!-- Nav tabs -->
<ul class="nav nav-tabs">

</ul>

<!-- Tab panes -->
<div class="tab-content">

</div>
   <div id="Loaderr" style="display:none; text-align:center;"><br /><br /><br /><br /><h3 style="">无过程数据！</h3></div>
    
</body>
</html>
